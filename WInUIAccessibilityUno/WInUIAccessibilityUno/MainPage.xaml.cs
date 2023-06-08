namespace WInUIAccessibilityUno
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void RootNavigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var name = args.SelectedItemContainer.Tag as string;

            NavigationFrame.Navigate(Type.GetType("WInUIAccessibilityUno.Pages." + name));
        }
    }
}