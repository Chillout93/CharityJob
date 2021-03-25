using CharityJob.Models;
using CharityJob.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CharityJob
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load candidates into memory, I wouldn't couple this to repo class if expanded and instead keep it as a separate responsiblity.
            // If we had logic unrelated to repo we could also wrap this in a service layer to e.g. trigger emails as part of the same call, then a controller layer to handle routing if we're
            // going full onion architecture. 
            var candidates = JsonConvert.DeserializeObject<IList<Candidate>>(File.ReadAllText($@"..\..\..\candidates.json"));
            var repo = new CandidateRepository(candidates);

            Console.WriteLine("Welcome to the CharityJob super awesome candidate matcher.");
            while (true)
            {
                Console.WriteLine("What job would you like to search for?");
                Console.Write("Answer: ");
                var result = repo.GetBestCandidateForJobWithXYearsExperience(Console.ReadLine(), 5);
                var response = result == null
                    ? "Sorry no result this time."
                    : $"Congrats we found a match! Name: {result.Name}, Email: {result.Email}, Phone: {result.Phone}";
                Console.WriteLine(response);
            }
        }
    }
}
