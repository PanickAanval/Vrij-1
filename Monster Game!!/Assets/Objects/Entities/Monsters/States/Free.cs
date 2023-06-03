using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Joeri.Tools.Structure;

public abstract partial class Monster
{
    public class Free : State<Monster>
    {
        public override void OnTick(float deltaTime)
        {
            root.TickSubclass(deltaTime);
        }
    }
}