﻿namespace E_Commerce.Core.Entities.Identity
{
	public class Address:BaseEntity
	{
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!; //Fk
        public ApplicationUser User { get; set; } = null!;
    }
}