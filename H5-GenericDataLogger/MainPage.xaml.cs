namespace H5_GenericDataLogger;

public partial class MainPage : ContentPage {
	public MainPage() {
		InitializeComponent();
	}

	protected override bool OnBackButtonPressed() {
		_ = MainWebView.EvaluateJavaScriptAsync("history.back()");
		return true;
	}
}