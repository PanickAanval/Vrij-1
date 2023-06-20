using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerInstance : MonoBehaviour
{
    [SerializeField] private Player m_player;
    [SerializeField] private Camera m_camera;
    [SerializeField] private PlayerShadow m_shadow;

    private Controls m_controls = new Controls();

    public Player player { get => m_player; }

    public void Setup()
    {
        m_player.Setup();
        m_camera.Setup(m_player.center);
        m_shadow.Setup(player.controller);
    }

    public void Tick(float deltaTime)
    {
        var input = m_controls.GetInput();

        m_player.Tick(input, deltaTime, -m_camera.transform.eulerAngles.y);
        m_camera.Tick(-input.rightInput, m_player.flatVelocity, deltaTime);
        m_shadow.Tick(m_player.transform.position, m_player.movement.onGround);
    }

    public void DrawGizmos()
    {
        m_camera.DrawGizmos();
        m_player.DrawGizmos();
    }
}
