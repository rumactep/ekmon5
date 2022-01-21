function Q_2000_CNT(QUESTIONS)
{
    for(var i=1;i<256;i++)
        QUESTIONS.Add(0x2607,i);
}
function A_2000_CNT(QUESTIONS,JSON)
{
    for(var i=1;i<256;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(0x2607,i).Byte(0)!=0);
            if(vStatus)
            {
                var vCounter=new Counter();
                vCounter.MPL=QUESTIONS.getData(0x2607,i).UInt16(1);
                vCounter.COUNTERUNIT=QUESTIONS.getData(0x2607,i).Byte(1);
                vCounter.RTD_SI=i;
                JSON[JSON.length]=vCounter;
            }
        }
        catch(e){}
    }
}
function Q_3000_CNT(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3007,JSON[i].RTD_SI);
}
function A_3000_CNT(QUESTIONS,JSON)
{
    try
	{
		for(var i=0;i<JSON.length;i++)
			JSON[i].setData(QUESTIONS.getData(0x3007,JSON[i].RTD_SI));
	}
	catch(e){}
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
        if(vDATA.DATA=="X")
        {
            //interpretation of value
            throw new Error();
        }
        else
        {
            return vDATA.UInt32();
        } 
    }
}

function calculate_counter_percentages(json) {
	var raw = new Array();
	var sum = 0;

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