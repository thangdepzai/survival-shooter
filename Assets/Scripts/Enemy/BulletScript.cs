using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    GameObject player;
    bool playerInRange;
    PlayerHealth playerHealth;
    public float speed = 20.0f;
    public float life = 3;
    public int damage = 10;
    public AudioClip hitSound;
    ParticleSystem buleteffect;
    //public ParticleSystem ImpactParticles;

    Vector3 velocity;
    Vector3 force;
    Vector3 newPos;
    Vector3 oldPos;
    Vector3 direction;
    bool hasHit = false;
    RaycastHit lastHit;
    // Reference to the audio source.
    AudioSource bulletAudio;
    float timer;
    // Start is called before the first frame update

    void Awake()
    {
        bulletAudio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        buleteffect = GetComponentInChildren<ParticleSystem>();
    }
    void Start()
    {
        playerInRange = false;
         newPos = transform.position;
        oldPos = newPos;


    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            playerHealth.TakeDamage(damage);
            bulletAudio.clip = hitSound;
            bulletAudio.volume = 0.5f;
            bulletAudio.pitch = Random.Range(0.6f, 0.8f);
            bulletAudio.Play();

            DelayedDestroy();
            return;
        }

        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // Schedule for destruction if bullet never hits anything.
        if (timer >= life)
        {
            Dissipate();
        }

        velocity = transform.forward;
        //velocity.y = 0;
        velocity = velocity.normalized * speed;

        // assume we move all the way
        newPos += velocity * Time.deltaTime;

        // Check if we hit anything on the way
        direction = newPos - oldPos;
        float distance = direction.magnitude;
        oldPos = transform.position;
        transform.position = newPos;

    }
    

    void Dissipate()
    {
        buleteffect.Play();
        Destroy(gameObject);
        
    }

    void DelayedDestroy()
    {
        Destroy(gameObject, hitSound.length);
        buleteffect.Play();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
            buleteffect.Play();
        }
    }


   
}
