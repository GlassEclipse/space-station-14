using Content.Server.Explosions;
using Content.Server.GameObjects.EntitySystems;
using Content.Shared.GameObjects.EntitySystems;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Server.GameObjects.Components.Explosion
{
    [RegisterComponent]
    public class EMPExplosiveComponent : Component, ITimerTrigger
    {
        public override string Name => "EMPExplosive";

        public int DevastatingEMPRange = 0;
        public int HeavyEMPRange = 0;
        public int LightEMPRange = 0;

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            serializer.DataField(ref DevastatingEMPRange, "devastationRange", 0);
            serializer.DataField(ref HeavyEMPRange, "heavyRange", 0);
            serializer.DataField(ref LightEMPRange, "lightRange", 0);
        }

        private bool DetonateEMP()
        {
            Owner.SpawnEMP(DevastatingEMPRange, HeavyEMPRange, LightEMPRange);
            Owner.Delete();
            return true;
        }

        bool ITimerTrigger.Trigger(TimerTriggerEventArgs eventArgs)
        {
            return DetonateEMP();
        }
    }
}
