using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public static PlayerAnimationManager instance;
    public Animator playerAnim;
    public Animator fishAnim;
    public Animator currentAnimator;
    InputManager input;
    PlayerMovement pm;
    void Start()
    {
        instance = this;
        pm = GetComponent<PlayerMovement>();
        //default
        playerAnim = pm.body.GetComponent<Animator>();
        currentAnimator = playerAnim;

        input = GetComponent<InputManager>();
    }

    void Update()
    {
        HandleAnimation();  
    }
    void HandleAnimation()
    {
        if(currentAnimator == playerAnim)
        {
            playerAnim.SetBool("isMoving", input.isMoving);

        }
        else if(currentAnimator == fishAnim)
        {
            fishAnim.SetBool("isMoving", input.isMoving);
            fishAnim.SetBool("isCharging", input.doChargeFishing);
        }
    }

    public void SetNewAnim(Animator anim)
    {
        if (currentAnimator == anim)
            return;

        currentAnimator = anim;
    }

   
}
