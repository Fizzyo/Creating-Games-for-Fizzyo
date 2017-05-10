using UnityEngine;

public class LerpedFollow : MonoBehaviour
{
    public GameObject target;
    public float zPosition = -10f;
    public float lerpSpeed = 6f;
    public Vector3 Offset = Vector3.zero;
    Vector3 targetPos;

    // Update is called once per frame
    void LateUpdate()
    {
        targetPos = target.transform.position + Offset;

        Vector3 newPos = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * lerpSpeed);

        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
