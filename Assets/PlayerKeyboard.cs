using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerKeyboard : MonoBehaviour {

    public float Mass;
    public float Gravity;
    public float MaxSpeed;
    public float TurnForce;
    public float Friction;
    public GameObject Character;
    public float TurnRotatation;

    private CharacterController _cc;
    private Vector3 _velocity;
    private Vector3 _turnForce;
    private int _turnDir = 1;

    private void Start() {
        _cc = GetComponent<CharacterController>();
    }

    void Update() {
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
                _turnForce += transform.right * TurnForce * Mass * Time.deltaTime * h;
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
}
