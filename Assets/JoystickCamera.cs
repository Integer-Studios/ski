using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float v = 0,h = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            v = 1;
        else if (Input.GetKey(KeyCode.DownArrow))
            v = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            h = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            h = -1;
        transform.RotateAround(Target.position, Vector3.up, h * RotationSpeed * Time.deltaTime);
        transform.RotateAround(Target.position, transform.right, v * RotationSpeed * Time.deltaTime);

    }
}
