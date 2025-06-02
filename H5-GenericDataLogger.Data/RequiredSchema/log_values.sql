CREATE TABLE IF NOT EXISTS integer_log_values (
	id INTEGER PRIMARY KEY,
	entry_id INTEGER REFERENCES log_entries(id),
	field_id INTEGER REFERENCES log_fields(id),
	val INTEGER
);

CREATE TABLE IF NOT EXISTS real_log_values (
	id INTEGER PRIMARY KEY,
	entry_id INTEGER REFERENCES log_entries(id),
	field_id INTEGER REFERENCES log_fields(id),
	val REAL
);

CREATE TABLE IF NOT EXISTS text_log_values (
	id INTEGER PRIMARY KEY,
	entry_id INTEGER REFERENCES log_entries(id),
	field_id INTEGER REFERENCES log_fields(id),
	val TEXT
);

CREATE TABLE IF NOT EXISTS blob_log_values (
	id INTEGER PRIMARY KEY,
	entry_id INTEGER REFERENCES log_entries(id),
	field_id INTEGER REFERENCES log_fields(id),
	val BLOB
);

CREATE TABLE IF NOT EXISTS integer_log_values (
	id INTEGER PRIMARY KEY,
	entry_id INTEGER REFERENCES log_entries(id),
	field_id INTEGER REFERENCES log_fields(id),
	val INTEGER
);
