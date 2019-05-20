using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShootEnemy : MonoBehaviour
{
    public Color bulletColor;
    public float bounceDuration = 10;
    public float pierceDuration = 10;
    // The damage inflicted by each bullet.
    public int damagePerShot = 20;
    public int numberOfBullets = 1;
    // The time between each shot.
    public float timeBetweenBullets = 0.15f;
    public float angleBetweenBullets = 10f;
    // The distance the gun can fire.
    public float range = 100f;
    // A layer mask so the raycast only hits things on the shootable layer.
    public LayerMask shootableMask;
   
    public GameObject bullet;
    public Transform bulletSpawnAnchor;

    // A timer to determine when to fire.
    float timer;
    // A ray from the gun end forwards.
    Ray shootRay;
    // A raycast hit to get information about what was hit.
    RaycastHit shootHit;
    // Reference to the particle system.
    ParticleSystem gunParticles;
    // Reference to the line renderer.
    LineRenderer gunLine;
    // Reference to the audio source.
    AudioSource gunAudio;
    // Reference to the light component.
    Light gunLight;
    // The proportion of the timeBetweenBullets that the effects will display for.
    float effectsDisplayTime = 0.2f;
  

    

    void Awake()
    {
        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponentInChildren<Light>();

        
    }

    void Update()
    {
        

        bulletColor = bulletColor;
       

        gunParticles.startColor = bulletColor;
        // For some reason the color I had selected originally looked extremely
        // reddish after I switched to deferred rendering and linear mode so 
        // I'm hardcoding in a lighter, more yellow light color if you have
        // both the pierce and bounce powerup active.
        gunLight.color =  bulletColor;

        // Add the time since Update was last called to the timer.
       
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if ( timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // ... shoot the gun.
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLight.enabled = false;
    }

    void Shoot()
    {
        // Reset the timer.
        timer = 0f;

        // Play the gun shot audioclip.
        gunAudio.pitch = Random.Range(1.2f, 1.3f);
       
        gunAudio.Play();

        // Enable the light.
        gunLight.intensity = 2 + (0.25f * (numberOfBullets - 1));
        gunLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.startSize = 1 + (0.1f * (numberOfBullets - 1));
        gunParticles.Play();

        // Set the shootRay so that it starts at the end ofres the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        for (int i = 0; i < numberOfBullets; i++)
        {
            // Make sure our bullets spread out in an even pattern.
            float angle = i * angleBetweenBullets - ((angleBetweenBullets / 2) * (numberOfBullets - 1));
            Quaternion rot = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
            GameObject instantiatedBullet = Instantiate(bullet, bulletSpawnAnchor.transform.position, rot) as GameObject;
            instantiatedBullet.GetComponent<Bullet>().bulletColor = bulletColor;
        }
    }
}
