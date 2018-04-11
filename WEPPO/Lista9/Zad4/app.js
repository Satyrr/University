var sql = require('mssql');

(async function()
{
    try {
        var conn = new sql.ConnectionPool('server=localhost;Database=TEST_DB;User Id=foo;Password=foo');
        await conn.connect();
        
        var request   = new sql.Request(conn);
        var res;

        res = await request.query("INSERT INTO Miejsce_Pracy(Name, Address) VALUES('Nokia','Wroclaw'); SELECT SCOPE_IDENTITY() as id")
        var jobId = res.recordset[0].id
        res.recordset.forEach( r => {
            console.log('Job ID:' + `${r.id}`);
        });

        //res = await request.query("INSERT INTO Osoba(ID, Name, Surname, Age, Address, Sex) VALUES((SELECT NEXT VALUE FOR OsobaSeq), 'Darek', 'Zieba', 43, 'Wodzislaw Slaski', 'male')")
        //console.log('Updated:' + res.rowsAffected[0])
        //res = await request.query('SELECT NEXT VALUE FOR OsobaSeq As id');
        //jobId = res.recordset[0].id;
        res = await request.query(`INSERT INTO Osoba2(Name, Surname, Age, Address, Sex, JobID) VALUES('Ania', 'Zieba', 43, 'Wodzislaw Slaski', 'male', ${jobId})`)
        console.log('Updated:' + res.rowsAffected[0])
        conn.close();
    }
    catch ( err ) {
        console.log( err );
    }
}())
