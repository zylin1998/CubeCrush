using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Loyufei
{
    public interface IFacade
    {
        
    }

    public class FacadeFactory : PlaceholderFactory<Type, IFacade> 
    {
        public TFacade Create<TFacade>() where TFacade : IFacade 
        {
            return Create(typeof(TFacade)).To<TFacade>();
        }
    }
}