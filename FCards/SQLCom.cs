using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace FCards
{
    public class SQLCom
    {
        static private string co = "";
        static private SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB; Integrated Security = True");
        static private SqlCommand cmd = new SqlCommand(co, conn);
        static private string database = "FCstaffnLu3IT";
        Dictionary<string, DateTime> difficultys = new Dictionary<string, DateTime>();
        public SQLCom() {
            difficultys.Add("easy", DateTime.Now.AddDays(7));
            difficultys.Add("intermediate", DateTime.Now.AddDays(4));
            difficultys.Add("hard", DateTime.Now.AddDays(1));
            difficultys.Add("wrong", DateTime.Now);
        }

        public bool databaseExists()
        {
            bool exists = true;
            cmd.CommandText = "select Count(*) from sys.databases where name = '" + database + "'";
            try
            {
                conn.Open();
                exists = cmd.ExecuteScalar().ToString().Equals("0") ? false : true;
                conn.Close();
            }
            catch (Exception ex) {
                conn.Close();
            }
            return exists;
        }

        public void createDatabaseIfNotExists()
        {
            
            if (!databaseExists())
            {
                cmd.CommandText = "create database " + database;
               try
               {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex) {
                    conn.Close();
                }
                createTables();
            }
        }

        public void cleanEverything()
        {
            cmd.CommandText = "drop table users";
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "drop table flashcards";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "drop database " + database;
            conn.Close();
        }

        public void connectToDB()
        {
            conn.Open();
            conn.ChangeDatabase(database);
            conn.Close();
        }

        public void createTables()
        {
            cmd.CommandText = "CREATE TABLE[dbo].[users] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [username] NCHAR(20) NULL, [password] NCHAR(100) NULL)";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE[dbo].[flashcards] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [question] NCHAR(1000) NOT NULL, [answer] NCHAR(1000) NULL, [dueDate] datetime2 null)";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) {
                conn.Close();
            }
        }
        public void createExamples()
        {
            DateTime time = DateTime.Now;
            cmd.CommandText = "insert into flashcards(question, answer) values('Is HTML a programming language?', 'No, just no')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) {
                conn.Close();
            }
        }
        public bool usernameDoesNotExist(string username)
        {
            cmd.CommandText = "select username from users";
            bool free = true;
            conn.Close();
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                conn.Close();
                while (reader.Read())
                {
                    if (reader["username"].ToString().Equals(username))
                        free = false;
                }
            }
            catch (SqlException ex) {
                conn.Close();
            }
            return free;
        }
        public int getCardID(string front, string back)
        {
            cmd.CommandText = "select Id from flashcards where question = '" + front + "' and answer = '" + back + "'";
            try
            {
                conn.Open();
                int cId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                return cId;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void createCard(string question, string answer)
        {
            cmd.CommandText = "insert into flashcards(question, answer) values('" + question + "', '" + answer+ "')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) { }
        }

        public List<String> getRandomCard()
        {
            //In production we would also check for the due date, but for presentation purposes this is ignored, because otherwise we would get an empty list on most days
            List<String> list = new List<String>();
            cmd.CommandText = "select * from flashcards";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Random rand = new Random();
            int max = 0;
            while(reader.Read())
            {
                max = (int)reader["Id"];
            }
            int getId = rand.Next(1, max);
            while (reader.Read())
            {
                if ((int)reader["Id"] == getId)
                {
                    list.Add(reader["Id"].ToString());
                    list.Add(reader["question"].ToString());
                    list.Add(reader["answer"].ToString());
                    break;
                }
            }
            conn.Close();
            return list;
        }

        public List<string> getAllCards()
        {
            List<string> cards = new List<string> ();
            cmd.CommandText = "select * from flashcards";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cards.Add(reader["Id"].ToString());
                cards.Add(reader["question"].ToString());
                cards.Add(reader["answer"].ToString());
            }
            conn.Close();
            return cards;
        }

        public void updateDueDate(string key, int cardID)
        {
            cmd.CommandText = "update flashcards set DueDate = @DTVal where cardID = " + cardID;
            //best practice to write secure code. Sorce ChatGPT
            cmd.Parameters.AddWithValue("@DTVal", difficultys[key]);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
