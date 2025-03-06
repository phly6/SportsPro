using System.Collections.Generic;
using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentFormViewModel
    {
        public Incident? Incident { get; set; }
        public List<Customer> Customers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Technician> Technicians { get; set; } = new();
        public string FormMode { get; set; } = "Add"; // "Add" or "Edit"
    }
}
