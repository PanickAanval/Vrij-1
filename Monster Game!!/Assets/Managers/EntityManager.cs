using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private PlayerInstance m_playerInstance;
    [SerializeField] private SpringyFella m_monster;

    private void Start()
    {
        m_playerInstance.Setup();
        m_monster.Setup(m_playerInstance.player);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        m_playerInstance.Tick(deltaTime);
        m_monster.Tick(deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        m_playerInstance.DrawGizmos();
        m_monster.DrawGizmos();
    }
}
