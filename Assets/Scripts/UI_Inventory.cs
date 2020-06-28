using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private bool showInventory;

    [SerializeField]
    private CustomKeys inputKeys;

    [SerializeField]
    private GameObject InventoryObject;

    [SerializeField]
    private Transform player;

    private void Awake()
    {
        showInventory = false;
        itemSlotContainer = InventoryObject.transform.Find("itemContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        InventoryObject.SetActive(false);
        itemSlotTemplate.gameObject.SetActive(false);
    }

    private void Update()
    {
        ShowInventory();
    }

    private void ShowInventory()
    {
        if (Input.GetKeyDown(inputKeys.FindInput("inventoryKey")))
        {
            if (InventoryObject.activeInHierarchy)
                InventoryObject.SetActive(false);
            else if (!InventoryObject.activeInHierarchy)
                InventoryObject.SetActive(true);
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void ItemButtonPressed()
    {
        var tempSlotTemplate = itemSlotContainer.Find("itemSlotTemplate(Clone)");
        Image itemImage = tempSlotTemplate.Find("Item_Image").GetComponent<Image>();
        bool success = int.TryParse(itemSlotTemplate.Find("item_count").GetComponent<Text>().text, out int itemCount);
        
        Item item = new Item();
        Item duplicateItem = new Item();

        switch(itemImage.sprite.name)
        {
            default: break;

            case "ITEM_log": item = new Item { itemType = Item.ItemType.Log, amount = itemCount }; duplicateItem = item; break;
            case "ITEM_stick": item = new Item { itemType = Item.ItemType.Stick, amount = itemCount }; duplicateItem = item; break;
            case "ITEM_rock": item = new Item { itemType = Item.ItemType.Rock, amount = itemCount }; duplicateItem = item;  break;
            case "ITEM_grass": item = new Item { itemType = Item.ItemType.Grass, amount = itemCount }; duplicateItem = item; break;
        }

        inventory.RemoveItem(item);
        Item.DropItem(player.position, duplicateItem);
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {

        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 60.0f;

        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
            Image itemImage = itemSlotRectTransform.Find("Item_Image").GetComponent<Image>();
            itemImage.sprite = item.GetSprite();

            Text amountText = itemSlotRectTransform.Find("item_count").GetComponent<Text>();
            if(item.amount > 1)
            {
                amountText.text = item.amount.ToString();
            }
            else
            {
                amountText.text = "";
            }

            x++;
            if(x >= 4)
            {
                x = 0;
                if(y == 0)
                    y++;
                else
                {
                    //! Inventory FULL
                    Item.DropItem(player.transform.position, item);
                    break;
                }
            }
        }
    }
}
