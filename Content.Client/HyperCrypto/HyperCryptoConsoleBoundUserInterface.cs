using Content.Client.UserInterface.HyperCrypto;
using Content.Shared.GameObjects.Components.HyperCrypto;
using Robust.Client.GameObjects.Components.UserInterface;
using Robust.Shared.GameObjects.Components.UserInterface;
using Robust.Shared.ViewVariables;

namespace Content.Client.GameObjects.Components.HyperCrypto
{
    public class HyperCryptoConsoleBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private HyperCryptoMenu _menu;

        public HyperCryptoConsoleBoundUserInterface(ClientUserInterfaceComponent owner, object uiKey) : base(owner, uiKey)
        {

        }

        protected override void Open()
        {
            base.Open();
            _menu = new HyperCryptoMenu(this);
            _menu.OpenCentered();
            _menu.OnClose += Close;
            //SendMessage(new HyperCryptoRequestUpdateMessage());
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            if (!(state is HyperCryptoConsoleInterfaceState cryptoState))
                return;

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;

            _menu?.Dispose();
        }
        

    }




}
