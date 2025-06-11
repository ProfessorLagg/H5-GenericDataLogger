const queryParams = new URLSearchParams(window.location.search);
const LogId = parseInt(queryParams.get('logid') ?? -1);

const CreateNew = LogId == -1;

const edit_title_input = document.getElementById('edit-title-input');
const edit_fields_section = document.getElementById('edit-fields-section');

const original_values = {
    title: "",
    fields: []
}
var current_values;

async function initPageEdit() {
    WriteDebug(arguments.callee.name);
    await updateLogInfo();

    if (!CreateNew) {
        // TODO handle editing existing logs
        alert("Editing existing logs is not implemented yet")
        navigation.navigate("/home.html")
    }

    current_values = structuredClone(original_values);
    await validateTitle();
}

function values_changed() {
    return !(JSON.stringify(original_values) === JSON.stringify(current_values));
}

async function save() {
    if (!values_changed()) return;
}

async function validateTitle() {
    edit_title_input.value = edit_title_input.value.trim()
    let isValid =
        edit_title_input.value.length > 0
        && edit_title_input.value.length < 256
        && isAlphaNumericASCII(edit_title_input.value);

    console.log("isValid:", isValid)
    if (isValid) {
        edit_title_input.setAttribute('valid', "");
        edit_title_input.removeAttribute('invalid');
    } else {
        edit_title_input.setAttribute('invalid', "");
        edit_title_input.removeAttribute('valid');

    }
}

initPageEdit();