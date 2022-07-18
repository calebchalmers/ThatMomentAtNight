using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 velocity;

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }
}
