function Q_2000_SPR(QUESTIONS)
{
    for(var i=0x2300;i<0x247F;i++)
        QUESTIONS.Add(i,1);
}
function A_2000_SPR(QUESTIONS,JSON)
{
    var error = "";
	for(var i=0x2300;i<0x247F;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(i,1).Byte(0)!=0);
            if(vStatus)
            {
                var vSpecialProtection=new SpecialProtection();
                vSpecialProtection.MPL=QUESTIONS.getData(i,1).UInt16(1);
                vSpecialProtection.RTD_SI=i-0x2300+1;
                JSON[JSON.length]=vSpecialProtection;
            }
        }
        catch(e){
			if (e.name == "ERROR")
            {
                if(error=="")
					error += "SPECIAL PROTECTIONS :<br />"+e.message;
				else
					error += e.message;
				error += "<br />";
			}
		}
    }
	return error;
}
function Q_3000_SPR(QUESTIONS,JSON)
{
    for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x300E,JSON[i].RTD_SI);
}
function A_3000_SPR(QUESTIONS,JSON)
{
    var error="";
	for(var i=0;i<JSON.length;i++)
	{
		try
		{
			JSON[i].setData(QUESTIONS.getData(0x300E,JSON[i].RTD_SI));
		}
		catch(e){
			if(error=="")
				error += "SPECIAL PROTECTIONS :<br />"+e.message;
			else
				error += e.message;
			error += "<br />";
		}
	}
	return error;
}
function SpecialProtection()
{
    //PRIVATE
    var vDATA=null;
    
    //PUBLIC
    this.MPL=null;
    this.RTD_SI=null;
    this.setData=function(DATA) {
        vDATA=DATA;
    }
    this.getStatus=function() {
        return vDATA.UInt16(0);
    }
}