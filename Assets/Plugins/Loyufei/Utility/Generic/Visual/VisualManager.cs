using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public class VisualManager
    {
        public VisualManager() 
        {
            Groups = new();
        }

        public VisualManager(IEnumerable<VisualGroup> groups)
        {
            Groups = groups.ToDictionary(key => key.Identity);
        }

        public Dictionary<object, VisualGroup> Groups { get; }
    }
}