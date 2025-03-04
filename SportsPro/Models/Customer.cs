using System.ComponentModel.DataAnnotations;
namespace SportsPro.Models
{
	public class Customer
	{
		public int CustomerID { get; set; }

		[Required(ErrorMessage = "First name is required.")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Last name is required.")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Address is required.")]
		public string Address { get; set; } = string.Empty;

		[Required(ErrorMessage = "City is required.")]
		public string City { get; set; } = string.Empty;

		[Required(ErrorMessage = "State is required.")]
		public string State { get; set; } = string.Empty;

		[Required(ErrorMessage = "Postal Code is required.")]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; } = string.Empty;
		public string? Phone { get; set; }
		public string? Email { get; set; }
		[Required(ErrorMessage = "Country is required.")]
		[Display(Name = "Country")]
		public string CountryID { get; set; } = string.Empty;    // foreign key property
		public Country Country { get; set; } = new Country();           // navigation property

		public string FullName => FirstName + " " + LastName;   // read-only property
	}
}