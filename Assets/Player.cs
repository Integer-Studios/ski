using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    public float Speed;
    public JoystickCamera _cam;
    public float Mass;
    public float Gravity;
    public float MaxSpeed;
    public float TurnForce;
    public float Friction;
    public GameObject Character;
    public float TurnRotatation;
    public bool IsSkiing;

    private CharacterController _cc;
    private Vector3 _velocity;
    private Vector3 _turnForce;
    private int _turnDir = 1;
    private Vector3 _targetForward;

    private void Start() {
        _cc = GetComponent<CharacterController>();
    }

    private void UpdateWalking() {

        float v = CrossPlatformInputManager.GetAxis("Vertical"), h = CrossPlatformInputManager.GetAxis("Horizontal");
        if (Mathf.Abs(v) + Mathf.Abs(h) > 0) {
            _targetForward = (_cam.transform.forward * v + _cam.transform.right * h).normalized;
            _targetForward = Vector3.ProjectOnPlane(_targetForward, Vector3.up);
            transform.forward = Vector3.Lerp(transform.forward, _targetForward, 0.3f);
            _cc.Move(transform.forward * Time.deltaTime * Speed);
        }
        if (!_cc.isGrounded) {
            _cc.Move(Vector3.down * Time.deltaTime * 9.8f);
        }

    }

    private void UpdateSkiing() {
        Vector3 netForce = Vector3.zero;
        Vector3 gravitationalForce = Vector3.down * Gravity * Mass;
        netForce += gravitationalForce;
        RaycastHit hit;
        if (_cc.isGrounded && Physics.Raycast(transform.position, Vector3.down, out hit, 5f)) {
            float d = Vector3.Dot(hit.normal, Vector3.up);
            Vector3 normalForce = hit.normal.normalized * (d * Mass * Gravity);
            netForce += normalForce;

            Vector3 up = transform.up;
            transform.up = hit.normal;
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // pole pivots
            if (h > 0.1f && _turnDir < 0) {
                _turnForce = Vector3.zero;
                _turnDir = 1;
            } else if (h < -0.1f && _turnDir > 0) {
                _turnForce = Vector3.zero;
                _turnDir = -1;
            }

            if (h > 0.1f || h < -0.1f) {
                _turnForce += _cam.transform.right * TurnForce * Mass * Time.deltaTime * h;
                Character.transform.localEulerAngles += new Vector3(0, 0, -1f) * TurnRotatation * Time.deltaTime * h;
            } else {
                _turnForce = Vector3.Lerp(_turnForce, Vector3.zero, 0.01f);
                Character.transform.localRotation = Quaternion.Lerp(Character.transform.localRotation, Quaternion.identity, 0.01f);
            }
            transform.up = up;
            netForce += _turnForce;
        } else {
            _turnForce = Vector3.Lerp(_turnForce, Vector3.zero, 0.01f);
            Character.transform.localRotation = Quaternion.Lerp(Character.transform.localRotation, Quaternion.identity, 0.01f);
        }

        Vector3 frictionalForce = _velocity * -1f * Friction * Mass;
        netForce += frictionalForce;
        _velocity += (netForce / Mass) * Time.deltaTime;
        if (_velocity.magnitude > MaxSpeed) {
            _velocity.Normalize();
            _velocity *= MaxSpeed;
        }
        transform.forward = _velocity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _cc.Move(_velocity * Time.deltaTime);
    }

    void Update() {
        if (IsSkiing)
            UpdateSkiing();
        else
            UpdateWalking();
    }
}
