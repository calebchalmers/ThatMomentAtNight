using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public Transform follow;

    void LateUpdate()
    {
        transform.position = follow.position;
    }
}
