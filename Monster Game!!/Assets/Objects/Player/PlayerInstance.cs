using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Player m_player;

    private void Start()
    {
        m_camera.Setup(m_player.center);
        m_player.Setup();
    }

    private void Update()
    {
        var leftInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) leftInput.y++;
        if (Input.GetKey(KeyCode.S)) leftInput.y--;
        if (Input.GetKey(KeyCode.A)) leftInput.x--;
        if (Input.GetKey(KeyCode.D)) leftInput.x++;

        var rightInput = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) rightInput.y++;
        if (Input.GetKey(KeyCode.DownArrow)) rightInput.y--;
        if (Input.GetKey(KeyCode.LeftArrow)) rightInput.x--;
        if (Input.GetKey(KeyCode.RightArrow)) rightInput.x++;

        m_player.Tick(leftInput, Time.deltaTime, -m_camera.transform.eulerAngles.y);
        m_camera.Tick(rightInput, m_player.horizontalVelocity, Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        m_camera.DrawGizmos();
        m_player.DrawGizmos();
    }
}
