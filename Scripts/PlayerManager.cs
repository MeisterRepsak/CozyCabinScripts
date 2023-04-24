using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum playerStates
{
    normal,
    fishing
};
public class PlayerManager : MonoBehaviour
{
    public playerStates playerStates;
    InputManager input;
    PlayerMovement pm;
    PlayerAnimationManager pam;
    // Start is called before the first frame update
    void Start()
    {
        playerStates = playerStates.normal;
        input = GetComponent<InputManager>();
        pm = GetComponent<PlayerMovement>();
        pam = GetComponent<PlayerAnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStates == playerStates.fishing)
        {
            pm.SwitchBody(pm.fishBody);
            pam.SetNewAnim(pam.fishAnim);
        }
        else if(playerStates == playerStates.normal)
        {
            pm.SwitchBody(pm.normalBody);
            pam.SetNewAnim(pam.playerAnim);
        }
    }
}
