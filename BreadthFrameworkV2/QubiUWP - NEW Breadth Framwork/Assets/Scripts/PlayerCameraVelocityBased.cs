using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraVelocityBased : MonoBehaviour
{
    public GameObject target;
    public float zPosition = -10f;
    public float lerpSpeed = 6f;
    public Vector3 Offset = Vector3.zero;
    public float NegativeVelocityMultiplier = 4f;
    Vector3 targetPos;
    private Rigidbody2D playerRigidBody;

    private void Start()
    {
        playerRigidBody = target.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveTarget();
        SetCameraPosition();
    }

    void MoveTarget()
    {
        float VelocityOffset = 0f;

        if (playerRigidBody.velocity.y < 0f)
        {
            VelocityOffset = playerRigidBody.velocity.y * NegativeVelocityMultiplier;
        }

        targetPos = target.transform.position + Offset + Vector3.up * VelocityOffset;
    }

    void SetCameraPosition()
    {
        Vector3 newPos = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * lerpSpeed);

        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
