using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IGrabbable
{
    /// <summary>
    /// Called whenever the player has grabbed this object.
    /// </summary>
    public abstract void OnGrab(Player player);

    /// <summary>
    /// Called whenever the player has released this object.
    /// </summary>
    public abstract void OnRelease(Player player, Vector3 releaseVelocity);
}
