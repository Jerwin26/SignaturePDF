using SignaturePDF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SignaturePDF.Repository
{
    public class LoginRepository
    {
        private string connectionString = "Data Source=SYSLP616;Initial Catalog=TeamTask;Integrated Security=True;";
        public Registration GetUser(Models.Login login)
        {
            Registration registration = new Registration();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_GeUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userName", login.UserName);
                    command.Parameters.AddWithValue("@password", login.Password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            registration.Id = (int)reader["id"];
                            registration.UserName = reader["userName"].ToString();
                            registration.PassWord = reader["password"].ToString();
                            registration.Name = reader["name"].ToString();
                 
                        }
                    }
                }
            }

            return registration;
        }
        public List<Document> GetAllDocumentsByUser(int userId)
        {
            List<Document> documents = new List<Document>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_GetAllDocByUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId_Fk", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Document document = new Document
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["documentName"].ToString(),
                                UserId = Convert.ToInt32(reader["userId_Fk"]),
                                Status = (Status)Convert.ToInt32(reader["status"]),
                                Documents = (byte[])reader["document"]
                            };

                            documents.Add(document);
                        }
                    }
                }
            }
            return documents;
        }
        public void uploadDocuments(Document document)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_InsertDocument", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@documentName", document.Name);
                    command.Parameters.AddWithValue("@document", document.Documents);
                    command.Parameters.AddWithValue("@status", document.Status);
                    command.Parameters.AddWithValue("@userId_Fk",document.UserId );
                    command.ExecuteReader();

                }
            }
        }

    }
}