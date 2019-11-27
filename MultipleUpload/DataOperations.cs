using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using static MultipleUpload.Properties.Settings;

namespace MultipleUpload
{
    public class DataOperations
    {
        public delegate void FileHandler(object sender, InsertFileArgs myArgs);
        public event FileHandler OnLineHandler;

        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Takes a list of files and inserts them into a table with a delegate
        /// which provides the caller information to see what's going on in real time.
        /// </summary>
        /// <param name="files">List of files including their path</param>
        /// <returns>Success or failure</returns>
        public bool InsertFiles(List<string> files)
        {
            /*
             * in line method to get a file byte array suitable for inserting
             * a new record into a table.
             */
            byte[] GetFileBytes(string fileName)
            {
                byte[] fileByes;

                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        fileByes = reader.ReadBytes((int)stream.Length);
                    }
                }

                return fileByes;
            }

            const string statement = "INSERT INTO Table1 (FileContents,FileName)" + 
                                     " VALUES (@FileContents,@FileName);" +
                                     "SELECT CAST(scope_identity() AS int);";


            using (var cn = new SqlConnection() {ConnectionString = Default.ConnectionString})
            {
                using (var cmd = new SqlCommand() {Connection = cn, CommandText = statement})
                {
                    cn.Open();

                    cmd.Parameters.Add("@FileContents", SqlDbType.VarBinary);
                    cmd.Parameters.Add("@FileName", SqlDbType.VarChar);

                    /*
                     * iterate the file array, insert file
                     */
                    foreach (var fileName in files)
                    {
                        var fileByes = GetFileBytes(fileName);
                        cmd.Parameters["@FileContents"].Size = fileByes.Length;
                        cmd.Parameters["@FileContents"].Value = fileByes;
                        cmd.Parameters["@FileName"].Value = Path.GetFileName(fileName);

                        OnLineHandler(this, new InsertFileArgs(new[]
                        {
                            Convert.ToInt32(cmd.ExecuteScalar()).ToString(),
                            fileName
                        })) ;

                    }
                }

            }

            return true;
        }

    }

    public class InsertFileArgs : EventArgs
    {
        protected string[] Line;

        public InsertFileArgs(string[] sender)
        {
            Line = sender;
        }
        public string Identifier => Line[0];
        public string FileName => Line[1];
    }
}
