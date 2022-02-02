

using System;
using smartlink;

var url = "http://192.168.11.208/cgi-bin/mkv.cgi";
Console.WriteLine($"ElektronikonReader reading url: {url}");

var reader = new QuestionReader();
var client = new HttpElektronikonClient(url);
ElektronikonRequest request = await reader.Run(client);
Console.WriteLine(request.GetDataString());
