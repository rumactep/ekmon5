function Q_2000_MS(QUESTIONS)
{
    QUESTIONS.Add(0x2601,1);
}

function A_2000_MS(QUESTIONS,JSON)
{
	var error = "";
	try
	{
		var RT = QUESTIONS.getData(0x2601,1).Byte(0);
		var MT = QUESTIONS.getData(0x2601,1).Byte(1);
		
		var vMachineState = new MachineState();
		vMachineState.COUNT=1;
		if((RT==79)||(RT==84))//79 NGP , 84 OGP
		{
			switch(MT)
			{
				case 39: vMachineState.COUNT=2;break;
				case 40: vMachineState.COUNT=3;break;
			}
		}
		JSON[0]=vMachineState;
	}
	catch(e){
		if(error=="")
			error += "MACHINE STATE :<br />"+e.message;
		else
			error += e.message;
		error += "<br />";
	}
	return error;
}

function Q_3000_MS(QUESTIONS,JSON)
{
    QUESTIONS.Add(0x3001,8);
	if(JSON[0].COUNT>1)
		QUESTIONS.Add(0x3001,9);
}

function A_3000_MS(QUESTIONS,JSON)
{
	var error = "";
	try
	{
		if(JSON[0].COUNT==1)
			JSON[0].setData(QUESTIONS.getData(0x3001,8),null);
		else
			JSON[0].setData(QUESTIONS.getData(0x3001,8),QUESTIONS.getData(0x3001,9));
	}
	catch(e){
		if(error=="")
			error += "MACHINE STATE :<br />"+e.message;
		else
			error += e.message;
		error += "<br />";
	}
	return error;
}

function MachineState()
{
    //PRIVATE
    var vDATA1=null;
	var vDATA2=null;
    
    //PUBLIC
	this.COUNT=null;
    this.setData=function(DATA1,DATA2)
    {
        vDATA1=DATA1;
		vDATA2=DATA2;
    }
    this.getPrimaryState=function()
    {
       return vDATA1.Int16(0);
    }
    this.getSecundaryState1=function()
    {
       return vDATA2.UInt16(0);
    }
	this.getSecundaryState2=function()
    {
       return vDATA2.UInt16(1);
    }
}