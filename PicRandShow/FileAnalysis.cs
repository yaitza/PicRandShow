using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicRandShow
{
    class FileAnalysis
    {
        private string filePath;

        private List<string> files = new List<string>();

        public FileAnalysis(string filePath)
        {
            this.filePath = filePath;
        }

        public List<string> GetAllFile()
        {
            if(!Directory.Exists(this.filePath))
            {
                throw new DirectoryNotFoundException($"{this.filePath} Not Exists!");
            }

            this.RecursionGetFiles(this.filePath);
            return files;
        }

        public void RecursionGetFiles(string filePath)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);

            foreach(DirectoryInfo folder in di.GetDirectories())
            {
                RecursionGetFiles(folder.FullName);
            }

            foreach(FileInfo file in di.GetFiles())
            {
                this.files.Add($"{filePath}\\{file.Name}");
            }
            
        }
    }
}
