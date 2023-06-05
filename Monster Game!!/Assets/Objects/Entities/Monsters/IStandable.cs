using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IStandable
{
    /// <summary>
    /// Called when an entity stands on a script with the IStandable interface. For now only the Player's state machine uses this.
    /// </summary>
    /// <param name="entity">The entity standing on the IStandable.</param>
    public abstract void OnStand(Entity entity);
}