using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpletest {

    public class CompressorInfo {
        public int cnumber { get; set; }
        public string cip { get; set; }

        public override string ToString() {
            return $"cnumber: {cnumber}, cip: {cip}";
        }
    }

    class Program {
        static void Main(string[] args) {
            string jsonText = @"[{cnumber:4,cip:""192.168.11.208""}, {cnumber:5,cip:""192.168.11.209""}, {cnumber:8,cip:""192.168.11.211""}, {cnumber:10,cip:""192.168.11.210""}, {cnumber:12,cip:""192.168.11.207""}, {cnumber:13,cip:""192.168.11.212""}, {cnumber:14,cip:""192.168.11.221""}]";
            //string jsonText = @"[{cnumber:1,cip:""192.168.1.1""}, {cnumber:2,cip:""192.168.1.2""}]";
            var compressors = JsonConvert.DeserializeObject<List<CompressorInfo>>(jsonText);
            Console.WriteLine(jsonText);
            compressors.ForEach(Console.WriteLine);
            Console.ReadKey();
        }
    }
}
