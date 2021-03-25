using System;

namespace CharityJob.Models
{
    public class CandidateJob
    {
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
