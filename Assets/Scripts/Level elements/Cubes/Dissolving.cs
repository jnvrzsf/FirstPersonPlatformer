using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolving : MonoBehaviour
{
    [SerializeField] private Material dissolveMat;
    private Renderer rend;
    private Pickupable cube;
    private Rigidbody rb;
    private Collider col;
    private bool isDissolving;
    private float dissolveSpeed = 1f;
    public event Action OnDestroyed;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        cube = GetComponent<Pickupable>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone") || other.CompareTag("DestructiveField"))
        {
            Dissolve();
            OnDestroyed?.Invoke();
        }
    }

    public void Dissolve()
    {
        isDissolving = true;
        cube.SetToUntouchable();
        rb.useGravity = false;
        Destroy(col);
        rend.material = dissolveMat;
    }

    private void Update()
    {
        if (isDissolving)
        {
            float dissolvePercentage = rend.material.GetFloat("_DissolvePercentage");
            if (dissolvePercentage < 1f)
            {
                rend.material.SetFloat("_DissolvePercentage", Mathf.MoveTowards(dissolvePercentage, 1f, dissolveSpeed * Time.deltaTime));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
