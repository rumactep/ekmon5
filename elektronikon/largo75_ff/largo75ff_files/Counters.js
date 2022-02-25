function Q_2000_CNT(QUESTIONS)
{
    for(var i=1;i<256;i++)
        QUESTIONS.Add(0x2607,i);
}
function A_2000_CNT(QUESTIONS,JSON)
{
    var error="";
	for(var i=1;i<256;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(0x2607,i).Byte(0)!=0);
            if(vStatus)
            {
                var vCounter=new Counter();
                vCounter.MPL=QUESTIONS.getData(0x2607,i).UInt16(1);
				if((vCounter.MPL==7878)||(vCounter.MPL==2790)||(vCounter.MPL==2757)||(vCounter.MPL==2758)||(vCounter.MPL==7319)||(vCounter.MPL==7324)||((vCounter.MPL>=2796)&&(vCounter.MPL<=2799))||((vCounter.MPL>=2808)&&(vCounter.MPL<=2811))||((vCounter.MPL>=7310)&&(vCounter.MPL<=7312))||((vCounter.MPL>=7495)&&(vCounter.MPL<=7502))||((vCounter.MPL>=2733)&&(vCounter.MPL<=2736))||((vCounter.MPL>=8464)&&(vCounter.MPL<=8469)))
					vCounter.COUNTERUNIT=255;
				else
					vCounter.COUNTERUNIT=QUESTIONS.getData(0x2607,i).Byte(1);
                vCounter.RTD_SI=i;
                JSON[JSON.length]=vCounter;
            }
        }
        catch(e){
			if (e.name == "ERROR")
            {
                if(error=="")
					error += "COUNTERS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";
			}
		}
    }
	return error;
}
function Q_3000_CNT(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3007,JSON[i].RTD_SI);
}
function A_3000_CNT(QUESTIONS,JSON)
{
    var error="";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x3007,JSON[i].RTD_SI));
		}
		catch(e){
			if(error=="")
					error += "COUNTERS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";
		}
	}
	return error;
}
function Counter()
{
    //PRIVATE
	var vDATA=null;
	
	//PUBLIC
	this.MPL=null;
	this.RTD_SI=null;
	this.COUNTERUNIT=null;
	this.setData=function(DATA)
	{
		vDATA=DATA;
	}	
	this.getValue=function()
	{		
//		try{	
			return vDATA.UInt32();
//		}
/* 		catch (e){
			return "No Data Found";
		} */
	}
}

function calculate_counter_percentages(json) {
	var raw = new Array();
	var sum = 0;

	try
	{
		for(var i in json) {
			if(json[i].MPL >= 2706 && json[i].MPL <= 2710) {
				raw[json[i].MPL] = json[i].getValue();
				sum += raw[json[i].MPL];
			}
		}

		if(raw.length != 0 && sum != 0) {
			var quotients = new Array();
			var rests = new Array();
			
			for(var i in raw) {
				quotients[i] = Math.floor(raw[i] * 100 / sum);
				rests[i] = ((raw[i]*100) % sum);
			}
			
			while(true) {
				var quotient_sum = 0;
				var max = 0;
				var max_index = 0;
				for(var i in quotients) {
					quotient_sum += quotients[i];
					
					if(rests[i] > max) {
						max = rests[i];
						max_index = i;
					}
				}
				
				if(quotient_sum == 100) {
					return quotients;
				} else{
					quotients[max_index]++;
					rests[max_index] = 0;
				}
			}
		}
		else if(raw.length ==0) {
			return new Array();
		} else {
			return raw;
		}
	}
	catch(e){
		//if(error=="")
				//error += "COUNTERS :<br />"+
				return e.message;
			//else
				//error += e.message;
			//error += "<br />";
	}
}