using Content.Shared.GameObjects.Components.Mobs;
using Content.Shared.Interfaces;
using Robust.Client.Graphics;
using Robust.Client.Graphics.Drawing;
using Robust.Client.Graphics.Overlays;
using Robust.Client.Graphics.Shaders;
using Robust.Client.Interfaces.Graphics;
using Robust.Client.Interfaces.Graphics.ClientEye;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;

namespace Content.Client.Graphics.Overlays
{
    public class EMPOverlay : Overlay, IConfigurable<TimedOverlayParameter>
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IClyde _displayManager = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IEyeManager _eyeManager = default!;

        public override OverlaySpace Space => OverlaySpace.ScreenSpace;
        private readonly ShaderInstance _shader;
        private double _startTime;
        private int _duration = 5000;
        private Texture _screenshotTexture;

        public EMPOverlay() : base(nameof(SharedOverlayID.EMPOverlay))
        {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("EMPShockwave").Instance().Duplicate();
            _startTime = _gameTiming.CurTime.TotalMilliseconds;
        }

        protected override void Draw(DrawingHandleBase handle, OverlaySpace currentSpace)
        {
            handle.UseShader(_shader);
            var percentComplete = (float) ((_gameTiming.CurTime.TotalMilliseconds - _startTime) / _duration);

            _shader?.SetParameter("radius", 6);
            _shader?.SetParameter("intensity", 30);
            _shader?.SetParameter("center", new Vector2(0,0));
            //_shader?.SetParameter("SCREEN_TEXTURE", screen_texture);

            var worldHandle = (DrawingHandleWorld) handle;
            var viewport = _eyeManager.GetWorldViewport();
            worldHandle.DrawRect(viewport, Color.White);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _screenshotTexture = null;
        }

        public void Configure(TimedOverlayParameter parameters)
        {
            _duration = parameters.Length;
        }
    }
}
