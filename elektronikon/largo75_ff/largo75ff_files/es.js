function Q_3000_ES(QUESTIONS)
{
	QUESTIONS.Add(0x3113,1);
	for(var i=3;i<=5;i++)
		QUESTIONS.Add(0x3113,i);
	for(var i=7;i<=30;i++)
		QUESTIONS.Add(0x3113,i);
	for(var i=31;i<=44;i++)
		QUESTIONS.Add(0x3113,i);
	for(var i=1;i<=18;i++)
		QUESTIONS.Add(0x3114,i);
}
function A_3000_ES(QUESTIONS,JSON)
{
	var error="";
	try
	{
		JSON.NRCOMPRESSORS=QUESTIONS.getData(0x3113,1).Byte(0);
		JSON.NRDRYERS=QUESTIONS.getData(0x3113,1).Byte(2);
		JSON.ACTIVE=(QUESTIONS.getData(0x3113,1).Byte(1)==1);
		JSON.STATE=QUESTIONS.getData(0x3113,3).UInt16(0);
		JSON.REGULATIONPRESSURE=QUESTIONS.getData(0x3113,4).UInt32();
		JSON.CONTROLVSD=QUESTIONS.getData(0x3113,5).Byte(2);
		
		for(var i=0;i<JSON.NRCOMPRESSORS;i++)
		{
			var vSlave=new Slave();
			vSlave.MIN=QUESTIONS.getData(0x3113,7+4*i).UInt16(1);
			vSlave.ACT=QUESTIONS.getData(0x3113,8+4*i).UInt16(0);
			vSlave.MAX=QUESTIONS.getData(0x3113,8+4*i).UInt16(1);
			vSlave.TYPE=QUESTIONS.getData(0x3113,7+4*i).Byte(1);
			vSlave.MS=QUESTIONS.getData(0x3113,7+4*i).Byte(0);
			vSlave.SUMMARY1=QUESTIONS.getData(0x3113,9+4*i).UInt32();
			vSlave.SUMMARY2=QUESTIONS.getData(0x3113,10+4*i).UInt32();
			
			if (i >= 0 && i < 4) {
				vSlave.FLOW=QUESTIONS.getData(0x3113,43).Byte(i); //FLOW for slave 1,2,3,4
				}
			if (i == 4) {
				vSlave.FLOW=QUESTIONS.getData(0x3113,44).Byte(0); //FLOW for slave 5
				}
			if (i == 5) {
				vSlave.FLOW=QUESTIONS.getData(0x3113,44).Byte(1); //FLOW for slave 6
				}
			JSON.COMPRESSORS[i]=vSlave;
		}
		for(var i=0;i<JSON.NRDRYERS;i++)
		{
			var vDryer=new Dryer();
			vDryer.UPPERICON=QUESTIONS.getData(0x3113,31+2*i).Int16(1);
			vDryer.LOWERICON=QUESTIONS.getData(0x3113,31+2*i).Int16(0);
			vDryer.BARVALUE=QUESTIONS.getData(0x3113,32+2*i).Byte(0);
			JSON.DRYERS[i]=vDryer;
		}
		if(JSON.NRCOMPRESSORS>0)
		{
			var vMasterBar=new MasterBar();
			vMasterBar.LEVEL1=QUESTIONS.getData(0x3114,1).Int32();
			vMasterBar.LEVEL2=QUESTIONS.getData(0x3114,2).Int32();
			vMasterBar.ACT=QUESTIONS.getData(0x3114,3).Int32();
			vMasterBar.INRANGE=QUESTIONS.getData(0x3114,4).Byte(1);
			vMasterBar.PERCENTAGE=QUESTIONS.getData(0x3114,4).Byte(0);
			vMasterBar.METHODOFFILLING=QUESTIONS.getData(0x3114,4).Byte(2);
			vMasterBar.TYPE=QUESTIONS.getData(0x3114,4).Byte(3);//0-->25/75 1-->0/100
			JSON.COMPRESSORMASTERBAR=vMasterBar;
		}
		
		if(JSON.NRDRYERS>0)
		{
			var vDryerMasterBar=new DryerMasterBar();
			vDryerMasterBar.LEVEL1=QUESTIONS.getData(0x3114,7).Int32();
			vDryerMasterBar.LEVEL2=QUESTIONS.getData(0x3114,8).Int32();
			vDryerMasterBar.ACT=QUESTIONS.getData(0x3114,9).Int32();
			vDryerMasterBar.INRANGE=QUESTIONS.getData(0x3114,10).Byte(1);
			vDryerMasterBar.PERCENTAGE=QUESTIONS.getData(0x3114,10).Byte(0);
			vDryerMasterBar.METHODOFFILLING=QUESTIONS.getData(0x3114,10).Byte(2);
			vDryerMasterBar.TYPE=QUESTIONS.getData(0x3114,10).Byte(3);//0-->25/75 1-->0/100
			JSON.DRYERMASTERBAR=vDryerMasterBar;
		}
	}
	catch(e){
		if (e.name == "ERROR")
		{
			if(error=="")
				error += "<br />ES :<br />"+e.message;
			else
				error += e.message;
			error += "<br />";
		}
	}
	return error;
}

function Dryer()
{
	//PUBLIC
	this.UPPERICON=null;
    this.LOWERICON=null;
    this.BARVALUE=null;
}

function Slave()
{
    //PUBLIC
    this.MIN=null;
    this.ACT=null;
    this.MAX=null;
	this.TYPE=null;
	this.MS=null;
	this.SUMMARY1=null;
	this.SUMMARY2=null;
	this.FLOW=null; //Flow for Power$ync
}

function DryerMasterBar()
{
	//PUBLIC
    this.LEVEL1=null;
    this.LEVEL2=null;
    this.ACT=null;
	this.INRANGE=null;
	this.PERCENTAGE=null;
	this.METHODOFFILLING=null;
	this.TYPE=null;
}

function MasterBar()
{
	//PUBLIC
    this.LEVEL1=null;
    this.LEVEL2=null;
    this.ACT=null;
	this.INRANGE=null;
	this.PERCENTAGE=null;
	this.METHODOFFILLING=null;
	this.TYPE=null;
}