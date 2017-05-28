using System.Collections;
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
    [SerializeField] private float _thrusterFuelBurnSpeed = 1f;
    [SerializeField] private float _thrusterFuelRegenSpeed = 0.3f;
    private float _thrusterFuelAmount = 1f;
    [SerializeField] private LayerMask _envorinmentMask;

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
        if (Input.GetButton("Jump") && _thrusterFuelAmount > 0)
        {
            _thrusterFuelAmount -= _thrusterFuelBurnSpeed * Time.deltaTime;

            if (_thrusterFuelAmount >= 0.01f)
            {
                thrusterForce = transform.up * _thrusterForce;
                SetJointSettings(0f);
            }

        }
        else
        {
            _thrusterFuelAmount += _thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(_jointSpring);
        }

        _thrusterFuelAmount = Mathf.Clamp(_thrusterFuelAmount, 0f, 1f);

        _motor.ApplyThruster(thrusterForce);
    }

    //Set target position for spring when flying over objects
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, _envorinmentMask))
        {
            _joint.targetPosition = new Vector3(0f, -hit.point.y, 0f);
        }
        else
        {
            _joint.targetPosition = new Vector3(0f, 0f, 0f);
        }
    }
#endregion

    public float GetThrusterFuelAmount()
    {
        return _thrusterFuelAmount;
    }

    private void SetJointSettings(float jSpring)
    {
        _joint.yDrive = new JointDrive
        {
            maximumForce = _jointMaxForce,
            positionSpring = jSpring
        };
    }
}
