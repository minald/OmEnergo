namespace OmEnergo.Models.ViewModels
{
    public class UploadFileFormViewModel
    {
        public string AspAction { get; set; }

        public string AspRouteEnglishName { get; set; }

        public UploadFileFormViewModel(string aspAction) => AspAction = aspAction;
    }
}
