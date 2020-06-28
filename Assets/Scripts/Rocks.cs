using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : Item
{
    Player player;
    int playerBS;
    bool mouseIsDown;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mouseIsDown = false;
        playerBS = player.GetRockBS();

        pickupTime = 5.0f; //! Base item time;
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
                player.GetInventory().AddItem(new Item { itemType = Item.ItemType.Rock, amount = 2 });
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
