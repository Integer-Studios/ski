using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float Mass;
    public float Gravity;
    public float MaxSpeed;
    public float TurnForce;
    public float Friction;

    private CharacterController _cc;
    private Vector3 _velocity;

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
        }
        Vector3 turnForce = Input.GetAxis("Horizontal") * transform.right * TurnForce * Mass;
        netForce += turnForce;
        Vector3 frictionalForce = _velocity * -1f * Friction * Mass;
        netForce += frictionalForce;
        _velocity += (netForce/Mass) * Time.deltaTime;
        if (_velocity.magnitude > MaxSpeed) {
            _velocity.Normalize();
            _velocity *= MaxSpeed;
        }
        transform.forward = _velocity;

        _cc.Move(_velocity * Time.deltaTime);

    }
}
