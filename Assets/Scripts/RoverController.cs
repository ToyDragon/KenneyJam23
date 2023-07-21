using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    private CharacterController characterController;
    public Vector2 input;
    public float maxSpeed = 3f;
    public float accelerationForce = 2f;
    public float turnStrength = 1f;
    public float lastDirection;
    public Transform wheelFrontLeft;
    public Transform wheelFrontRight;
    public Transform wheelBackLeft;
    public Transform wheelBackRight;
    public float animatedWheelDir;
    public float distanceTraveled;
    public Vector3 vel;
    private Vector3 posLastFrame;
    void OnEnable() {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        float yInput = 0;
        if (Input.GetKey(KeyCode.W)) {
            yInput = 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            yInput = -1;
        }
        float xInput = 0;
        if (Input.GetKey(KeyCode.A)) {
            xInput = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            xInput = 1;
        }
        input = new Vector2(xInput, yInput);
    }

    private Vector2 NoY(Vector3 v) {
        return new Vector2(v.x, v.z);
    } 
    void FixedUpdate() {
        float targetSpeed = input.y * maxSpeed;
        vel *= 1 - Time.fixedDeltaTime;

        if (posLastFrame.y >= transform.position.y - 0.001f && vel.y < 0) {
            vel = new Vector3(
                vel.x,
                0,
                vel.z
            );
        }

        var planarVel = NoY(vel);
        float planarVelMagnitude = planarVel.magnitude;
        float velMagnitude = vel.magnitude;
        if (Vector3.Dot(vel, transform.forward) < 0) {
            planarVelMagnitude *= -1;
            velMagnitude *= -1;
        }
        distanceTraveled += planarVelMagnitude * Time.deltaTime;
        float speedDel = targetSpeed - planarVelMagnitude;
        vel += speedDel * accelerationForce * transform.forward;
        vel += Physics.gravity * Time.fixedDeltaTime;
        characterController.Move(vel * Time.fixedDeltaTime);

        // TODO update wheel animations
        float targetWheelDir = 30 * input.x;
        animatedWheelDir += (targetWheelDir - animatedWheelDir) * Mathf.Clamp01(Time.deltaTime * 4);
        float wheelXRot = distanceTraveled * 360f;
        wheelFrontLeft.transform.localEulerAngles = new Vector3(
            wheelXRot,
            animatedWheelDir,
            wheelFrontLeft.transform.localEulerAngles.z
        );
        wheelFrontRight.transform.localEulerAngles = new Vector3(
            wheelXRot,
            animatedWheelDir,
            wheelFrontRight.transform.localEulerAngles.z
        );
        wheelBackLeft.transform.localEulerAngles = new Vector3(
            wheelXRot,
            wheelBackLeft.transform.localEulerAngles.y,
            wheelBackLeft.transform.localEulerAngles.z
        );
        wheelBackRight.transform.localEulerAngles = new Vector3(
            wheelXRot,
            wheelBackRight.transform.localEulerAngles.y,
            wheelBackRight.transform.localEulerAngles.z
        );

        if (input.x != 0) {
            float stillModifier = 1;
            if (planarVel.magnitude < 1) {
                stillModifier = .2f;
            }
            float turnThisFrame = input.x * turnStrength * (input.y < 0 ? -1 : 1) * stillModifier;
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + turnThisFrame,
                transform.rotation.eulerAngles.z
            );
            vel = Quaternion.Euler(0, turnThisFrame, 0) * vel;
        }

        if (planarVel.magnitude > .2) {
            lastDirection = -Vector2.SignedAngle(Vector2.up, new Vector2(vel.x, vel.z));
            if (planarVelMagnitude < 0) {
                lastDirection = -Vector2.SignedAngle(Vector2.up, new Vector2(-vel.x, -vel.z));
            }
        } else {
            lastDirection = transform.rotation.eulerAngles.y;
        }
        CameraFollow.instance.rotation = lastDirection;
        
        input = Vector2.zero;
        posLastFrame = transform.position;
    }
}
