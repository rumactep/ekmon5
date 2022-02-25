function Q_2000_ID(QUESTIONS)
{
    for(var i=1;i<256;i++)
        QUESTIONS.Add(0x2619,i);
	QUESTIONS.Add(0x260B,2);
}
function A_2000_ID(QUESTIONS,JSON)
{
    var error="";
	var SubIndexPresent = false;
	
    try {
				if (QUESTIONS.getData(0x260B,2).Byte(3) > -1) {//this will result in an error if the subindex is NOT present
						SubIndexPresent = true;
				}
	}
	catch (e) {
				if ((e.name == "NO DATA") && (e.message == "[260b,2]")) {
						SubIndexPresent = false;
				}
	}
	for(var i=1;i<256;i++)
    {
        try {
            if (QUESTIONS.getData(0x2619,i).Byte(0) != 0) {
                var vInternalData = new InternalData();
                vInternalData.MPL = QUESTIONS.getData(0x2619,i).UInt16(1);
                vInternalData.TYPE = QUESTIONS.getData(0x2619,i).Byte(1);
                vInternalData.RTD_SI = i;
				if (vInternalData.TYPE == 5 && SubIndexPresent) {
							vInternalData.CurrencyUnit = QUESTIONS.getData(0x260B,2).Byte(3); //read currency out of the controller
				}
                JSON[JSON.length] = vInternalData;
            }
        }
        catch (e) {
            if (e.name == "ERROR")
            {
                if(error=="")
					error += "INTERNAL DATA :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";	
			}
        }
    }
    return error;
}
function Q_3000_ID(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3014,JSON[i].RTD_SI);
}
function A_3000_ID(QUESTIONS,JSON)
{
    var error = "";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x3014,JSON[i].RTD_SI));
		}
		catch (e) {
			if(error=="")
					error += "INTERNAL DATA :<br />"+e.message;
				else
					error += e.message;
			    error += "<br />";
		}
	}
	return error;
}
function InternalData()
{
    //PRIVATE
    var vDATA=null;
    
    //PUBLIC
    this.MPL=null;
    this.TYPE=null;
    this.RTD_SI=null;
	this.CurrencyUnit="";
    this.setData=function(DATA)
    {
        vDATA=DATA;
    }
    this.getValue=function(MPL)
    {
		if (this.MPL==1710 || this.MPL==8408 || this.MPL==8484) //if surge number, choke number or startline => read as SIGNED INT32 !!
			{
						return vDATA.Int32();
			}
			else
			{
						return vDATA.UInt32();
			}
    }
}