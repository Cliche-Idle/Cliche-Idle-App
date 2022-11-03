using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Cliche.System;

public class AdventureBoard : UIScript
{
    public VisualTreeAsset AdventureItemUXML;
    private AdventureHandler Adventures;

    private VisualElement AdventureQueueContainer;
    private VisualElement AdventureAvailableContainer;

    void Start()
    {
        DisplayView();
    }

    protected override void UIUpdate()
    {
        if (Adventures.AdventureQueue.Count != 0)
        {
            foreach (var item in Adventures.AdventureQueue)
            {
                var queueItemUIElement = AdventureQueueContainer.Q(item.ID);
                // Check if the adventure is marked as completed in the UI
                bool isMarkedForFinish = queueItemUIElement.ClassListContains("AdventureComplete");
                if (item.Finished == false)
                {
                    // Update UI with the remaining time
                    var secondsDuration = item.EndTime - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    TimeSpan duration = TimeSpan.FromSeconds(secondsDuration);
                    string durationText = TimeSpan.FromSeconds(Math.Floor(secondsDuration)).ToString();
                    queueItemUIElement.Q<Label>("RequestTimer").text = durationText;
                }
                else if (isMarkedForFinish == false)
                {
                    // If the adventure is completed but not marked in the UI, update it
                    queueItemUIElement.Q<Label>("RequestTimer").text = TimeSpan.FromSeconds(0).ToString();
                    queueItemUIElement.AddToClassList("AdventureComplete");
                    // Add event for finishing the adventure
                    queueItemUIElement.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                        evt.PreventDefault();
                        evt.StopImmediatePropagation();
                        var adventure = Adventures.AdventureQueue.Find(element => element.ID == item.ID);
                        if (adventure.Finished)
                        {
                            Adventures.FinishAdventure(adventure.ID);
                        }
                    });
                }
            }
        }
    }

    protected override void OnEnterFocus(object sender, EventArgs e)
    {
        Adventures = GameObject.Find("Player").GetComponent<AdventureHandler>();
        // Get queue container
        AdventureQueueContainer = GetViewContainer().Q("RequestQueueContainer");
        // Get available container
        AdventureAvailableContainer = GetViewContainer().Q("RequestAvailableContainer");
        Adventures.OnAdventuresUpdate += RenderAdventureBoard;
        RenderAdventureBoard(null, null);
    }

    protected override void OnLeaveFocus(object sender, EventArgs e)
    {
        // Unsubscribe from the update event
        Adventures.OnAdventuresUpdate -= RenderAdventureBoard;
    }

    private void RenderAdventureBoard(object sender, EventArgs e)
    {
        // Render queue items first
        AdventureQueueContainer.Clear();
        foreach (var item in Adventures.AdventureQueue)
        {
            AdventureItemUXML.CloneTree(AdventureQueueContainer);
            VisualElement adventureRequest = AdventureQueueContainer.Query("RequestContainer").Build().Last();
            adventureRequest.name = item.ID;
            var adventure = Manifests.GetByID<AdventureManifest>(item.ID);
            adventureRequest.Q<Label>("RequestName").text = adventure.Title;
            adventureRequest.Q<Label>("RequestDesc").text = adventure.Description;
            var secondsDuration = item.EndTime - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            TimeSpan duration = TimeSpan.FromSeconds(Math.Floor(secondsDuration));
            adventureRequest.Q<Label>("RequestTimer").text = duration.ToString();
        }
        // Check if there is an active queue item
        bool adventureQueueActive = (Adventures.AdventureQueue.Count > 0);
        // Render available items last
        AdventureAvailableContainer.Clear();
        foreach (var item in Adventures.AvailableAdventures)
        {
            AdventureItemUXML.CloneTree(AdventureAvailableContainer);
            VisualElement adventureRequest = AdventureAvailableContainer.Query("RequestContainer").Build().Last();
            adventureRequest.name = item;
            var adventure = Manifests.GetByID<AdventureManifest>(item);
            adventureRequest.Q<Label>("RequestName").text = adventure.Title;
            adventureRequest.Q<Label>("RequestDesc").text = adventure.Description;
            TimeSpan duration = TimeSpan.FromSeconds(adventure.BaseLength);
            adventureRequest.Q<Label>("RequestTimer").text = duration.ToString();
            if (adventureQueueActive)
            {
                // Disable available request cards if we have an active queue item
                adventureRequest.style.unityBackgroundImageTintColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                // If there is no active queue item, allow adventures to be started
                // * No need to deregister this event, it will be destroyed when the board is re-drawn
                adventureRequest.RegisterCallback<ClickEvent>((ClickEvent evt) => {
                    //var item = (VisualElement)evt.target;
                    evt.PreventDefault();
                    evt.StopImmediatePropagation();
                    Adventures.StartAdventure(item);
                });
            }
        }
    }
}

[CustomEditor(typeof(AdventureBoard))]
public class AdventureBoardEditor : UIScriptEditor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        base.OnInspectorGUI();
    }
}
