using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace form_video
{
    public delegate void ProgressoDownload(double progresso);
    public class HandleM3U8
    {
        public event ProgressoDownload ProgressoDownload;

        private readonly HttpClient client;
        public string Url { get; private set; }

        private double _progresso;
        public double Progresso { get => _progresso ; private set
            {
                _progresso = value;
                ProgressoDownload?.Invoke(_progresso);
            } }

        private Regex regex = new Regex(@"^video\d+\.ts$");
        public HandleM3U8(string url)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "xrl-enconded");

            Url = url;
        }

        public async Task<string> Download(string path, string fileName = null, bool fileTs = false)
        {
            var url = fileTs ? Url.Replace("video.m3u8","") + fileName : Url;
            var file = await client.GetStreamAsync(url);

            var filePath = fileName != null ? Path.Combine(path, fileName) : Path.Combine(path, "video.m3u8");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task DownloadTSFiles(string filePathM3u8, string path)
        {
                int totalLinhas = File.ReadAllLines(filePathM3u8).Length;
                var sr = new StreamReader(filePathM3u8);
                string linha;
                int numLinhas = 0;
                
                while ((linha = sr.ReadLine()) != null)
                {
                numLinhas++;
                Progresso = (double)numLinhas / totalLinhas * 100;

                    var isMatch = regex.IsMatch(linha);
                    if (isMatch)
                    {
                        await Download(path, linha, true);
                    }
                }
                sr.Close();
        }
    }
}
