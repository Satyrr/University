var sql = require('mssql');

(async function()
{
    try {
        var conn = new sql.ConnectionPool('server=localhost;Database=TEST_DB;User Id=foo;Password=foo');
        await conn.connect();
        
        var request   = new sql.Request(conn);
        var res;
        var personId; 

        res = await request.query("INSERT INTO Miejsce_Pracy(Name, Address) VALUES('Uniwersytet Wroclawski','Wroclaw'); SELECT SCOPE_IDENTITY() as id")
        var UniWrocId = res.recordset[0].id

        res = await request.query("INSERT INTO Miejsce_Pracy(Name, Address) VALUES('Luxoft','Wroclaw'); SELECT SCOPE_IDENTITY() as id")
        var LuxoftId = res.recordset[0].id

        res = await request.query(`INSERT INTO Osoba2(Name, Surname, Age, Address, Sex) VALUES('Ania', 'Zieba', 43, 'Wodzislaw Slaski', 'male'); SELECT SCOPE_IDENTITY() As id`)
        personId = res.recordset[0].id
        await request.query(`INSERT INTO Osoba_Miejsce_Pracy(JobID, PersonID) VALUES(${UniWrocId}, ${personId})`)

        res = await request.query(`INSERT INTO Osoba2(Name, Surname, Age, Address, Sex) VALUES('Martyna', 'Chuda', 35, 'Katowice', 'female'); SELECT SCOPE_IDENTITY() As id`)
        personId = res.recordset[0].id
        await request.query(`INSERT INTO Osoba_Miejsce_Pracy(JobID, PersonID) VALUES(${LuxoftId}, ${personId})`)

        res = await request.query(`INSERT INTO Osoba2(Name, Surname, Age, Address, Sex) VALUES('Tomasz', 'Tkocz', 19, 'Kraków', 'male'); SELECT SCOPE_IDENTITY() As id`)
        personId = res.recordset[0].id
        await request.query(`INSERT INTO Osoba_Miejsce_Pracy(JobID, PersonID) VALUES(${LuxoftId}, ${personId})`)

        conn.close();
    }
    catch ( err ) {
        console.log( err );
    }
}())
