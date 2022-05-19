using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


[System.Serializable]
public struct FlightControls
{
    public float maxClamp;
    [Range(0, 1)] public float minMouseMov;
    public float rotateSpeed;
    public float mouseSens;
}

[Serializable]
struct FlightPhysics
{
    public float speed;
    public float boostMultiplier;
    public float maxDrag;
    public float minDrag;
    public float verticalMultiplier;
}


public class PlayerShip : MonoBehaviour
{

    [SerializeField] private Transform canvas;
    
    [SerializeField] private FlightPhysics flightPhysics = new()
    {
        speed = 10f,
        boostMultiplier = 5f,
        maxDrag = 5f,
        minDrag = 1f,
        verticalMultiplier = 150f,
    };

    public FlightControls ControlSettings => flightControls;

    [SerializeField] private FlightControls flightControls = new()
    {
        maxClamp = 2f,
        minMouseMov = 0.2f,
        mouseSens = 0.005f,
        rotateSpeed = 50f,
    };

    bool CanProcessInput()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return false;
        return true;
    }

    public Vector2 LookDir => _lookMov;

    private Vector2 _lookMov = Vector2.zero;
    private Vector3 _moveVel = Vector3.zero;
    private Vector3 _forwardVel = Vector3.zero;

    private Vector3 _lookMovBak;
    private Vector3 _moveBak;

    private Rigidbody _body;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if (CanProcessInput())
        {
            HandleRotation();
            HandleMovement();
        }
    }

    public void ResetLook()
    {
        _lookMov = Vector2.zero;
    }
    
    private void HandleRotation()
    {
        var hor = Input.GetAxis("Horizontal");
        var yaw = Input.GetAxis("Mouse X");
        var pitch = Input.GetAxis("Mouse Y");

        _lookMov = Vector2.ClampMagnitude(
            _lookMov + new Vector2(yaw * flightControls.mouseSens, pitch * flightControls.mouseSens),
            flightControls.maxClamp);

        var rot = new Vector3(0, 0, -hor * Time.deltaTime * flightControls.rotateSpeed);
        if (_lookMov.magnitude > flightControls.minMouseMov)
        {
            rot.x = -_lookMov.y * Time.deltaTime * flightControls.rotateSpeed;
            rot.y = _lookMov.x * Time.deltaTime * flightControls.rotateSpeed;
        }

        transform.Rotate(rot);
    }

    private void HandleMovement()
    {
        var up = Input.GetKey(KeyCode.Space);
        var down = Input.GetKey(KeyCode.LeftControl);
        var vert = Input.GetAxisRaw("Vertical");

        var multiplier = Input.GetKey(KeyCode.LeftShift) ? flightPhysics.boostMultiplier : 1f;

        _forwardVel = Vector3.zero;
        _forwardVel += transform.forward.normalized * (vert * flightPhysics.speed * multiplier);

        _moveVel = _forwardVel;

        if (up)
        {
            _moveVel += transform.up * (flightPhysics.speed * flightPhysics.verticalMultiplier * Time.deltaTime);
        }
        else if (down)
        {
            _moveVel -= transform.up * (flightPhysics.verticalMultiplier * flightPhysics.speed * Time.deltaTime);
        }

        _body.AddForce(_moveVel * Time.deltaTime, ForceMode.VelocityChange);

        var t = Mathf.InverseLerp(flightPhysics.speed, 0, GetForwardSpeed());
        _body.drag = Mathf.Lerp(flightPhysics.minDrag, flightPhysics.maxDrag, t);
    }

    public float GetForwardSpeed()
    {
        return _forwardVel.magnitude;
    }

    public float GetMaxSpeed()
    {
        return flightPhysics.speed;
    }

    public float GetMaxBoostSpeed()
    {
        return flightPhysics.speed * flightPhysics.boostMultiplier;
    }
}