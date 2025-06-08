namespace H5_GenericDataLogger.Controls;

public partial class CardControl : ContentView {

	#region TitleProperty
	public BindableProperty TitleProperty = BindableProperty.Create(
			nameof(Title),
			typeof(string),
			typeof(CardControl),
			defaultValue: "CardControl",
			propertyChanged: TitleChaged
		);
	public string Title {
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}
	private static void TitleChaged(BindableObject bindable, object oldValue, object newValue) {
		var control = (CardControl)bindable;
		control.TitleLabel.Text = (string)newValue;
	}
	#endregion

	#region AccentColorProperty
	public BindableProperty AccentColorProperty = BindableProperty.Create(
			nameof(AccentColor),
			typeof(Brush),
			typeof(CardControl),
			propertyChanged: AccentBorderChanged
		);
	public Brush AccentColor {
		get => (Brush)GetValue(AccentColorProperty);
		set => SetValue(AccentColorProperty, value);
	}
	private static void AccentBorderChanged(BindableObject bindable, object oldValue, object newValue) {
		var control = (CardControl)bindable;
		control.AccentBorder.Background = (Brush)newValue;
	}
	#endregion

	public CardControl() {
		this.InitializeComponent();
	}
}