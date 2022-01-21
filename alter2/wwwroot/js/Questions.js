function Questions() {
    //PRIVATE
    var vQUESTIONS = new Array();

    function Post(URL, QUESTION) {
        $('#error').hide();

        var return_data = null;
        $.ajax({
            url: URL,
            type: 'POST',
            crossDomain: true,
            data: 'QUESTION=' + QUESTION,
            success: function (data, textStatus, jqXHR) {
                return_data = data;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                show_error('Error occured during data retrieval');
                throw new Error();
            }
        });

        return return_data;
    }

    function HexString(VALUE, LENGTH) {
        var v = VALUE.toString(16);
        while (v.length < LENGTH)
            v = "0" + v;
        return v;
    }

    //PUBLIC
    this.Add = function (INDEX, SUBINDEX) {
        var vQuestion = new Question();
        vQuestion.INDEX = INDEX;
        vQuestion.SUBINDEX = SUBINDEX;
        vQUESTIONS[vQUESTIONS.length] = vQuestion;
    }
    this.getData = function (INDEX, SUBINDEX) {
        for (var i = 0; i < vQUESTIONS.length; i++) {
            if ((vQUESTIONS[i].INDEX == INDEX) && (vQUESTIONS[i].SUBINDEX == SUBINDEX)) {
                return vQUESTIONS[i].getData();
            }
        }
    }
    this.SendReceive = function () {
        var v1000 = 2000;
        for (var idx = 0; idx < vQUESTIONS.length; idx += v1000) {
            var vQuestionsSlice;
            if ((vQUESTIONS.length - idx) <= v1000)
                vQuestionsSlice = vQUESTIONS.slice(idx, vQUESTIONS.length);
            else
                vQuestionsSlice = vQUESTIONS.slice(idx, idx + v1000);

            var vQuestions = "";
            for (var iQ = 0; iQ < vQuestionsSlice.length; iQ++)
                vQuestions += HexString(vQuestionsSlice[iQ].INDEX, 4) + HexString(vQuestionsSlice[iQ].SUBINDEX, 2);

            var url208 = "http://" + document.location.hostname + "/cgi-bin/mkv.cgi";
            var url208 = "http://" + "192.168.11.208" + "/cgi-bin/mkv.cgi";
            var vAnswers = Post(url208, vQuestions); //

            for (var iQ = 0, iA = 0; iQ < vQuestionsSlice.length; iQ++) {
                if (vAnswers != null && vAnswers.charAt(iA) != "X") {
                    vQuestionsSlice[iQ].setData(vAnswers.substring(iA, iA + 8));
                    iA += 8;
                } else {
                    vQuestionsSlice[iQ].setData("X");
                    iA++;
                }
            }
        }
    }
}

function Question() {
    //PRIVATE
    var vDATA = null;

    //PUBLIC
    this.INDEX = 0;
    this.SUBINDEX = 0;
    this.setData = function (DATA) {
        var vData = new Data();
        vData.DATA = DATA;
        vDATA = vData;
    }
    this.getData = function () {
        return vDATA;
    }
}

function Data() {
    this.DATA = null;

    this.UInt32 = function () {
        if (this.DATA == "X")
            throw new Error();
        return parseInt(this.DATA, 16);
    }
    this.Int32 = function () {
        if (this.DATA == "X")
            throw new Error();
        var v = parseInt(this.DATA, 16);
        if (v >>> 31)
            v = -2147483648 + (v & 0x7FFFFFFF);
        return v;
    }
    this.UInt16 = function (WORD) {
        if (this.DATA == "X")
            throw new Error();
        return parseInt(this.DATA.substring((1 - WORD) * 4, (2 - WORD) * 4), 16);
    }
    this.Int16 = function (WORD) {
        if (this.DATA == "X")
            throw new Error();
        var v = parseInt(this.DATA.substring((1 - WORD) * 4, (2 - WORD) * 4), 16);
        if (v >>> 15)
            v = -32768 + (v & 0x00007FFF);
        return v;
    }
    this.Byte = function (BYTE) {
        if (this.DATA == "X")
            throw new Error();
        return parseInt(this.DATA.substring((3 - BYTE) * 2, (4 - BYTE) * 2), 16);
    }
}