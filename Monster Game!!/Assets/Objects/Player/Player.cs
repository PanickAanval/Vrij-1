using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;
public class Player : MonoBehaviour
{
    public float speed;
    public float grip;
    public Transform m_camera = null;

    private AccelerationLogic.Flat m_acceleration = new AccelerationLogic.Flat();

    public Vector3 velocity { get => Calc.FlatToVector(m_acceleration.velocity); }
    public Vector2 horizontalVelocity { get => m_acceleration.velocity; }

    private void Update()
    {
        var input = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) input.y++;
        if (Input.GetKey(KeyCode.S)) input.y--;
        if (Input.GetKey(KeyCode.A)) input.x--;
        if (Input.GetKey(KeyCode.D)) input.x++;

        //  We rotate the input vector by the angle of the camera, so that the player moves forward in relation to the camera at all times.
        //  For now, the angle by which we rotate is negative. I have yet to find out why it rotates the wrong way.
        input = Calc.RotateVector2(input, -m_camera.eulerAngles.y);
        transform.position += m_acceleration.Get3DVelocity(input, speed, grip, Time.deltaTime) * Time.deltaTime;
    }
}
