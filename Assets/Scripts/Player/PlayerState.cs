using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private MouseLook cam;
    private PlayerMovement player;
    [HideInInspector] public bool isRewinding;
    [HideInInspector] public bool isDead;

    private void Awake()
    {
        cam = GetComponent<MouseLook>();
        player = GetComponent<PlayerMovement>();
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
        if (other.CompareTag(Tags.DeathZone) && !isRewinding)
        {
            isDead = true;
        }
    }

    public void FreezePlayer()
    {
        cam.Freeze();
        player.Freeze();
    }

    public void UnfreezePlayer()
    {
        cam.Unfreeze();
        player.Unfreeze();
    }
}
