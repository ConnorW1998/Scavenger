using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected float pickupTime;

    public enum ItemType
    {
        Log,
        Stick,
        Rock,
        Grass
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default: return null;

            case ItemType.Log: return ItemAssets.Instance.LogSprite;
            case ItemType.Stick: return ItemAssets.Instance.StickSprite;
            case ItemType.Rock: return ItemAssets.Instance.RockSprite;
            case ItemType.Grass: return ItemAssets.Instance.GrassSprite;
        }
    }

    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.Grass:
            case ItemType.Stick:
            case ItemType.Rock:
                return true;
            case ItemType.Log:
                return false;
        }
    }

    public static void DropItem(Vector3 dropPosition, Item item)
    {
        dropPosition.x += 1.0f;
        Quaternion spawnRotate = new Quaternion();
        spawnRotate.SetLookRotation(Vector3.up);
        Instantiate(ItemAssets.Instance.findObject(item.itemType), dropPosition, spawnRotate);
    }

    public ItemType GetItemType()
    {
        return this.itemType;
    }
}
