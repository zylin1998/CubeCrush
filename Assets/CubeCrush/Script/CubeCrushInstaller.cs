using UnityEngine;
using Zenject;
using Loyufei;
using Loyufei.DomainEvents;

namespace CubeCrush
{
    public class CubeCrushInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _MapOffset;
        [SerializeField]
        private GameObject _Cube;

        public override void InstallBindings()
        {
            #region Factory

            Container
                .BindMemoryPool<MapOffset, MapOffset.Pool>()
                .FromComponentInNewPrefab(_MapOffset)
                .AsCached();

            Container
                .BindMemoryPool<Cube, Cube.Pool>()
                .FromComponentInNewPrefab(_Cube)
                .AsCached();

            #endregion

            #region Event

            SignalBusInstaller.Install(Container);

            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsSingle();

            #endregion

            #region Data Structure

            Container
                .Bind<CubeGrid>()
                .AsSingle();

            Container
                .Bind<CubeGridMetrix>()
                .AsSingle();

            Container
                .Bind<CubeGridQuery>()
                .AsSingle();

            Container
                .Bind<Report>()
                .AsSingle();

            #endregion

            #region Model

            Container
                .Bind<DataUpdater>()
                .AsSingle();

            Container
                .Bind<CubeCrushModel>()
                .AsSingle();

            #endregion

            #region Presenter

            Container
                .Bind<GridViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<InfoViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<GameOverViewPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<CubeCrushPresenter>()
                .AsSingle()
                .NonLazy();

            #endregion

            #region Verified

            Container
                .Bind<GridVerify>()
                .AsSingle();

            #endregion
        }
    } 
}