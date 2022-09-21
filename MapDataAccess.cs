
// import the elements of the PostgreSQL data provider
using Npgsql;
namespace robot_controller_api.Persistence;

using System.Data.SqlClient;




public static class MapDataAccess 
{
    private const string TABLE_NAME = "mapcommand";

    private const string CONNECTION_STRING =
    "Host=localhost;Username=postgres;Password=alexolbert;Database=secondTry";

    //                              GET ALL (select)
    public static List<Map> GetMapCommands()
    {
        // RobotCommand = This class is used to perform operations of reading and writing
        // into the database
        var MapCommands = new List<Map>();

        //  NpgsqlConnection object is created. This object is used to open a connection to a database.
        //  The using statement releases the database connection resource when the variable goes
        //  out of scope
        using var conn = new NpgsqlConnection(CONNECTION_STRING);

        // This line opens the database connection
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM mapcommand", conn);

        // The DataReader object is used to get all the data specified by the SQL query.
        // We can then read all the table rows one by one using the data reader.
        using var dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            // read values off the data reader and create a new robotCommand here
            // and then add it to the result list

            var mc = new Map();
            mc.ID = dr.GetInt32(0);
            mc.Name = dr.GetString(1);
            mc.Columns = dr.GetInt32(2);
            mc.Rows = dr.GetInt32(3);
            mc.CreatedDate = dr.GetDateTime(5);
            mc.ModifiedDate = dr.GetDateTime(6);
            mc.Description = dr.IsDBNull(2) ? null : dr.GetString(4);

            MapCommands.Add(mc);

        }
        return MapCommands;

    }



    //                              GET BY ID
    public static List<Map> Get(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        var MapCommands2 = new List<Map>();

        string commandText = $"SELECT * FROM {TABLE_NAME} WHERE ID = @id";
        using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
        {
            // add new  parameter; like a filter  -> we try to match with the id provided
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            // Read in the select results
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var mc = new Map();
                mc.ID = dr.GetInt32(0);
                mc.Name = dr.GetString(1);
                mc.Columns = dr.GetInt32(2);
                mc.Rows = dr.GetInt32(3);
                mc.CreatedDate = dr.GetDateTime(5);
                mc.ModifiedDate = dr.GetDateTime(6);
                mc.Description = dr.IsDBNull(2) ? null : dr.GetString(4);

                MapCommands2.Add(mc);
            }

            return MapCommands2;
        }
    }



    //                              ADD (create)

    public static async Task Add(Map command)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        string commandText =
            $"INSERT INTO {TABLE_NAME} ( Name, Columns, Rows,Description, CreatedDate, ModifiedDate) " +
            $"VALUES( @name, @columns, @rows, @description, @createddate, @modifieddate)";
        await using (var cmd = new NpgsqlCommand(commandText, conn))
        {
            // id is a PK so we dont need to check for duplicate records
            //cmd.Parameters.AddWithValue("id", command.ID);
            cmd.Parameters.AddWithValue("name", command.Name);
            cmd.Parameters.AddWithValue("columns", command.Columns);
            cmd.Parameters.AddWithValue("rows", command.Rows);
            cmd.Parameters.AddWithValue("description", command.Description);
            cmd.Parameters.AddWithValue("createddate", command.CreatedDate);
            cmd.Parameters.AddWithValue("modifieddate", command.ModifiedDate);

            await cmd.ExecuteNonQueryAsync();
        }
    }


    //                              UPDATE


    public static async Task Update(int id, Map command)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();


        var cText = $@"UPDATE {TABLE_NAME}
                    SET Name = @name, Columns = @columns,Rows = @rows ,Description = @description, ModifiedDate = @modifieddate
                     WHERE id = @id";


        using (var cmd = new NpgsqlCommand(cText, conn))
        {


            // Add new SqlParameter to the command.
            //cmd.Parameters.Add(new NpgsqlParameter("@id", id));  // package manager 'Install-Package System.Data.SqlClient'


            cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer));
            cmd.Parameters.Add(new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters.Add(new NpgsqlParameter("columns", NpgsqlTypes.NpgsqlDbType.Integer));
            cmd.Parameters.Add(new NpgsqlParameter("rows", NpgsqlTypes.NpgsqlDbType.Integer));
            cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters.Add(new NpgsqlParameter("modifieddate", NpgsqlTypes.NpgsqlDbType.Date));

            cmd.Prepare();

            

            cmd.Parameters[0].Value = id;
            cmd.Parameters[1].Value = command.Name;
            cmd.Parameters[2].Value = command.Columns;
            cmd.Parameters[3].Value = command.Rows;
            cmd.Parameters[4].Value = command.Description;
            cmd.Parameters[5].Value = command.ModifiedDate;  // how can we record the date time now?

            await cmd.ExecuteNonQueryAsync();



    // #2
            //using var dr = cmd.ExecuteReader();
            //while (dr.Read())
            //{

            //    // change
            //    cmd.Parameters.AddWithValue("name", command.Name);
            //    cmd.Parameters.AddWithValue("columns", command.Columns);
            //    cmd.Parameters.AddWithValue("rows", command.Rows);
            //    cmd.Parameters.AddWithValue("description", command.Description);
            //    cmd.Parameters.AddWithValue("modifieddate", command.ModifiedDate);

            //    // method to run db commands
            //    await cmd.ExecuteNonQueryAsync();

            //}
        }
    }


    //                              DELETE

    public static async Task Delete(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        string commandText = $"DELETE FROM {TABLE_NAME} WHERE ID=(@p)";
        await using (var cmd = new NpgsqlCommand(commandText, conn))
        {
            cmd.Parameters.AddWithValue("p", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }


}


// source:
// https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/connect-csharp