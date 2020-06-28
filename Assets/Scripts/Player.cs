using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //! Movement:
    [SerializeField] private CustomKeys inputKeys;

    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private bool isJumping;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private KeyCode jumpKey;
    float jumpForceMultiplier;

    private bool isInWater;
    private float waterSlowForce;

    private CharacterController charController;

    float walk_speed;
    float sprnt_speed;
    bool canSprnt;

    //! Break_Ability
    int BS_Tree, BS_Rock;

    //! Interaction:
    [SerializeField]
    private Camera p_camera;
    [SerializeField]
    private Image completionRing;
    [SerializeField]
    private LayerMask layermask;
    private Item itemBeingPickedUp;

    [SerializeField]
    private List<GameObject> actionImages; //! SET IN INSPECTOR

    //! Inventory:
    private Inventory inventory;
    [SerializeField]
    private UI_Inventory UIInventory;
    
    private void Awake()
    {
        charController = GetComponent<CharacterController>();

        inventory = new Inventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        walk_speed = 6.0f;
        sprnt_speed = 10.0f;
        canSprnt = true;
        jumpForceMultiplier = 5.0f;

        isInWater = false;
        waterSlowForce = 2.0f;
        BS_Tree = BS_Rock = 1; //! 1 is default as that's fist strength

        completionRing.gameObject.SetActive(false);

        foreach(GameObject go in actionImages)
        {//! Set all action prompts invisble
            go.SetActive(false);
        }

        UIInventory.SetInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        //! Movement:
        PlayerMovement();

        //! Interacting:
        SelectItemFromRay();

        if(HasItemTargeted())
        {
            completionRing.gameObject.SetActive(true);
            actionImages[0].SetActive(true);
        }
        else
        {
            completionRing.gameObject.SetActive(false);
            actionImages[0].SetActive(false);
            ResetProgressImage();
        }
    }

    //! INTERACTION:
    private void SelectItemFromRay()
    {
        Ray ray = p_camera.ViewportPointToRay(Vector3.one / 2.0f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 2.0f, layermask))
        {
            var hitItem = hitInfo.collider.GetComponent<Item>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
                actionImages[0].SetActive(true);
            }
        }
        else
        {
            itemBeingPickedUp = null;
        }
    }

    private bool HasItemTargeted()
    {
        return itemBeingPickedUp != null;
    }

    public void UpdateProgressImage(float currentTimerElapsed, float totalTime)
    {
        float pct = currentTimerElapsed / totalTime;

        completionRing.fillAmount = pct;
    }

    public void ResetProgressImage()
    {
        completionRing.fillAmount = 0.0f;
    }

    //! MOVEMENT:
    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        if (isInWater)
        {
            horizInput /= waterSlowForce;
            vertInput /= waterSlowForce;
        }

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        if(Input.GetKey(inputKeys.FindInput("sprintKey")) && canSprnt)
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * sprnt_speed);
        }
        else
        {
            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * walk_speed);
        }

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        JumpInput();
    }

    private bool OnSlope()
    {
        if(isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;

        return false;
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(inputKeys.FindInput("jumpKey")) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpingForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpingForce * jumpForceMultiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;

            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }

    public void IsInWater(bool water)
    {
        switch(water)
        {
            case true:
                canSprnt = false;
                break;

            case false:
                canSprnt = true;
                break;
        }

        isInWater = water;
    }


    public Inventory GetInventory()
    {
        return inventory;
    }
    //! Resource getters:
    public int GetTreeBS()
    {
        return BS_Tree;
    }
    public int GetRockBS()
    {
        return BS_Rock;
    }
}
