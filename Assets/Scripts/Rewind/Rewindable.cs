using System;
using UnityEngine;

public abstract class Rewindable : MonoBehaviour
{
    protected bool isRewinding;
    protected float recordTimeInSeconds = 15f;
    protected int maximumCount => Mathf.RoundToInt(recordTimeInSeconds / Time.fixedDeltaTime);
    public abstract int currentRecordCount { get; }

    public event Action OutOfRecords;
    protected void OnOutOfRecords() => OutOfRecords?.Invoke();

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
