using System.Data;
using System.Text.Json;
using ConfigurationLibrary.Classes;
using Microsoft.Data.SqlClient;

namespace SqlServerUtilitiesLibrary;

public class DataOperations
{
   

    public static (bool success, int identifier, byte[] image, Exception exception) InsertFileSimple(string filePath)
    {
        byte[] fileByes;

        var fileName = Path.GetFileName(filePath);

        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                fileByes = reader.ReadBytes((int)stream.Length);
            }
        }

        using var cn = new SqlConnection() { ConnectionString = ConfigurationHelper.ConnectionString() };

        const string statement = "INSERT INTO Table1 (FileContents,FileName) VALUES (@FileContents,@FileName);" + 
                                 "SELECT CAST(scope_identity() AS int);";

        using var cmd = new SqlCommand() { Connection = cn, CommandText = statement };
        cmd.Parameters.Add("@FileContents", 
            SqlDbType.VarBinary, fileByes.Length).Value = fileByes;

        cmd.Parameters.AddWithValue("@FileName", fileName);

        try
        {
            cn.Open();
            var identifier = Convert.ToInt32(cmd.ExecuteScalar());
            return (true, identifier, fileByes, null);

        }
        catch (Exception ex)
        {
            byte[] bytes = {};
            return (false, 0, bytes, ex);
        }
    }
    public static async Task<(bool success, Exception exception)> ReadFileFromDatabaseTableSimple(int identifier, string fileName, CancellationToken ct)
    {
        await using var cn = new SqlConnection() { ConnectionString = ConfigurationHelper.ConnectionString() };
        const string statement = "SELECT id, [FileContents], FileName FROM Table1  WHERE id = @id;";

        await using var cmd = new SqlCommand() { Connection = cn, CommandText = statement};
        cmd.Parameters.AddWithValue("@id", identifier);

        try
        {
            await cn.OpenAsync(ct);

            var reader = await cmd.ExecuteReaderAsync(ct);

            if (reader.HasRows)
            {
                await reader.ReadAsync(ct); 

                // the blob field
                var fieldOrdinal = reader.GetOrdinal("FileContents");

                var blob = new byte[(reader.GetBytes(
                    fieldOrdinal, 0, 
                    null, 0, 
                    int.MaxValue))];

                reader.GetBytes(fieldOrdinal, 0, blob, 0, blob.Length);

                using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(blob, 0, blob.Length);

            }

            return (true, null);

        }

        catch (Exception ex)
        {
            return (false,ex);
        }
    }

    public static DataTable GetAttachmentsForEvent()
    {
        var dt = new DataTable();
        using var cn = new SqlConnection() { ConnectionString = ConfigurationHelper.ConnectionString() };
        var statement = @"SELECT id,[FileContents],[FileName] FROM dbo.Table1";

        using var cmd = new SqlCommand() { Connection = cn, CommandText = statement };
        cn.Open();
        dt.Load(cmd.ExecuteReader());
        dt.Columns["id"]!.ColumnMapping = MappingType.Hidden;

        return dt;

    }
}