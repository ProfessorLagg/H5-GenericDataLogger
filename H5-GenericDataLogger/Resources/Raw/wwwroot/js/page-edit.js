const queryParams = new URLSearchParams(window.location.search);
const LogId = queryParams.get('logid') ?? -1

async function initPageEdit() {
    WriteDebug(arguments.callee.name);
    await updateLogInfo();

    
}

initPageEdit();