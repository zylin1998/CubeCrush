using System.Collections;
using System.Collections.Generic;

namespace Loyufei 
{
    public static class VisualManagerExtensions
    {
        public static void Register(this VisualManager self, string groupName, IVisual visual)
        {
            var groups = self.Groups;
            var match  = groups.TryGetValue(groupName, out var group);

            if (match) { group.Add(visual); }

            else { groups.Add(groupName, new(groupName, visual)); }
        }

        public static void Refresh(this VisualManager self)
        {
            var groups = self.Groups;

            groups.Values.ForEach(group =>
            {
                group.ForEach(visual => visual.Refresh());
            });
        }

        public static void Refresh(this VisualManager self, string groupName)
        {
            var groups  = self.Groups;
            var visuals = groups.GetorReturn(groupName, () => new());

            visuals.ForEach(visual => visual.Refresh());
        }

        public static void Refresh<TVisual>(this VisualManager self, string groupName) where TVisual : IVisual
        {
            var groups = self.Groups;
            var group  = groups.GetorReturn(groupName, () => new());
            var visual = group.Visuals.GetorReturn(typeof(TVisual), () => IVisual.Default);

            visual.Refresh();
        }

        public static void Enable<TVisual>(this VisualManager self, string groupName) where TVisual : IVisual
        {
            var groups = self.Groups;
            var group  = groups.GetorReturn(groupName, () => new());
            var visual = group.Visuals.GetorReturn(typeof(TVisual), () => IVisual.Default);

            visual.Enable = true;
        }

        public static void EnableOnly<TVisual>(this VisualManager self, string groupName) where TVisual : IVisual
        {
            var groups = self.Groups;
            var group  = groups.GetorReturn(groupName, () => new());

            group.Visuals.ForEach(pair =>
            {
                pair.Value.Enable = pair.Key == typeof(TVisual);
            });
        }

        public static void Disable<TVisual>(this VisualManager self, string groupName) where TVisual : IVisual
        {
            var groups = self.Groups;
            var group  = groups.GetorReturn(groupName, () => new());
            var visual = group.Visuals.GetorReturn(typeof(TVisual), () => IVisual.Default);

            visual.Enable = false;
        }
    }
}