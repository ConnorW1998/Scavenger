using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject findObject(Item.ItemType itemType)
    {
        switch (itemType)
        {
            default: return GameObject.CreatePrimitive(PrimitiveType.Capsule);

            case Item.ItemType.Log:
                return logObject;
            case Item.ItemType.Rock:
                return rockObject;
            case Item.ItemType.Grass:
                return grassObject;
            case Item.ItemType.Stick:
                return stickObject;
        }
    }

    public Sprite LogSprite;
    public Sprite StickSprite;
    public Sprite RockSprite;
    public Sprite GrassSprite;

    public GameObject logObject;
    public GameObject rockObject;
    public GameObject grassObject;
    public GameObject stickObject;
}
