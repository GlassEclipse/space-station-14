#nullable enable
using Content.Server.Cargo;
using Content.Server.GameObjects.Components.Power.ApcNetComponents;
using Content.Server.GameObjects.EntitySystems;
using Content.Server.Utility;
using Content.Shared.GameObjects.Components.Cargo;
using Content.Shared.GameObjects.Components.HyperCrypto;
using Content.Shared.Interfaces.GameObjects.Components;
using Content.Shared.Prototypes.Cargo;
using Robust.Server.GameObjects.Components.UserInterface;
using Robust.Server.Interfaces.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Systems;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Serialization;
using Robust.Shared.ViewVariables;

namespace Content.Server.GameObjects.Components.HyperCrypto
{
    [RegisterComponent]
    [ComponentReference(typeof(IActivate))]
    public class HyperCryptoConsoleComponent : Component, IActivate
    {
        public sealed override string Name => "HyperCryptoConsole";

        [ViewVariables] private BoundUserInterface? UserInterface => Owner.GetUIOrNull(HyperCryptoConsoleUiKey.Key);
        [ViewVariables] private bool Powered => !Owner.TryGetComponent(out PowerReceiverComponent? receiver) || receiver.Powered;

        public override void Initialize()
        {
            base.Initialize();

            if (UserInterface != null)
            {
                UserInterface.OnReceiveMessage += UserInterfaceOnOnReceiveMessage;
            }
        }

        public override void OnRemove()
        {
            if (UserInterface != null)
            {
                UserInterface.OnReceiveMessage += UserInterfaceOnOnReceiveMessage;
            }

            base.OnRemove();
        }

        void IActivate.Activate(ActivateEventArgs eventArgs)
        {
            if (!eventArgs.User.TryGetComponent(out IActorComponent? actor))
                return;
            if (!Powered)
                return;

            UserInterface?.Open(actor.playerSession);
        }

        private void UserInterfaceOnOnReceiveMessage(ServerBoundUserInterfaceMessage serverMsg)
        {
            var message = serverMsg.Message;
            if (!Powered)
                return;
            switch (message)
            {

            }
        }



        private void UpdateUIState()
        {
            if (!Owner.IsValid())
                return;

            UserInterface?.SetState(new HyperCryptoConsoleInterfaceState());
        }
    }
}
