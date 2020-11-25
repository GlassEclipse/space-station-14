using System;

namespace Content.Server.HyperCrypto
{
    class HyperCryptoCoin
    {
        public string Name { get; }
        public string ShortName { get; }
        public double CurrentValue => _currentValue;
        private double _currentValue;

        private float _varianceFactor;
        private float _lifespan;
        private float _timeSinceTick = 0f;


        public HyperCryptoCoin(string name, string shortName)
        {
            Name = name;
            ShortName = ShortName;
            _currentValue = 1.0;
        }

        public void Tick(float tickTime)
        {
            _timeSinceTick += tickTime;
            if (_timeSinceTick > HyperCryptoSystem.HyperCryptoUpdateInterval)
            {
                _timeSinceTick -= HyperCryptoSystem.HyperCryptoUpdateInterval;
                TickBehavior();
            }
        }

        private void TickBehavior()
        {
            Random random = new Random();
            _currentValue += (random.NextDouble()-0.5)*0.2*HyperCryptoSystem.HyperCryptoUpdateInterval;
        }
    }


}
