using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private MouseLook look;
    private PlayerMovement characterController;
    public bool isRewinding;
    public bool isDead;

    private void Awake()
    {
        look = GetComponent<MouseLook>();
        characterController = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (isRewinding)
        {
            isDead = false;
            FreezePlayer();
        }
        else
        {
            UnfreezePlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DeathZone") && !isRewinding)
        {
            isDead = true;
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
