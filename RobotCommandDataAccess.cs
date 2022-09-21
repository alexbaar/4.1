
// import the elements of the PostgreSQL data provider
using Npgsql;
namespace robot_controller_api.Persistence;

using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class RobotCommandDataAccess
{
    private const string TABLE_NAME = "robotcommand";

    // This is the connection string. It is used by the data provider to establish a connection
    // to the database.We specify the host name, user name, password and a database name
    private const string CONNECTION_STRING =
        "Host=localhost;Username=postgres;Password=alexolbert;Database=secondTry";


    //                              GET ALL
    public static List <RobotCommand> GetRobotCommands()
    {
        // RobotCommand = This class is used to perform operations of reading and writing
        // into the database
        var RobotCommands = new List<RobotCommand>();

        //  NpgsqlConnection object is created. This object is used to open a connection to a database.
        //  The using statement releases the database connection resource when the variable goes
        //  out of scope
        using var conn = new NpgsqlConnection(CONNECTION_STRING);

        // This line opens the database connection
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);

        // The DataReader object is used to get all the data specified by the SQL query.
        // We can then read all the table rows one by one using the data reader.
        using var dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            // read values off the data reader and create a new robotCommand here
            // and then add it to the result list
            
            var rc = new RobotCommand();
            rc.ID = dr.GetInt32(0);
            rc.Name = dr.GetString(1);
            rc.IsMoveCommand = dr.GetBoolean(3);
            rc.CreatedDate = dr.GetDateTime(4);
            rc.ModifiedDate = dr.GetDateTime(5);
            rc.Description = dr.IsDBNull(2) ? null : dr.GetString(2);

            RobotCommands.Add(rc);
            
        }
        return RobotCommands;
       
    }



    //                              ADD
    public static async Task Add(RobotCommand command)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        try
        {
            conn.Open();

            string commandText =
                $"INSERT INTO {TABLE_NAME} ( Name, Description, IsMoveCommand, CreatedDate, ModifiedDate) " +
                $"VALUES( @name, @description, @ismovecommand, now() , now())";
            await using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                // id is a PK so we dont need to check for duplicate records
                //cmd.Parameters.AddWithValue("id", command.ID);
                cmd.Parameters.AddWithValue("name", command.Name);
                cmd.Parameters.AddWithValue("description", command.Description);
                cmd.Parameters.AddWithValue("ismovecommand", command.IsMoveCommand);
                cmd.Parameters.AddWithValue("createddate", command.CreatedDate);
                cmd.Parameters.AddWithValue("modifieddate", command.ModifiedDate);

                await cmd.ExecuteNonQueryAsync();
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            
        }
    }


    //                              GET BY ID
    public static List<RobotCommand> Get(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        var RobotCommands2 = new List<RobotCommand>();

        string commandText = $"SELECT * FROM {TABLE_NAME} WHERE ID = @id";
        using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
        {
            // add new sql parameter 
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            // Read in the select results
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var rc = new RobotCommand();
                rc.ID = dr.GetInt32(0);
                rc.Name = dr.GetString(1);
                rc.IsMoveCommand = dr.GetBoolean(3);
                rc.CreatedDate = dr.GetDateTime(4);
                rc.ModifiedDate = dr.GetDateTime(5);
                rc.Description = dr.IsDBNull(2) ? null : dr.GetString(2);

                RobotCommands2.Add(rc);
            }

            return RobotCommands2;
        }
    }




    //                              UPDATE (why updates all?)
    public static async Task<RobotCommand> Update(int id, RobotCommand command)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();

        var commandText = $@"UPDATE {TABLE_NAME}
                    SET Name = @name, Description =@description, IsMoveCommand=@ismovecommand, ModifiedDate=now()
                    WHERE id = @id";


        await using (var cmd = new NpgsqlCommand(commandText, conn))
        {
            // we will not allow to modify id (id = PK)
            // cmd.Parameters.AddWithValue("ID", command.ID);

            cmd.Parameters.AddWithValue("name", command.Name);
            cmd.Parameters.AddWithValue("description", command.Description);
            cmd.Parameters.AddWithValue("ismovecommand", command.IsMoveCommand);
            cmd.Parameters.AddWithValue("modifieddate", command.ModifiedDate);

            await cmd.ExecuteNonQueryAsync();
        }
        return command;
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
    


    // some functions
    public static bool DoesIdAlreadyExist(RobotCommand addnew)
    {
        bool IDtaken;
        var RC = GetRobotCommands();
        var isIDtaken = RC.FirstOrDefault(x => x.ID == addnew.ID);

        if (isIDtaken == null) IDtaken = false;
        else IDtaken = true;

        return IDtaken;

    }

    public static bool DoesCommandIdAlreadyExist(int id)
    {
        bool IDtaken;
        var RC = GetRobotCommands();
        var isIDtaken = RC.FirstOrDefault(x => x.ID == id);

        if (isIDtaken == null) IDtaken = false;
        else IDtaken = true;

        return IDtaken;

    }

    public static bool DoesNameAlreadyExist(RobotCommand addnew)
    {
        bool Nametaken;
        var RC = GetRobotCommands();
        var isNametaken = RC.FirstOrDefault(x => x.Name == addnew.Name);

        if (isNametaken == null) Nametaken = false;
        else Nametaken = true;

        return Nametaken;

    }
}






// SOURCE:
// https://www.code4it.dev/blog/postgres-crud-operations-npgsql#executenonqueryasync-vs-executereaderasync
