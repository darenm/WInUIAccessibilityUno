// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using Microsoft.Xaml.Interactivity;

namespace WInUIAccessibilityUno.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AriaAttributes : Page
    {
        public AriaAttributes()
        {
            this.InitializeComponent();
            this.Loaded += AriaAttributes_Loaded;
        }

        private void AriaAttributes_Loaded(object sender, RoutedEventArgs e)
        {
            IteratePanelChildren(ContentRoot);
        }

        private void IteratePanelChildren(Panel p)
        {
            foreach (var child in p.Children)
            {
                var behavior = new AriaBehavior();
                Interaction.GetBehaviors(child).Add(behavior);
                if (child is Panel panel)
                {
                    IteratePanelChildren(panel);                
                }
            }
        }

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

