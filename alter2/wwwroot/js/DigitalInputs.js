function Q_2000_DI(QUESTIONS) {
    for (var i = 0x20b0; i < 0x2100; i++)
        QUESTIONS.Add(i, 1);
}

function A_2000_DI(QUESTIONS, JSON) {
    for (var i = 0x20b0; i < 0x2100; i++) {
        try {
            var vData = QUESTIONS.getData(i, 1);
            var byte = vData.Byte(0);
            if (byte != 0) {
                var vDigitalInput = new DigitalInput();
                vDigitalInput.MPL = vData.UInt16(1);
                vDigitalInput.RTD_SI = i - 0x20b0 + 1;
                JSON[JSON.length] = vDigitalInput;
            }
        } catch (e) {
        }
    }
}

function Q_3000_DI(QUESTIONS, JSON) {
    for (var i = 0; i < JSON.length; i++)
        QUESTIONS.Add(0x3003, JSON[i].RTD_SI);
}

function A_3000_DI(QUESTIONS, JSON) {
    try {
        for (var i = 0; i < JSON.length; i++)
            JSON[i].setData(QUESTIONS.getData(0x3003, JSON[i].RTD_SI));
    } catch (e) {
    }
}

function DigitalInput() {
    //PRIVATE
    var vDATA = null;

    //PUBLIC
    this.MPL = null;
    this.RTD_SI = null;
    this.setData = function(DATA) {
        vDATA = DATA;
    };
    this.getValue = function() {
        if (vDATA.DATA == "X") {
            //interpretation of value
            throw new Error();
        } else {
            return vDATA.UInt16(1);
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