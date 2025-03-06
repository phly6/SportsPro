using System.Collections.Generic;
using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentManagerViewModel
    {
        public List<Incident> Incidents { get; set; } = new(); // Initialize the list
        public string Filter { get; set; } = "All"; // Default to show all incidents
    }
}
