using System;
using System.Collections.Generic;
using Content.Client.GameObjects.Components.HyperCrypto;
using Content.Client.UserInterface.Stylesheets;
using Robust.Client.Graphics.Drawing;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Maths;
using Content.Shared.HyperCrypto;

namespace Content.Client.UserInterface.HyperCrypto
{
    public class HyperCryptoMenu : SS14Window
    {
        protected override Vector2? CustomSize => (720, 640);

        public HyperCryptoConsoleBoundUserInterface Owner { get; private set; }

        public HyperCryptoMenu(HyperCryptoConsoleBoundUserInterface owner)
        {
            IoCManager.InjectDependencies(this);
            Owner = owner;

            Title = Loc.GetString("HyperCrypto Uplink");

            


            var graphBox = new VBoxContainer();
            Contents.AddChild(graphBox);
            HyperCryptoGraph testGraph = new HyperCryptoGraph();
            graphBox.AddChild(testGraph);

            

            var infoBox = new HBoxContainer();
            Contents.AddChild(infoBox);
            infoBox.AddChild(new Label { Text = "PJBCoin" });
            infoBox.AddChild(new Label { Text = "34.29$" });
            infoBox.AddChild(new Label { Text = "-15.34%" });
        }

    }

    class HyperCryptoGraph : Control
    {
        public readonly Vector2i MinSize = new Vector2i(400, 350);

        private LineGraphData _data;
        private Vector2 _currentCursorPos = new Vector2(0, 0);

        public HyperCryptoGraph()
        {
            MouseFilter = MouseFilterMode.Pass;

            _data = new LineGraphData();
            double currentValue = 10.0;
            Random random = new Random();
            for (int i = 0; i < 500; i++)
            {
                currentValue += (random.NextDouble() - 0.5) * 0.2;
                _data.AddPoint(currentValue);
            }
        }

        public void UpdateState(LineGraphData data)
        {
            _data = data;
        }

        protected override Vector2 CalculateMinimumSize()
        {
            return (MinSize.X, MinSize.Y);
        }

        protected override void Draw(DrawingHandleScreen handle)
        {



            Box2 graphBounds = new Box2(new Vector2(0, 0), Size);
            //handle.DrawRect(new UIBox2(graphBounds.TopLeft, graphBounds.BottomRight), new Color(), true);
            handle.DrawRect(new UIBox2(graphBounds.TopLeft, graphBounds.BottomRight), Color.Black, false);

            List<double> points = _data.GetData();
            float xSize = graphBounds.Width / points.Count;
            int pointHover = (int) ((_currentCursorPos.X / graphBounds.Width) * points.Count);
            for (int i = 0; i < points.Count - 1; i++)
            {
                float startX = xSize * i;
                float endX = xSize * (i + 1);
                float startY = (float) (((points[i] - _data.LowestValue) / (_data.HighestValue - _data.LowestValue)) + 0.2f) * 0.66666f * graphBounds.Height;
                float endY = (float) (((points[i + 1] - _data.LowestValue) / (_data.HighestValue - _data.LowestValue)) + 0.2f )* 0.66666f * graphBounds.Height;
                handle.DrawLine(new Vector2(startX, startY), new Vector2(endX, endY), Color.Yellow);
                if (i == pointHover)
                {
                    handle.DrawCircle(new Vector2(startX, startY), 10, Color.Yellow);
                }
            }

        }

        protected override void MouseMove(GUIMouseMoveEventArgs args)
        {
            base.MouseMove(args);
            _currentCursorPos = args.RelativePixelPosition;
        }
    }

}
