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

    private readonly string _completeAdventureClassName = "__complete";
    private readonly string _completeAdventureUIAnimationClassName = "__completeAnim";

    protected override void UIUpdate()
    {
        if (Adventures.AdventureQueue.Count != 0)
        {
            foreach (var item in Adventures.AdventureQueue)
            {
                var queueItemUIElement = GetBoundRequestSlot(item.ID)[0];
                if (queueItemUIElement != null)
                {
                    // Check if the adventure is marked as completed in the UI
                    bool isMarkedForFinish = queueItemUIElement.ClassListContains(_completeAdventureClassName);
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
                        queueItemUIElement.Q<Label>("RequestTimer").text = "Complete!";
                        queueItemUIElement.AddToClassList(_completeAdventureClassName);
                        StartCoroutine(AdventureCompleteUIAnimation(queueItemUIElement));
                    }
                }
            }
        }
    }

    IEnumerator AdventureCompleteUIAnimation(VisualElement slot)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            slot.ToggleInClassList(_completeAdventureUIAnimationClassName);
            yield return new WaitForSeconds(1f);
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
        foreach (var adventureQueueItem in Adventures.AdventureQueue)
        {
            // Check if request is already rendered
            var slot = GetBoundRequestSlot(adventureQueueItem.ID);
            if (slot == null)
            {
                // If not, get empty slot to fill
                slot = GetEmptyRequestSlot();
                if (slot != null)
                {
                    SpawnRequestVisualElement(slot, adventureQueueItem.ID, OpenAdventureCompletePopup);
                }
            }
        }

        foreach (var availableAdventureID in Adventures.AvailableAdventures)
        {
            // Check if request is already rendered
            var slot = GetBoundRequestSlot(availableAdventureID);
            if (slot == null)
            {
                // If not, get empty slot to fill
                slot = GetEmptyRequestSlot();
                if (slot != null)
                {
                    SpawnRequestVisualElement(slot, availableAdventureID, OpenAdventureStartPopup);
                }
            }
        }

        UpdateSlotStates();
    }

    private VisualElement GetEmptyRequestSlot()
    {
        return RequestSlots.Find(slot => slot.childCount == 0);
    }

    private VisualElement GetBoundRequestSlot(string id)
    {
        return RequestSlots.Find(slot => slot.childCount != 0 && slot[0].name == id);
    }

    private VisualElement SpawnRequestVisualElement(VisualElement slot, string id, EventCallback<ClickEvent> onClickFunc)
    {
        var adventure = Manifests.GetByID<AdventureManifest>(id);
        AdventureItemUXML.CloneTree(slot);        
        var slotCard = slot[0];
        slotCard.name = id;
        string durationText = TimeSpan.FromSeconds(Math.Floor(adventure.BaseLength)).ToString();
        slotCard.Q<Label>("RequestTimer").text = durationText;
        // TODO: Adventure titles should be displayed, so promote this poor label from debug to a big boy
        slotCard.Q<Label>("DebugTitle").text = adventure.Title;
        slotCard.RegisterCallback<ClickEvent>(onClickFunc);
        return slotCard;
    }

    // TODO: fix this
    private void UpdateSlotStates()
    {
        // Check if there is an active queue item
        bool adventureQueueActive = (Adventures.AdventureQueue.Count > 0);
        foreach (var requestSlot in RequestSlots)
        {
            if (adventureQueueActive)
            {
                if (requestSlot.childCount != 0 && Adventures.AdventureQueue.Find(q => q.ID == requestSlot[0].name) != null)
                {
                    // Enable queued adventures
                    requestSlot.SetEnabled(true);
                    requestSlot.style.unityBackgroundImageTintColor = Color.white;
                }
                else
                {
                    // Disable available adventures, queue in progress
                    requestSlot.SetEnabled(false);
                    requestSlot.style.unityBackgroundImageTintColor = Color.grey;
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
        if (requestSlot.ClassListContains(_completeAdventureClassName))
        {
            // Stop animation
            StopAllCoroutines();
            FinishPopup.AdventureSlot = requestSlot;
            FinishPopup.ShowView();
        }
    }
}