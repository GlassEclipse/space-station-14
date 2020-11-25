using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Robust.Shared.GameObjects.Systems;

namespace Content.Server.HyperCrypto
{

    [UsedImplicitly]
    class HyperCryptoSystem : EntitySystem
    {
        public static float HyperCryptoUpdateInterval = 0.5f;

        private (string, string)[] _cryptoNames = new (string, string)[]{
            ("Buttcoin", "BTC"),
            ("Beethereum","BEETH"),
            ("PJBCoin", "PJB"),
            ("UnityCoin","UNI"),
            ("HonkLite", "HONK"),
            ("Yellow Glove", "GLUB"),
            ("Securicoin", "VALID"),
            ("Singularity Cash", "LOOSE"),
            ("NotAScamUltra", "LEGIT"),
            ("Adamantium", "ADMNT"),
            ("CreampieUltra", "CREAM"),
            ("Xonotia", "XONOT"),
            ("Oranges Token", "NYA"),
        };

        private List<HyperCryptoCoin> _activeCoins = new List<HyperCryptoCoin>();

        public override void Initialize()
        {
            base.Initialize();
            for (int i = 0; i < 10; i++)
            {
                GenerateRandomCoin();
            }
        }

        public override void Update(float frameTime)
        {
            foreach (HyperCryptoCoin coin in _activeCoins)
            {
                coin.Tick(frameTime);
            }
        }

        private HyperCryptoCoin GenerateRandomCoin()
        {
            int attempts = 0;
            Random random = new Random();
            while (true) { 
                (string, string) newName = _cryptoNames[random.Next(_cryptoNames.Length)];
                if (NoCoinOfNameExists(newName.Item2)) {
                    attempts++;
                    if (attempts > 1000) //Look it works just fine
                        return null;
                }
                else {
                    var newCoin = new HyperCryptoCoin(newName.Item1, newName.Item2);
                    _activeCoins.Add(newCoin);
                    return newCoin;
                }
            }
        }

        private bool NoCoinOfNameExists(string shortName) {
            foreach (var coin in _activeCoins)
            {
                if (coin.ShortName == shortName)
                    return false;
            }
            return true;
        }
    }
}
