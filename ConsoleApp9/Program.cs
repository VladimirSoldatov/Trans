using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=SharpDemo;Trusted_Connection=True;");
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            Console.Write("Enter Yes to begin transection:\t");
            string choice = Console.ReadLine();
            switch(choice)
            {
                case "yes":
                    SqlTransaction sqlTransaction = null;
                    SqlDataReader sqlDataReader = null;
                    try
                    {
                        sqlConnection.Open();
                        sqlTransaction = sqlConnection.BeginTransaction();
                        sqlCommand = sqlConnection.CreateCommand();
                        sqlCommand.Transaction = sqlTransaction;
                        sqlCommand.CommandText = @"update Persons set age = age + 10 Where name = 'Kolyan'";
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.CommandText = "Select * from Persons";
                        sqlDataReader = sqlCommand.ExecuteReader();
                        {
                            if(sqlDataReader.HasRows)
                            {
                                while (sqlDataReader.Read()) 
                                {
                                    Console.WriteLine($"{sqlDataReader.GetString(1)}:{sqlDataReader.GetValue(2)}");
                                }
                            }
                        }
                        sqlDataReader.Close();
                        sqlCommand.CommandText = @"update Persons set age = age - 10 Where name = 'Sergey'";
                        sqlCommand.ExecuteNonQuery();

                        sqlTransaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        sqlTransaction.Rollback();

                    }
                    break;
                case "no":
                    try
                    {
                        sqlConnection.Open();

                        sqlCommand.CommandText = "Select * from Persons";
                        sqlDataReader = sqlCommand.ExecuteReader();
                        {
                            if (sqlDataReader.HasRows)
                            {
                                while (sqlDataReader.Read())
                                {
                                    Console.WriteLine($"{sqlDataReader.GetString(1)}:{sqlDataReader.GetValue(2)}");
                                }
                            }
                        }
                        sqlDataReader.Close();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
