using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{

    #region Variables
    [SerializeField] private Camera cam;
    [SerializeField] private float cameraRotationLimit = 85f;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private float _cameraRotationX = 0;
    private float _currentCameraRotationX = 0;
    private Vector3 _thrusterForce = Vector3.zero;

    private Rigidbody _rb;
    #endregion

    #region Unity
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    #endregion  

    #region SetMoveVariables
    //gets rotational vector
    public void Rotate(Vector3 rot)
    {
        _rotation = rot;
    }

    //gets moving vector
    public void Move(Vector3 vel)
    {
        _velocity = vel;
    }

    //gets rotation vector for camera
    public void RotateCamera(float rotX)
    {
        _cameraRotationX = rotX;
    }

    //gets a force vector for thruster
    public void ApplyThruster(Vector3 force)
    {
        _thrusterForce = force;
    }
    #endregion

    #region ApplyMove
    private void PerformMovement()
    {
        if (_velocity != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + _velocity * Time.deltaTime);
        }

        if (_thrusterForce != Vector3.zero)
        {
            _rb.AddForce(_thrusterForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation()
    {
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(_rotation));

        if (cam != null)
        {
            _currentCameraRotationX -= _cameraRotationX;
            _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0, 0);
        }
    }
    #endregion

}
