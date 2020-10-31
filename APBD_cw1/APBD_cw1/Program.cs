using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APBD_cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if(args.Length < 1)
            {
                throw new ArgumentNullException("args", "not enough arguments");
            }

            if(!jestPoprawnymURL(args[0]))
            {
                throw new ArgumentException("przekazany parametr nie jest poprawnym adresem URL");
            }


            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://www.pja.edu.pl");


            if(response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                var regex = new Regex("[a-z0-9]+@[a-z.]+");

                MatchCollection matches = regex.Matches(html);
                foreach(var match in matches.OfType<Match>().Select(m => m.Value).Distinct())
                {
                    Console.WriteLine(match);
                }

                if(matches.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono zadnego adresu email");
                }

                httpClient.Dispose();  
            }
            else
            {
                Console.WriteLine("Błąd wczasie pobierania strony");
            }
          
           Console.WriteLine("Koniec");
        }

        public static bool jestPoprawnymURL(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
