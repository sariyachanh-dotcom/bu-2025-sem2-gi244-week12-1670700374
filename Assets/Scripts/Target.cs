using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// IPointerClickHandler interface is used to detect pointer click events.
/// This is part of Unity's Event System.
/// it requires UI/EventSystem on scene, and the main camera should attach PhysicsRaycaster.
public class Target : MonoBehaviour, IPointerClickHandler
{
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;
    private Rigidbody rb;

    public int point;
    public ParticleSystem explosionParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(RandomForce(), ForceMode.Impulse);
        rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque());
        transform.position = RandomSpawnPos();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    // NOTE: OnPointerClick is part of IPointerClickHandler interface
    public void OnPointerClick(PointerEventData eventData)
    {
        //var go = GameObject.Find("GameManeger");
        //var gm = go.GetComponent<GameManager>();

        var gm = FindAnyObjectByType<GameManager>();

        gm.UpdateScore(point);
        Debug.Log("Clicked");
        Instantiate(explosionParticle, transform.position,
            Quaternion.identity);
        Destroy(this.gameObject);

        Debug.Log("Clicked");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sensor"))
        {
            if (!this.CompareTag("Bad"))
            {
                var gm = FindAnyObjectByType<GameManager>();
                gm.UpdateScore(-point);
            }
            Destroy(gameObject);
        }
    }
}