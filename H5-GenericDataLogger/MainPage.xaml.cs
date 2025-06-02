namespace H5_GenericDataLogger {
    public partial class MainPage : ContentPage {
        int count = 0;

        public MainPage() { InitializeComponent(); }

        private void OnCounterClicked(object? sender, EventArgs e) {
            CounterBtn.Text = $"SQLite connection string: " + GlobalEnvironment.Connector.ConnectionString;
            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
