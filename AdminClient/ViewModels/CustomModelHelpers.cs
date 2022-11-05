namespace AdminClient.ViewModels
{
    public class SessionManager
    {
        public string IdKey { get; set; }
        public string IdValue { get; set; }
        public string NameKey { get; set; }
        public string NameValue { get; set; }
    }

    public static class SessionKeys
    {
        public const string httpAcYearId = "httpAcYearId";
        public const string httpAcYear = "httpAcYear";
        public const string httpSchoolId = "httpSchoolId";
        public const string httpSchool = "httpSchool";
    }
}
