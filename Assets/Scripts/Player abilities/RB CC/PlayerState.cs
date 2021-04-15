using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private MouseLook look;
    private FirstPersonRBCharacterController characterController;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        look = GetComponent<MouseLook>();
        characterController = GetComponent<FirstPersonRBCharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            IsDead = true;
            FreezePlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            IsDead = false;
            UnfreezePlayer(); // ? what if still in trigger
        }
    }

    public void FreezePlayer()
    {
        look.Freeze();
        characterController.Freeze();
    }

    public void UnfreezePlayer()
    {
        look.Unfreeze();
        characterController.Unfreeze();
    }
}
