using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    Player player;
    GameObject playerOBJ;
    // Start is called before the first frame update
    void Start()
    {
        playerOBJ = GameObject.FindGameObjectWithTag("Player");
        player = playerOBJ.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == playerOBJ.tag)
            player.IsInWater(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == playerOBJ.tag)
            player.IsInWater(false);
    }
}
