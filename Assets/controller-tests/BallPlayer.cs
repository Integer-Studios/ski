using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BallPlayer : MonoBehaviour {

    public float TurnDrag;
    public float Force;
    public JoystickCamera _cam;
    private Rigidbody _rigidbody;
    private Vector3 _prevForce;


	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = _rigidbody.velocity;
        float v = CrossPlatformInputManager.GetAxis("Vertical"), h = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector3 force = (_cam.transform.forward * v + _cam.transform.right * h).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f)) {
            force = Vector3.ProjectOnPlane(force, hit.normal);
        }
        float mag = _rigidbody.velocity.magnitude;
        force *= Force * Time.deltaTime * mag;
        _rigidbody.velocity += force;
        _rigidbody.velocity = _rigidbody.velocity.normalized * mag;
	}
}
