using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> Items)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            };
            List.Add(sli);
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    sli = new SelectListItem
                    {
                        Text = item.GetPropertyValue("Name"),
                        Value = item.GetPropertyValue("Id"),
                    };
                    List.Add(sli);
                }
            }
            return List;
        }
    }
}
