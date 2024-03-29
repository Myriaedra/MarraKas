﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;

    float distance = 5.0f;
    float currentX = 0.0f;
    float currentY = 0.0f;
    float sensivityX = 4.0f;
    float sensivityY = 1.0f;

    bool replacing = false;

    float wantedAngle;

    const float ANGLE_Y_MIN = 0.0f;
    const float ANGLE_Y_MAX = 50.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentX += Input.GetAxis("CameraX")*sensivityX;
        currentY += Input.GetAxis("CameraY")*sensivityY;

        currentY = Mathf.Clamp(currentY, ANGLE_Y_MIN, ANGLE_Y_MAX);

        if (Input.GetButtonDown("ResetCamera"))
        {
            replacing = true;
            wantedAngle = target.eulerAngles.y;
        }

        if (replacing)
        {
            currentX = Mathf.Lerp(currentX, wantedAngle, 0.2f);
            if (Mathf.Abs(currentX - Mathf.Abs(wantedAngle)) < 0.5f)
            {
                currentX = wantedAngle;
                wantedAngle = 0;
                replacing = false;
            }
        }
	}

    void LateUpdate ()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * dir;
        transform.LookAt(target.position);
    }
}
