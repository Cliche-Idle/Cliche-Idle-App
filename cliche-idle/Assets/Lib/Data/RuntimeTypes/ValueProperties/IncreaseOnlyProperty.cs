using UnityEngine;

[System.Serializable]
public class IncreaseOnlyProperty
{
    [field: SerializeField]
    public int Value { get; private set; }

    public void Grant (int amount)
    {
        Value += Mathf.Abs(amount);
    }
}