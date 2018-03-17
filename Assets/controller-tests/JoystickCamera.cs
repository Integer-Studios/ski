using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class JoystickCamera : MonoBehaviour {

    public Transform Target;
    public float RotationSpeed = 60f;
    private Vector3 _prevTargetPos;

	// Use this for initialization
	void Start () {
        _prevTargetPos = Target.position;
	}

    // Update is called once per frame
    void Update() {
        transform.position += Target.position - _prevTargetPos;
        _prevTargetPos = Target.position;
        float v = CrossPlatformInputManager.GetAxis("RightAnalogY"), h = CrossPlatformInputManager.GetAxis("RightAnalogX");
        transform.RotateAround(Target.position, Vector3.up, h * RotationSpeed * Time.deltaTime);
        transform.RotateAround(Target.position, transform.right, v * RotationSpeed * Time.deltaTime);

    }
}
