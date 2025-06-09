namespace H5_GenericDataLogger;

public partial class EditLogPage : ContentPage {
	private long LogId = -1;
	private string LogTitle = "";
	private readonly List<LogField> LogFields = new();
	public EditLogPage() {
		InitializeComponent();
		//GlobalEnvironment.NavPage.Navigation. += this.NavigatingFromHandler;
		//this.SetDebugBackgrounds();
	}

	public async Task EnsureUserSaved() {
		bool doSave = await DisplayAlert("Unsaved Progress?", "Return without saving?", "Yes", "No");
	}

	public async void NavigatingFromHandler(NavigatingFromEventArgs e) {
		await this.EnsureUserSaved();
	}

	public void CancelButton_Clicked(object? e, EventArgs args) {
		this.SendBackButtonPressed();
	}

	public async void SaveButton_Clicked(object? e, EventArgs args) {
		// TODO save this log
	}
}