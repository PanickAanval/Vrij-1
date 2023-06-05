using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private PlayerInstance m_playerInstance;
    [SerializeField] private Monster[] m_monsters;

    private void Start()
    {
        m_playerInstance.Setup();
        for (int i = 0; i < m_monsters.Length; i++) m_monsters[i].Setup(m_playerInstance.player);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        m_playerInstance.Tick(deltaTime);
        for (int i = 0; i < m_monsters.Length; i++) m_monsters[i].Tick(deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        m_playerInstance.DrawGizmos();
        for (int i = 0; i < m_monsters.Length; i++) m_monsters[i].DrawGizmos();
    }
}
