using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLauncher : MonoBehaviour
{
    [SerializeField] private float m_launchPower = 1f;

    public event System.Action onLaunch = null;

    private void OnTriggerStay(Collider other)
    {
        //  Quite an unoptimized way to detect the player falling on the launcher, but alas it's simple.
        if (!other.TryGetComponent(out Player player) || player.velocity.y >= 0) return;

        player.Launch(m_launchPower);
        onLaunch?.Invoke();
    }
}
