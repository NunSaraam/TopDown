using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Aiming : MonoBehaviour
{
    //총알 발사
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10.0f;

    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private RectTransform reloadUI;
    [SerializeField] private Vector2 reloadUIOffset = new Vector2(0, 0);
    [SerializeField] WeaponData data;


    [SerializeField] private Transform mouseTransform;

    public Camera mainCamera;
    public Animator animator;

    private float lastShotTime;

    private void Awake()
    {
        Cursor.visible = false;     //커서 비활성화
        Cursor.lockState = CursorLockMode.Confined;     //게임 화면 안에서만 움직임

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        mouseTransform = transform.Find("Aim");
    }

    private void Update()
    {
        UpdataCrosshairPosition();
        UpdateReloadUI();
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
        if (Time.time < lastShotTime + data.GetShootRate())
        {
            return;
        }

        if (!data.IsAuto() && Input.GetMouseButtonDown(0))
        {
            lastShotTime = Time.time;
            Fire();
        }

        else if (data.IsAuto() && Input.GetMouseButton(0))
        {
            lastShotTime = Time.time;
            Fire();
        }
    }

    private void Fire()
    {
        if (data.GetCurrentAmmo() <= 0)
        {
            Debug.Log("탄약이 없습니다. 재장전");
            data.TryReload();
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

        data.ConsumeAmmo();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateReloadUI()
    {
        if (reloadUI == null || data == null) return;

        Vector2 mousePos = Input.mousePosition;
        reloadUI.position = mousePos + reloadUIOffset;

        reloadUI.gameObject.SetActive(data.IsReloading());
    }

    private void UpdataCrosshairPosition()
    {
        if (crosshairUI == null) return;

        Vector2 mousePos = Input.mousePosition;
        crosshairUI.position = mousePos;
    }


    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
