namespace Library.Web.Core.ViewModel
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public int? StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionStackTrace { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowException => !string.IsNullOrEmpty(ExceptionMessage);
        public bool ShowStackTrace => !string.IsNullOrEmpty(ExceptionStackTrace);
    }
}
