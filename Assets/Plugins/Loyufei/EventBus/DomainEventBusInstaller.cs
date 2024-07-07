using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei.DomainEvents
{
    [CreateAssetMenu(fileName = "DomainEventBusInstaller", menuName = "Loyufei/DomainEventBusInstaller")]
    public class DomainEventBusInstaller : ScriptableObjectInstaller<DomainEventBusInstaller>
    {
        [SerializeField]
        private string              _GroupId;
        [SerializeField]
        private List<BindableAsset> _Assets;

        public override void InstallBindings()
        {
            Container
                .DeclareSignal<IDomainEvent>()
                .WithId(_GroupId);

            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsCached()
                .WithArguments(_GroupId);

            _Assets.ForEach(a => a.BindToContainer(Container, _GroupId));
        }
    } }