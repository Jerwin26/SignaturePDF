using SignaturePDF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace SignaturePDF.Repository
{
    public class LoginRepository
    {
        private string connectionString = "Data Source=syslp616;Initial Catalog=TeamTask;Integrated Security=True;Encrypt=False";
        public Registration GetUser(Login login)
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
                            // Populate the registration object with user data
                            registration.Id = (int)reader["id"];
                            registration.UserName = reader["userName"].ToString();
                            registration.PassWord = reader["password"].ToString();
                            registration.Name = reader["name"].ToString();
                            // Add other properties as needed

                            // Example: registration.Email = reader["email"].ToString();
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

                    // Add parameter for the stored procedure
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
                                // Add other properties as needed
                            };

                            documents.Add(document);
                        }
                    }
                }
            }

            return documents;
        }

    }
}