﻿CREATE TABLE IF NOT EXISTS log_fields (id INTEGER PRIMARY KEY, log_id INTEGER REFERENCES logs(id), label TEXT, data_type INTEGER);
