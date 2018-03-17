using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BallPlayer : MonoBehaviour {

    public float Force;
    public JoystickCamera _cam;
    public Transform Character;
    [Range(10, 40)]
    public float TurnDamping;
    [Range(10, 40)]
    public float SkidDamping;
    private Rigidbody _rigidbody;
    private Vector3 _force;


	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.forward = _rigidbody.velocity;
        float v = CrossPlatformInputManager.GetAxis("Vertical"), h = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector3 targetForce = (_cam.transform.forward * v + _cam.transform.right * h).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f)) {
            targetForce = Vector3.ProjectOnPlane(targetForce, hit.normal);
        }
        float mag = _rigidbody.velocity.magnitude;

        targetForce *= Force * Time.deltaTime * mag;
        _force = Vector3.Lerp(_force, targetForce, 1/TurnDamping);

        _rigidbody.velocity += _force;
        _rigidbody.velocity = _rigidbody.velocity.normalized * mag;

        Character.transform.forward = Vector3.Lerp(Character.transform.forward, targetForce.normalized, 1/SkidDamping);
        Character.transform.position = transform.position;

	}
}
