using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest
{
    class SQLiteHandler
    {
    
       
       private static readonly Lazy<SQLiteHandler>
       lazy =
       new Lazy<SQLiteHandler>
           (() => new SQLiteHandler());

        public static SQLiteHandler Instance { get { return lazy.Value; } }

        private SQLiteHandler()
        {
        }

        string source = null;
        string password = null;

        string connectionString = null;
        bool isSecure = false;

        public bool CreateDatabase(String source,String password = null)
        {
            try
            {
                if(connectionString == null)
                {
                    if(source != null)
                    {
                        if(password != null)
                        {
                            isSecure = true;
                            connectionString = "Data Source=" + source + ";Version=3;Password=" + password + ";";
                        }
                        else
                        {
                            isSecure = false;
                            connectionString = "Data Source=" + source + ";Version=3;";

                        }

                        if (!File.Exists(source))
                        {
                           SQLiteConnection.CreateFile(source);
                        }

                        using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
                        {
                            string sql = "CREATE TABLE IF NOT EXISTS USERS (id integer PRIMARY KEY AUTOINCREMENT, name VARCHAR(20), date TEXT DEFAULT CURRENT_TIMESTAMP);";
                            cnn.Open();
                            if(password != null)
                            {
                                //Encrypts Database
                                cnn.ChangePassword(password);

                            }

                            SQLiteCommand command = new SQLiteCommand(sql, cnn);
                            command.ExecuteNonQuery();

                        }


                    }
                    else
                    {
                        throw new Exception("Please Specify the source and database version");
                    }
                }
                else
                {
                    throw new Exception("Alrady Initiated");
                }
                

                return true;
            }
            catch (Exception e)
            {
                connectionString = null;
                Console.WriteLine(e);
                return false;
            }
        }

       

        public List<User> LoadUsers()
        {
            try
            {
                if(connectionString != null)
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
                    {
                        List<User> list = new List<User>();
                        cnn.Open();
                        string sql = "SELECT * FROM USERS";
                        SQLiteCommand command = new SQLiteCommand(sql, cnn);
                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine("Name: " + reader[1] + "Birthday: " + reader[2]);
                            list.Add(new User() { Id = int.Parse(reader[0].ToString()), Name = reader[1].ToString(), TimeStamp = DateTime.Parse(reader[2].ToString()) });
                   
                        }

                        return list;
                    }
                }
                else
                {
                    throw new Exception("Connection Not Initialized");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            
        }

        public bool SaveUser(User user)
        {
            try
            {
                if (connectionString != null)
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
                    {
                        cnn.Open();
                        string sql = "INSERT INTO USERS (name) VALUES('"+user.Name+"');";
                        SQLiteCommand command = new SQLiteCommand(sql, cnn);
                        if(command.ExecuteNonQuery() == 0)
                        {
                            throw new Exception("User Save Failed");
                        }
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Connection Not Initialized");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public bool DeleteUser(int id)
        {
            try
            {
                if (connectionString != null)
                {
                    using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
                    {
                        cnn.Open();
                        string sql = "DELETE FROM USERS WHERE id="+id+";";
                        SQLiteCommand command = new SQLiteCommand(sql, cnn);
                        if(command.ExecuteNonQuery() == 0)
                        {
                            throw new Exception("User Save Failed");
                        }
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Connection Not Initialized");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

 
    }
}
