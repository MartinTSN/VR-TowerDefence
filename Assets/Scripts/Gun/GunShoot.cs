using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunShoot : MonoBehaviour
{
    public SteamVR_Action_Boolean shootAction;
    public SteamVR_Action_Boolean reloadAction;
    public GameObject bulletPrefab;
    public Transform barrelExit;
    public Text currentAmmoText;
    public Text maxAmmoText;

    public int maxAmmo;
    int currentAmmo;

    public float fireRate = 0.2f;
    float fireCounter = 0f;

    public static Ray raycast;

    // Use this for initialization
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
            shoot();
        }
        timeToShoot();
    }

    void shoot()
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

    void timeToShoot()
    {
        if (fireCounter != 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    void ReloadGun()
    {
        currentAmmo = maxAmmo;
        currentAmmoText.text = currentAmmo.ToString();
    }
}
