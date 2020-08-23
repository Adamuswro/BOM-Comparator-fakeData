using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels.Helpers
{
    internal static class FilterHelper
    {
        internal static void CreatePropertyViewModel(IList<PropertyViewModel> filterProperty, IEnumerable<string> input)
        {
            filterProperty?.Clear();
            foreach (var property in input)
            {
                filterProperty.Add(new PropertyViewModel() { Property = property, IsSelected = true });
            }
            filterProperty.OrderBy(p => p.Property);
        }
    }
}
