function Questions()
{
    //PRIVATE
    var vQUESTIONS=new Array();
    function Post(URL,QUESTION)
    {
		var return_data = null;
		$.ajax({
		    url: URL,
		    type: "POST",
			timeout : 2000,
		    data: "QUESTION=" + QUESTION,
		    success: function(data, textStatus, jqXHR) {
		        return_data = data;
		    },
		    error: function(jqXHR, textStatus, errorThrown) {
		        throw new Error("Unable to complete HTTP post (url = '"+URL+"' , status = '"+jqXHR.status+"')" );
		    }
		});

		return return_data;
    }
    function HexString(VALUE,LENGTH)
    {
        var v=VALUE.toString(16);
        while(v.length<LENGTH)
            v="0"+v;
        return v;
    }
    
    //PUBLIC
    this.Add=function(INDEX,SUBINDEX)
    {
        var vQuestion=new Question();
        vQuestion.INDEX=INDEX;
        vQuestion.SUBINDEX=SUBINDEX;
        vQUESTIONS[vQUESTIONS.length]=vQuestion;
    }
    this.getData = function(INDEX, SUBINDEX) {
        for (var i = 0; i < vQUESTIONS.length; i++) {
            if ((vQUESTIONS[i].INDEX == INDEX) && (vQUESTIONS[i].SUBINDEX == SUBINDEX)) {
                var Data = vQUESTIONS[i].getData();
                if (Data.DATA == "X")
                    throw { name: "NO DATA", message: "[" + HexString(INDEX, 4) + "," + SUBINDEX + "]" };
                else
                    return Data;
            }
        }
        throw { name: "ERROR", message: "[" + HexString(INDEX, 4) + "," + SUBINDEX + "]" };
    }
    this.SendReceive = function() {
		for (var idx = 0; idx < vQUESTIONS.length; idx += 1000) {
			var vQuestionsSlice;
			if ((vQUESTIONS.length - idx) <= 1000)
				vQuestionsSlice = vQUESTIONS.slice(idx, vQUESTIONS.length);
			else
				vQuestionsSlice = vQUESTIONS.slice(idx, idx + 1000);

			var vQuestions = "";
			for (var iQ = 0; iQ < vQuestionsSlice.length; iQ++)
				vQuestions += HexString(vQuestionsSlice[iQ].INDEX, 4) + HexString(vQuestionsSlice[iQ].SUBINDEX, 2);

			var vAnswers = Post($(location).attr("href") + "cgi-bin/mkv.cgi", vQuestions);

			for (var iQ = 0, iA = 0; iQ < vQuestionsSlice.length; iQ++) {
				if (vAnswers != null && vAnswers.charAt(iA) != "X") {
					vQuestionsSlice[iQ].setData(vAnswers.substring(iA, iA + 8));
					iA += 8;
				}
				else {
					vQuestionsSlice[iQ].setData("X");
					iA++;
				}
			}
		}
    }
}
function Question() {
    //PRIVATE
    var vDATA=null;
    
    //PUBLIC
    this.INDEX=0;
    this.SUBINDEX=0;
    this.setData = function(DATA) {
        var vData = new Data();
        vData.DATA = DATA;
        vDATA = vData;
    }
    this.getData = function(){return vDATA;}
}
function Data() {
    this.DATA=null;

    this.UInt32=function()  {
        return parseInt(this.DATA,16);
    }
    this.Int32=function() {
        var v=parseInt(this.DATA,16);
        if(v>>>31)
            v=-2147483648+(v&0x7FFFFFFF);
        return v;
    }
    this.UInt16=function(WORD) {
        return parseInt(this.DATA.substring((1-WORD)*4,(2-WORD)*4),16);
    }
    this.Int16=function(WORD) {
        var v=parseInt(this.DATA.substring((1-WORD)*4,(2-WORD)*4),16);
        if(v>>>15)
            v=-32768+(v&0x00007FFF);
        return v;
    }
    this.Byte = function(BYTE) {
        return parseInt(this.DATA.substring((3-BYTE)*2,(4-BYTE)*2),16);
    }
}