using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                _turnForce = Vector3.zero;
                _turnDir = 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                _turnForce = Vector3.zero;
                _turnDir = -1;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
                _turnForce += transform.right * TurnForce * Mass * _turnDir * Time.deltaTime;
                Character.transform.localEulerAngles += new Vector3(0, 0, _turnDir) * TurnRotatation * Time.deltaTime * -1f;
            } else {
                _turnForce = Vector3.Lerp(_turnForce, Vector3.zero, 0.01f);
                Character.transform.localRotation = Quaternion.Lerp(Character.transform.localRotation, Quaternion.identity, 0.01f);
            }
            transform.up = up;
            netForce += _turnForce;
        }

        Vector3 frictionalForce = _velocity * -1f * Friction * Mass;
        netForce += frictionalForce;
        _velocity += (netForce/Mass) * Time.deltaTime;
        if (_velocity.magnitude > MaxSpeed) {
            _velocity.Normalize();
            _velocity *= MaxSpeed;
        }
        transform.forward = _velocity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _cc.Move(_velocity * Time.deltaTime);

    }
}
