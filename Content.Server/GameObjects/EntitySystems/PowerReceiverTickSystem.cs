using Content.Server.GameObjects.Components.Power.ApcNetComponents;
using Content.Server.GameObjects.Components.Power.PowerNetComponents;
using Robust.Shared.GameObjects.Systems;
using Robust.Shared.IoC;

namespace Content.Server.GameObjects.EntitySystems
{
    public class PowerReceiverTickSystem : EntitySystem
    {


        public override void Update(float frameTime)
        {
            base.Update(frameTime);
            foreach (var powerReceiver in ComponentManager.EntityQuery<PowerReceiverComponent>())
            {
                powerReceiver.Update(frameTime);
            }
        }
    }
}
