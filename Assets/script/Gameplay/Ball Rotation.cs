using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotation : MonoBehaviour
{
    private float torqueMultiplier = 0.005f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Get the contact point of the collision
        ContactPoint contact = collision.contacts[0];

        // Calculate the direction of the torque based on the contact point and velocity
        Vector3 torqueDirection = Vector3.Cross(contact.normal, rb.velocity).normalized;

        // Apply the torque to the ball
        rb.AddTorque(torqueDirection * rb.velocity.magnitude * torqueMultiplier, ForceMode.Impulse);
    }

    void OnCollisionStay(Collision collision)
    {
        // Continuously apply torque while the ball is in contact with a surface
        ContactPoint contact = collision.contacts[0];
        Vector3 torqueDirection = Vector3.Cross(contact.normal, rb.velocity).normalized;
        rb.AddTorque(torqueDirection * rb.velocity.magnitude * torqueMultiplier, ForceMode.Force);
    }
}
