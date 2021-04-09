using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewindable : MonoBehaviour
{
    private InputManager input;
    protected bool isRewinding;
    protected float recordTimeInSeconds = 5f;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
    }
    protected abstract void Start();

    private void Update()
    {
        if (input.PressedRewind)
        {
            StartRewinding();
        }
        if (input.ReleasedRewind)
        {
            StopRewinding();
        }
    }

    protected virtual void StartRewinding() => isRewinding = true;
    protected virtual void StopRewinding() => isRewinding = false;

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    protected abstract void Record();
    protected abstract void Rewind();
}
