using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    public float Speed;
    public JoystickCamera _cam;
    private CharacterController _cc;
    private Vector3 _targetForward;

    private void Start() {
        _cc = GetComponent<CharacterController>();
    }

    void Update() {
        float v = CrossPlatformInputManager.GetAxis("Vertical"), h = CrossPlatformInputManager.GetAxis("Horizontal");
        if (Mathf.Abs(v) + Mathf.Abs(h) > 0) {
            _targetForward = (_cam.transform.forward * v + _cam.transform.right * h).normalized;
            _targetForward.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, _targetForward, 0.3f);
            _cc.Move(transform.forward * Time.deltaTime * Speed);
        }
        if (!_cc.isGrounded) {
            _cc.Move(Vector3.down * Time.deltaTime * 9.8f);
        }
    }
}
