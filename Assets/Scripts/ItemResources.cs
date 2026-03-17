using UnityEngine;

public static class ItemResources
{
    public static bool TryGetItemDefinition(int itemID, out Item itemDefinition)
    {
        itemDefinition = null;

        GameObject itemPrefab = Resources.Load<GameObject>($"Items/{itemID}");
        if (itemPrefab == null)
        {
            return false;
        }

        itemDefinition = itemPrefab.GetComponent<Item>();
        return itemDefinition != null;
    }
}
