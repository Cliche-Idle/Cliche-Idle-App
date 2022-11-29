using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections;

namespace UIViews
{
    /// <summary>
    /// Provides base functionality to PopUp windows.
    /// </summary>
    public abstract class PopUp : UIScript
    {
        /// <summary>
        /// The background colour of the PopUp's cover element.
        /// </summary>
        [HideInInspector]
        public Color PopupBackgroundColor = new Color32(55, 55, 55, 199);

        private int _animMinSize = 90;
        private int _animMaxSize = 100;
        private float _animSpeed = .005f;

        /// <summary>
        /// The PopUp's base container.
        /// </summary>
        private VisualElement BaseContainer;

        /// <summary>
        /// The PopUp window's content container. Load the actual window contents here.
        /// </summary>
        protected VisualElement ContentContainer { get; private set; }

        void Start()
        {
            ContainerID = WrapperVisualElementName;            
        }

        public override void ShowView()
        {
            VisualElement popupBase = BuildPopUpBaseElement();

            // OnEnterFocus
            popupBase.RegisterCallback<AttachToPanelEvent>(evt => {
                OnEnterFocus();
                IsViewActive = true;
            });

            // OnLeaveFocus
            popupBase.RegisterCallback<DetachFromPanelEvent>(evt => {
                OnLeaveFocus();
                IsViewActive = false;
            });

            Navigator.Target.rootVisualElement.Add(popupBase);
            BaseContainer = popupBase;
            StartCoroutine(AnimationOpen());
        }

        public override void HideView()
        {
            StartCoroutine(AnimationClose());
        }

        /// <summary>
        /// Builds and sets up the base PopUp window, with all events attached. This is done in code because reliably referencing a VisualTreeAsset in code is pain.
        /// </summary>
        /// <returns></returns>
        private VisualElement BuildPopUpBaseElement()
        {
            VisualElement popupBase = new VisualElement()
            {
                name = WrapperVisualElementName,
                style = {
                    position = Position.Absolute,
                    top = 0,
                    display = DisplayStyle.Flex,
                    overflow = Overflow.Visible,
                    alignItems = Align.Center,
                    justifyContent = Justify.Center,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                    backgroundColor = PopupBackgroundColor
                } 
            };
            VisualElement popupContentContainer = new VisualElement()
            {
                name = "PopUp_ContentContainer",
                style = {
                    //flexGrow = 1,
                    width = Length.Percent(100),
                    height = Length.Percent(100),
                    alignItems = Align.Center,
                    justifyContent = Justify.Center
                }
            };
            Button popupCloseButton = new Button()
            {
                name = "PopUp_CloseButton",
                text = "X",
                style = {
                    position = Position.Absolute,
                    top = 0,
                    right = 0,
                    width = 150,
                    height = 150,
                    backgroundColor = new Color(0, 0, 0, 0),
                    borderBottomWidth = 0,
                    borderLeftWidth = 0,
                    borderTopWidth = 0,
                    borderRightWidth = 0,
                    marginTop = 25,
                    marginBottom = 25,
                    marginLeft = 25,
                    marginRight = 25,
                    paddingLeft = 15,
                    fontSize = 100,
                    color = new Color(1, 1, 1, 1),
                    unityFontStyleAndWeight = FontStyle.Bold,
                },
            };
            popupCloseButton.clicked += ClosePopUpWindow;

            UXMLDocument.CloneTree(popupContentContainer);

            popupBase.Add(popupContentContainer);
            popupBase.Add(popupCloseButton);

            ContentContainer = popupContentContainer;

            return popupBase;
        }

        /// <summary>
        /// Closes the PopUp. Same as <see cref="UIScript.HideView()"/>.
        /// </summary>
        protected void ClosePopUpWindow()
        {
            HideView();
        }

        IEnumerator AnimationOpen()
        {
            for (int containerSize = _animMinSize; containerSize <= _animMaxSize; containerSize++)
            {
                BaseContainer.style.opacity = containerSize;
                ContentContainer.style.width = Length.Percent(containerSize);
                ContentContainer.style.height = Length.Percent(containerSize);
                yield return new WaitForSeconds(_animSpeed);
            }
            // Set state to open
        }

        IEnumerator AnimationClose()
        {
            for (int containerSize = _animMaxSize; containerSize >= _animMinSize; containerSize--)
            {
                BaseContainer.style.opacity = containerSize;
                ContentContainer.style.width = Length.Percent(containerSize);
                ContentContainer.style.height = Length.Percent(containerSize);
                yield return new WaitForSeconds(_animSpeed);
            }
            // Set state to closed
            ContentContainer = null;
            Navigator.Target.rootVisualElement.Remove(BaseContainer);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PopUp), true, isFallback = true)]
    public class PopUpEditor : Editor
    {
        private bool _isFoldoutOpen = true;

        private SerializedProperty _viewNavigator;
        private SerializedProperty _viewID;
        private SerializedProperty _viewUIDoc;
        private SerializedProperty _viewStatic;

        void OnEnable()
        {
            _viewNavigator = serializedObject.FindProperty("<Navigator>k__BackingField");
            _viewID = serializedObject.FindProperty("<ID>k__BackingField");
            _viewUIDoc = serializedObject.FindProperty("<UXMLDocument>k__BackingField");
            _viewStatic = serializedObject.FindProperty("<IsStatic>k__BackingField");
        }

        public override void OnInspectorGUI()
        {
            // Get the view data
            var popup = target as PopUp;
            serializedObject.Update();

            // View setup is in its own foldout so it doesn't take up too much space on derived scripts
            _isFoldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_isFoldoutOpen, "View setup");
            if (_isFoldoutOpen)
            {
                // Increase ident by 1 so the foldout is more visually separated
                EditorGUI.indentLevel++;

                // Set the UI Navigator instance
                _viewNavigator.objectReferenceValue = (ViewNavigator)EditorGUILayout.ObjectField("UI Navigator", popup.Navigator, typeof(ViewNavigator), true);

                EditorGUILayout.Space(10);

                _viewID.stringValue = EditorGUILayout.TextField("ID", popup.ID);
                _viewUIDoc.objectReferenceValue = (VisualTreeAsset)EditorGUILayout.ObjectField("UI Document", popup.UXMLDocument, typeof(VisualTreeAsset), true);
                _viewStatic.boolValue = EditorGUILayout.Toggle("Is Static", popup.IsStatic);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Toggle("Is View Active", popup.IsViewActive);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space(10);

                popup.PopupBackgroundColor = EditorGUILayout.ColorField("PopUp Background Color", popup.PopupBackgroundColor);

                // Reset ident to normal
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);
            serializedObject.ApplyModifiedProperties();
            // Draw the default inspector last so the script's UI can be separated. This will draw the auto UI as normal for derived scripts
            DrawDefaultInspector();
        }
    }
#endif
}