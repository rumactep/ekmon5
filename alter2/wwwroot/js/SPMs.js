function Q_2000_SPM(QUESTIONS) {
    for (var i = 0x2560; i < 0x2570; i++)
        QUESTIONS.Add(i, 1);
}

function A_2000_SPM(QUESTIONS, JSON) {
    for (var i = 0x2560; i < 0x2570; i++) {
        try {
            var vStatus = (QUESTIONS.getData(i, 1).Byte(0) != 0);
            if (vStatus) {
                var vSPM = new SPM1();
                vSPM.MPL = QUESTIONS.getData(i, 1).UInt16(1);
                vSPM.RTD_SI = 2 * (i - 0x2560) + 1;
                JSON[JSON.length] = vSPM;
            }
        } catch (e) {
        }
    }
}

function Q_3000_SPM(QUESTIONS, JSON) {
    for (var i = 0; i < JSON.length; i++) {
        QUESTIONS.Add(0x3015, JSON[i].RTD_SI);
        QUESTIONS.Add(0x3015, JSON[i].RTD_SI + 1);
    }
}

function A_3000_SPM(QUESTIONS, JSON) {
    try {
        for (var i = 0; i < JSON.length; i++) {
            var data1 = QUESTIONS.getData(0x3015, JSON[i].RTD_SI);
            var data2 = QUESTIONS.getData(0x3015, JSON[i].RTD_SI + 1);
            JSON[i].setData(data1, data2);
        }
    } catch (e) {
    }
}

function SPM1() {
    //PRIVATE
    var vDATA1 = null;
    var vDATA2 = null;

    //PUBLIC
    this.MPL = null;
    this.RTD_SI = null;
    this.setData = function(DATA1, DATA2) {
        vDATA1 = DATA1;
        vDATA2 = DATA2;
    };
    this.getValue = function() {
        if ((vDATA1.DATA == "X") || (vDATA2 == "X")) {
            //interpretation of value
            throw new Error();
        } else {
            var dBm = vDATA1.Byte(3);
            dBm = (dBm < 10 ? "0" : "") + dBm;
            var dBc = vDATA1.Byte(2);
            dBc = (dBc < 10 ? "0" : "") + dBc;

            var date = new Date(vDATA2.UInt32() * 1000);
            var day = date.getDate();
            day = (day < 10 ? "0" : "") + day;
            var month = date.getMonth() + 1;
            month = (month < 10 ? "0" : "") + month;
            var year = date.getFullYear();
            var hours = date.getHours() - 2;
            hours = (hours < 10 ? "0" : "") + hours;
            var minutes = date.getMinutes();
            minutes = (minutes < 10 ? "0" : "") + minutes;
            var timestamp = day + "/" + month + "/" + year + " - " + hours + ":" + minutes;

            return timestamp +
                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                dBc +
                " dBcsv / " +
                dBm +
                " dBmsv";
        }
    };
    this.getStatus = function() {
        if (vDATA1.DATA == "X") {
            //interpretation of status
            throw new Error();
        } else {
            return vDATA1.Byte(0);
        }
    };
}