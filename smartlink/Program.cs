

using System;
using smartlink;

var url = "http://192.168.11.208/cgi-bin/mkv.cgi";
Console.WriteLine($"ElektronikonReader reading url: {url}");

var reader = new QuestionReader();
reader.Logger = ConsoleLogger.Instance;
var client = new HttpElektronikonClient(url);
await reader.Run(client);
Console.WriteLine("press any key");
Console.ReadKey();
