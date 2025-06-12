function WriteDebug(msg) {
    let str = "";
    if (msg instanceof Array) {
        str = msg.map(x => `${x}`).join(' ');
    } else {
        str = `${msg}`;
    }

    if (window.HybridWebView.IsWebView()) { window.HybridWebView.InvokeDotNet('DebugWriteLine', ["JavaScript: " + str]); }
    console.debug(str);
}

const LogInfo = []
async function updateLogInfo() {
    const logsJson = await getLogsJson();
    LogInfo.length = 0;
    const logs = JSON.parse(logsJson);
    while (logs.length > 0) {
        LogInfo.push(logs.pop());
    }
}
async function getLogsJson() {
    WriteDebug(["window.navigator.userAgent:", window.navigator.userAgent]);

    if (window.HybridWebView.IsWebView()) {
        return await window.HybridWebView.InvokeDotNet('GetLogsJson');
    } else {
        const req = fetch("dbg/logs.json");
        const rsp = await req;
        return await rsp.text();
    }
}

async function getValueTypeOptionsHTML() {
    if (window.HybridWebView.IsWebView()) {
        return await window.HybridWebView.InvokeDotNet('GetValueTypeOptionsHTML');
    } else {
        const req = fetch("dbg/value_type_options.htm");
        const rsp = await req;
        return await rsp.text();
    }
}

function isAlphaNumericASCII(str) {
    var code, i, len;

    for (i = 0, len = str.length; i < len; i++) {
        code = str.charCodeAt(i);
        if (!(code > 47 && code < 58) && // numeric (0-9)
            !(code > 64 && code < 91) && // upper alpha (A-Z)
            !(code > 96 && code < 123)) { // lower alpha (a-z)
            return false;
        }
    }
    return true;
};

async function saveLog(id, title, fields) {
    const fields_json = JSON.stringify(fields);
    console.log(`app.saveLog(id: ${id}, title: ${title}, fields_json: ${fields_json}"`);
    if (!window.HybridWebView.IsWebView()) {
        console.error("Cannot save in browser mode");
        return;
    }

    return await window.HybridWebView.InvokeDotNet('SaveLog', [id, title, fields_json]);
}