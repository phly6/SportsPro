namespace SportsPro.Models
{
	public class Customer
	{
		public int CustomerID { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }

		public string CountryID { get; set; }    // foreign key property
		public Country Country { get; set; } = null!;           // navigation property

		public string FullName => FirstName + " " + LastName;   // read-only property
	}
}