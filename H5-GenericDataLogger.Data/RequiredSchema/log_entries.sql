﻿CREATE TABLE IF NOT EXISTS log_entries (id INTEGER PRIMARY KEY, log_id INTEGER REFERENCES logs(id));
