namespace AdminClient.ViewModels
{
    public class TeachingPlanData
    {
        public int TeachingPlanId { get; set; }
        public string? TeachingPlanName { get; set; }
        public int eBookChapterId { get; set; }
        public string eBookChapterTitle { get; set; }
        public int DayNumber { get; set; }
        public int? AcademyYearId { get; set; }
        public string AcademyYearName { get; set; }
    }
}
