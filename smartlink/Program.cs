

using System;
using smartlink;

Console.WriteLine("Hello, World!");

var reader = new ElektronikonReader();
var client = new ElektronikonClient("192.168.100.100/cgi-bin/mkv.cgi");
ElektronikonRequest request = await reader.Run(client);
Console.WriteLine(request.GetDataString());
