using System.Collections.Generic;

namespace CharityJob.Models
{
    public class Candidate
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public IList<CandidateJob> WorkHistory { get; set; }
    }
}
