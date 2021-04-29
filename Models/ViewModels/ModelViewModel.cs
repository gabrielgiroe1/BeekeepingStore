using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Models.ViewModels
{
    public class ModelViewModel
    {
        public Model Model { get; set; }
        public IEnumerable<Make> Makes { get; set; }

        public IEnumerable<SelectListItem> CSelectListItem(IEnumerable<Make> Items)
        {
            List<SelectListItem> MakeList = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            };
            MakeList.Add(sli);
            if (Items != null)
            {
                foreach (Make make in Items)
                {
                    sli = new SelectListItem
                    {
                        Text = make.Name,
                        Value = make.Id.ToString()
                    };
                    MakeList.Add(sli);
                }
            }
            return MakeList;
        }

    }
}
