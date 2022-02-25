function Q_2000_CNV(QUESTIONS)
{
    QUESTIONS.Add(0x2601,1);
	for(var i=0x2681;i<0x2689;i++)
	{
        QUESTIONS.Add(i,1); //status
		QUESTIONS.Add(i,2); //current readout
		QUESTIONS.Add(i,7); //converter device type
		QUESTIONS.Add(i,8); //show flowbar
	}
}
function A_2000_CNV(QUESTIONS,JSON)
{
    var error="";
	for(var i=0x2681;i<0x2689;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(i,1).Byte(0)!=0);
            if(vStatus)
            {
                var vConverter=new Converter();
				vConverter.MACHINETYPE=QUESTIONS.getData(0x2601,1).Byte(1);
                vConverter.CONVERTERTYPE=QUESTIONS.getData(i,1).Byte(1);
				vConverter.CONVERTERDEVICETYPE=QUESTIONS.getData(i,7).Byte(0);
				
				if (QUESTIONS.getData(0x2601,1).Byte(0)==21)  //Standalone dryer(regulation type = 21), don't show flowbar, set SHOW=FALSE
					vConverter.SHOW = false;
				else
					vConverter.SHOW=(i==0x2681?true:QUESTIONS.getData(i,8).Byte(3)==1);
				
				if (QUESTIONS.getData(i,2).Byte(0)==1) //Current readout ? true or false
					vConverter.CURRENT = true;
				else
					vConverter.CURRENT = false;
					
                vConverter.RTD_SI=i-0x2681+1;
                JSON[JSON.length]=vConverter;
            }
        }
        catch(e){
			if (e.name == "ERROR")
			{
                if(error=="")
					error += "CONVERTERS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";
			}
		}
    }
	return error;
}
function Q_3000_CNV(QUESTIONS,JSON)
{
	for(var i=0;i<JSON.length;i++)
	{
		QUESTIONS.Add(0x3020+JSON[i].RTD_SI,1);
		if(JSON[i].SHOW){	
			QUESTIONS.Add(0x3020+JSON[i].RTD_SI,5);
		}
		if(JSON[i].CURRENT){	
			QUESTIONS.Add(0x3020+JSON[i].RTD_SI,10);
		}
	}
}
function A_3000_CNV(QUESTIONS,JSON)
{
    var error="";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x3020+JSON[i].RTD_SI,1),(JSON[i].SHOW? QUESTIONS.getData(0x3020+JSON[i].RTD_SI,5):null),(JSON[i].CURRENT? QUESTIONS.getData(0x3020+JSON[i].RTD_SI,10):null));
		}
		catch(e){
			if(error=="")
					error += "CONVERTERS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";	
		}
	}
	return error;
}
function Converter()
{
	//PRIVATE
    var vDATA=null;
	var vFLOW=null;
	var vCURRENT=null;
    
    //PUBLIC
    this.CONVERTERTYPE=null;
	this.CONVERTERDEVICETYPE=null;
	this.MACHINETYPE=null;
	this.SHOW=null;
    this.RTD_SI=null;
    this.setData=function(DATA,FLOW,CURRENT)
    {
        vDATA=DATA;
		vFLOW=FLOW;
		vCURRENT=CURRENT;
    }
    this.getValue=function()
    {
        return vDATA.UInt16(1); 
    }
	this.getFlow=function()
	{
		return vFLOW.UInt16(0);
	}
	this.getCurrent=function()
	{
		return vCURRENT.UInt16(1); //Byte 1 of subindex 10 (index 0x3021)
	}
}