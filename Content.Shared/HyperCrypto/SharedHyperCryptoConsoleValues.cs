using System;
using Robust.Shared.GameObjects;
using Robust.Shared.GameObjects.Components.UserInterface;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.GameObjects.Components.HyperCrypto
{


    [NetSerializable, Serializable]
    public class HyperCryptoConsoleInterfaceState : BoundUserInterfaceState
    {


        public HyperCryptoConsoleInterfaceState()
        {

        }
    }


    [Serializable, NetSerializable]
    public class HyperCryptoRequestUpdateMessage : BoundUserInterfaceMessage
    {
        public HyperCryptoRequestUpdateMessage(){ }
    }



    [Serializable, NetSerializable]
    public enum HyperCryptoConsoleUiKey
    {
        Key
    }
}
