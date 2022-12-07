using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Cliche.System;
using UIViews;

public class AdventureBoard : UIScript
{
    public VisualTreeAsset AdventureItemUXML;
    public AdventureHandler Adventures;

    public AdventureDetailsPopup DetailsPopup;
    public AdventureFinishPopup FinishPopup;

    private List<VisualElement> RequestSlots;

    void Start()
    {
        ShowView();
    }

    protected override void UIUpdate()
    {
        if (Adventures.AdventureQueue.Count != 0)
        {
            foreach (var item in Adventures.AdventureQueue)
            {
                var queueItemUIElement = RequestSlots.Find(element => element.ClassListContains("ActiveAdventure") == true);
                if (queueItemUIElement != null)
                {
                    // Check if the adventure is marked as completed in the UI
                    bool isMarkedForFinish = queueItemUIElement.ClassListContains("CompletedAdventure");
                    if (item.Finished == false)
                    {
                        // Update UI with the remaining time
                        var secondsDuration = item.EndTime - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        //TimeSpan duration = TimeSpan.FromSeconds(secondsDuration);
                        string durationText = TimeSpan.FromSeconds(Math.Floor(secondsDuration)).ToString();
                        queueItemUIElement.Q<Label>("RequestTimer").text = durationText;
                    }
                    else if (isMarkedForFinish == false)
                    {
                        // If the adventure is completed but not marked in the UI, update it
                        queueItemUIElement.Q<Label>("RequestTimer").text = TimeSpan.FromSeconds(0).ToString();
                        queueItemUIElement.AddToClassList("CompletedAdventure");
                    }
                }
            }
        }
    }

    protected override void OnEnterFocus()
    {
        GetViewContainer().style.height = Length.Percent(100);
        RequestSlots = GetViewContainer().Query(className: "requestSlot").ToList();
        Adventures.OnAdventuresUpdate += PopulateAdventureBoard;
        PopulateAdventureBoard(null, null);
    }

    protected override void OnLeaveFocus()
    {
        Adventures.OnAdventuresUpdate -= PopulateAdventureBoard;
    }

    private void PopulateAdventureBoard(object sender, EventArgs e)
    {
        foreach (var availableAdventureID in Adventures.AvailableAdventures)
        {
            foreach (var requestSlot in RequestSlots)
            {
                if (requestSlot.name == "request")
                {
                    // Empty, fill the slot
                    requestSlot.name = availableAdventureID;
                    var adventure = Manifests.GetByID<AdventureManifest>(availableAdventureID);
                    AdventureItemUXML.CloneTree(requestSlot);
                    //
                    string durationText = TimeSpan.FromSeconds(Math.Floor(adventure.BaseLength)).ToString();
                    requestSlot.Q<Label>("RequestTimer").text = durationText;
                    //
                    requestSlot.Q<Label>("DebugTitle").text = adventure.Title;
                    //
                    // Start adventure on tile click
                    // TODO: open adventure details dialogue instead
                    requestSlot.UnregisterCallback<ClickEvent>(OpenAdventureCompletePopup);
                    requestSlot.RegisterCallback<ClickEvent>(OpenAdventureStartPopup);
                    break;
                }
            }
        }

        foreach (var adventureQueueItem in Adventures.AdventureQueue)
        {
            foreach (var requestSlot in RequestSlots)
            {
                if (requestSlot.name == adventureQueueItem.ID && requestSlot.ClassListContains("ActiveAdventure"))
                {
                    // Finish adventure on tile click
                    // TODO: open adventure details dialogue instead
                    requestSlot.UnregisterCallback<ClickEvent>(OpenAdventureStartPopup);
                    requestSlot.RegisterCallback<ClickEvent>(OpenAdventureCompletePopup);
                    break;
                }
                else if (requestSlot.name == "request")
                {
                    // Empty, fill the slot
                    requestSlot.name = adventureQueueItem.ID;
                    var adventure = Manifests.GetByID<AdventureManifest>(adventureQueueItem.ID);
                    AdventureItemUXML.CloneTree(requestSlot);
                    requestSlot.AddToClassList("ActiveAdventure");
                    //
                    string durationText = TimeSpan.FromSeconds(Math.Floor(adventure.BaseLength)).ToString();
                    requestSlot.Q<Label>("RequestTimer").text = durationText;
                    //
                    requestSlot.Q<Label>("DebugTitle").text = adventure.Title;
                    //
                    // Start adventure on tile click
                    // TODO: open adventure details dialogue instead
                    requestSlot.UnregisterCallback<ClickEvent>(OpenAdventureStartPopup);
                    requestSlot.RegisterCallback<ClickEvent>(OpenAdventureCompletePopup);
                    break;
                }
            }
        }
        UpdateRequestsState();
    }

    private void UpdateRequestsState()
    {
        // Check if there is an active queue item
        bool adventureQueueActive = (Adventures.AdventureQueue.Count > 0);
        foreach (var requestSlot in RequestSlots)
        {
            if (adventureQueueActive)
            {
                if (requestSlot.ClassListContains("ActiveAdventure") == false)
                {
                    // Disable available adventures, queue in progress
                    requestSlot.SetEnabled(false);
                    requestSlot.style.unityBackgroundImageTintColor = Color.grey;
                }
                else
                {
                    // Enable queued adventures
                    requestSlot.SetEnabled(true);
                    requestSlot.style.unityBackgroundImageTintColor = Color.white;
                }
            }
            else
            {
                // Enable all tiles
                requestSlot.SetEnabled(true);
                requestSlot.style.unityBackgroundImageTintColor = Color.white;
            }
        }
    }

    private void OpenAdventureStartPopup(ClickEvent evt)
    {
        evt.PreventDefault();
        evt.StopImmediatePropagation();
        var requestSlot = (VisualElement)evt.currentTarget;
        DetailsPopup.AdventureSlot = requestSlot;
        DetailsPopup.ShowView();
    }

    private void OpenAdventureCompletePopup(ClickEvent evt)
    {
        evt.PreventDefault();
        evt.StopImmediatePropagation();
        var requestSlot = (VisualElement)evt.currentTarget;
        if (requestSlot.ClassListContains("CompletedAdventure"))
        {
            FinishPopup.AdventureSlot = requestSlot;
            FinishPopup.ShowView();
        }
    }
}