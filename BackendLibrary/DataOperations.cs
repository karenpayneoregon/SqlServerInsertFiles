using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BackendLibrary;

public class DataOperations
{

    /// <summary>
    /// Resets the data in the <c>dbo.Table1</c> table by deleting all records 
    /// and reseeding the identity column to start from 0.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="bullet">
    /// <item>Deletes all rows from the <c>dbo.Table1</c> table.</item>
    /// <item>Resets the identity seed of the <c>dbo.Table1</c> table to 0.</item>
    /// </list>
    /// Use this method with caution as it will remove all data from the table.
    /// </remarks>
    public void ResetTable1()
    {
         using SqlConnection cn = new(ConnectionString());

         cn.Execute("DELETE FROM dbo.Table1");
         cn.Execute("DBCC CHECKIDENT (Table1, RESEED, 0)");
    }

    /// <summary>
    /// Resets the data in the <c>dbo.EventAttachments</c> table by deleting all records 
    /// and reseeding the identity column to start from 0.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="bullet">
    /// <item>Deletes all rows from the <c>dbo.EventAttachments</c> table.</item>
    /// <item>Resets the identity seed of the <c>dbo.EventAttachments</c> table to 0.</item>
    /// </list>
    /// Use this method with caution as it will remove all data from the table.
    /// </remarks>
    public void ResetEventAttachments()
    {
        using SqlConnection cn = new(ConnectionString());

        cn.Execute("DELETE FROM dbo.EventAttachments");
        cn.Execute("DBCC CHECKIDENT (EventAttachments, RESEED, 0)");
    }

    /// <summary>
    /// Resets the data in the <c>dbo.Events</c> table by deleting all records 
    /// and reseeding the identity column to start from 0.
    /// </summary>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="bullet">
    /// <item>Deletes all rows from the <c>dbo.Events</c> table.</item>
    /// <item>Resets the identity seed of the <c>dbo.Events</c> table to 0.</item>
    /// </list>
    /// Use this method with caution as it will remove all data from the table.
    /// </remarks>
    public void ResetEvents()
    {
        using SqlConnection cn = new(ConnectionString());

        cn.Execute("DELETE FROM dbo.Events");
        cn.Execute("DBCC CHECKIDENT (Events, RESEED, 0)");
    }

    public bool FileExistInTable1(string fileName)
    {
        using var connection = new SqlConnection(ConnectionString());
        string query = """
                       SELECT 1 
                       FROM InsertImagesDatabase.dbo.Table1 
                       WHERE FileName = @FileName
                       """;

        return connection.QueryFirstOrDefault<int?>(query, new { FileName = fileName }) != null;
    }
    /// <summary>
    /// Inserts a file into the database by reading its contents and storing it along with its name.
    /// </summary>
    /// <param name="filePath">The full path of the file to be inserted.</param>
    /// <param name="fileName">The name of the file to be stored in the database.</param>
    /// <param name="newIdentifier"> Holds the new identifier generated for the inserted file. </param>
    /// <returns>
    /// A tuple containing a success flag and an exception object:
    /// <list type="bullet">
    /// <item><c>success</c>: <see langword="true"/> if the operation succeeds; otherwise, <see langword="false"/>.</item>
    /// <item><c>exception</c>: The exception encountered during the operation, or <see langword="null"/> if no exception occurred.</item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// This method reads the file from the specified path, converts it to a byte array, and inserts it into the database.
    /// If the operation is successful, the method returns <see langword="true"/> and sets the <paramref name="newIdentifier"/> 
    /// to the newly generated identifier for the record. If an error occurs, the method returns <see langword="false"/> 
    /// and provides the exception details.
    /// </remarks>
    /// <exception cref="IOException">Thrown if there is an error reading the file from the specified path.</exception>
    /// <exception cref="SqlException">Thrown if there is an error executing the database query.</exception>
    public (bool success, Exception exception) InsertFileSimple(string filePath, string fileName, ref int newIdentifier)
    {
        byte[] fileBytes;

        try
        {
            fileBytes = File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }

        using var cn = new SqlConnection(ConnectionString());
        const string query = 
            """
            INSERT INTO Table1 (FileContents, FileName) 
            VALUES (@FileContents, @FileName);
            SELECT CAST(SCOPE_IDENTITY() AS int);
            """;

        try
        {
            newIdentifier = cn.ExecuteScalar<int>(query, new { FileContents = fileBytes, FileName = fileName });
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }
    }


    public (bool success, Exception exception) ReadFileFromDatabaseTableSimple(string fileNameInDatabase, string fileName)
    {
        using SqlConnection cn = new() { ConnectionString = ConnectionString() };
        using SqlCommand cmd = new()
        {
            Connection = cn, 
            CommandText =
                """
                SELECT id, [FileContents], FileName
                FROM Table1
                WHERE FileName = @FileName;
                """
        };
        cmd.Parameters.Add(new SqlParameter()
        {
            ParameterName = "@FileName",
            DbType = DbType.String

        }).Value = fileNameInDatabase;

        try
        {
            cn.Open();

            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                Console.WriteLine($"\tRead back id: {reader.GetInt32(0)}");
                // the blob field
                var fieldOrdinal = reader.GetOrdinal("FileContents");

                var blob = new byte[(reader.GetBytes(fieldOrdinal, 0, null, 0, int.MaxValue))];

                reader.GetBytes(fieldOrdinal, 0, blob, 0, blob.Length);

                using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(blob, 0, blob.Length);

            }

            return (true, null);

        }

        catch (Exception ex)
        {
            return (false, ex);
        }
    }

    /// <summary>
    /// Reads a file from the database table by its identifier and writes it to the specified file path.
    /// </summary>
    /// <param name="id">The identifier of the file to retrieve from the database.</param>
    /// <param name="fileName">The full path, including the name, where the retrieved file will be saved.</param>
    /// <returns>
    /// A tuple containing a <c>bool</c> indicating success or failure, and an <see cref="Exception"/> if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method queries the database for a file with the specified identifier. If the file is found, 
    /// it writes the file contents to the specified location on the file system. If no file is found, 
    /// or if an error occurs during the operation, the method returns an appropriate exception.
    /// </remarks>
    public (bool success, Exception exception) ReadFileFromDatabaseTableById(int id, string fileName)
    {
        try
        {
            using var cn = new SqlConnection(ConnectionString());

            const string query = @"SELECT [FileContents] FROM Table1 WHERE id = @id;";

            var fileContents = cn.QueryFirstOrDefault<byte[]>(query, new { id });

            if (fileContents != null)
            {
                File.WriteAllBytes(fileName, fileContents);
                return (true, null);
            }

            return (false, new Exception("No file found with the given ID."));
        }
        catch (Exception ex)
        {
            return (false, ex);
        }
    }

    /// <summary>
    /// Inserts a new event attachment into the database and associates it with a specified event.
    /// </summary>
    /// <param name="fileName">
    /// The full path of the file to be inserted as an attachment.
    /// </param>
    /// <param name="newIdentifier">
    /// An output parameter that will contain the identifier of the newly inserted record upon successful execution.
    /// </param>
    /// <param name="eventIdentifier">
    /// The identifier of the event to which the attachment will be associated.
    /// </param>
    /// <returns>
    /// A tuple containing a boolean indicating success or failure, and an <see cref="Exception"/> object if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method reads the content of the specified file, extracts its extension and base name, 
    /// and inserts the data into the <c>EventAttachments</c> table in the database. 
    /// The method also associates the attachment with the specified event using the <paramref name="eventIdentifier"/>.
    /// </remarks>
    public (bool success, Exception exception) InsertNewEvent(string fileName, ref int newIdentifier, int eventIdentifier)
    {
        var fileExtenstion = Path.GetExtension(fileName);
        var fileBaseName = Path.GetFileNameWithoutExtension(fileName);

        byte[] fileByes;

        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                fileByes = reader.ReadBytes((int)stream.Length);
            }
        }

        using SqlConnection cn = new() { ConnectionString = ConnectionString() };
        using SqlCommand cmd = new()
        {
            Connection = cn, 
            CommandText =
                """
                INSERT INTO EventAttachments (FileContent,FileExtention,FileBaseName,EventId)
                VALUES (@FileContent,@FileExtention,@FileBaseName,@EventIdentifier);
                SELECT CAST(scope_identity() AS int);
                """
        };

        cmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, fileByes.Length).Value = fileByes;
        cmd.Parameters.Add("@FileExtention", SqlDbType.Text).Value = fileExtenstion;
        cmd.Parameters.Add("@FileBaseName", SqlDbType.Text).Value = fileBaseName;
        cmd.Parameters.Add("@EventIdentifier", SqlDbType.Int).Value = eventIdentifier;

        try
        {
            cn.Open();

            newIdentifier = Convert.ToInt32(cmd.ExecuteScalar());
            return (true, null);

        }
        catch (Exception ex)
        {
            return (false,ex);
        }
    }

    /// <summary>
    /// Retrieves the attachments associated with a specific event.
    /// </summary>
    /// <param name="id">The unique identifier of the event for which attachments are to be retrieved.</param>
    /// <returns>
    /// A <see cref="DataTable"/> containing the following columns:
    /// <list type="bullet">
    /// <item><c>Description</c>: The description of the event.</item>
    /// <item><c>FileName</c>: The name of the attached file, including its extension.</item>
    /// <item><c>id</c>: The identifier of the attachment (hidden in the result).</item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The <c>id</c> column in the returned <see cref="DataTable"/> is hidden by default.
    /// </remarks>
    public DataTable GetAttachmentsForEvent(int id)
    {
        var dt = new DataTable();
        using SqlConnection cn = new() { ConnectionString = ConnectionString() };

        using SqlCommand cmd = new()
        {
            Connection = cn, 
            CommandText =
                """
                SELECT Events.Description,EventAttachments.id,EventAttachments.FileBaseName + EventAttachments.FileExtention As FileName
                FROM EventAttachments
                INNER JOIN Events ON EventAttachments.EventId = Events.id WHERE dbo.Events.id = @Id
                """
        };

        cmd.Parameters.AddWithValue("@id", id);
        cn.Open();
        dt.Load(cmd.ExecuteReader());
        dt.Columns["id"].ColumnMapping = MappingType.Hidden;

        return dt;
    }

    /// <summary>
    /// This is a reversal from how to obtain a value in that usually
    /// a primary key is known, in this case we only know the course name
    /// which can happen but rare.
    /// </summary>
    /// <param name="courseName"></param>
    /// <returns></returns>
    public int GetCourseIdentifier(string courseName)
    {

        using SqlConnection cn = new() { ConnectionString = ConnectionString() };

        using SqlCommand cmd = new()
        {
            Connection = cn, CommandText = 
                """
                SELECT id FROM [Events]
                WHERE Description = @Title
                """
        };

        cmd.Parameters.AddWithValue("@Title", courseName);
        cn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());

    }

}