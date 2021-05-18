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

    private const float minSpeed = 0;
    protected abstract float maxSpeed { get; }

    private void Awake()
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

    public virtual void SetToPickedUp(Carrier c)
    {
        carrier = c;
        gameObject.layer = Layers.PickedUp;
    }

    public virtual void SetToDropped()
    {
        carrier = null;
        gameObject.layer = Layers.Pickupable;
    }

    /// <summary>
    /// So it can't be picked up while being destroyed.
    /// </summary>
    public void SetToUntouchable() {
        carrier?.Drop();
        canBePickedUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.DeathZone) || other.CompareTag(Tags.DestructiveField))
        {
            spawner?.SpawnNewCube();
            Destroy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(Tags.Player))
        {
            AudioManager.instance.PlayAtPoint(AudioType.Collision, collision.GetContact(0).point);
        }
    }

    public void Destroy()
    {
        SetToUntouchable();
        rb.useGravity = false;
        Destroy(col);
        AudioManager.instance.PlayOnGameObject(AudioType.CubeDissolve, gameObject);
        rend.material = dissolveMat;
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        float seconds = AudioManager.instance.GetAudioLength(AudioType.CubeDissolve);
        float percentage = 0f;
        while (percentage < 1f)
        {
            percentage += Time.deltaTime / seconds;
            rend.material.SetFloat(Constants.DissolvePercentage, percentage);
            yield return null;
        }
        Destroy(gameObject);
    }
}
