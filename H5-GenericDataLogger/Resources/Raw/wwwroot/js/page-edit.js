const queryParams = new URLSearchParams(window.location.search);
const LogId = parseInt(queryParams.get('logid') ?? -1);

const CreateNew = LogId == -1;

const edit_title_input = document.getElementById('edit-title-input');
const edit_fields_section = document.getElementById('edit-fields-section');
const back_btn = document.getElementById('back-btn');
const save_btn = document.getElementById('save-btn');

const original_value = {
    title: "",
    fields: []
}
var current_value;

async function initPageEdit() {
    WriteDebug(arguments.callee.name);
    await updateLogInfo();

    if (!CreateNew) {
        // TODO handle editing existing logs
        alert("Editing existing logs is not implemented yet")
        navigation.navigate("/home.html")
    }

    current_value = structuredClone(original_value);
}

function get_field_sections() {
    return Array.from(edit_fields_section.querySelectorAll('.edit-field-section'));
}

async function update_current_value() {
    const field_sections = get_field_sections();
    current_value.title = edit_title_input.value;
    current_value.fields.length = field_sections.length;

    for (let i = 0; i < field_sections.length; i++) {
        const name_input = field_sections[i].querySelector('.edit-field-name-input');
        const type_input = field_sections[i].querySelector('.edit-field-type-input');
        const type_option = type_input.selectedOptions[0];

        const name = name_input.value;
        const type = parseInt(type_option.value);
        current_value.fields[i] = { name: name, type: type };
    }
}

async function value_changed() {
    return !(JSON.stringify(original_value) === JSON.stringify(current_value));
}

async function can_save() {
    const edit_title_input_valid = edit_title_input.checkValidity();
    if (!edit_title_input_valid) {
        alert(`Cannot save due to: invalid log title '${edit_title_input.value}'`);
        return false;
    }

    const field_sections = get_field_sections();
    if (field_sections.length === 0) {
        alert(`Cannot save due to: Logs must have at least 1 field`);
        return false;
    }
    for (let i = 0; i < field_sections.length; i++) {
        const name_input = field_sections[i].querySelector('.edit-field-name-input');
        const name_input_valid = name_input.checkValidity();
        if (!name_input_valid) {
            alert(`Cannot save due to: Invalid field name '${name_input.value}'`);
            return false;
        }

        const type_input = field_sections[i].querySelector('.edit-field-type-input');
        const type_option = type_input.selectedOptions[0];
        const type = parseInt(type_option.value);
        if (type === 0) {
            alert(`Cannot save due to: Invalid value type '${type_option.textContent}'`);
            return false;
        }
    }

    return true;
}

async function save() {
    if (!can_save()) return false;
    await update_current_value();
    if (!value_changed()) return false;
    await saveLog(LogId, current_value.title, current_value.fields);
}

async function save_btn_click() {
    if (save()) {
        alert("Saved log");
        navigation.back();
    }
}

async function back_btn_click() {
    await update_current_value();
    if(value_changed()){
        // TODO handle this whenever user tries to leave page
        // TODO Allow not saving
        alert("You have unsaved changes, save before leaving");
        return;
    }

    navigator.back();
}

async function add_field() {
    const f = await create_field_edit();
    edit_fields_section.appendChild(f);
}

async function create_field_edit(name, value_type) {
    let e = document.createElement('div');
    e.classList.add("edit-field-section");

    let e_name_label = document.createElement("span");
    e_name_label.classList.add('edit-field-label');
    e_name_label.textContent = "Name";
    e.appendChild(e_name_label);

    let e_name_input = document.createElement("input");
    e_name_input.classList.add('edit-field-name-input');
    e_name_input.setAttribute("type", "text");
    e_name_input.setAttribute("minLength", 1);
    e_name_input.setAttribute("maxLength", 255);
    e_name_input.setAttribute("pattern", "\\w\\w*");
    e_name_input.setAttribute("required", "");
    e.appendChild(e_name_input);

    let e_type_label = document.createElement("span");
    e_type_label.classList.add("edit-field-label");
    e_type_label.textContent = "Type";
    e.appendChild(e_type_label);

    let e_type_input = document.createElement("select");
    e_type_input.classList.add("edit-field-type-input");
    e_type_input.innerHTML = await getValueTypeOptionsHTML();
    e_type_input.selectedIndex = value_type ?? 0;
    e.appendChild(e_type_input);

    return e;
}

initPageEdit();