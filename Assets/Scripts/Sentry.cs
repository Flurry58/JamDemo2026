using UnityEngine;

public class LeftFacingTurret : MonoBehaviour
{
    [Header("Detection Setup")]
    public float detectionRange = 10f;
    public LayerMask playerLayer;

    [Header("Shooting Setup")]
    public GameObject bulletPrefab;
    public Transform firePoint; 
    public float fireRate = 1.5f;
    public AudioClip Noise;

    

    private float nextFireTime;

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, detectionRange, playerLayer);

        if (hit.collider != null)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                AudioSource.PlayClipAtPoint(Noise, transform.position);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

}