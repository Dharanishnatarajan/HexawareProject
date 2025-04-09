using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using CareerHub.Entities;
using CareerHub.Utilities;

namespace CareerHub.DAO
{
    public class DatabaseManager
    {
       
        public int InsertJobListing(Job job)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
                INSERT Jobs (CompanyID, JobTitle, JobDescription, JobLocation, Salary, JobType)
                VALUES (@CompanyID, @JobTitle, @JobDescription, @JobLocation, @Salary, @JobType)";
                

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyID", job.CompanyID);
                    command.Parameters.AddWithValue("@JobTitle", job.JobTitle);
                    command.Parameters.AddWithValue("@JobDescription",job.JobDescription);
                    command.Parameters.AddWithValue("@JobLocation", job.JobLocation);
                    command.Parameters.AddWithValue("@Salary", job.Salary);
                    command.Parameters.AddWithValue("@JobType", job.JobType);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public int InsertCompany(Company company)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
                    INSERT INTO Companies (CompanyName, Location)
                    VALUES (@CompanyName, @Location);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyName", company.CompanyName);
                    command.Parameters.AddWithValue("@Location", company.Location);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public int InsertApplicant(Applicant applicant)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
                    INSERT INTO Applicants (FirstName, LastName, Email, Phone, Resume)
                    VALUES (@FirstName, @LastName, @Email, @Phone, @Resume);";
                

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", applicant.FirstName);
                    command.Parameters.AddWithValue("@LastName", applicant.LastName);
                    command.Parameters.AddWithValue("@Email", applicant.Email);
                    command.Parameters.AddWithValue("@Phone", applicant.Phone);
                    command.Parameters.AddWithValue("@Resume", applicant.Resume);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool InsertJobApplication(JobApplication application)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
                    INSERT INTO Applications (JobID, ApplicantID, CoverLetter)
                    VALUES (@JobID, @ApplicantID, @CoverLetter);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobID", application.JobID);
                    command.Parameters.AddWithValue("@ApplicantID", application.ApplicantID);
                    command.Parameters.AddWithValue("@CoverLetter", application.CoverLetter);

                    return Convert.ToInt32(command.ExecuteScalar()) == 1;
                }
            }
        }

        public List<Job> GetJobListings()
        {
            List<Job> jobs = new List<Job>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
                SELECT j.*, c.CompanyName 
                FROM Jobs j
                JOIN Companies c ON j.CompanyID = c.CompanyID";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        jobs.Add(new Job
                        {
                            JobID = Convert.ToInt32(reader["JobID"]),
                            CompanyID = Convert.ToInt32(reader["CompanyID"]),
                            JobTitle = reader["JobTitle"].ToString(),
                            JobDescription = reader["JobDescription"]?.ToString(),
                            JobLocation = reader["JobLocation"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            JobType = reader["JobType"].ToString(),
                            PostedDate = Convert.ToDateTime(reader["PostedDate"]),
                        });
                    }
                }
            }

            return jobs;
        }

        public List<Company> GetCompanies()
        {
            List<Company> companies = new List<Company>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "SELECT * FROM Companies";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        companies.Add(new Company
                        {
                            CompanyID = Convert.ToInt32(reader["CompanyID"]),
                            CompanyName = reader["CompanyName"].ToString(),
                            Location = reader["Location"].ToString()
                        });
                    }
                }
            }

            return companies;
        }

        public List<Applicant> GetApplicants()
        {
            List<Applicant> applicants = new List<Applicant>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "SELECT * FROM Applicants";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        applicants.Add(new Applicant
                        {
                            ApplicantID = Convert.ToInt32(reader["ApplicantID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Resume = reader["Resume"].ToString()
                        });
                    }
                }
            }

            return applicants;
        }

        public List<JobApplication> GetApplicationsForJob(int jobID)
        {
            List<JobApplication> applications = new List<JobApplication>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
                SELECT a.*, app.FirstName, app.LastName
                FROM Applications a
                JOIN Applicants app ON a.ApplicantID = app.ApplicantID
                WHERE a.JobID = @JobID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobID", jobID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            applications.Add(new JobApplication
                            {
                                ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                                JobID = Convert.ToInt32(reader["JobID"]),
                                ApplicantID = Convert.ToInt32(reader["ApplicantID"]),
                                ApplicationDate = Convert.ToDateTime(reader["ApplicationDate"]),
                                CoverLetter = reader["CoverLetter"].ToString(),
                            });
                        }
                    }
                }
            }

            return applications;
        }
    }
}
