using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace FileSearchEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class File : ControllerBase
    {
        [HttpGet]
        [Route("GetAvailableFiles")]
        public List<string> GetAvailableFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            return files.ToList();
        }

        [HttpGet]
        [Route("SearchByFile")]
        public List<string> SearchByFile(string keyword, string file)
        {
            int no = 0;
            var matchKeywords = new List<string>();
            using(var sr = new StreamReader(file))
            {
                while(!sr.EndOfStream)
                {
                    no++;
                    var line = sr.ReadLine();
                    if(string.IsNullOrEmpty(line)) continue;
                    if(line.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >=0)
                    {
                        matchKeywords.Add("Line" + no + "    " + line);
                    }
                }
            }
            return matchKeywords;
        }

        [HttpGet]
        [Route("SearchByPath")]
        public List<MyFile> SearchByPath(string keyword, string path)
        {
            var foundFiles = new List<MyFile>();
            var files = GetAvailableFiles(path);
            foreach(var file in files)
            {
                var found = SearchByFile(keyword,file);
                if(found.Count > 0)
                {
                    foundFiles.Add(new MyFile
                    {
                        Filename = file,
                        MatchedKeyWords = found
                    });
                }

            }
            return foundFiles;
        }

    }
}
