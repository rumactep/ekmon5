using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using smartlink;

//var url = "http://192.168.11.208/cgi-bin/mkv.cgi";
//var url = "http://192.168.11.221/cgi-bin/mkv.cgi";


string[] ips = {"192.168.11.208", "192.168.11.209", "192.168.11.211", "192.168.11.210", "192.168.11.207", "192.168.11.212", "192.168.11.221"}; 
//string[] ips = {"192.168.11.208"}; 

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
//Console.WriteLine("press any key");Console.ReadKey();

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

