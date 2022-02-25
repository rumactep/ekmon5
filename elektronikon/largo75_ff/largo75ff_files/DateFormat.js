function Q_2000_DF(QUESTIONS)
{
    QUESTIONS.Add(0x2615,1); //display preferences
}
function A_2000_DF(QUESTIONS,JSON)
{
    var error="";
    
	try {
		JSON.TYPE = QUESTIONS.getData(0x2615,1).Byte(0);
	}
	catch (e) {
		if (e.name == "ERROR")
		{
			if(error=="")
				error += "DateFormat :<br />"+e.message;
			else
				error += e.message;
			error += "<br />";	
		}
	}
    
    return error;
}