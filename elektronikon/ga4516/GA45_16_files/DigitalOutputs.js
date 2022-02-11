function Q_2000_DO(QUESTIONS)
{
    for(var i=0x2100;i<0x2150;i++)
        QUESTIONS.Add(i,1);
}
function A_2000_DO(QUESTIONS,JSON)
{
    var error="";
	for(var i=0x2100;i<0x2150;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(i,1).Byte(0)!=0);
            if(vStatus)
            {
                var vDigitalOutput=new DigitalOutput();
                vDigitalOutput.MPL=QUESTIONS.getData(i,1).UInt16(1);
                vDigitalOutput.RTD_SI=i-0x2100+1;
                JSON[JSON.length]=vDigitalOutput;
            }
        }
        catch(e){
			if (e.name == "ERROR")
			{
                if(error=="")
					error += "DIGITAL OUTPUTS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";
			}
		}
    }
	return error;
}
function Q_3000_DO(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3005,JSON[i].RTD_SI);
}
function A_3000_DO(QUESTIONS,JSON)
{
    var error="";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x3005,JSON[i].RTD_SI));
		}
		catch(e){
			if(error=="")
				error += "<br />DIGITAL OUTPUTS :<br />"+e.message;
			else
				error += e.message;
			error += "<br />";
		}
	}
	return error;
}
function DigitalOutput()
{
    //PRIVATE
    var vDATA=null;
    
    //PUBLIC
    this.MPL=null;
    this.RTD_SI=null;
    this.setData=function(DATA)
    {
        vDATA=DATA;
    }
    this.getValue=function()
    {
        return vDATA.UInt16(1); 
    }
    this.getStatus=function()
    {
        return vDATA.UInt16(0); 
    }
}