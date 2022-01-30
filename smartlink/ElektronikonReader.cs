using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace smartlink;

public class ElektronikonReader {
    public static ElektronikonRequest GetElektronikonRequest4() {
        ElektronikonRequest er = new ElektronikonRequest();
        LoadSettings(er);
        return er;
    }

    public static void LoadSettings(ElektronikonRequest er) {
        Q_2000_AI(er);
        Q_2000_DI(er);
        Q_2000_CNT(er);
        Q_2000_CNV(er); // 1st
        Q_2000_DO(er);
        Q_2000_CAI(er);

        Q_2000_CNV(er); // 2nd for some reason !

        Q_2000_SPR(er);
        Q_2000_AO(er);
        Q_2000_SPM(er);
        Q_3000_ES(er);
        Q_2000_SPL(er);
        Q_2000_MMT(er);
    }

    private static void Q_2000_AI(ElektronikonRequest er) {
        for (var i = 0x2010; i < 0x2090; i++) {
            er.Add(i, 1);
            er.Add(i, 4);
        }
    }

    private static void Q_2000_DI(ElektronikonRequest er) {
        for (var i = 0x20b0; i < 0x2100; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_CNT(ElektronikonRequest er) {
        for (var i = 1; i < 256; i++)
            er.Add(0x2607, i);
    }

    private static void Q_2000_CNV(ElektronikonRequest er) {
        for (var i = 0x2681; i < 0x2689; i++) {
            er.Add(i, 1);
            er.Add(i, 7);
        }
    }

    private static void Q_2000_DO(ElektronikonRequest er) {
        for (var i = 0x2100; i < 0x2150; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_CAI(ElektronikonRequest er) {
        for (var i = 0x2090; i < 0x20b0; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }

    private static void Q_2000_SPR(ElektronikonRequest er) {
        for (var i = 0x2300; i < 0x247F; i++)
            er.Add(i, 1);
    }

    private static void Q_2000_AO(ElektronikonRequest er) {
        for (var i = 0x2150; i < 0x2170; i++) {
            er.Add(i, 1);
            er.Add(i, 3);
        }
    }

    private static void Q_2000_SPM(ElektronikonRequest er) {
        for (var i = 0x2560; i < 0x2570; i++)
            er.Add(i, 1);
    }

    private static void Q_3000_ES(ElektronikonRequest er) {
        er.Add(0x3113, 1);
        for (var i = 3; i <= 5; i++)
            er.Add(0x3113, i);
        for (var i = 7; i <= 30; i++)
            er.Add(0x3113, i);
        for (var i = 31; i <= 42; i++)
            er.Add(0x3113, i);
        for (var i = 1; i <= 18; i++)
            er.Add(0x3114, i);
    }

    private static void Q_2000_SPL(ElektronikonRequest er) {
        for (var i = 1; i < 21; i++)
            er.Add(0x2602, i);
    }

    private static void Q_2000_MMT(ElektronikonRequest er) {
        er.Add(0x2001, 1);
    }

    public async Task<ElektronikonRequest> Run(IElektronikonClient client) {
        ElektronikonRequest er = GetElektronikonRequest4();
        const int step1000 = 1000;
        for (int idx = 0; idx < er.Length; idx += step1000) {
            int to = Math.Min(idx + step1000, er.Length);
            string questionsString = er.GetRequestString(idx, to);
            string answersString = await client.AskAsync(questionsString);

            for (int iQ = idx, iA = 0; iQ < to; iQ++) {
                if (answersString != null && answersString[iA] != 'X') {
                    string substring = answersString.Substring(iA, 8);
                    er.SetData(iQ, substring);
                    iA += 8;
                }
                else {
                    er.SetData(iQ, "X");
                    iA++;
                }
            }
        }
        return er;
    }
}