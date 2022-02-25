function Q_2000_DI(QUESTIONS)
{
    for(var i=0x20b0;i<0x2100;i++)
        QUESTIONS.Add(i,1);
}
function A_2000_DI(QUESTIONS,JSON)
{
    var error="";
	for(var i=0x20b0;i<0x2100;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(i,1).Byte(0)!=0);
            if(vStatus)
            {
                var vDigitalInput=new DigitalInput();
                vDigitalInput.MPL=QUESTIONS.getData(i,1).UInt16(1);
                vDigitalInput.RTD_SI=i-0x20b0+1;
                JSON[JSON.length]=vDigitalInput;
            }
        }
        catch(e){
			if (e.name == "ERROR")
            {
                if(error=="")
					error += "DIGITAL INPUTS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";
			}
		}
    }
	return error;
}
function Q_3000_DI(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3003,JSON[i].RTD_SI);
}
function A_3000_DI(QUESTIONS,JSON)
{
    var error = "";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x3003,JSON[i].RTD_SI));
		}
		catch(e){
			if(error=="")
				error += "DIGITAL INPUTS :<br />"+e.message;
			else
				error += e.message;
			error += "<br />";
		}
	}
	return error;
}
function DigitalInput()
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