namespace H5_GenericDataLogger;

public partial class AppNavPage : NavigationPage {

	private void init() {
		InitializeComponent();
	}
	public AppNavPage(): base() {
		this.init();
	}

	public AppNavPage(Page root) : base(root) {
		this.init();
	}
}