using System;
using System.Collections.Generic;

namespace smartlink; 

public class ElektronikonReader{
    public static ElektronikonRequest GetElektronikonRequest4() {
        ElektronikonRequest er = new ElektronikonRequest();
        for (int i = 0x2010; i < 0x2090; i++) {
            er.AddQuestion(new DataItem(i, 1));
            er.AddQuestion(new DataItem(i, 4));
            // er.AddQuestion(new DataItem(i, 6));
        }
        return er;
    }
    public static ElektronikonRequest GetElektronikonRequest6() {
        ElektronikonRequest er = new ElektronikonRequest();
        for (int i = 0x2010; i < 0x2090; i++) {
            er.AddQuestion(new DataItem(i, 1));
            er.AddQuestion(new DataItem(i, 4));
            er.AddQuestion(new DataItem(i, 6));
        }
        return er;
    }

    public async void Run() {
        // 192.168.11.208/cgi-bin/mkv.cgi
        ElektronikonClient client = new ElektronikonClient("192.168.100.100/cgi-bin/mkv.cgi");
        ElektronikonRequest request = GetElektronikonRequest4();
        await client.AskAsync(request);

//        foreach ((DataItem key, string value) in answer) 
//            Console.WriteLine($"{key} -> {value}"); 
    }


    // public void SenReceive() {
    //     var v1000 = 1000;
    //     for (var idx = 0; idx < vQUESTIONS.length; idx += v1000) {
    //         var vQuestionsSlice;
    //         if ((vQUESTIONS.length - idx) <= v1000)
    //             vQuestionsSlice = vQUESTIONS.slice(idx, vQUESTIONS.length);
    //         else
    //             vQuestionsSlice = vQUESTIONS.slice(idx, idx + v1000);
    //
    //         var vQuestions = "";
    //         for (var iQ = 0; iQ < vQuestionsSlice.length; iQ++)
    //             vQuestions += HexString(vQuestionsSlice[iQ].INDEX, 4) + HexString(vQuestionsSlice[iQ].SUBINDEX, 2);
    //
    //         //var url208 = "http://" + document.location.hostname + "/cgi-bin/mkv.cgi";
    //         var url208 = "http://" + "192.168.11.208" + "/cgi-bin/mkv.cgi";
    //         var vAnswers = Post(url208, vQuestions); //
    //
    //         for (var iQ = 0, iA = 0; iQ < vQuestionsSlice.length; iQ++) {
    //             if (vAnswers != null && vAnswers.charAt(iA) != "X") {
    //                 vQuestionsSlice[iQ].setData(vAnswers.substring(iA, iA + 8));
    //                 iA += 8;
    //             }
    //             else {
    //                 vQuestionsSlice[iQ].setData("X");
    //                 iA++;
    //             }
    //         }
    //     }
    // }

    private Dictionary<DataItem, string> ConvertToAnswer(string answerString) {
        return new Dictionary<DataItem, string>();
    }
}