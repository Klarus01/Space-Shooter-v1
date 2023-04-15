using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private GameObject[] shootingPoints;
    [SerializeField] private int howManyShootingPoints;
    [SerializeField] private Transform shootingTrans;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifeTime = 5f;
    [SerializeField] private float baseFiringRate = 0.2f;
    [SerializeField] private float firingRateVariance = 0;
    [SerializeField] private float minimumFiringRate = 0.1f;
    [SerializeField] private bool useAI;
    [SerializeField] private int currentPrefab = 0;

    [HideInInspector]
    public bool isFiring;

    private Coroutine firingCor;
    private Vector2 moveDirection;
    private void Start()
    {
        if (useAI)
        {
            isFiring = true;
            moveDirection = transform.up * -1;
        }
        else
        {
            moveDirection = transform.up;
        }
    }

    private void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCor == null)
        {
            firingCor = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCor != null)
        {
            StopCoroutine(firingCor);
            firingCor = null;
        }

    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            switch (howManyShootingPoints)
            {
                case 1:
                    {
                        shootingTrans = transform;
                        GameObject projectile = Instantiate(projectilePrefabs[currentPrefab], shootingTrans.position, Quaternion.identity);

                        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = moveDirection * projectileSpeed;
                        }

                        Destroy(projectile, projectileLifeTime);
                    }
                    break;
                case 2:
                    for (int i = 0; i < howManyShootingPoints; i++)
                    {
                        shootingTrans = shootingPoints[i].transform;
                        GameObject projectile = Instantiate(projectilePrefabs[currentPrefab], shootingTrans.position, Quaternion.identity);

                        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = moveDirection * projectileSpeed;
                        }

                        Destroy(projectile, projectileLifeTime);
                    }
                    break;
                case 3:
                    {
                        for (int i = 0; i < howManyShootingPoints - 1; i++)
                        {
                            shootingTrans = shootingPoints[i].transform;
                            GameObject projectile = Instantiate(projectilePrefabs[currentPrefab], shootingTrans.position, Quaternion.identity);

                            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                            if (rb != null)
                            {
                                rb.velocity = moveDirection * projectileSpeed;
                            }

                            Destroy(projectile, projectileLifeTime);
                        }

                        shootingTrans = transform;
                        GameObject middleProjectile = Instantiate(projectilePrefabs[currentPrefab], shootingTrans.position, Quaternion.identity);

                        Rigidbody2D mrb = middleProjectile.GetComponent<Rigidbody2D>();
                        if (mrb != null)
                        {
                            mrb.velocity = moveDirection * projectileSpeed;
                        }

                        Destroy(middleProjectile, projectileLifeTime);
                    }
                    break;
                default:
                    {
                        shootingTrans = transform;
                        GameObject projectile = Instantiate(projectilePrefabs[currentPrefab], shootingTrans.position, Quaternion.identity);

                        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = moveDirection * projectileSpeed;
                        }

                        Destroy(projectile, projectileLifeTime);
                    }
                    break;

            }



            currentPrefab++;
            if (currentPrefab.Equals(3))
            {
                currentPrefab = 0;
            }



            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance, baseFiringRate + firingRateVariance);

            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
