#nullable enable
using Content.Shared.GameObjects.Components;
using Content.Shared.Maps;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.ViewVariables;

namespace Content.Shared.GameObjects.EntitySystems
{
    /// <summary>
    ///     Entity system backing <see cref="SubFloorHideComponent"/>.
    /// </summary>
    public class SubFloorHideSystem : EntitySystem
    {
        [Dependency] private readonly IMapManager _mapManager = default!;
        [Dependency] private readonly ITileDefinitionManager _tileDefinitionManager = default!;

        private bool _showAll;

        [ViewVariables(VVAccess.ReadWrite)]
        public bool ShowAll
        {
            get => _showAll;
            set
            {
                if (_showAll == value) return;
                _showAll = value;

                UpdateAll();
            }
        }

        private void UpdateAll()
        {
            foreach (var comp in EntityManager.ComponentManager.EntityQuery<SubFloorHideComponent>(true))
            {
                if (!_mapManager.TryGetGrid(comp.Owner.Transform.GridID, out var grid)) return;

                var snapPos = comp.Owner.GetComponent<SnapGridComponent>();
                UpdateTile(grid, snapPos.Position);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            IoCManager.InjectDependencies(this);

            _mapManager.GridChanged += MapManagerOnGridChanged;
            _mapManager.TileChanged += MapManagerOnTileChanged;

            SubscribeLocalEvent<SubFloorHideDirtyEvent>(HandleDirtyEvent);
        }

        private void HandleDirtyEvent(SubFloorHideDirtyEvent ev)
        {
            if (!_mapManager.TryGetGrid(ev.Sender.Transform.GridID, out var grid))
            {
                return;
            }

            var indices = grid.WorldToTile(ev.Sender.Transform.WorldPosition);
            UpdateTile(grid, indices);
        }

        private void MapManagerOnTileChanged(object? sender, TileChangedEventArgs e)
        {
            UpdateTile(_mapManager.GetGrid(e.NewTile.GridIndex), e.NewTile.GridIndices);
        }

        private void MapManagerOnGridChanged(object? sender, GridChangedEventArgs e)
        {
            foreach (var modified in e.Modified)
            {
                UpdateTile(e.Grid, modified.position);
            }
        }

        private void UpdateTile(IMapGrid grid, Vector2i position)
        {
            var tile = grid.GetTileRef(position);
            var tileDef = (ContentTileDefinition) _tileDefinitionManager[tile.Tile.TypeId];
            foreach (var snapGridComponent in grid.GetSnapGridCell(position, SnapGridOffset.Center))
            {
                var entity = snapGridComponent.Owner;
                if (!entity.TryGetComponent(out SubFloorHideComponent? subFloorComponent))
                {
                    continue;
                }

                // Show sprite
                if (entity.TryGetComponent(out SharedSpriteComponent? spriteComponent))
                {
                    spriteComponent.Visible = ShowAll || !subFloorComponent.Running || tileDef.IsSubFloor;
                }

                // So for collision all we care about is that the component is running.
                if (entity.TryGetComponent(out PhysicsComponent? physicsComponent))
                {
                    physicsComponent.CanCollide = !subFloorComponent.Running;
                }
            }
        }
    }
}
