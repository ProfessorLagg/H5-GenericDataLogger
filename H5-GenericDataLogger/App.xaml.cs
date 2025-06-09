namespace H5_GenericDataLogger {
	public partial class App : Application {
		public App() {
			this.InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState) {
			return new Window(new MainPage());
		}
	}
}