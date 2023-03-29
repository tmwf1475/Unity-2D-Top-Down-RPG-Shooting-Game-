using UnityEngine;
using System.Collections;
using GameStartStudio;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform shootTrail;
    [SerializeField] private Transform muzzleFlash;
    [SerializeField] private Transform hitParticle;

    //weapon system
    [Header("WeaponAttribute")]
    public LayerMask whatToHit;
    public FireMode fireMode = FireMode.Burst;
    public bool isReloading;
    public float nextShotTime;
    public int shotsNumber;
    public int clipSize;
    public int fireRate;
    public float reloadTime;
    public bool isTriggerReleased;

    public void OnTriggerHold(Vector2 mousePosition)
    {
        if (isReloading) 
            return;

        if (shotsNumber >= clipSize)
        {
            StartCoroutine(ReloadCoroutine());
            return;
        }

        switch (fireMode)
        {
            case FireMode.Single:
            {
                playerController.SetShootingState(false);
                if (!isTriggerReleased)
                    return;
                Shoot(mousePosition);
                isTriggerReleased = false;
                break;
            }
            case FireMode.Burst:
            {
                if (Time.time < nextShotTime)
                {
                    playerController.SetShootingState(false);
                    return;
                }
                nextShotTime = Time.time + 1 / (float)fireRate;
                Shoot(mousePosition);
                break;
            }
        }
    }

    public void OnTriggerRelease()
    {
        isTriggerReleased = true;
        playerController.SetShootingState(false);
    }

    #region reload

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        //to do sound animation ...
        Debug.Log("reload");
        isReloading = true;
        playerController.SetShootingState(false);
        yield return new WaitForSeconds(reloadTime);
        shotsNumber = 0;
        isReloading = false;
    }

    #endregion

    #region shoot and shoot effect
    
    private void Shoot(Vector2 mousePosition)
    {
        shotsNumber++;
        playerController.SetShootingState(true);
        SoundManager.Instance.PlaySound("Assault Shoot");

        Vector2 shootDirection = GetShootdirection(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDirection, 100, whatToHit);
        if (hit.collider != null)
        {
            ShootEffect(hit);
        }
        else
        {
            ShootEffect(shootDirection);
        }

    }

    private void ShootEffect(Vector2 shootDirection)
    {
        float trailAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion trailRotation = Quaternion.AngleAxis(trailAngle, Vector3.forward);
        Transform trail = Instantiate(shootTrail, firePoint.position, trailRotation);
        Destroy(trail.gameObject, 0.05f);
        //Debug.DrawLine(firePointPosition, shootDirection * 100, Color.red);

        MuzzleFlash();
    }
    private void ShootEffect(RaycastHit2D hit)
    {
        Vector3 firePointPosition = firePoint.position;
        Transform trail = Instantiate(shootTrail, firePointPosition, Quaternion.identity);
        LineRenderer lineRenderer = trail.GetComponent<LineRenderer>();
        Vector3 endPosition = new Vector3(hit.point.x, hit.point.y, firePointPosition.z);
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, firePointPosition);
        lineRenderer.SetPosition(1, endPosition);
        Destroy(trail.gameObject, 0.05f);

        //hit effect
        Transform sparks = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(sparks.gameObject, 0.2f);

        //make damage
        IDamageable damageable = hit.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable?.TakeDamage(10);
            PopupText.Create(hit.point, Random.Range(1, 100), Random.Range(0, 100) < 30);
        }
        
        MuzzleFlash();
    }

    //Shoot fire light
    private void MuzzleFlash()
    {
        Transform flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        flash.SetParent(firePoint);
        float randomSize = Random.Range(0.6f, 0.9f);
        flash.localScale = new Vector3(randomSize, randomSize, randomSize);
        Destroy(flash.gameObject, 0.05f);
    }

    #endregion

    #region shooting direction fixed
    
    private Vector2 GetShootdirection(Vector2 mousePosition)
    {
         if (Vector2.Distance(mousePosition, firePoint.position) > 0.5f)
        {
            return mousePosition - (Vector2)firePoint.position;
        }
        
        return transform.up; //firePoint.parent.up
    }

    #endregion
}
