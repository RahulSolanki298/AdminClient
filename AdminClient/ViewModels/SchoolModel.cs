using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminClient.ViewModels
{
    public class SchoolModel
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolState { get; set; }
        public string SchoolPinCode { get; set; }
        public string SchoolPhone { get; set; }

        public string AcademicYear { get; set; }

        public string? SchoolCode { get; set; }
        public DateTime? EstablishmentDate { get; set; }

        public DateTime? AcademicStartDate { get; set; }

        public DateTime? AcademicEndDate { get; set; }

        public string? GeoLocation { get; set; }

        public string SchoolOwnerFirstName { get; set; }

        public string SchoolOwnerMiddelName { get; set; }

        public string SchoolOwnerLastName { get; set; }

        public string SchoolOwnerEmail { get; set; }

        public string SchoolOwnerPhone { get; set; }


        public string SchoolPrincipleFirstName { get; set; }
        public string SchoolPrincipleMiddelName { get; set; }

        public string SchoolPrincipleLastName { get; set; }

        public string SchoolPrincipleEmail { get; set; }

        public string SchoolPrinciplePhone { get; set; }

        public string? SchoolCoordinatorFirstName { get; set; }

        public string? SchoolCoordinatorMiddelName { get; set; }

        public string? SchoolCoordinatorLastName { get; set; }

        public string? SchoolCoordinatorEmail { get; set; }

        public string? SchoolCoordinatorPhone { get; set; }

        public string? SchoolLogo1Path { get; set; }
        public string? SchoolLogo2Path { get; set; }

        public string? DashboardPicPath { get; set; }

        //public string AdminPhone { get; set; }

        public string? SchoolEmail { get; set; }
        public string? SchoolWebsite { get; set; }
        public string? SchoolTrustName { get; set; }

        public string? SchoolPAN { get; set; }

        public string? SchoolGSTNo { get; set; }

        public int TotalDivisions { get; set; }

        public decimal ClassBookPrice { get; set; }

        public int IsActive { get; set; }

        public IDictionary<int,int> ClassDivision { get; set; }

        public IDictionary<int,decimal> BookPrice { get; set; }

    }
}
