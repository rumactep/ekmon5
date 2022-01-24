function Q_2000_AI(QUESTIONS) {
    for (var i = 0x2010; i < 0x2090; i++) {
        QUESTIONS.Add(i, 1);
        QUESTIONS.Add(i, 4);
    }
}

function A_2000_AI(QUESTIONS, JSON) {
    for (var i = 0x2010; i < 0x2090; i++) {
        try {
            var vData1 = QUESTIONS.getData(i, 1);
            var byte = vData1.Byte(0);
            if (byte != 0) {
                var vAnalogInput = new AnalogInput();
                vAnalogInput.MPL = vData1.UInt16(1);
                vAnalogInput.INPUTTYPE = vData1.Byte(1);
                var data4 = QUESTIONS.getData(i, 4);
                vAnalogInput.DISPLAYPRECISION = data4.Byte(3);
                vAnalogInput.RTD_SI = i - 0x2010 + 1;
                JSON[JSON.length] = vAnalogInput;
            } else {
                var q = 0;
                q++;
            }
        } catch (e) {
        }
    }
}

function Q_3000_AI(QUESTIONS, JSON) {
    for (var i = 0; i < JSON.length; i++)
        QUESTIONS.Add(0x3002, JSON[i].RTD_SI);
}

function A_3000_AI(QUESTIONS, JSON) {
    try {
        for (var i = 0; i < JSON.length; i++)
            JSON[i].setData(QUESTIONS.getData(0x3002, JSON[i].RTD_SI));
    } catch (e) {
    }
}

function AnalogInput() {
    //PRIVATE
    var vDATA = null;

    //PUBLIC
    this.MPL = null;
    this.INPUTTYPE = null;
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