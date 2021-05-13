using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Models.ViewModels
{
    public class HoneyViewModel
    {
        public Honey Honey { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }

        private List<Currency> CList = new List<Currency>();
        private List<Currency> CreateList()
        {
            CList.Add(new Currency("EUR", "EUR"));
            CList.Add(new Currency("LEI", "LEI"));
            CList.Add(new Currency("USD", "USD"));
            return  CList;
        }

        public HoneyViewModel()
        {
            Currencies = CreateList();
        }

        public class Currency
        {
            public string Id { get; set; }
            public string  Name { get; set; }
            public Currency(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }


    }
}
