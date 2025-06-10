using Microsoft.Maui.Controls;

using System.Data;

namespace H5_GenericDataLogger;

public partial class MainPage : ContentPage {
    public MainPage() {
        InitializeComponent();
        MainWebView.SetInvokeJavaScriptTarget(this);
    }

    public async Task<String> GetLogsJson() {
        Debug.Print($"MainPage.GetLogsJson");
        var logs_json = GlobalEnvironment
            .Connector
            .GetLogs()
            .AsParallel()
            .Select(log => log.ConvertToJson());
        return $"[{string.Join(',', logs_json)}]";
    }

    protected override bool OnBackButtonPressed() {
        _ = MainWebView.EvaluateJavaScriptAsync("history.back()");
        return true;
    }
}