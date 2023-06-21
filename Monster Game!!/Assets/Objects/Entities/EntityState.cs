using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

 public partial class Entity
{
    public class EntityState<Root> : FlexState<Root> where Root : Entity
    {
        public Settings settings { get => GetSettings<Settings>(); }

        public EntityState(Root root, Settings settings) : base(root, settings)
        {

        }

        public override void OnEnter()
        {
            root.SwitchAnimation(settings.animation);
        }

        [System.Serializable]
        public class Settings : FlexState<Root>.Settings
        {
            public AnimationClip animation;
        }
    }
}
