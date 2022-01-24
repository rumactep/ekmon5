function Questions(NUMBEROF) {
    //PRIVATE
    var vQUESTIONS = new Array();
    var numberOfQuestion = NUMBEROF;
    var myAnswers = [
        "01FD00010114003CXX03F00101000500C8XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX083401010839010108350001XXXXXX084C0201XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0A8C00010A8D00010A8E01010A9101010A9200010A9300010A9400010A9500010A9600010AD801000AD901010A9002010A8F00010AEA00000AEB00000AE600001C9201001C9301001C9601001C9801001C9B01001C9C0000XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0906040100000001XXXXXXXXXXXXXX09050001XX0909000109020001X090300010901000109060001XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0906040100000001XXXXXXXXXXXXXXXX0B9F00011CD90401XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0BAF0001XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064XXXXXXXXXXXXXX000007D00000223800000FA00000447000001F400000000000005DC00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001F40000000001000002",
        "237D0080036F0080000100800001008000000080000000800A9658120A8F9E320000073300000C9B0000156700652E6901C4EDA803B8EA7204B33C280004471900001FBF0ABA85D40000005A0EEF0EEF0001008000010080000100800001008000010080000100802302000023030000232900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000064XXXXXXXXXXXXXX00000201006420E6006490EF006420E6006490EF006420E4006420E40011BB260000001C",
        ""
    ]

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
        var v1000 = 1000;
        for (var idx = 0; idx < vQUESTIONS.length; idx += v1000) {
            var vQuestionsSlice;
            if ((vQUESTIONS.length - idx) <= v1000)
                vQuestionsSlice = vQUESTIONS.slice(idx, vQUESTIONS.length);
            else
                vQuestionsSlice = vQUESTIONS.slice(idx, idx + v1000);

            var vQuestionsString = "";
            for (var iQ = 0; iQ < vQuestionsSlice.length; iQ++)
                vQuestionsString += HexString(vQuestionsSlice[iQ].INDEX, 4) + HexString(vQuestionsSlice[iQ].SUBINDEX, 2);

            //var url208 = "http://" + document.location.hostname + "/cgi-bin/mkv.cgi";
            var url208 = "http://192.168.11.208/cgi-bin/mkv.cgi";
            //var vAnswersString = Post(url208, vQuestionsString); //
            if (numberOfQuestion > 2)
                numberOfQuestion = 2;
            var vAnswersString = myAnswers[numberOfQuestion];
            numberOfQuestion++;
            


            for (var iQ = 0, iA = 0; iQ < vQuestionsSlice.length; iQ++) {
                if (vAnswersString != null && vAnswersString.charAt(iA) != "X") {
                    var substring = vAnswersString.substring(iA, iA + 8);
                    vQuestionsSlice[iQ].setData(substring);
                    iA += 8;
                } else {
                    vQuestionsSlice[iQ].setData("X");
                    iA++;
                }
            }
            var q = 0;
            q++;
        }
    }
}

function Question() {
    //PRIVATE
    var _vData = null;
    //this._vData = null;

    //PUBLIC
    this.INDEX = 0;
    this.SUBINDEX = 0;
    this.setData = function (DATA) {
        var data = new Data();
        data.DATA = DATA;
        _vData = data;
    }
    this.getData = function () {
        return _vData;
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