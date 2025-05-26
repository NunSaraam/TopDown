using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Aiming : MonoBehaviour
{
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
            animator.SetTrigger("Shoot");
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
