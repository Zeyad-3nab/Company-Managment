﻿namespace Company.Test.PL.ViewModels
{
	public class UserVM
	{
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
