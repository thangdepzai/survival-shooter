using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
   public ParticleSystem deathParticle;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    public bool isDead;
    bool isSinking;
    // The health bar slider instance for this enemy.
    Slider sliderInstance;
    // The health bar we display over our head.
    public Slider healthBarSlider;
    GameObject enemyHealthbarManager;
    PickupManager pickupManager;
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        enemyHealthbarManager = GameObject.Find("EnemyHealthbarsCanvas");
        
        currentHealth = startingHealth;
       pickupManager = GameObject.Find("PickupManager").GetComponent<PickupManager>();
        Debug.Log("PickupManager", pickupManager);
    }
    void Start()
    {
        sliderInstance = Instantiate(healthBarSlider, gameObject.transform.position, Quaternion.identity) as Slider;
       sliderInstance.gameObject.transform.SetParent(enemyHealthbarManager.transform, false);
        sliderInstance.GetComponent<Healthbar>().enemy = gameObject;
        sliderInstance.gameObject.SetActive(false);
    }

    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;

        // Set the health bar's value to the current health.
        if (currentHealth <= startingHealth)
        {
            sliderInstance.gameObject.SetActive(true);
        }
        int sliderValue = (int)Mathf.Round(((float)currentHealth / (float)startingHealth) * 100);
        sliderInstance.value = sliderValue;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        //deathParticle.Play();
        Debug.Log(gameObject.name, gameObject);
        
      // deathParticle.transform.position = transform.position;
        deathParticle.Play();
        Destroy(sliderInstance.gameObject);
        
        Destroy(gameObject, 2f);
        SpawnPickup();
    }

    void SpawnPickup()
    {
        // We spawn our pickups slightly above the ground.
        Vector3 spawnPosition = transform.position + new Vector3(0, 0.3f, 0);

        float rand = Random.value;
        if (rand <= 0.5f)
        {
            
            Instantiate(pickupManager.healthPickup, spawnPosition, transform.rotation);
    
            
        }
    }
}
