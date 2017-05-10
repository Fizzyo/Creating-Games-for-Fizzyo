using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphRotationSnap : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    // some handy angles
    private float mitre = 45f;
    private float corner = 90f;

    // Use this for initialization
    void Start()
    {
        playerRigidBody = this.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rigidbodyRotation = playerRigidBody.transform.localEulerAngles.z;

        Vector3 newRotation = Vector3.zero;

        // Determine the rotation of the player and set rotation to cancel it out, snapped to 90 degrees

        if (rigidbodyRotation < mitre)
            newRotation.z = 0f;

        else if (rigidbodyRotation < mitre + corner)
            newRotation.z = corner;

        else if (rigidbodyRotation < mitre + corner * 2f)
            newRotation.z = 2 * corner;

        else if (rigidbodyRotation < mitre + corner * 3f)
            newRotation.z = 3 * corner;

        else
            newRotation.z = 0f;

        // set the euler rotation negatively
        this.transform.localEulerAngles = -newRotation;
    }
}
