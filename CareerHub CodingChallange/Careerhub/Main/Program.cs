using System;
using System.Collections.Generic;
using CareerHub.DAO;
using CareerHub.Entities;

namespace CareerHub.Main
{
    class Program
    {
        private static DatabaseManager dbManager = new DatabaseManager();

        static void Main(string[] args)
        {
            // Initialize database on startup

            while (true)
            {
                Console.WriteLine("\nCareerHub Job Board System");
                Console.WriteLine("1. Company Details");
                Console.WriteLine("2. Job Details");
                Console.WriteLine("3. Applicant Details");
                Console.WriteLine("4. Apply for a Job");
                Console.WriteLine("5. View All Job List");
                Console.WriteLine("6. View All Companies");
                Console.WriteLine("7. View All Applicants");
                Console.WriteLine("8. View Job Applications");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

               
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        RegisterCompany();
                        break;
                    case 2:
                        PostJobListing();
                        break;
                    case 3:
                        RegisterApplicant();
                        break;
                    case 4:
                        ApplyForJob();
                        break;
                    case 5:
                        ViewJobListings();
                        break;
                    case 6:
                        ViewCompanies();
                        break;
                    case 7:
                        ViewApplicants();
                        break;
                    case 8:
                        ViewJobApplications();
                        break;
                    case 9:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void PostJobListing()
        {
            Console.WriteLine("\nJob Details");

            Console.Write("Enter Company ID: ");
            int companyId = Convert.ToInt32(Console.ReadLine());


            Console.Write("Enter Job Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Job Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Job Location: ");
            string location = Console.ReadLine();

            Console.Write("Enter Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Job Type (Full-time/Part-time/Contract): ");
            string jobType = Console.ReadLine();

            var job = new Job
            {
                CompanyID = companyId,
                JobTitle = title,
                JobDescription = description,
                JobLocation = location,
                Salary = salary,
                JobType = jobType
            };

            int jobId = dbManager.InsertJobListing(job);
            Console.WriteLine($"Job posted successfully !!");
        }

        private static void RegisterCompany()
        {
            Console.WriteLine("\nCompany Details");

            Console.Write("Enter Company Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Location: ");
            string location = Console.ReadLine();

            var company = new Company
            {
                CompanyName = name,
                Location = location
            };

            int companyId = dbManager.InsertCompany(company);
            Console.WriteLine($"Company registered successfully!! ");
        }

        private static void RegisterApplicant()
        {
            Console.WriteLine("\nApplicant Details");

            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone : ");
            string phone = Console.ReadLine();

            Console.Write("Enter Resume: ");
            string resume = Console.ReadLine();

            var applicant = new Applicant
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Resume = resume
            };

            int applicantId = dbManager.InsertApplicant(applicant);
            Console.WriteLine($"Applicant registered successfully!!");
        }

        private static void ApplyForJob()
        {
            Console.WriteLine("\nApply for a Job");

            Console.Write("Enter Applicant ID: ");
            int applicantId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Job ID: ");
            int jobId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Cover Letter: ");
            string coverLetter = Console.ReadLine();

            var application = new JobApplication
            {
                JobID = jobId,
                ApplicantID = applicantId,
                CoverLetter = coverLetter
            };

            bool success = dbManager.InsertJobApplication(application);
            Console.WriteLine("Application submitted successfully!");
        }

        private static void ViewJobListings()
        {
            var jobs = dbManager.GetJobListings();
            Console.WriteLine("\nAll Job Listings:");
            foreach (var job in jobs)
            {
                Console.WriteLine($"ID: {job.JobID}, Title: {job.JobTitle}");
                Console.WriteLine($"Location: {job.JobLocation}, Salary: {job.Salary}, Type: {job.JobType}");
                Console.WriteLine($"Posted: {job.PostedDate}\n");
            }
        }

        private static void ViewCompanies()
        {
            var companies = dbManager.GetCompanies();
            Console.WriteLine("\nAll Companies:");
            foreach (var company in companies)
            {
                Console.WriteLine($"ID: {company.CompanyID}, Name: {company.CompanyName}, Location: {company.Location}");
            }
        }

        private static void ViewApplicants()
        {
            var applicants = dbManager.GetApplicants();
            Console.WriteLine("\nAll Applicants:");
            foreach (var applicant in applicants)
            {
                Console.WriteLine($"ID: {applicant.ApplicantID}, Name: {applicant.FirstName} {applicant.LastName}");
                Console.WriteLine($"Email: {applicant.Email}, Phone: {applicant.Phone}\n");
            }
        }

        private static void ViewJobApplications()
        {
            Console.Write("\nEnter Job ID to view applications: ");
            int jobId = Convert.ToInt32(Console.ReadLine());


            var applications = dbManager.GetApplicationsForJob(jobId);
            Console.WriteLine($"\nApplications for Job ID {jobId}:");

            foreach (var app in applications)
            {
                Console.WriteLine($"Application ID: {app.ApplicationID}");
                Console.WriteLine($"Application Date: {app.ApplicationDate:d}");
                Console.WriteLine($"Cover Letter: {app.CoverLetter}\n");
            }
        }
    }
}