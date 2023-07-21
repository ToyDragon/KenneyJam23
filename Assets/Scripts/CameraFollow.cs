using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    public Rigidbody target;
    public float snappiness = 2f;
    public Vector3 offset;
    public float angle;
    public float actualAngle;
    public float rotation;
    private float actualRotation;
    private Vector3 trackedLocation;
    public float distanceModifier = 1f;
    private float actualDistanceModifier = 1f;
    public float predictionDistance = .5f;
    public float verticalOffset = 0;
    public float actualVerticalOffset = 0;
    void OnEnable() {
        instance = this;
        actualAngle = angle;
        actualRotation = rotation;
    }
    public static Vector3 PlanarForward() {
        var planarForward = new Vector3(instance.transform.forward.x, 0, instance.transform.forward.z).normalized;
        if (planarForward == Vector3.zero) {
            planarForward = Vector3.forward;
        }
        return planarForward;
    }
    public static Vector3 PlanarRight() {
        var planarRight = new Vector3(instance.transform.right.x, 0, instance.transform.right.z).normalized;
        if (planarRight == Vector3.zero) {
            planarRight = Vector3.right;
        }
        return planarRight;
    }
    public void Reset(float angle) {
        actualRotation = rotation = angle;
        actualDistanceModifier = distanceModifier;
        actualVerticalOffset = verticalOffset;
        var rotatedOffset = Quaternion.Euler(actualAngle, actualRotation, 0) * offset;
        transform.position = trackedLocation + rotatedOffset * actualDistanceModifier;
    }
    void Update()
    {
        actualAngle += (angle - actualAngle) * snappiness * Time.deltaTime;
        // actual and target rotations are [0, 360)
        float targetRotation = (rotation + 360) % 360;
        actualRotation = (actualRotation + 360) % 360;
        float rotationDelta = targetRotation - actualRotation;
        if (rotationDelta > 180) {
            rotationDelta -= 360;
        }
        if (rotationDelta < -180) {
            rotationDelta += 360;
        }

        var delta = (target.transform.position + target.velocity * predictionDistance) - trackedLocation;

        distanceModifier = Mathf.Clamp01(target.velocity.magnitude / 1.5f) + 1f;

        // You might think MANGitude is misspelled magnitude, but it's not. It's mang0's attitude.
        float deltaMangitude = delta.magnitude;
        float snapThisFrame = Time.deltaTime * (snappiness + Mathf.Max(0, deltaMangitude - 1) * snappiness * 5f);
        if (deltaMangitude > 10) {
            snapThisFrame = 1f;
        }
        trackedLocation += Mathf.Min(snapThisFrame, 1) * delta;

        var cameraOffset = Vector3.zero;
        // if (LevelManager.instance.waitingForNext) {
        //     distanceModifier += 3f;
        //     rotation += Time.deltaTime * 20f;
        //     verticalOffset = -3;
        // } else {
            actualVerticalOffset = verticalOffset = 0;
        // }
        actualRotation += rotationDelta * snappiness * 2f * Time.deltaTime;
        actualDistanceModifier += (distanceModifier - actualDistanceModifier) * snappiness * 2f * Time.deltaTime;
        actualVerticalOffset += (verticalOffset - actualVerticalOffset) * snappiness * 5f * Time.deltaTime;

        var rotatedOffset = Quaternion.Euler(actualAngle, actualRotation, 0) * offset;
        transform.position = trackedLocation + rotatedOffset * actualDistanceModifier + cameraOffset + actualVerticalOffset*Vector3.up;
        transform.LookAt(trackedLocation + Quaternion.Euler(0, actualRotation, 0) * Vector3.forward * 1f);
    }
}
