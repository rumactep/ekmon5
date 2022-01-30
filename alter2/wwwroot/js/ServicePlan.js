function Q_2000_SPL(QUESTIONS) {
    for (var i = 1; i < 21; i++)
        QUESTIONS.Add(0x2602, i);
}

function A_2000_SPL(QUESTIONS, JSON) {
    for (var i = 1; i < 21; i++) {
        try {
            var vSTATICVALUE = QUESTIONS.getData(0x2602, i).UInt32();
            if (vSTATICVALUE != 0) {
                var vServicePlan = new ServicePlan();
                vServicePlan.STATICVALUE = vSTATICVALUE;
                vServicePlan.RTD_SI = (i % 2 == 0 ? 16 + i / 2 : 6 + (i - 1) / 2);
                vServicePlan.LEVEL = Math.ceil(i / 2);
                vServicePlan.setType(i % 2);
                JSON[JSON.length] = vServicePlan;
            }
        } catch (e) {
        }
    }
}

function Q_3000_SPL(QUESTIONS, JSON) {
    QUESTIONS.Add(0x3009, 1);
    for (var i = 0; i < JSON.length; i++)
        QUESTIONS.Add(0x3009, JSON[i].RTD_SI);
}

function A_3000_SPL(QUESTIONS, JSON) {
    try {
        for (var i = 0; i < JSON.length; i++)
            JSON[i].setData(QUESTIONS.getData(0x3009, JSON[i].RTD_SI), QUESTIONS.getData(0x3009, 1));
    } catch (e) {
    }
}

function ServicePlan() {
    //PRIVATE
    var vDATA = null;
    var vNEXT = null;
    var vType = null;

    //PUBLIC
    this.STATICVALUE = null;
    this.RTD_SI = null;
    this.LEVEL = null;
    this.getNext = function() {
        return vNEXT;
    };
    this.setType = function(type) {
        vType = type;
    };
    this.getType = function() {
        return vType == 1;
    };
    this.setData = function(DATA, NEXT) {
        vDATA = DATA;
        vNEXT = ((NEXT.UInt32() >>> this.LEVEL - 1) & 1 == 1);
    };
    this.getValue = function() {
        if (vDATA.DATA == "X") {
            //interpretation of value
            throw new Error();
        } else {
            return vDATA.UInt32();
        }
    };
}