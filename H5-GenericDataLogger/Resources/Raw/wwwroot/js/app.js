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