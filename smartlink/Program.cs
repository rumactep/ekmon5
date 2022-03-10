using System;
using System.Threading.Tasks;
using smartlink;

string[] ips = {"192.168.11.208", "192.168.11.209", "192.168.11.211", "192.168.11.210", "192.168.11.207", "192.168.11.212", "192.168.11.221"};
//string[] ips = {"192.168.11.208"}; 

await ReadElektronikon();

async Task ReadElektronikon() { 
    var reader = new QuestionReader {
        Logger = ConsoleLogger.Instance
    };
    // TODO: сделать заргузку языка с каждого компрессора в отдельности, а не пользоваться общим файлом
    reader.LoadLanguage("Russian.txt");
    foreach (var ip in ips) {
        var url = "http://" + ip + "/cgi-bin/mkv.cgi";
        Console.WriteLine($"ElektronikonReader reading url: {url}");
        var client = new HttpElektronikonClient(url);
        //var client = new ElektronikonClientStub();
        await reader.Run(client);
        Console.WriteLine("");
    }    
}

public class CompressorInfo {
    // UnitId - номер устройства в модбасе
    public byte UnitId { get; set; }

    // Cnumber - номер компрессора - для понимания человеком
    public ushort Cnumber { get; set; }
    public string Cip { get; set; } = string.Empty;

    public override string ToString() {
        return $"Cnumber: {Cnumber}, UnitId: {UnitId}, Cip: {Cip}";
    }
}

