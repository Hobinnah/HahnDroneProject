using HahnDroneAPI.Profiles.Models;
using System.Collections.Generic;

namespace HahnDroneAPI.ViewModel
{
    public class ModelResponse
    {

        public IEnumerable<ModelDto> Models { get; set; }
        public int Count { get; set; }
    }
}
