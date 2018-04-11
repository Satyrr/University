var sql = require('mssql');

(async function()
{
    try {
        var conn = new sql.ConnectionPool('server=localhost;Database=TEST_DB;User Id=foo;Password=foo');
        await conn.connect();
        var request   = new sql.Request(conn);

        console.time('With index');
        //for(var i=0; i<100; i++)
            res = await request.query("SELECT * FROM BigTable Where KeyWithNum='64356abc'")
        console.timeEnd('With index');
        res.recordset.forEach(r => 
            {
                console.log(r)
            })
            
            
        console.time('Without index');
        //for(var i=0; i<100; i++)
            res = await request.query("SELECT * FROM BigTable Where IndexedKeyWithNum='64356abc'")
        console.timeEnd('Without index');
        res.recordset.forEach(r => 
        {
            console.log(r)
        })
        

        

        conn.close();
    }
    catch ( err ) {
        console.log( err );
    }
}())
