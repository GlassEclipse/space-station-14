using System;
using System.Linq;
using Content.Server.GameObjects.Components.Mobs;
using Content.Shared.GameObjects.Components.Mobs;
using Content.Shared.GameObjects.EntitySystems;
using Content.Shared.Maps;
using Robust.Server.GameObjects.EntitySystems;
using Robust.Server.Interfaces.GameObjects;
using Robust.Server.Interfaces.Player;
using Robust.Shared.GameObjects.EntitySystemMessages;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Map;
using Robust.Shared.Interfaces.Random;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Random;

namespace Content.Server.Explosions
{
    public static class EMPHelper
    {

        public static void SpawnEMP(this EntityCoordinates coords, int devastationRange, int heavyRange, int lightRange)
        {
            var tileDefinitionManager = IoCManager.Resolve<ITileDefinitionManager>();
            var serverEntityManager = IoCManager.Resolve<IServerEntityManager>();
            var entitySystemManager = IoCManager.Resolve<IEntitySystemManager>();
            var mapManager = IoCManager.Resolve<IMapManager>();
            var robustRandom = IoCManager.Resolve<IRobustRandom>();
            var entityManager = IoCManager.Resolve<IEntityManager>();

            var maxRange = MathHelper.Max(devastationRange, heavyRange, lightRange, 0f);

            //Call HandleEMP on all entities in range with IEMPAct
            var entitiesAll = serverEntityManager.GetEntitiesInRange(coords, maxRange).ToList();

            foreach (var entity in entitiesAll)
            {
                if (entity.Deleted)
                    continue;
                if (!entity.Transform.IsMapTransform)
                    continue;

                if (!entity.Transform.Coordinates.TryDistance(entityManager, coords, out var distance))
                {
                    continue;
                }

                EMPSeverity severity;
                if (distance < devastationRange)
                {
                    severity = EMPSeverity.Devastation;
                }
                else if (distance < heavyRange)
                {
                    severity = EMPSeverity.Heavy;
                }
                else if (distance < lightRange)
                {
                    severity = EMPSeverity.Light;
                }
                else
                {
                    continue;
                }
                var exAct = entitySystemManager.GetEntitySystem<ActSystem>();
                exAct.HandleEMP(coords, entity, severity);
            }

            //Effects and sounds
            var time = IoCManager.Resolve<IGameTiming>().CurTime;
            var playerManager = IoCManager.Resolve<IPlayerManager>();
             
            foreach (var player in playerManager.GetAllPlayers())
            {
                if (player.AttachedEntity != null && player.AttachedEntity.TryGetComponent(out ServerOverlayEffectsComponent overlayEffectsComponent) && mapManager.TryGetGrid(coords.GetGridId(entityManager), out var mapGrid) && player.AttachedEntity.Transform.MapID != mapGrid.ParentMapId){

                    var playerPos = player.AttachedEntity.Transform.WorldPosition;
                    var delta = coords.ToMapPos(entityManager) - playerPos;
                    var distance = delta.LengthSquared;

                    if (distance < 40) {
                        var container = new OverlayContainer(SharedOverlayID.FlashOverlay, new TimedOverlayParameter(2));
                        overlayEffectsComponent.AddOverlay(container);
                    } 
                }
            }

            entitySystemManager.GetEntitySystem<AudioSystem>().PlayAtCoords("/Audio/Effects/emp.ogg", coords);
        }

        public static void SpawnEMP(this IEntity entity, int devastationRange, int heavyRange,
            int lightRange)
        {
            entity.Transform.Coordinates.SpawnEMP(devastationRange, heavyRange, lightRange);
        }
    }
}
