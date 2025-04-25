using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float speed = 180;// speed in degrees

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -speed * Time.deltaTime));
    }
}
