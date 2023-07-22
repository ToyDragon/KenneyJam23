using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{
    public static RoverController instance;
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
    public float distanceTraveledLeft;
    public float distanceTraveledRight;
    public Vector3 vel;
    private Vector3 posLastFrame;
    public Transform model;
    public bool groundedRecently;
    public AudioSource audioSource;
    void OnEnable() {
        characterController = GetComponent<CharacterController>();
        instance = this;
        audioSource = GetComponent<AudioSource>();
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
        float speedDel = targetSpeed - planarVelMagnitude;
        vel += speedDel * accelerationForce * transform.forward;
        vel += Physics.gravity * Time.fixedDeltaTime * 5;
        characterController.Move(vel * Time.fixedDeltaTime);
        distanceTraveledLeft += planarVelMagnitude * Time.fixedDeltaTime;
        distanceTraveledRight += planarVelMagnitude * Time.fixedDeltaTime;

        // TODO update wheel animations
        float targetWheelDir = 30 * input.x;
        animatedWheelDir += (targetWheelDir - animatedWheelDir) * Mathf.Clamp01(Time.deltaTime * 4);
        float wheelXRot = distanceTraveledLeft * 360f;
        wheelFrontLeft.transform.localEulerAngles = new Vector3(
            wheelXRot,
            animatedWheelDir,
            0
        );
        wheelBackLeft.transform.localEulerAngles = new Vector3(
            wheelXRot,
            0,
            0
        );
        wheelXRot = distanceTraveledRight * 360f;
        wheelFrontRight.transform.localEulerAngles = new Vector3(
            wheelXRot,
            animatedWheelDir,
            0
        );
        wheelBackRight.transform.localEulerAngles = new Vector3(
            wheelXRot,
            0,
            0
        );

        var frontPos = transform.position + transform.forward * .125f + Vector3.up * .25f;
        var backPos = transform.position - transform.forward * .125f + Vector3.up * .25f;
        float modelXRot = 0;
        if (Physics.Raycast(frontPos, Vector3.down, out var frontHit, .5f, Physics.AllLayers) && Physics.Raycast(backPos, Vector3.down, out var backHit, .5f, Physics.AllLayers)) {
            var delta = (frontHit.point - backHit.point).normalized;
            modelXRot = -Mathf.Asin(delta.y)*180/Mathf.PI;
        }

        var leftPos = transform.position + transform.right * .1f + Vector3.up * .25f;
        var rightPos = transform.position - transform.right * .1f + Vector3.up * .25f;
        float modelZRot = 0;
        if (Physics.Raycast(leftPos, Vector3.down, out var leftHit, .45f, Physics.AllLayers) && Physics.Raycast(rightPos, Vector3.down, out var rightHit, .45f, Physics.AllLayers)) {
            var delta = (rightHit.point - leftHit.point).normalized;
            modelZRot = -Mathf.Asin(delta.y)*180/Mathf.PI;
        }
    
        model.localEulerAngles = new Vector3(
            modelXRot,
            0,
            modelZRot
        );

        if (input.x != 0) {
            float stillModifier = 1;
            if (planarVel.magnitude < 1) {
                stillModifier = .7f;
            }
            distanceTraveledLeft += input.x * stillModifier * .01f;
            distanceTraveledRight -= input.x * stillModifier * .01f;
            float turnThisFrame = input.x * turnStrength * (input.y < 0 ? -1 : 1) * stillModifier;
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + turnThisFrame,
                transform.rotation.eulerAngles.z
            );
            vel = Quaternion.Euler(0, turnThisFrame, 0) * vel;
        }

        audioSource.volume = Mathf.Clamp01(planarVel.magnitude + .1f);

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
