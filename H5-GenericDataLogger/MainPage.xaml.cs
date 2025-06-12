using Microsoft.Maui.Controls;

using System.Data;
using System.Text.Json.Nodes;

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

    public string GetValueTypeOptionsHTML() {
        string result = string.Join('\n', FieldValueTypeExtentions.OptionsHTML);
        return result;
    }

    public void SaveLog(long id, string title, string fields_json_str) {
        if (id != -1) { throw new NotImplementedException("Editing existing logs not implemented"); }

        JsonArray fields_json = (JsonArray)JsonArray.Parse(fields_json_str)!;

        LogField[] fields = new LogField[fields_json.Count];
        for(int i = 0; i < fields_json.Count; i++) {
            JsonObject field_json = (JsonObject)fields_json[i]!;
            fields[i] = new LogField() {
                Label = (string)field_json["name"]!,
                ValueType = (FieldValueType)((long)field_json["type"]!)
            };
        }
        Log log = GlobalEnvironment.Connector.CreateLog(title, fields);
    }

    public void SaveEntry(long log_id, string values_json_str) {
        throw new NotImplementedException();
    }

    protected override bool OnBackButtonPressed() {
        _ = MainWebView.EvaluateJavaScriptAsync("history.back()");
        return true;
    }
}