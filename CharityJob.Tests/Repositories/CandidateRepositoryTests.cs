using CharityJob.Models;
using CharityJob.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CharityJob.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithCandidatesMatchingCriteria_ReturnTheCorrectCandidate()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { 
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1).AddDays(1) } 
            } };
            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("Software Developer", 1);

            // Assert
            Assert.AreEqual(candidate.Name, result.Name);
        }

        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithCandidateWithoutEndDate_CalculatesEndDateAsStartDate()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { 
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow.AddYears(-1).AddDays(-1) } 
            } };
            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("Software Developer", 1);

            // Assert
            Assert.AreEqual(candidate.Name, result.Name);
        }

        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithCandidateWithMultipleJobRolesMatching_SumsUpAllResults()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { 
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(6) },
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(6).AddDays(1) }
            } };

            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("Software Developer", 1);

            // Assert
            Assert.AreEqual(candidate.Name, result.Name);
        }

        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithCandidatesNotMatchingJobDescription_AreNotReturnedInResponse()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { 
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1).AddDays(1) } 
            } };
            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("Software Developer 123", 1);

            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithCandidatesNotMatchingTimeRequired_AreNotReturnedInResponse()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { 
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1).AddDays(-1) } 
            } };
            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("Software Developer", 1);

            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithMultipleCandidatesMatchingCriteria_ReturnsCandidateWithMostExperience()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { 
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1).AddDays(1) } 
            } };
            var candidate2 = new Candidate { Name = "Nick 2", WorkHistory = new List<CandidateJob> {
                new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1).AddDays(2) }
            } };
            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate, candidate2 });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("Software Developer", 1);

            // Assert
            Assert.AreEqual(candidate2.Name, result.Name);
        }

        [Test]
        public void GetBestCandidateForJobWithXYearsExperience_WithEmptyJobTitle_ReturnsNull()
        {
            // Arrange
            var candidate = new Candidate { Name = "Nick", WorkHistory = new List<CandidateJob> { new CandidateJob { JobTitle = "Software Developer", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1).AddDays(1) } } };
            var candidateRepo = new CandidateRepository(new List<Candidate> { candidate });

            // Act
            var result = candidateRepo.GetBestCandidateForJobWithXYearsExperience("", 1);

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}