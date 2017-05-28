﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lookSensitivity = 4f;
    [SerializeField] private float _thrusterForce = 1000f;

    [Header("Joint options")]
    [SerializeField] private float _jointSpring = 20f;
    [SerializeField] private float _jointMaxForce = 40f;

    private PlayerMotor _motor;
    private ConfigurableJoint _joint;
    private Animator _animator;

    #endregion

    #region Unity
    void Start()
    {
        _motor = GetComponent<PlayerMotor>();
        _joint = GetComponent<ConfigurableJoint>();
        _animator = GetComponent<Animator>();
        SetJointSettings(_jointSpring);
    }

    void Update()
    {
        //move
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        Vector3 velocity = (moveVertical + moveHorizontal) * _speed;

        _motor.Move(velocity);

        //animator movement
        _animator.SetFloat("ForwardVelocity", zMove);

        //turning around
        float yRot = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0f, yRot, 0f);

        _motor.Rotate(rotation * _lookSensitivity);

        //camera rotation

        float xRot = Input.GetAxis("Mouse Y");

        float cameraRotation = xRot * _lookSensitivity;

        _motor.RotateCamera(cameraRotation);


        //thruster force
        Vector3 thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            thrusterForce = transform.up * _thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(_jointSpring);
        }

        _motor.ApplyThruster(thrusterForce);
    }
#endregion

    private void SetJointSettings(float jSpring)
    {
        _joint.yDrive = new JointDrive
        {
            maximumForce = _jointMaxForce,
            positionSpring = jSpring
        };
    }
}
