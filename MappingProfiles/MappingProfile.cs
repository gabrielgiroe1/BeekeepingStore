using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeekeepingStore.Controllers.Resources;
using BeekeepingStore.Models;

namespace BeekeepingStore.MappingProfiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Model, ModelResources>();
            CreateMap<Make, ModelResources>();
        }
    }
}
