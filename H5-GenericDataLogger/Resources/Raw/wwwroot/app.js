const LogDatas = []
async function updateLogDatas() {
    const logsJson = getLogsJson();
    LogDatas.length = 0;
    const logs = JSON.parse(logsJson);
    while (logs.length > 0) {
        LogDatas.push(logs.pop());
    }
}
async function getLogsJson() {
    return await window.HybridWebView.InvokeDotNet('GetLogsJson');
}


// Home
function createCard(log_id, log_title) {
    //<div class="home-log-card border-red70">
    //    <span class="home-log-title">Ciggies</span>
    //    <button class="home-log-edit-btn"></button>
    //</div>
    let e = document.createElement('div');
    e.classList.add('home-log-card');
    e.setAttribute("id", log_id);

    let e_title = document.createElement('span');
    e_title.classList.add('home-log-title');
    e_title.textContent = log_title;
    e.appendChild(e_title);

    let e_editbtn = document.createElement('button');
    e_editbtn.classList.add('home-log-edit-btn');

}
async function initHome() {
    await updateLogDatas();
    for (let i = 0; i < LogDatas.length; i++) {
        document.appendChild(createCard(LogDatas[i].id, LogDatas[i].title))
    }
}
