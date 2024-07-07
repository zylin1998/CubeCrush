using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei 
{
    public class VisualGroup : IEnumerable<IVisual>, IIdentity
    {
        #region Constructor

        public VisualGroup()
        {
            Identity = string.Empty;
            Visuals = new Dictionary<Type, IVisual>();
        }

        public VisualGroup(string name, params IVisual[] visuals)
        {
            Identity = name;
            Visuals = visuals.ToDictionary(key => key.GetType());
        }

        public VisualGroup(string name, IEnumerable<IVisual> visuals)
        {
            Identity = name;
            Visuals = visuals.ToDictionary(key => key.GetType());
        }

        #endregion

        public object Identity { get; }

        public Dictionary<Type, IVisual> Visuals { get; }

        public void Add(IVisual visual)
        {
            if (Visuals.ContainsKey(visual.GetType())) { return; }

            Visuals.Add(visual.GetType(), visual);
        }

        public bool Remove(IVisual visual)
        {
            var type = visual.GetType();
            var v = Visuals.GetorReturn(type, () => IVisual.Default);

            return visual.IsEqual(v) ? Visuals.Remove(type) : false;
        }

        public IEnumerator<IVisual> GetEnumerator()
            => Visuals.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
