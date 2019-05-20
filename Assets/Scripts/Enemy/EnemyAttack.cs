using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
    public float distanceToPlayer = 5f;


    public int damagePerShot = 5;
    public float timeBetweenBullets = 0.15f;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
      //  gunParticles = GetComponentInChildren<ParticleSystem>();
      //  gunLine = GetComponentInChildren<LineRenderer>();
      //  gunAudio = GetComponentInChildren<AudioSource>();
      //  gunLight = GetComponentInChildren<Light>();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0)
        {
            if(playerInRange) Attack ();
            //else if(playerHealth.currentHealth >0 && Vector3.Distance(transform.position, player.transform.position) < distanceToPlayer && Vector3.Distance(transform.position, player.transform.position)>0.02*distanceToPlayer)
           // {
           //     Shoot();
           // }
           
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
        if (timer >= timeBetweenBullets * effectsDisplayTime || playerHealth.currentHealth <= 0)
        {
           // DisableEffects();
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
    void Shoot()
    {
           //transform.LookAt(player.transform);
        //timer = 0f;

       // gunAudio.Play();

        //gunLight.enabled = true;
        //gunParticles.Stop();
        //gunParticles.Play();

      //  gunLine.enabled = true;
       // gunLine.SetPosition(0,transform.position);

        
        

       // if (Physics.Raycast(shootRay, out shootHit, distanceToPlayer))
     //  {
            // Try and find an EnemyHealth script on the gameobject hit.
        //    if (shootHit.transform.tag == "Player")
          //  {
          //      playerHealth.TakeDamage(100);
          //  }
        //}
       // gunLine.SetPosition(1, player.transform.position + Vector3.up * 1.5f);


    }
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;   
    }
}
