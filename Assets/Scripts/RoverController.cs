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
    public Transform trailerWheel1;
    public Transform trailerWheel2;
    public Transform trailerWheel3;
    public Transform trailerWheel4;
    public Transform trailerWheel5;
    public bool trailerAttached = false;
    public float animatedWheelDir;
    public float distanceTraveledLeft;
    public float distanceTraveledRight;
    public Vector3 vel;
    private Vector3 posLastFrame;
    public Transform model;
    public bool groundedRecently;
    public AudioSource audioSource;
    public Animator roverAnimator;
    void OnEnable() {
        characterController = GetComponent<CharacterController>();
        roverAnimator = GetComponentInChildren<Animator>();
        instance = this;
    }
    void Update()
    {
        if (CamCutsceneController.instance.running) { return; }
        if (CameraFollow.instance.mapEnabled) {
            input = Vector2.zero;
            return;
        }
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
        if (CamCutsceneController.instance.running) { return; }
        if (RampCreator.instance.rampCreationStart.HasValue) {
            audioSource.volume = .1f;
            return;
        }
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
        roverAnimator.SetBool("Driving", NoY(vel).magnitude > 0.1f);
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

        if(trailerAttached){
            trailerWheel1.transform.localEulerAngles = new Vector3(
                wheelXRot,
                0,
                0
            );
            trailerWheel2.transform.localEulerAngles = new Vector3(
                wheelXRot,
                0,
                0
            );
            trailerWheel3.transform.localEulerAngles = new Vector3(
                wheelXRot,
                0,
                0
            );
            trailerWheel4.transform.localEulerAngles = new Vector3(
                wheelXRot,
                0,
                0
            );
            trailerWheel5.transform.localEulerAngles = new Vector3(
                wheelXRot,
                0,
                0
            );
        }

        var frontPos = transform.position + transform.forward * .1f + Vector3.up * .25f;
        var backPos = transform.position - transform.forward * .2f + Vector3.up * .25f;
        float modelXRot = 0;
        Debug.DrawLine(frontPos, frontPos + Vector3.down * 1.5f, Color.blue, .1f);
        Debug.DrawLine(backPos, backPos + Vector3.down * 1.5f, Color.blue, .1f);
        if (Physics.Raycast(frontPos, Vector3.down, out var frontHit, 1.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore) && Physics.Raycast(backPos, Vector3.down, out var backHit, 1.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
            var delta = (frontHit.point - backHit.point).normalized;
            modelXRot = -Mathf.Asin(delta.y)*180/Mathf.PI;
        }

        var leftPos = transform.position + transform.right * .1f + Vector3.up * .25f - transform.forward * .05f;
        var rightPos = transform.position - transform.right * .1f + Vector3.up * .25f - transform.forward * .05f;
        float modelZRot = 0;
        Debug.DrawLine(leftPos, leftPos + Vector3.down * 1.45f, Color.blue, .1f);
        Debug.DrawLine(rightPos, rightPos + Vector3.down * 1.45f, Color.blue, .1f);
        if (Physics.Raycast(leftPos, Vector3.down, out var leftHit, 1.45f, Physics.AllLayers, QueryTriggerInteraction.Ignore) && Physics.Raycast(rightPos, Vector3.down, out var rightHit, 1.45f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
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
