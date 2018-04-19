using System;
using System.Collections.Generic;
using System.Text;

namespace AprioritWebCalendar.ViewModel.Event
{
    // TODO: Validation.
    public class LocationViewModel
    {
        public double? Longitude { get; set; }
        public double? Lattitude { get; set; }
        public string Description { get; set; }
    }
}
