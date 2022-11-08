namespace AdminClient.ViewModels
{
    public class SchoolClassDivModel
    {
        public int SchoolClassDivisionId { get; set; }

        public int SchoolId { get; set; }
        public string SchoolName { get; set; }

        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public string Division { get; set; }

        public int IsActive { get; set; }

        public int AcademyYearId { get; set; }

        public string AcademyYearName { get; set; }
    }
}
