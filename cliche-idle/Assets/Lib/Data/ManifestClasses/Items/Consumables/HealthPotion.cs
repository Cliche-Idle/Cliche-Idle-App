using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Items/Healing potion")]
public class HealthPotion : ConsumableManifest
{
    public override void Use()
    {
        var player = GameObject.Find("Player");
        var statsHandler = player.GetComponent<StatsHandler>();
        int maxHealth = statsHandler.Health.Max;
        int healAmount = Mathf.FloorToInt(maxHealth * (MainStatValue / 100f));
        statsHandler.Health.Grant(healAmount);
    }

    protected override void OnValidate() {
        ItemType = ItemTypes.Consumable;
        IsInstanceItem = false;
    }
}