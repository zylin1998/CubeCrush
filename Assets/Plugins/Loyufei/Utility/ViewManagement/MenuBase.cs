using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Loyufei.ViewManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuBase : ViewMono
    {
        [SerializeField]
        protected CanvasGroup _CanvasGroup;
        [SerializeField, Range(0f, 1f)]
        protected float       _FadeDuration = 0.5f;

        public override object ViewId { get; }

        public override Tween Open()
        {
            gameObject.SetActive(true);

            _CanvasGroup.alpha = 0;

            return _CanvasGroup.DOFade(1f, _FadeDuration);
        }

        public override Tween Close()
        {
            _CanvasGroup.alpha = 1;

            return _CanvasGroup
                    .DOFade(0f, _FadeDuration)
                    .OnComplete(() => gameObject.SetActive(false)); ;
        }

        public override IEnumerator<IListenerAdapter> GetEnumerator()
        {
            return GetComponentsInChildren<IListenerAdapter>()
                    .To<IEnumerable<IListenerAdapter>>()
                    .GetEnumerator();
        }
    }
}