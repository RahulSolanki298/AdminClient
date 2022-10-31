using System;
using System.ComponentModel.DataAnnotations;

namespace AdminClient.ViewModels
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string SchoolAddress { get; set; }
        [Required]
        public string SchoolCity { get; set; }
        [Required]
        public string SchoolState { get; set; }
        [Required]
        public string SchoolPinCode { get; set; }
        [Required]
        public string SchoolPhone { get; set; }
        [Required]
        public string  AcademicYear { get; set; }

        public string? SchoolCode { get; set; }
        public DateTime? EstablishmentDate { get; set; }

        public DateTime? AcademicStartDate { get; set; }

        public DateTime? AcademicEndDate { get; set; }

        public string? GeoLocation { get; set; }

        public int? SchoolOwnerId { get; set; }

        public int? SchoolPrincipleId { get; set; }

        public int? SchoolCoordinatorId { get; set; }

        public string? SchoolLogo1Path { get; set; }
        public string? SchoolLogo2Path { get; set; }

        public string? DashboardPicPath { get; set; }

        //public string AdminPhone { get; set; }
        
        public string? SchoolEmail { get; set; }
        public string? SchoolWebsite { get; set; }
        public string? SchoolTrustName { get; set; }

        public string? SchoolPAN { get; set; }

        public string? SchoolGSTNo { get; set; }

        public int IsActive { get; set; }

        
    }
}