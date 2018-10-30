using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    private Rigidbody rb;

    private AudioSource source;
    public AudioClip weaponClip;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update ()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject clone = 
            Instantiate(shot, transform.position, transform.rotation);
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(weaponClip);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
             Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
             0.0f,
             Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}