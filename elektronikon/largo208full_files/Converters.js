function Q_2000_CNV(QUESTIONS)
{
    for(var i=0x2681;i<0x2689;i++)
	{
        QUESTIONS.Add(i,1);
		QUESTIONS.Add(i,7);
	}
}
function A_2000_CNV(QUESTIONS,JSON)
{
    for(var i=0x2681;i<0x2689;i++)
    {
        try
        {
            var vStatus=(QUESTIONS.getData(i,1).Byte(0)!=0);
            if(vStatus)
            {
                var vConverter=new Converter();
                vConverter.CONVERTERTYPE=QUESTIONS.getData(i,1).Byte(1);
				vConverter.CONVERTERDEVICETYPE=QUESTIONS.getData(i,7).Byte(0);
                vConverter.RTD_SI=i-0x2681+1;
                JSON[JSON.length]=vConverter;
            }
        }
        catch(e){}
    }
}
function Q_3000_CNV(QUESTIONS,JSON)
{
    QUESTIONS.Add(0x3021,5);
	for(var i=0;i<JSON.length;i++)
        QUESTIONS.Add(0x3020+JSON[i].RTD_SI,1);
}
function A_3000_CNV(QUESTIONS,JSON)
{
    try
	{
		for(var i=0;i<JSON.length;i++)
			JSON[i].setData(QUESTIONS.getData(0x3020+JSON[i].RTD_SI,1),QUESTIONS.getData(0x3021,5));
	}
	catch(e){alert('The following error has been thrown by "Converters (3000)" :\n'+e.message);}
}
function Converter()
{
    //PRIVATE
    var vDATA=null;
	var vFLOW=null;
    
    //PUBLIC
    this.CONVERTERTYPE=null;
	this.CONVERTERDEVICETYPE=null;
    this.RTD_SI=null;
    this.setData=function(DATA,FLOW)
    {
        vDATA=DATA;
		vFLOW=FLOW;
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
            return vDATA.UInt16(1);
        } 
    }
	this.getFlow=function()
	{
		if(vFLOW.DATA=="X")
        {
            //interpretation of value
            throw new Error();
        }
        else
        {
            return vFLOW.UInt16(0);
        } 
	}
}