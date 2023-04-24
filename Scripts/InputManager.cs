using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Vector3 moveInput;
    public Vector2 pullInput;

    public float xAxis, zAxis;
    public bool isMoving;
    public bool isInterrupted;

    public bool doOpenInventory;
    public bool doPickUpItem;
    public bool doGoInCabin;
    public bool doChargeFishing;
    public bool doPullOutFish;
    public bool doPullRod;
    public int scrollHotbar;
    public float turnDir;
    FishingRod fish;
    PlayerManager pManager;

    public static InputManager instance;
    private void Awake()
    {
        instance = this;
        pManager = GetComponent<PlayerManager>();
        fish = GetComponent<FishingRod>();
    }
    void Update()
    {
       
        HandleInput();
    }

    private void LateUpdate()
    {
        HandleGetKeyDowns();
    }

    void HandleGetKeyDowns()
    {
        if (doOpenInventory)
            doOpenInventory = false;
        if(doPickUpItem)
            doPickUpItem = false;
        if (doPullOutFish)
            doPullOutFish = false;
        if (doPullRod)
            doPullRod = false;
    }

    void HandleInput()
    {
        xAxis = moveInput.x;
        zAxis = moveInput.z;
        isMoving = moveInput != Vector3.zero;
    }

    public void OnMove(InputValue value)
    {
        if (isInterrupted)
            return;
        if (fish.isFishing || fish.isThrowing || doChargeFishing)
        {
            moveInput = Vector3.zero;
            return;
        }

        moveInput = value.Get<Vector3>();

    }

    public void OnOpenInventory(InputValue value) 
    {
        if (isInterrupted)
            return;
        doOpenInventory = true; 
    }

    public void OnPickUpItem(InputValue value) 
    {
        
        if (isInterrupted)
            return;
        doPickUpItem = true;
    }
    public void OnGoInCabin(InputValue value)
    {
        if (isInterrupted)
            return;
        doGoInCabin = true;
    }

    public void OnChargeFishing(InputValue value)
    {
        if (isInterrupted)
            return;
        if (pManager.playerStates != playerStates.fishing)
            return;
        moveInput = Vector3.zero;

        if (value.Get<float>() == 1)
        {
            doChargeFishing = true;
        }
        else
        {
            doChargeFishing = false;
        }
    }

    public void OnPullFish(InputValue value)
    {
        if (isInterrupted)
            return;
        if (pManager.playerStates != playerStates.fishing)
            return;
        if (!fish.isFishing)
            return;

        doPullOutFish = true;

    }

    public void OnPullOnFish(InputValue value)
    {
        if (pManager.playerStates != playerStates.fishing)
            return;
        if (!fish.isFishing)
            return;
        pullInput = value.Get<Vector2>();
    }
    public void OnScrollHotbar(InputValue value)
    {
        if (isInterrupted)
            return;
        if (value.Get<Vector2>().y > 0)
        {

            scrollHotbar--;
            if (scrollHotbar < 0)
                scrollHotbar = 7;
        }
        else if (value.Get<Vector2>().y < 0)
        {
            scrollHotbar++;
            if (scrollHotbar > 7)
                scrollHotbar = 0;
        }
    }
    public void OnTurnSalmon(InputValue value) => turnDir = value.Get<Vector2>().x;
}
