using System.Threading.Tasks;

namespace H5_GenericDataLogger {
	public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
			this.NewLogButton.Clicked += this.NewLogButton_Click;
		}

		public void NewLogButton_Click(object? sender, EventArgs e) {
			GlobalEnvironment.NavPage.PushAsync(new EditLogPage());
		}
	}
}
