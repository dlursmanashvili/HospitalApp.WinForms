using System.Collections.Generic;
using System.Data.SqlClient;

namespace HospitalApp.DBHelper
{
    public class PatientHelper
    {
        public List<PatientModel> LoadPatientsFromDatabase(string connectionString)
        {
            List<PatientModel> patients = new List<PatientModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT ID, FullName, BirthDate, GenderID, PhoneNumber, Address FROM dbo.Patients";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PatientModel patient = new PatientModel
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            BirthDate = reader.GetDateTime(2),
                            GenderId = reader.GetInt32(3),
                            PhoneNumber = reader.GetString(4),
                            Address = reader.GetString(5)
                        };
                        patients.Add(patient);
                    }
                }
            }
            return patients;
        }


        public bool AddPatientToDatabase(PatientModel patient, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO dbo.Patients (FullName, Dob, GenderID,Phone,Address) VALUES (@FullName, @Dob, @GenderId, @Phone,@Address)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", patient.FullName);
                    command.Parameters.AddWithValue("@Dob", patient.BirthDate);
                    command.Parameters.AddWithValue("@GenderId", patient.GenderId);
                    command.Parameters.AddWithValue("@Phone", patient.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", patient.Address);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
