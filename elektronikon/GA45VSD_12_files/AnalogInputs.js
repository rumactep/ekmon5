function Q_2000_AI(QUESTIONS)
{
    for(var i=0x2010;i<0x2090;i++)
    {
        QUESTIONS.Add(i,1);
        QUESTIONS.Add(i,4);
		QUESTIONS.Add(i,6);
    }
	QUESTIONS.Add(0x260B,3);
}
function A_2000_AI(QUESTIONS,JSON)
{
    var error="";
	var SubIndexPresent = false;
	
	try {
		if (QUESTIONS.getData(0x260B,3).UInt16(0) > -1) //this will result in an error if the subindex is NOT present
				SubIndexPresent = true;
	}
	catch (e) {
		if ((e.name == "NO DATA") && (e.message == "[260b,3]"))
												SubIndexPresent = false;
	}
    for(var i=0x2010;i<0x2090;i++)
    {
        try {
            if (QUESTIONS.getData(i, 1).Byte(0) != 0) {
                var vAnalogInput = new AnalogInput();
                vAnalogInput.MPL = QUESTIONS.getData(i, 1).UInt16(1);
                vAnalogInput.INPUTTYPE = QUESTIONS.getData(i, 1).Byte(1);
                vAnalogInput.DISPLAYPRECISION = QUESTIONS.getData(i, 4).Byte(3);
				vAnalogInput.PRESSUREMEASUREMENT = QUESTIONS.getData(i, 6).Byte(2);
                vAnalogInput.RTD_SI = i - 0x2010 + 1;
				//pressure input AND subindex is present
				if (((vAnalogInput.INPUTTYPE == 0) || (vAnalogInput.INPUTTYPE == 35) || (vAnalogInput.INPUTTYPE == 9)) && SubIndexPresent) 
				{
				vAnalogInput.absATMpres = QUESTIONS.getData(0x260B,3).UInt16(0);
				}
				// && (QUESTIONS.getData(0x2601,1).Byte(1) == 44))
                JSON[JSON.length] = vAnalogInput;
            }
        }
        catch (e) {
            if (e.name == "ERROR")
            {
                if(error=="")
					error += "ANALOG INPUTS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";	
			}
        }
    }
    return error;
}
function Q_3000_AI(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3002,JSON[i].RTD_SI);
}
function A_3000_AI(QUESTIONS,JSON)
{
    var error = "";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x3002,JSON[i].RTD_SI));
		}
		catch (e) {
			if(error=="")
					error += "ANALOG INPUTS :<br />"+e.message;
				else
					error += e.message;
			    error += "<br />";
		}
	}
	return error;
}
function AnalogInput()
{
    //PRIVATE
    var vDATA=null;
    
    //PUBLIC
    this.MPL=null;
    this.INPUTTYPE=null;
    this.DISPLAYPRECISION=null;
    this.RTD_SI=null;
	this.PRESSUREMEASUREMENT=null;
	this.absATMpres=null;
    this.setData=function(DATA)
    {
        vDATA=DATA;
    }
    this.getValue=function()
    {
       return vDATA.Int16(1);
    }
    this.getStatus=function()
    {
       return vDATA.UInt16(0);
    }
}