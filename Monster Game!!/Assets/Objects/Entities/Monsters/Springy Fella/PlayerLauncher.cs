using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLauncher : MonoBehaviour
{
    [SerializeField] private float m_launchPower = 1f;

    public event System.Action onLaunch = null;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player player) || player.velocity.y >= 0) return;

        player.Launch(m_launchPower);
        onLaunch?.Invoke();
    }
}
