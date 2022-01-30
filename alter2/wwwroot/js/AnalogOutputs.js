function Q_2000_AO(QUESTIONS) {
    for (var i = 0x2150; i < 0x2170; i++) {
        QUESTIONS.Add(i, 1);
        QUESTIONS.Add(i, 3);
    }
}

function A_2000_AO(QUESTIONS, JSON) {
    for (var i = 0x2150; i < 0x2170; i++) {
        try {
            var vStatus = (QUESTIONS.getData(i, 1).Byte(0) != 0);
            if (vStatus) {
                var vAnalogOutput = new AnalogOutput();

                vAnalogOutput.MPL = QUESTIONS.getData(i, 1).UInt16(1);
                vAnalogOutput.OUTPUTTYPE = QUESTIONS.getData(i, 1).Byte(1);
                vAnalogOutput.DISPLAYPRECISION = QUESTIONS.getData(i, 3).Byte(3);
                vAnalogOutput.RTD_SI = i - 0x2150 + 1;
                JSON[JSON.length] = vAnalogOutput;
            }
        } catch (e) {
        }
    }
}

function Q_3000_AO(QUESTIONS, JSON) {
    for (var i = 0; i < JSON.length; i++)
        QUESTIONS.Add(0x3006, JSON[i].RTD_SI);
}

function A_3000_AO(QUESTIONS, JSON) {
    try {
        for (var i = 0; i < JSON.length; i++)
            JSON[i].setData(QUESTIONS.getData(0x3006, JSON[i].RTD_SI));
    } catch (e) {
    }
}

function AnalogOutput() {
    //PRIVATE
    var vDATA = null;

    //PUBLIC
    this.MPL = null;
    this.OUTPUTTYPE = null;
    this.DISPLAYPRECISION = null;
    this.RTD_SI = null;
    this.setData = function(DATA) {
        vDATA = DATA;
    };
    this.getValue = function() {
        if (vDATA.DATA == "X") {
            //interpretation of value
            throw new Error();
        } else {
            return vDATA.Int16(1);
        }
    };
    this.getStatus = function() {
        if (vDATA.DATA == "X") {
            //interpretation of status
            throw new Error();
        } else {
            return vDATA.UInt16(0);
        }
    };
}