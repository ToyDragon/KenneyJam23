using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    private Rigidbody rb;
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
    void OnEnable() {
        rb = GetComponent<Rigidbody>();
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
        var planarVel = NoY(rb.velocity);
        float planarVelMagnitude = planarVel.magnitude;
        if (Vector3.Dot(rb.velocity, transform.forward) < 0) {
            planarVelMagnitude *= -1;
        }
        distanceTraveled += planarVelMagnitude * Time.deltaTime;
        float speedDel = targetSpeed - planarVelMagnitude;
        rb.AddForce(speedDel * accelerationForce * transform.forward);

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
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + input.x * turnStrength * (input.y < 0 ? -1 : 1) * stillModifier,
                transform.rotation.eulerAngles.z
            );
        }

        if (planarVel.magnitude > .2) {
            lastDirection = -Vector2.SignedAngle(Vector2.up, new Vector2(rb.velocity.x, rb.velocity.z));
            if (planarVelMagnitude < 0) {
                lastDirection = -Vector2.SignedAngle(Vector2.up, new Vector2(-rb.velocity.x, -rb.velocity.z));
            }
        } else {
            lastDirection = transform.rotation.eulerAngles.y;
        }
        CameraFollow.instance.rotation = lastDirection;
        
        input = Vector2.zero;
    }
}
