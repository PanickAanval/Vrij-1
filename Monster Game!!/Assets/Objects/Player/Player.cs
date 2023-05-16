using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Transform m_camera = null;

    private void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var movementDir = Quaternion.Euler(0f, m_camera.eulerAngles.y, 0f) * new Vector3(input.x, 0f, input.y);

        transform.position += movementDir * (speed * Time.deltaTime);
    }
}
