CREATE TABLE "Student" (
	"Id"	INTEGER NOT NULL,
	"FName"	TEXT,
	"LName"	TEXT,
	"Email"	TEXT,
	"Phone"	TEXT,
	"Country"	TEXT,
	"City"	TEXT,
	"Street"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "School" (
	"Id"	INTEGER NOT NULL,
	"Name"	TEXT,
	"Country"	TEXT,
	"City"	TEXT,
	"Street"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE TABLE "SchoolPrograms" (
	"Id"	INTEGER NOT NULL,
	"School"	INTEGER NOT NULL,
	"Name"	TEXT,
	"Description"	TEXT,
	"Capacity"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("School") REFERENCES "School"("Id") ON DELETE CASCADE
);

CREATE TABLE "Applications" (
	"Id"	INTEGER NOT NULL,
	"Student"	INTEGER NOT NULL,
	"Date"	TEXT NOT NULL,
	"Program1"	INTEGER NOT NULL,
	"Program2"	INTEGER,
	"Program3"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("Program1") REFERENCES "SchoolPrograms"("Id"),
	FOREIGN KEY("Program2") REFERENCES "SchoolPrograms"("Id"),
	FOREIGN KEY("Program3") REFERENCES "SchoolPrograms"("Id"),
	FOREIGN KEY("Student") REFERENCES "Student"("Id") ON DELETE CASCADE
);