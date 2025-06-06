using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Aiming : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;

    //총알 발사
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10.0f;

    [SerializeField] private Transform mouseTransform;

    public Camera mainCamera;
    public Animator animator;



    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        mouseTransform = transform.Find("Aim");
    }

    private void Update()
    {
        HandleAiming();
        Shooting();

    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 mouseDirection = (mousePosition - mouseTransform.position).normalized;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        mouseTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void Shooting()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (weaponData.GetCurrentAmmo() <= 0)
            {
                Debug.Log("탄약이 없습니다. 재장전");
                weaponData.TryReload();
                return;
            }

            animator.SetTrigger("Shoot");
        
            Vector3 mousePos = GetMouseWorldPosition();
            Vector3 direction = (mousePos - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
            rb.velocity = direction * bulletSpeed;
            }

            weaponData.ConsumeAmmo();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
