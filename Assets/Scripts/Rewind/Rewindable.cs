using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewindable : MonoBehaviour
{
    protected bool isRewinding;
    protected float recordTimeInSeconds = 10f;
    protected float maximumCount => Mathf.Round(recordTimeInSeconds / Time.fixedDeltaTime);

    protected abstract void Awake();

    public virtual void StartRewinding() => isRewinding = true;
    public virtual void StopRewinding() => isRewinding = false;

    protected virtual void FixedUpdate()
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
