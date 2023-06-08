namespace WInUIAccessibilityUno.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoAutomation : Page
    {
        public string[] States = {
    "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut",
    "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa",
    "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan",
    "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire",
    "New Jersey", "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio",
    "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota",
    "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia",
    "Wisconsin", "Wyoming"
};
        public NoAutomation()
        {
            this.InitializeComponent();
        }

        public void ShowMessage()
        {
            var cd = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                PrimaryButtonText = "OK",
                IsPrimaryButtonEnabled = true,
                Content = "Button Clicked"
            };

            _ = cd.ShowAsync();
        }
    }
}
