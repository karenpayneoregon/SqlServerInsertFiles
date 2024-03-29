﻿using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using static WindowsFormsApplication1.Properties.Settings;

namespace WindowsFormsApplication1
{
    public class DataOperations
    {
        public string ExceptionMessage { get; set; } 

        public bool InsertFileSimple(string FilePath, string FileName, ref int NewIdentifier)
        {
            byte[] fileByes;

            using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    fileByes = reader.ReadBytes((int)stream.Length);
                }
            }

            using (var cn = new SqlConnection() { ConnectionString = Default.ConnectionString })
            {
                const string statement = "INSERT INTO Table1 (FileContents,FileName) VALUES (@FileContents,@FileName);" + 
                                         "SELECT CAST(scope_identity() AS int);";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement })
                {
                    cmd.Parameters.Add("@FileContents", 
                        SqlDbType.VarBinary, fileByes.Length).Value = fileByes;

                    cmd.Parameters.AddWithValue("@FileName", FileName);

                    try
                    {
                        cn.Open();

                        NewIdentifier = Convert.ToInt32(cmd.ExecuteScalar());
                        return true;

                    }
                    catch (Exception ex)
                    {
                        ExceptionMessage = ex.Message;
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Extract file by name in database, the original code extracted by id and seemed to be
        /// a good idea at the time yest there is no guarantee of the idea for a specific file.
        /// </summary>
        /// <param name="fileNameInDatabase">file name in table, column FileName</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool ReadFileFromDatabaseTableSimple(string fileNameInDatabase, string fileName)
        {
            using (var cn = new SqlConnection() { ConnectionString = Default.ConnectionString })
            {
                const string statement = "SELECT id, [FileContents], FileName FROM Table1  WHERE FileName = @FileName;";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement})
                {
                    cmd.Parameters.AddWithValue("@FileName", fileNameInDatabase);

                    try
                    {
                        cn.Open();

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read(); 

                        // the blob field
                        var fieldOrdinal = reader.GetOrdinal("FileContents");

                        var blob = new byte[(reader.GetBytes(
                            fieldOrdinal, 0, 
                            null, 0, 
                            int.MaxValue))];

                        reader.GetBytes(fieldOrdinal, 0, blob, 0, blob.Length);

                        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                            fs.Write(blob, 0, blob.Length);

                    }
                        return true;
                    }

                    catch (Exception ex)
                    {
                        ExceptionMessage = ex.Message;
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName">Path and file name to insert</param>
        /// <param name="NewIdentifier">id for new record</param>
        /// <param name="EventIdentifier">id for parent row</param>
        /// <returns></returns>
        /// <remarks>
        /// Note that I'm storing file name and extension in two fields, I could have
        ///  done just the file name or had a column for file type or mime type.
        /// 
        /// What matters for a real app is do we need the original file name or
        /// allow the user a new file name.
        /// </remarks>
        public bool InsertNewEvent(string FileName, ref int NewIdentifier, int EventIdentifier)
        {
            var fileExtenstion = Path.GetExtension(FileName);
            var fileBaseName = Path.GetFileNameWithoutExtension(FileName);

            byte[] fileByes;

            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    fileByes = reader.ReadBytes((int)stream.Length);
                }
            }

            using (var cn = new SqlConnection() { ConnectionString = Default.ConnectionString })
            {

                var statement = "INSERT INTO EventAttachments (FileContent,FileExtention,FileBaseName,EventId) " + 
                                "VALUES (@FileContent,@FileExtention,@FileBaseName,@EventIdentifier);" + 
                                "SELECT CAST(scope_identity() AS int);";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement })
                {

                    cmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, fileByes.Length).Value = fileByes;
                    cmd.Parameters.AddWithValue("@FileExtention", fileExtenstion);
                    cmd.Parameters.AddWithValue("@FileBaseName", fileBaseName);
                    cmd.Parameters.AddWithValue("@EventIdentifier", EventIdentifier);

                    try
                    {
                        cn.Open();

                        NewIdentifier = Convert.ToInt32(cmd.ExecuteScalar());
                        return true;

                    }
                    catch (Exception ex)
                    {
                        ExceptionMessage = ex.Message;
                        return false;
                    }
                }
            }
        }
        public DataTable GetAttachmentsForEvent(int id)
        {
            var dt = new DataTable();
            using (var cn = new SqlConnection() { ConnectionString = Default.ConnectionString })
            {
                var statement = @"
                SELECT Events.Description,EventAttachments.id,EventAttachments.FileBaseName + EventAttachments.FileExtention As FileName
                FROM EventAttachments 
                INNER JOIN Events ON EventAttachments.EventId = Events.id WHERE dbo.Events.id = @Id";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement })
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    dt.Load(cmd.ExecuteReader());
                    dt.Columns["id"].ColumnMapping = MappingType.Hidden;
                }

            }

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
            int identifier = 0;
            using (var cn = new SqlConnection() { ConnectionString = Default.ConnectionString })
            {
                const string statement = "SELECT id FROM [Events] WHERE Description = @Title";

                using (var cmd = new SqlCommand() { Connection = cn, CommandText = statement })
                {
                    cmd.Parameters.AddWithValue("@Title", courseName);
                    cn.Open();
                    identifier = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return identifier;
        }

    }
}
