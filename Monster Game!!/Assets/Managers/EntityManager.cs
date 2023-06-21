using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private PlayerInstance m_playerInstance;
    [SerializeField] private Transform m_monsterParent;

    private Monster[] m_monsters = null;

    public void Setup()
    {
        m_monsters = m_monsterParent.GetComponentsInChildren<Monster>();

        m_playerInstance.Setup();
        for (int i = 0; i < m_monsters.Length; i++) m_monsters[i].Setup(m_playerInstance.player);
    }

    public void Tick(float deltaTime)
    {
        m_playerInstance.Tick(deltaTime);
        for (int i = 0; i < m_monsters.Length; i++) m_monsters[i].Tick(deltaTime);
    }

    public void DrawGizmos()
    {
        if (!Application.isPlaying) return;

        m_playerInstance.DrawGizmos();
        for (int i = 0; i < m_monsters.Length; i++) m_monsters[i].DrawGizmos();
    }
}
