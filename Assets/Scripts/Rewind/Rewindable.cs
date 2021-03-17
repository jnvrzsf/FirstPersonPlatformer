using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewindable : MonoBehaviour
{
    protected bool isRewinding;
    protected float recordTimeInSeconds = 5f;

    protected abstract void Start();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewinding();
        }
        if (Input.GetKeyUp(KeyCode.Return))
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
