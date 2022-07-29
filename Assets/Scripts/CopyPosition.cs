using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public Transform follow;
    public bool x = true;
    public bool y = true;
    public bool z = true;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = x ? follow.position.x : pos.x;
        pos.y = y ? follow.position.y : pos.y;
        pos.z = z ? follow.position.z : pos.z;
        transform.position = pos;
    }
}
