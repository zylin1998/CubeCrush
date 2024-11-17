using Loyufei.DomainEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeCrush
{
    public class Presenter : Loyufei.MVP.Presenter
    {
        public override object GroupId => Declarations.CubeCrush;
    }
}