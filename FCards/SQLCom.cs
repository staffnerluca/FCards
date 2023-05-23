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

        public SQLCom() { }

        public bool databaseExists()
        {
            bool exists = false;
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
                    conn.ChangeDatabase("staffnLu3IT");
                    conn.Close();
                }
                catch (Exception ex) {
                    conn.Close();
                    throw new NotImplementedException(); 
                }
                createTables();
            }
        }

        public void createTables()
        {
            cmd.CommandText = "CREATE TABLE[dbo].[users] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [username] NCHAR(20) NULL, [password] NCHAR(100) NULL)";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE[dbo].[flashcards] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [question] NCHAR(1000) NOT NULL, [answer] NCHAR(1000) NULL, [dueDate] datetime2 not null)";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) {
                conn.Close();
                throw new NotImplementedException();
            }
        }
        public void createExamples()
        {
            cmd.CommandText = "insert into flashcards(question, answer) values('Is HTML a programming language?', 'No, just no')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
                return -1;
            }
        }

        public void updateDueDate(DateTime newDate, int id)
        {
            cmd.CommandText = "update flashcards set dueDate = " + newDate + "where Id=" + id;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void createCard(string question, string answer)
        {
            cmd.CommandText = "insert into flashcards(question, answer, dueDate) values('" + question + "', '" + answer + "', " + DateTime.Now;
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

            int getId = rand.Next(0, max);
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
    }
}
