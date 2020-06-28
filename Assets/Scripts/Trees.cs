using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : Item
{
    Player player;
    int playerBS;
    bool mouseIsDown;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mouseIsDown = false;
        playerBS = player.GetTreeBS();
        
        pickupTime = 4.0f; //! Base item time;
    }

    private IEnumerator Harvest()
    {
        float currentPickupTimerElapsed = 0.0f;

        while (mouseIsDown)
        {
            if (FindDistance() > 3.0f)
            {
                player.ResetProgressImage();
                currentPickupTimerElapsed = 0.0f;
                yield return null;
            }

            if (currentPickupTimerElapsed < CalcBreakTime())
            {
                currentPickupTimerElapsed += Time.deltaTime;
                player.UpdateProgressImage(currentPickupTimerElapsed, CalcBreakTime());
            }
            else if (currentPickupTimerElapsed >= CalcBreakTime())
            {
                for (int i = 0; i < 3; i++)
                {
                    player.GetInventory().AddItem(new Item { itemType = Item.ItemType.Log, amount = 1 });
                }
                player.GetInventory().AddItem(new Item { itemType = Item.ItemType.Stick, amount = 5 });
                Destroy(gameObject);
            }
            yield return null;
        }
        player.ResetProgressImage();
    }

    void OnMouseDown()
    {
        mouseIsDown = true;
        StartCoroutine(Harvest());
    }

    private void OnMouseUp()
    {
        mouseIsDown = false;
        StopCoroutine(Harvest());
    }
    public float CalcBreakTime()
    {
        return pickupTime / playerBS;
    }

    private float FindDistance()
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position);
    }

}
