var sql = require('mssql');

(async function()
{
    try {
        var conn = new sql.ConnectionPool('server=localhost;Database=TEST_DB;User Id=foo;Password=foo');
        await conn.connect();
        
        var request   = new sql.Request(conn);
        var res = await request.query('SELECT Name, Surname, Address from Osoba2 WHERE Age = 20')	
        
        res.recordset.forEach( r => {
            console.log( `${r.Surname} ${r.Name} ${r.Address}`);
        });

        res = await request.query("INSERT INTO Osoba2(Name, Surname, Age, Address, Sex) VALUES('Marcin', 'Zieba', 43, 'Wodzislaw Slaski', 'male'); SELECT SCOPE_IDENTITY() as id")
        res.recordset.forEach( r => {
            console.log('Inserted ID:' + `${r.id}`);
        });

        res = await request.query("UPDATE Osoba2 SET Name='Marek' WHERE Name='Marcin'")
        console.log('Updated:' + res.rowsAffected[0])

        res = await request.query("DELETE FROM Osoba2 WHERE Name='Marek'")
        console.log('Deleted:' + res.rowsAffected[0])
        
        conn.close();
    }
    catch ( err ) {
        console.log( err );
    }
}())
