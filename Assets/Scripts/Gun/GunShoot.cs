/*

            Handles the Gun logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A script that is put on the Gun object.
/// </summary>
public class GunShoot : MonoBehaviour
{
    /// <summary>
    /// The steam-controller action for Shoot.
    /// </summary>
    public SteamVR_Action_Boolean shootAction;
    /// <summary>
    /// The steam-controller action for Reload.
    /// </summary>
    public SteamVR_Action_Boolean reloadAction;
    /// <summary>
    /// The bullet prefab.
    /// </summary>
    public GameObject bulletPrefab;
    /// <summary>
    /// The place that the bullet goes out of.
    /// </summary>
    public Transform barrelExit;
    /// <summary>
    /// A text indicating how many bullets are remaining.
    /// </summary>
    public Text currentAmmoText;
    /// <summary>
    /// A text indicating how many bullets in a magazine 
    /// </summary>
    public Text maxAmmoText;

    /// <summary>
    /// How many bullets in a magazine.
    /// </summary>
    public int maxAmmo;
    /// <summary>
    /// Current amount of bullets.
    /// </summary>
    int currentAmmo;

    /// <summary>
    /// How fast can the gun fire.
    /// </summary>
    public float fireRate = 0.2f;
    /// <summary>
    /// Used to count down the fireRate.
    /// </summary>
    float fireCounter = 0f;

    public static Ray raycast;

    void Awake()
    {
        currentAmmo = maxAmmo;
        currentAmmoText.text = currentAmmo.ToString();
        maxAmmoText.text = maxAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        raycast = new Ray(transform.position, transform.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * 100);

        if (reloadAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            ReloadGun();
        }
        if (shootAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Shoot();
        }
        TimeToShoot();
    }

    /// <summary>
    /// Shoots the bullet.
    /// </summary>
    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            currentAmmoText.text = currentAmmo.ToString();
        }
        else
        {
            if (fireCounter <= 0)
            {
                Instantiate(bulletPrefab, barrelExit.position, Quaternion.identity);
                fireCounter = fireRate;
                currentAmmo = currentAmmo - 1;
                currentAmmoText.text = currentAmmo.ToString();
            }
        }
    }

    /// <summary>
    /// Counts down until the next bullet can be shot.
    /// </summary>
    void TimeToShoot()
    {
        if (fireCounter != 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Reloads the gun.
    /// </summary>
    void ReloadGun()
    {
        currentAmmo = maxAmmo;
        currentAmmoText.text = currentAmmo.ToString();
    }
}
