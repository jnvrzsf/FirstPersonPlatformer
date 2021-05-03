using System.Collections;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    protected Rigidbody rb;
    private Collider col;
    private Renderer rend;
    protected Carrier carrier;
    [HideInInspector] public SpawnButton spawner;
    [SerializeField] private Material dissolveMat;
    public bool canBePickedUp { get; private set; } = true;
    protected bool isCarried => carrier != null;
    private float minSpeed = 0;
    private float maxSpeed = 10000;
    private AudioObject dissolveAudio;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlayAtPoint(AudioType.Collision, collision.GetContact(0).point);
        }
    }

    public void Destroy()
    {
        SetToUntouchable();
        rb.useGravity = false;
        Destroy(col);
        dissolveAudio = AudioManager.instance.PlayOnGameObject(AudioType.CubeDissolve, gameObject);
        rend.material = dissolveMat;
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        float seconds = dissolveAudio.source.clip.length;
        float percentage = 0f;
        while (percentage < 1f)
        {
            percentage += Time.deltaTime / seconds;
            rend.material.SetFloat("DissolvePercentage", percentage);
            yield return null;
        }
        Destroy(gameObject);
    }
}
