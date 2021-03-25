using CharityJob.Models;
using CharityJob.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CharityJob.Repositories
{
    public class CandidateRepository
    {
        private IList<Candidate> _candidates;

        public CandidateRepository(IList<Candidate> candidates)
        {
            _candidates = candidates;
        }

        public Candidate GetBestCandidateForJobWithXYearsExperience(string jobTitle, int yearsExperience)
        {
            if (string.IsNullOrEmpty(jobTitle)) return null;

            // A naive approach converting years required to days required so we can sum a candidate with e.g. 4x6month roles qualifying for an "at least 2 years exp." role. 
            var totalDaysExperienceRequired = yearsExperience * 365;

            return _candidates
                .Select(c => c.WorkHistory
                    .Where(wh => StringUtil.FuzzySearch(wh.JobTitle.ToLower(), jobTitle.ToLower()) < 5) // Fuzzy search for fun, potential performance implications would not scale well.  
                    .Select(wh => new { Candidate = c, DaysExperience = ((wh.EndDate ?? DateTime.UtcNow) - wh.StartDate).TotalDays }))
                .SelectMany(x => x)
                .GroupBy(x => x.Candidate.Name)
                .Select(x => new { x.Key, Candidate = x.First().Candidate, TotalDaysExperience = x.Sum(y => y.DaysExperience) })
                .Where(x => x.TotalDaysExperience >= totalDaysExperienceRequired)
                .OrderByDescending(x => x.TotalDaysExperience)
                .Select(x => x.Candidate)
                .FirstOrDefault();
        }
    }
}
