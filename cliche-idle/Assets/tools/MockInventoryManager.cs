using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MockInventoryManager : MonoBehaviour
{
    public UIDocument docu;
    public VisualElement m_Root;
    public ItemData item;
    public int iconCount = 0;
    public bool switchSize = false;

    // Start is called before the first frame update
    void Start()
    {
        docu = GetComponent<UIDocument>();
        m_Root = docu.rootVisualElement.Q<VisualElement>("Container");

        for (int i = 0; i < iconCount; i++)
        {
            OverlayIcon testIcon = new OverlayIcon("testItem", Resources.Load<Sprite>("icons/placeholder_250x250_cross"), 250, 250);

            testIcon.AddOverlayLayer("testLayerTransparent-Center", 0, Resources.Load<Sprite>("icons/placeholder_overlay_75_opacity_50x50"));
            //testIcon.AddOverlayLayer("testLayerTransparent-TopLeft", 1, Resources.Load<Sprite>("icons/placeholder_overlay_75_opacity_50x50"));
            //testIcon.AddOverlayLayer("testLayerOpaque-TopRight", 2, Resources.Load<Sprite>("icons/placeholder_overlay_50x50"));
            //testIcon.AddOverlayLayer("testLayerOpaque-BottomLeft", 3, Resources.Load<Sprite>("icons/placeholder_overlay_50x50"));
            //testIcon.AddOverlayLayer("testLayerTransparent-BottomRight", 4, Resources.Load<Sprite>("icons/placeholder_overlay_75_opacity_50x50"));

            //testIcon.GetOverlayLayer<Label>("testLayerTransparent-BottomRight");
            m_Root.Add(testIcon);
        }
    }

    void Update() {
        if (switchSize == true)
        {
            switchSize = false;
            m_Root.Q<OverlayIcon>(item.InternalVariantID).style.width = 200;
        }
    }
}
