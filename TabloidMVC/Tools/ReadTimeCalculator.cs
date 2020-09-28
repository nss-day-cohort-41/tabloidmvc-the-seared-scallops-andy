using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Tools
{
    public class ReadTimeCalculator : IReadTimeCalculator
    {
        public int CalculateReadTime(string content)
        {
            List<string> wordsOfContent = content.Split(" ").ToList();
            int wordCount = wordsOfContent.Count();
            int timeToRead = wordCount / 265;
            return timeToRead;
        }
    }
}
