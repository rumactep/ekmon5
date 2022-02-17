using System;
using smartlink;

var url = "http://192.168.11.208/cgi-bin/mkv.cgi";
//var url = "http://192.168.11.221/cgi-bin/mkv.cgi";
Console.WriteLine($"ElektronikonReader reading url: {url}");

var reader = new QuestionReader {
    Logger = ConsoleLogger.Instance
};
reader.LoadLanguage("Russian.txt");
var client = new HttpElektronikonClient(url);
//var client = new ElektronikonClientStub();
await reader.Run(client);
Console.WriteLine("press any key");
Console.ReadKey();
