using Microsoft.Maui.Controls;

using System.Data;

namespace H5_GenericDataLogger;

public partial class MainPage : ContentPage {
	public MainPage() {
		InitializeComponent();
		MainWebView.SetInvokeJavaScriptTarget(this);
	}

	public void DebugWriteLine(object? msg) {
		Debug.WriteLine(msg);
	}

	/// <summary>
	/// TODO summary
	/// </summary>
	/// <returns></returns>
	public string GetLogsJson() {
		var logs_json = GlobalEnvironment.Connector
			.GetLogs()
			.AsParallel()
			.Select(log => log.ToJson());
		string result = $"[{string.Join(',', logs_json)}]";
		return result;
	}

	protected override bool OnBackButtonPressed() {
		_ = MainWebView.EvaluateJavaScriptAsync("history.back()");
		return true;
	}
}