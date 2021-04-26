using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    protected Rigidbody rb;
    private Collider col;
    private Renderer rend;
    protected Carrier carrier;
    [HideInInspector] public SpawnButton spawner;
    public bool canBePickedUp { get; private set; } = true;
    protected bool isCarried => carrier != null;
    private float minSpeed = 0;
    private float maxSpeed = 10000;

    [SerializeField] private Material dissolveMat;
    private bool isDissolving;
    private const float dissolveSpeed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    public void FollowCarrier(float maxDistance)
    {
        Vector3 direction = (carrier.carryPoint.position - transform.position).normalized;
        float distance = Vector3.Distance(carrier.carryPoint.position, transform.position);
        float speed = Mathf.SmoothStep(minSpeed, maxSpeed, distance / maxDistance) * Time.fixedDeltaTime;
        rb.velocity = direction * speed;
    }
    public abstract void SetToPickedUp(Carrier c);
    public abstract void SetToDropped();
    public virtual void SetToUntouchable() {
        carrier?.Drop();
        canBePickedUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone") || other.CompareTag("DestructiveField"))
        {
            spawner?.SpawnNewCube();
            Destroy();
        }
    }

    public void Destroy()
    {
        isDissolving = true;
        SetToUntouchable();
        rb.useGravity = false;
        Destroy(col);
        rend.material = dissolveMat;
        AudioManager.instance.Play(AudioType.CubeDissolve, transform.position); // TODO: play on audiosource attached to gameobject
    }

    private void Update() // TODO: coroutine instead
    {
        if (isDissolving)
        {
            Dissolve();
        }
    }

    private void Dissolve()
    {
        float dissolvePercentage = rend.material.GetFloat("DissolvePercentage");
        if (dissolvePercentage < 1f)
        {
            rend.material.SetFloat("DissolvePercentage", Mathf.MoveTowards(dissolvePercentage, 1f, dissolveSpeed * Time.deltaTime));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
