using System;

namespace AdminClient.ViewModels
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? LastUpdatedBy { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FatherFirstName { get; set; }
        public string? FatherMiddleName { get; set; }
        public string FatherLastName { get; set; }
        public string FatherMobile { get; set; }
        public string FatherEmail { get; set; }

        public string MotherFirstName { get; set; }
        public string? MotherMiddleName { get; set; }
        public string MotherLastName { get; set; }
        public string MotherMobile { get; set; }
        public string MotherEmail { get; set; }

        public string GuardianFirstName { get; set; }
        public string? GuardianMiddleName { get; set; }
        public string GuardianLastName { get; set; }
        public string GuardianMobile { get; set; }
        public string GuardianEmail { get; set; }

        public int? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? SchoolClassDivisionId { get; set; }

        public bool? FirstTimeLogin { get; set; }
        public bool IsPasswordChange { get; set; }
    }
}
