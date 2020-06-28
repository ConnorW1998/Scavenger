using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomKeys : MonoBehaviour
{
    [SerializeField]
    private KeyCode interactKey;
    [SerializeField]
    private KeyCode menuKey;
    [SerializeField]
    private KeyCode sprintKey;
    [SerializeField]
    private KeyCode jumpKey;
    [SerializeField]
    private KeyCode inventoryKey;

    public KeyCode FindInput(string key)
    {
        switch(key)
        {
            default: return KeyCode.None;

            case "interactKey": return interactKey;
            case "menuKey": return menuKey;
            case "sprintKey": return sprintKey;
            case "jumpKey": return jumpKey;
            case "inventoryKey": return inventoryKey;
        }        
    }
}
