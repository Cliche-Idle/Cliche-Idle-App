using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

public class IconPreview : EditorWindow
{
    private List<OverlayLayer> OverlayLayers = new List<OverlayLayer>();
    private Sprite DefaultIcon;
    // ---- EDITOR ----
    private ObjectField IconBaseSpriteSelector;
    // -- Preview Icons:
    private List<OverlayIcon> Preview_Icons = new List<OverlayIcon>();
    private int[] Preview_Sizes = { 250, 200, 150, 100, 75, 50, 25 };
    private OverlayIcon Preview_Icon_Custom;
    private IntegerField Preview_Icon_Custom_Height;
    private IntegerField Preview_Icon_Custom_Width;
    private float Preview_Icon_Custom_Aspect_Ratio;
    private Toggle Preview_Icon_Custom_Lock_Aspect_Ratio;

    //

    [MenuItem("Tools/Icon preview")]
    public static void Init()
    {
        IconPreview wnd = GetWindow<IconPreview>();
        wnd.titleContent = new GUIContent("Icon preview");
        Vector2 size = new Vector2(900, 500);
        wnd.minSize = size;
    }
    public void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Tooling/Editor/ItemIconPreview/UI/IconPreviewMain.uxml");

        VisualElement rootFromUXML = visualTree.Instantiate();

        rootVisualElement.Add(rootFromUXML);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Tooling/Editor/ItemIconPreview/UI/IconPreviewMain.uss");

        rootVisualElement.styleSheets.Add(styleSheet);

        m_ItemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Tooling/Editor/ItemIconPreview/UI/OverlayRowItemTemplate.uxml");

        m_ItemsTab = rootVisualElement.Q<VisualElement>("ItemsTab");

        m_DetailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details");

        DefaultIcon = Resources.Load<Sprite>("icons/placeholder_250x250_cross2");

        // Initialise Icon displays
        IconsInit();
        // Initialise base editor controls
        BaseEditInit();
        // Initialise overlay editor controls
        OverlayEditInit();
        // Initialise overlay list view
        OverlayListInit();
    }
    // ----- Icons -----

    private void IconsInit()
    {
        VisualElement PreviewContainer = rootVisualElement.Q<VisualElement>("PreviewContainer");
        rootVisualElement.Q<VisualElement>("Icon_Custom_Container").Add(new OverlayIcon("Icon_Custom", DefaultIcon, 250, 250));
        Preview_Icon_Custom = rootVisualElement.Q<OverlayIcon>("Icon_Custom");
        // Initialise previews:
        foreach (int size in Preview_Sizes)
        {
            PreviewContainer.Add(new VisualElement {
                name = $"Icon_{size}_Container",
                style = {
                    flexDirection = FlexDirection.Column,
                    marginTop = 15
                }
            });
            PreviewContainer.Q<VisualElement>($"Icon_{size}_Container").Add(new Label {
                text = $"Preview: {size}px by {size}px",
                style = {
                    flexDirection = FlexDirection.Column,
                    marginTop = 15,
                    marginBottom = 10
                }
            });
            PreviewContainer.Q<VisualElement>($"Icon_{size}_Container").Add(new OverlayIcon($"Icon_{size}", DefaultIcon, size, size));
            Preview_Icons.Add(PreviewContainer.Q<OverlayIcon>($"Icon_{size}"));
        }
    }

    // ----- Base Edit -----

    private void BaseEditInit()
    {
        // Set the sprite selector event handler to update all icon previews
        IconBaseSpriteSelector = m_DetailSection.Q<ObjectField>("IconSprite");
        IconBaseSpriteSelector.RegisterValueChangedCallback(sprite => {
            if (sprite.newValue != null)
            {
                Texture2D IconBase = ((Sprite)sprite.newValue).texture;
                Preview_Icon_Custom.style.backgroundImage = IconBase;
                foreach (var previewIcon in Preview_Icons)
                {
                    previewIcon.style.backgroundImage = IconBase;
                }
            }
        });
        // Set aspect ratio toggle event handler to save the aspect ratio
        Preview_Icon_Custom_Lock_Aspect_Ratio = m_DetailSection.Q<Toggle>("IconAspectRatioToggle");
        Preview_Icon_Custom_Lock_Aspect_Ratio.RegisterValueChangedCallback(enabled => {
            Preview_Icon_Custom_Aspect_Ratio = ((float)Preview_Icon_Custom_Width.value / (float)Preview_Icon_Custom_Height.value);
        });
        // Set the height editor event handler to update the display height (and width if aspect ratio is enabled)
        Preview_Icon_Custom_Height = m_DetailSection.Q<IntegerField>("IconHeight");
        Preview_Icon_Custom_Height.RegisterValueChangedCallback(height => {
            if (height.newValue > 0)
            {
                if (Preview_Icon_Custom_Lock_Aspect_Ratio.value)
                {
                    int newWidth = Convert.ToInt32(height.newValue * Preview_Icon_Custom_Aspect_Ratio);
                    Preview_Icon_Custom.style.width = newWidth;
                    Preview_Icon_Custom_Width.value = newWidth;
                }
                Preview_Icon_Custom.style.height = height.newValue;
            }
            else
            {
                Preview_Icon_Custom_Height.SetValueWithoutNotify(1);
            }
        });
        // Set the width editor event handler to update the display width (and height if aspect ratio is enabled)
        Preview_Icon_Custom_Width = m_DetailSection.Q<IntegerField>("IconWidth");
        Preview_Icon_Custom_Width.RegisterValueChangedCallback(width => {
            if (width.newValue > 0)
            {
                if (Preview_Icon_Custom_Lock_Aspect_Ratio.value)
                {
                    int newHeight = Convert.ToInt32((float)width.newValue / Preview_Icon_Custom_Aspect_Ratio);
                    Preview_Icon_Custom.style.height = newHeight;
                    Preview_Icon_Custom_Height.value = newHeight;
                }
                Preview_Icon_Custom.style.width = width.newValue;
            }
            else
            {
                Preview_Icon_Custom_Width.SetValueWithoutNotify(1);
            }
        });
    }

    // ----- Overlay Edit -----

    private TextField Overlay_ID;
    private ObjectField Overlay_Icon;
    private TextField Overlay_Text;
    private EnumField Overlay_Position_Friendly;
    private enum Overlay_Position_Friendly_Values
    {
        Center = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 3,
        BottomRight = 4
    }
    private IntegerField Overlay_Position_Index;
    private Button Add_Overlay_Btn;
    private Button Delete_Overlay_Btn;

    private void OverlayEditInit() {
        Overlay_ID = m_DetailSection.Q<TextField>("OverlayID");
        /*Overlay_ID.RegisterValueChangedCallback(value => {
            if (SelectedLayer != null && value.newValue == SelectedLayer.ID)
            {
                Add_Overlay_Btn.text = "Update";
                Add_Overlay_Btn.clicked -= AddOverlayButton_OnClick;
                Add_Overlay_Btn.clicked += AddOverlayButton_Update_OnClick;
            }
            else
            {
                Add_Overlay_Btn.text = "Add as new";
                Add_Overlay_Btn.clicked -= AddOverlayButton_Update_OnClick;
                Add_Overlay_Btn.clicked += AddOverlayButton_OnClick;
            }
        });*/
        Overlay_Icon = m_DetailSection.Q<ObjectField>("OverlayIcon");
        Overlay_Text = m_DetailSection.Q<TextField>("OverlayText");
        Overlay_Position_Friendly = m_DetailSection.Q<EnumField>("OverlayPositionFriendly");
        Overlay_Position_Friendly.Init(new Overlay_Position_Friendly_Values());
        Overlay_Position_Friendly.RegisterValueChangedCallback(value => {
            Overlay_Position_Index.value = Convert.ToInt32(Enum.Parse(typeof(Overlay_Position_Friendly_Values), value.newValue.ToString()));
        });
        Overlay_Position_Index = m_DetailSection.Q<IntegerField>("OverlayPositionIndex");
        Add_Overlay_Btn = m_DetailSection.Q<Button>("Btn_AddOverlay");
        Add_Overlay_Btn.clicked += AddOverlayButton_OnClick;
        Delete_Overlay_Btn = m_DetailSection.Q<Button>("Btn_DeleteOverlay");
        Delete_Overlay_Btn.clicked += RemoveOverlayButton_OnClick;
    }

    private void OverlayEditUpdateData(OverlayLayer layer) {
        Overlay_ID.value = layer.ID;
        Overlay_Icon.value = layer.Icon;
        Overlay_Text.value = layer.Text;
        Overlay_Position_Index.value = layer.Position;
        Overlay_Position_Friendly.value = (Overlay_Position_Friendly_Values)layer.Position;
    }

    private void AddOverlayButton_OnClick() {
        OverlayLayer layer = new OverlayLayer(Overlay_ID.value, Overlay_Position_Index.value, (Sprite)Overlay_Icon.value);
        Preview_Icon_Custom.AddOverlayLayer(layer.ID, layer.Position, layer.Icon);
        OverlayLayers.Add(layer);
        foreach (var previewIcon in Preview_Icons)
        {
            previewIcon.AddOverlayLayer(layer.ID, layer.Position, layer.Icon);
        }
        OverlayLayerListView.Rebuild();
    }
    
    private void AddOverlayButton_Update_OnClick() {
        // Maybe if and when overlay updates are added this could be fun!
    }

    private void RemoveOverlayButton_OnClick() {
        Preview_Icon_Custom.RemoveOverlayLayer(Overlay_ID.value);
        OverlayLayers.RemoveAll(layer => layer.ID == Overlay_ID.value);
        foreach (var previewIcon in Preview_Icons)
        {
            previewIcon.RemoveOverlayLayer(Overlay_ID.value);
        }
        OverlayLayerListView.Rebuild();
    }

    // ----- Overlay list view -----

    private ListView OverlayLayerListView;
    private OverlayLayer SelectedLayer;

    private void OverlayListInit()
    {
        OverlayLayerListView = rootVisualElement.Q<ListView>("LayerListView");
        OverlayLayerListView.itemsSource = OverlayLayers;
        OverlayLayerListView.fixedItemHeight = 50;
        OverlayLayerListView.makeItem = () => m_ItemRowTemplate.CloneTree();
        OverlayLayerListView.bindItem = (element, index) => {
            element.Q<VisualElement>("Icon").style.backgroundImage = OverlayLayers[index].Icon != null ?
                OverlayLayers[index].Icon.texture
                :
                DefaultIcon.texture;
            element.Q<VisualElement>("Icon").style.width = 35;
            element.Q<VisualElement>("Icon").style.height = 35;
            element.Q<Label>("ID").text = OverlayLayers[index].ID;
            element.Q<Label>("Position").text = $"Position: {(Overlay_Position_Friendly_Values)OverlayLayers[index].Position} ({OverlayLayers[index].Position})";
        };
        OverlayLayerListView.style.flexGrow = 1.0f;
        OverlayLayerListView.selectionType = SelectionType.Single;
        OverlayLayerListView.onSelectionChange += OverlayLayerList_OnSelectionChange;
    }

    private void OverlayLayerList_OnSelectionChange(IEnumerable<object> selectedItems)
    {
        var selectedItem = (OverlayLayer)selectedItems.First();
        SelectedLayer = selectedItem;
        OverlayEditUpdateData(selectedItem);
    }


// FIXME: clean this up
private VisualElement m_ItemsTab;
private static VisualTreeAsset m_ItemRowTemplate;
private ScrollView m_DetailSection;
}

public class OverlayLayer : ScriptableObject
{
    public string ID;
    // Position for the overlay
    public int Position = 0;
    // Display types
    public Sprite Icon;
    public string Text;
    // Text specific parameters
    public Font TextFont;
    public Color TextColor;
            
    /// <summary>
    /// Creates a new OverlayLayer that displays an image.
    /// </summary>
    /// <param name="overlayPosition"></param>
    /// <param name="overlayImage"></param>
    public OverlayLayer(string overlayID, int overlayPosition, Sprite overlayImage)
    {
        ID = overlayID;
        Icon = overlayImage;
        Position = overlayPosition;
    }

    /*
    /// <summary>
    /// Creates a new OverlayLayer that displays a string.
    /// </summary>
    /// <param name="overlayPosition"></param>
    /// <param name="overlayText"></param>
    /// <param name="overlayTextFont"></param>
    /// <param name="overlayTextColor"></param>
    public OverlayLayer(int overlayPosition, string overlayText, Font overlayTextFont, Color overlayTextColor)
    {
        Text = overlayText;
        Position = overlayPosition;
        TextFont = new Font(overlayTextFont.OriginalFontName);
        TextColor = new SolidBrush(overlayTextColor);
    }*/
}