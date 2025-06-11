function createCard(log_id, log_title) {
    let e = document.createElement('div');
    e.classList.add('home-log-card');
    e.setAttribute("log_id", log_id);

    let e_editbtn = document.createElement('a');
    e_editbtn.classList.add('home-log-edit-btn');
    e_editbtn.classList.add('icon');
    e_editbtn.classList.add('icon-pencil');
    e_editbtn.href = `/edit.html?logid=${log_id}`;
    e_editbtn.ariaDescription = "Edit log " + log_title;
    e.appendChild(e_editbtn);

    let e_title = document.createElement('span');
    e_title.classList.add('home-log-title');
    e_title.textContent = log_title;
    e.appendChild(e_title);

    let e_addbtn = document.createElement('button');
    e_addbtn.classList.add('home-log-add-btn');
    e_addbtn.classList.add('icon');
    e_addbtn.classList.add('icon-plus');
    // TODO functionality
    e_addbtn.ariaDescription = "Add log entry to " + log_title;
    e.appendChild(e_addbtn);
    
    return e;
}
async function initPageHome() {
    WriteDebug(arguments.callee.name);
    await updateLogInfo();
    for (let i = 0; i < LogInfo.length; i++) {
        const card = createCard(LogInfo[i].id, LogInfo[i].title);
        document.body.appendChild(card);
    }

    const navbar = document.getElementById('navbar')
    const newLogButton = document.createElement('a');
    newLogButton.id = 'new-log-btn';
    newLogButton.href = `/edit.html?logid=-1`;

}

initPageHome();