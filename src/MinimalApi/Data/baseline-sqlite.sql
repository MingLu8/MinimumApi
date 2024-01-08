 CREATE TABLE IF NOT EXISTS [Person]
    (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT,
        Age INTEGER,
        CreatedDateUtc DATETIME
    );

  CREATE TABLE IF NOT EXISTS [TestData]
    (
         Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT,
        Age INTEGER,
        CreatedDateUtc DATETIME
	);
   