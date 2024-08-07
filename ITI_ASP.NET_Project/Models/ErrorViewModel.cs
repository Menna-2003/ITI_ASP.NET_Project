namespace ITI_ASP.NET_Project.Models {
    public class ErrorViewModel {
        public string? RequestId {
            get; set;
        }

        public bool ShowRequestId => !string.IsNullOrEmpty( RequestId );
    }
}
