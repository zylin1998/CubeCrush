using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.DomainEvents
{
    public interface IDomainEvent
    {
        public float  InvokeTime   { get; }
        
        #region Static Properties

        public static float         DefaultTime   
        {
            get 
            {
                return Time.realtimeSinceStartup;
            } 
        }

        #endregion
    }

    public class DomainEventBase : IDomainEvent 
    {
        public float  InvokeTime   { get; }

        public DomainEventBase() 
            : this(IDomainEvent.DefaultTime)
        {
            
        }

        public DomainEventBase(float invokeTime)
        {
            InvokeTime   = invokeTime;
        }
    }
}