using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;

public class DustCloud : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().duration);
    }
}
