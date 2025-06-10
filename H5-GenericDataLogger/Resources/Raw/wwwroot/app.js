async function getLogsJson() {
    return await window.HybridWebView.InvokeDotNet('GetLogsJson');
}

async function initHome(){
    document.getElementById('temp').innerText = await getLogsJson();
}
