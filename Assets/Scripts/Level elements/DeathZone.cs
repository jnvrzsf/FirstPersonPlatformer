using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private RewindOnDeath rewindScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rewindScript = other.GetComponent<RewindOnDeath>();
            rewindScript?.SetDead();
        }
    }
}
