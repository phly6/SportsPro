﻿using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
	public class Incident
	{
		public int IncidentID { get; set; }

		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; } = string.Empty;

		[Required(ErrorMessage = "Description is required.")]
		public string Description { get; set; } = string.Empty;

		[Display(Name = "Date Opened")]
		public DateTime DateOpened { get; set; } = DateTime.Now;

		[Display(Name = "Date Closed")]
		public DateTime? DateClosed { get; set; } = null;

		[Required(ErrorMessage = "Customer selection is required.")]
		[Display(Name = "Customer")]
		public int CustomerID { get; set; }                   // Foreign key property

		public Customer? Customer { get; set; } = null;       // Nullable navigation property

		[Display(Name = "Product")]
		[Required(ErrorMessage = "Product selection is required.")]
		public int ProductID { get; set; }                    // Foreign key property

		public Product? Product { get; set; } = null;         // Nullable navigation property

		[Required(ErrorMessage = "Technician selection is required.")]
		[Display(Name = "Technician")]
		public int TechnicianID { get; set; }                 // Foreign key property 

		public Technician? Technician { get; set; } = null;   // Nullable navigation property

	}
}
