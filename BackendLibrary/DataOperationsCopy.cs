using System.Data;
using System.Reflection.PortableExecutable;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BackendLibrary;

public class DataOperationsOld 
{

    public void ResetTable1()
    {
         using SqlConnection cn = new(ConnectionString());

         cn.Execute("DELETE FROM dbo.Table1");
         cn.Execute("DBCC CHECKIDENT (Table1, RESEED, 0)");
    }

    public void ResetEventAttachments()
    {
        using SqlConnection cn = new(ConnectionString());

        cn.Execute("DELETE FROM dbo.EventAttachments");
        cn.Execute("DBCC CHECKIDENT (EventAttachments, RESEED, 0)");
    }

    public void ResetEvents()
    {
        using SqlConnection cn = new(ConnectionString());

        cn.Execute("DELETE FROM dbo.Events");
        cn.Execute("DBCC CHECKIDENT (Events, RESEED, 0)");
    }

    public (bool success, Exception exception) InsertFileSimple(string filePath, string fileName, ref int newIdentifier)
    {
        byte[] fileByes;

        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                fileByes = reader.ReadBytes((int)stream.Length);
            }
        }

        using var cn = new SqlConnection() { ConnectionString = ConnectionString() };
        using var cmd = new SqlCommand()
        {
            Connection = cn,
            CommandText =
                """
                INSERT INTO Table1 (FileContents,FileName) VALUES (@FileContents,@FileName);
                SELECT CAST(scope_identity() AS int);
                """
        };

        cmd.Parameters.Add("@FileContents", SqlDbType.VarBinary, fileByes.Length).Value = fileByes;
        cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.Text)).Value = fileName;

        try
        {
            cn.Open();

            newIdentifier = Convert.ToInt32(cmd.ExecuteScalar());
            Console.WriteLine($"\tinserted identifier: {newIdentifier}");
            return (true,null);

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
    public (bool success, Exception exception) ReadFileFromDatabaseTableById(int id, string fileName)
    {
        using SqlConnection cn = new() { ConnectionString = ConnectionString() };
        using SqlCommand cmd = new()
        {
            Connection = cn,
            CommandText =
                """
                SELECT id, [FileContents], FileName
                FROM Table1
                WHERE id = @id;
                """
        };
        cmd.Parameters.Add(new SqlParameter()
        {
            ParameterName = "@id",
            DbType = DbType.Int32

        }).Value = id;

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
    /// 
    /// </summary>
    /// <param name="fileName">Path and file name to insert</param>
    /// <param name="newIdentifier">id for new record</param>
    /// <param name="eventIdentifier">id for parent row</param>
    /// <returns></returns>
    /// <remarks>
    /// Note that I'm storing file name and extension in two fields, I could have
    ///  done just the file name or had a column for file type or mime type.
    /// 
    /// What matters for a real app is do we need the original file name or
    /// allow the user a new file name.
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