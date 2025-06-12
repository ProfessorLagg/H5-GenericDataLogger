using Microsoft.Data.Sqlite;
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
        for (int i = 0; i < fields_json.Count; i++) {
            JsonObject field_json = (JsonObject)fields_json[i]!;
            fields[i] = new LogField() {
                Label = (string)field_json["name"]!,
                ValueType = (FieldValueType)((long)field_json["type"]!)
            };
        }
        Log log = GlobalEnvironment.Connector.CreateLog(title, fields);
    }

    public bool TrySaveEntry(long log_id, string values_json_str) {
        // log_id: 1, values_json: [{"field":{"id":"1","label":"Timestamp","value_type":4},"value":"2025-06-12T10:32:00.000Z"}]"
        try {


            JsonNode? values_json = JsonObject.Parse(values_json_str)!;
            ArgumentNullException.ThrowIfNull(nameof(values_json));
            if (values_json is not JsonArray) { throw new ArgumentException(nameof(values_json)); }

            long? entry_id = null;
            using (SqliteCommand cmd = new()) {
                cmd.CommandText = "INSERT INTO log_entries (log_id) VALUES ($log_id) RETURNING id;";
                cmd.Parameters.AddWithValue("$log_id", log_id);
                entry_id = (long?)GlobalEnvironment.Connector.ExecuteScalar(cmd);
            }
            ArgumentNullException.ThrowIfNull(nameof(entry_id));

            using (SqliteCommand cmd = new()) {
                JsonArray jsonarr = (JsonArray)values_json!;
                for (int i = 0; i < jsonarr.Count; i++) {
                    JsonNode? jsonval = jsonarr[i];
                    ArgumentNullException.ThrowIfNull(nameof(jsonval));
                    if (jsonval is not JsonObject) { throw new ArgumentException(nameof(jsonval)); }

                    JsonNode field_json = jsonval["field"]!;
                    string field_id_str = field_json["id"]!.ToString();
                    long field_id = long.Parse(field_id_str);

                    string field_value_type_str = field_json["value_type"]!.ToString();
                    long value_type_int = long.Parse(field_value_type_str);
                    FieldValueType value_type = (FieldValueType)value_type_int;
                    string table_name = value_type.GetTableName();

                    string value_str = jsonval["value"]!.ToString();
                    ILogFieldValue value = LogFieldValueCreator.ParseJsonString(value_type, value_str);

                    string paramname = $"v{i}";

                    cmd.CommandText += $"INSERT INTO {table_name} (entry_id, field_id, val) VALUES ({entry_id}, {field_id}, $v{i})";
                    cmd.Parameters.AddWithValue(paramname, value.GetValue());
                }

                GlobalEnvironment.Connector.ExecuteScalar(cmd);
            }

            return true;
        }
        catch(Exception err) {
            Debug.WriteLine(err);
            return false;
        }
    }

    protected override bool OnBackButtonPressed() {
        _ = MainWebView.EvaluateJavaScriptAsync("history.back()");
        return true;
    }
}