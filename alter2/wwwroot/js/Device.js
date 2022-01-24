function Q_3000_MS(QUESTIONS) {
    QUESTIONS.Add(0x3001, 8);
}

function A_3000_MS(QUESTIONS, JSON) {
    try {
        var vData = QUESTIONS.getData(0x3001, 8);
        JSON[0] = vData.UInt16(0);
    } catch (e) {
    }
}

function Q_2000_MMT(QUESTIONS) {
    QUESTIONS.Add(0x2001, 1);
}

function A_2000_MMT(QUESTIONS, JSON) {
    try {
        JSON[0] = QUESTIONS.getData(0x2001, 1).Byte(0);
    } catch (e) {
    }
}