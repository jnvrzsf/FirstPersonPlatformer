﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableTransform : Rewindable
{
    private LinkedList<Record> records;

    protected override void Awake()
    {
        records = new LinkedList<Record>();
    }

    protected override void Record()
    {
        records.AddFirst(new Record(transform.position, transform.rotation));
        if (records.Count > maximumCount)
        {
            records.RemoveLast();
        }
    }

    protected override void Rewind()
    {
        if (records.Count > 0)
        {
            Record record = records.First.Value;
            transform.position = record.Position;
            transform.rotation = record.Rotation;
            records.RemoveFirst();
        }
    }
}
