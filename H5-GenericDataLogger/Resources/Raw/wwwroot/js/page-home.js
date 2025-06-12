var new_entry_dialog = document.getElementById('new-entry-dialog');
var new_entry_form = document.getElementById('new-entry-form');
var new_entry_logid_input = document.getElementById('new-entry-logid-input');
var new_entry_title = document.getElementById('new-entry-title');
var new_entry_true_btn = document.getElementById('new-entry-true-btn');
var new_entry_false_btn = document.getElementById('new-entry-false-btn');

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
    e.addEventListener("click", new_log_entry_btn_click)
    e_addbtn.ariaDescription = "Add log entry to " + log_title;
    e.appendChild(e_addbtn);

    return e;
}

async function new_log_entry_btn_click(event) {
    console.log("new_log_entry_btn_click(e:", event, ")");
    const log_id_str = event.target.parentElement.getAttribute("log_id")
    const log_id = parseInt(log_id_str);
    show_new_entry_modal(log_id)
}

/**Returns the current entr fields in new_entry_form */
async function get_entry_fields() {
    return Array.from(new_entry_form.querySelectorAll('.entry-field-label,.entry-field-input'));
}

async function clear_entry_fields() {
    const entry_fields = await get_entry_fields();
    for (let i = 0; i < entry_fields.length; i++) {
        entry_fields[i].remove()
    }
}

async function create_entry_fields() {
    // TODO validate log_id
    const log_id = parseInt(new_entry_logid_input.value);
    const log = LogInfo.find(x => x.id === log_id);
    for (let i = 0; i < log.fields.length; i++) {
        const f = log.fields[i];
        const inp_name = 'field_' + f.id;

        const e_label = document.createElement('label');
        e_label.classList.add('entry-field-label');
        e_label.textContent = f.label;
        e_label.setAttribute("for", inp_name);
        new_entry_form.appendChild(e_label);


        const e_input = document.createElement("input");
        e_input.classList.add('entry-field-input');
        const input_type = ValueTypeToInputType[f.value_type];
        e_input.setAttribute("type", input_type);
        
        if (input_type === 'datetime-local') {
            // '2025-06-12T11:58'
            let now = new Date();
            // e_input.value = now.toISOString().substring(0, 'yyyy-MM-ddTHH:MM'.length);
            const yr = now.getFullYear().toString().padStart(4, 0)
            const mo = (now.getMonth() + 1).toString().padStart(2, 0);
            const da = now.getDate().toString().padStart(2, 0);
            const ho = now.getHours().toString().padStart(2, 0);
            const mi = now.getMinutes().toString().padStart(2, 0);
            e_input.value = `${yr}-${mo}-${da}T${ho}:${mi}`;
        }
        e_input.setAttribute("name", inp_name);
        e_input.setAttribute("field", JSON.stringify(f));
        // TODO default value
        new_entry_form.appendChild(e_input);
    }

    new_entry_true_btn.style.gridRow = log.fields.length + 1;
    new_entry_false_btn.style.gridRow = log.fields.length + 1;
}

async function show_new_entry_modal(log_id) {
    // TODO validate log_id
    const log = LogInfo.find(x => x.id === log_id);
    new_entry_logid_input.value = log_id;
    new_entry_title.textContent = "Add entry to " + log.title;

    clear_entry_fields();
    create_entry_fields();

    new_entry_dialog.showModal();
}

async function initEntryDialog() {
    new_entry_dialog = document.getElementById('new-entry-dialog');
    new_entry_form = document.getElementById('new-entry-form');
    new_entry_logid_input = document.getElementById('new-entry-logid-input');
    new_entry_title = document.getElementById('new-entry-title');
    new_entry_true_btn = document.getElementById('new-entry-true-btn');
    new_entry_false_btn = document.getElementById('new-entry-false-btn');

    new_entry_dialog.addEventListener("close", async (e) => {
        const return_value = new_entry_dialog.returnValue;
        console.log("return_value:", return_value);
        if (return_value === 'true') {
            const log_id_str = new_entry_logid_input.value;
            // TODO validate log_id 
            const log_id = parseInt(log_id_str);
            const field_elems = (await get_entry_fields())
            const values = new Array(field_elems);
            let val_i = 0;
            for (let i = 0; i < field_elems.length; i++) {
                const e = field_elems[i];
                const is_entry_field_input = e.classList.contains('entry-field-input')
                if (!is_entry_field_input) {
                    values.length -= 1;
                    continue;
                }
                const field = JSON.parse(e.getAttribute("field"));
                const val_str = e.value;
                const val = parseFieldInputValue(field.value_type, val_str);
                values[val_i] = { field: field, value: val }
            }
            saveEntry(log_id, values);
        }
    });
}
async function initPageHome() {
    WriteDebug(arguments.callee.name);
    await updateLogInfo();
    for (let i = 0; i < LogInfo.length; i++) {
        const card = createCard(LogInfo[i].id, LogInfo[i].title);
        document.body.appendChild(card);
    }

    const newLogButton = document.createElement('a');
    newLogButton.id = 'new-log-btn';
    newLogButton.href = `/edit.html?logid=-1`;

    await initEntryDialog();
}

initPageHome();