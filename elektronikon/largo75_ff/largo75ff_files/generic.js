function show_error(message) {
    $('#checks').hide();
    $('#TABLELEFT_L1').hide();
    $('#TABLELEFT_L12').hide();
    $('#TABLERIGHT_L1').hide();
    $('#units').hide();
    $('#tablees_l3').hide();

    $('#error').html(message);
    $('#error').show();
}

function load_compressor() {
    page = "machine";
    $('#page').html(machine['Model']);

    $('#units').hide();
    $('#tablees_l3').hide();
    $('#error').hide();

    $('#checks').show();
    $('#TABLELEFT_L1').show();
    $('#TABLELEFT_L12').show();
    $('#TABLERIGHT_L1').show();
}

function load_es() {
    page = "es";
    $('#page').html(language.get('LINK', 2));

    $('#units').hide();
    $('#checks').hide();
    $('#TABLELEFT_L1').hide();
    $('#TABLELEFT_L12').hide();
    $('#TABLERIGHT_L1').hide();
    $('#error').hide();

    $('#tablees_l3').show();
}

function load_preferences() {
    page = "preferences";
    $('#page').html(language.get('LINK', 3));

    $('#units').show();

    $('#checks').hide();
    $('#TABLELEFT_L1').hide();
    $('#TABLELEFT_L12').hide();
    $('#TABLERIGHT_L1').hide();
    $('#tablees_l3').hide();
    $('#error').hide();
}
function format_ID_value(value, ID_type, ID_MPL, CurrUnit){
	var unit='';
        switch(ID_type) {
			case 0: unit="s"; break; //seconds
			case 1: break; //count
			case 2: unit=(value==0?'':'000'); break; //1000m³
			case 3: unit="%"; break; //%
			case 4: value=value/10;unit="KW"; break; //0.1 KW
			case 5: unit=Currency_lookup(CurrUnit);
					break; //currency
			case 6: unit=$('#E option:selected').attr('value'); break; //value is internally saved as KWh
			case 7: value=value/10;unit="%"; break; //0.1%
			case 8: value=value/100;unit="%"; break; //0.01%
			case 9: var date=new Date(value*1000); //real time clock, running hours
					var day=date.getDate();
					day=(day<10?'0':'')+day;
					var month=date.getMonth()+1;
					month=(month<10?'0':'')+month;
					var year=date.getFullYear();
					var hours=date.getUTCHours();
					hours=(hours<10?'0':'')+hours;
					var minutes=date.getMinutes();
					minutes=(minutes<10?'0':'')+minutes;
					value=day+'/'+month+'/'+year+' - '+hours+':'+minutes;
					break;
			case 10: value=value/10;unit=$('#T option:selected').attr('value'); break;//0.1°C
			case 11: value=value/1000;unit=$('#P option:selected').attr('value'); break; //mbar
			case 12: unit="RPM"; break; //RPM
			case 13: value=value/10;unit="RPH"; break; //0.1 RPH
			
			case 14: unit=$('#F option:selected').attr('value');break;//internally stored in l/s
			case 15: value=value/10000; unit="%"; break; //0.0001%
			
			case 255: return format_ID_discrete_value(value,ID_MPL);
        }
		
        switch(unit) {
			case "mbar(a)":
			case "mbar":value=value*1000;break;
			case "psi(a)":
            case "psi":value=14.5038*value;break;
			case "MPa(a)":
            case "MPa":value=0.1*value;break;
			case "kPa(a)":
			case "kPa":value=100*value;break;
			case "kg/cm\u00b2(a)":
            case "kg/cm\u00b2":value=1.019716*value;break;
            case "\u00b0F":value=(9/5)*value+(AI_type==16?0:32);break;
            case "K":value=value+(AI_type==16?0:273.15);break;
            case "mmHg":value=value*750.061683;break;
			case "inHg":value=value*29.5301;break;
			case "Torr":value=value*750.063755;break;
			case "kWh": break;
			case "MWh": value=value/1000; break;
			case "kBTu": value=value*3.41214;
						 value=(Math.round(value+'e2')/100);break;
			case "MBTu": value=(value*3.41214)/1000;
						 value=(Math.round(value+'e4')/10000);break;
			case "cfm":value=value/0.471951898662488;break;
			case "l/min":value=value*60;break;
			case "m³/h":value=value/0.277777778;break;
        }
		
		return value.toString() + " " + unit;
}
function format_ID_discrete_value(value,ID_MPL){
	switch(ID_MPL){
		case 2515:
			switch(value){
				case 0: return language.get('DRYCONT', 68);
				case 1:
				case 2:
				case 3:
				case 4: return language.get('DRYCONT', 32 + value);
				case 5: return language.get('DRYCONT', 38);
				case 6: return language.get('DRYCONT', 37);
			}	
	}	
}
function Currency_lookup(Currency_value){
	var unit="";
	
	switch (Currency_value){
		case 0: unit="$"; break; //dollar
		case 1: unit="€"; break; //euro
		case 2: unit="£"; break; //pound
		case 3: unit="\u00A5"; break; //Japanese yen
		case 4: unit="Fr."; break; //Swiss franc
		case 5: unit="Rs"; break; //Sri Lanka Rupee //Pakistani rupee
		case 6: unit="kr"; break; //Norwegian krone
		case 7: unit="R"; break; //South African rand //Russian ruble
		case 8: unit="R$"; break; //Brazilian real
		case 9: unit="KR"; break; //Estonian kroon
		case 10: unit="Ft"; break; //Hungarian forint
		case 11: unit="Rp"; break; //Indonesian rupiah
		case 12: unit="K\u010D"; break; //Czech koruna
		case 13: unit="Ls"; break; //Latvian lats
		case 14: unit="Lt"; break; //Lithuanian litas
		case 15: unit="RM"; break; //Malaysian ringgit
		case 16: unit="L"; break; //Honduran lempira //Romanian leu
		case 17: unit="Sk"; break; //Swedisch krones
		case 18: unit="YTL"; break; //Turkish new lira
		case 19: unit="\u20A9"; break; //North Korean won //South Korean won
		case 20: unit="p."; break; //Belarus Ruble
	}
	return unit;
}
function format_AI_value(AI_value, AI_type, AI_display, AI_pressuremeasurement, AI_absATMpres) {
    var str="";
    var value=0;
	
    if(AI_value>>>15) {
        value=-32767;
	}
	
    var value=value+(AI_value&0x7FFF);
    
    if(value==32767) {
        str=language.get('SENSORERROR', 1);
    } else {
        var unit;
		var AI = {unt:"",val:0};
		
        switch(AI_type) {
            case 0:	AI.unt = unit;
					AI.val = value;
					AI = format_AbsRel_pressure(AI_pressuremeasurement,AI_absATMpres,AI); //only possible to return 1 value, we need 2 values, so place it into a struct
																	//AI_absATMpres in mbar
					value = AI.val;
					unit = AI.unt;
					value=value/1000; //convert to bar
					break;
			
            case 1:value=value/10;unit=$('#T option:selected').attr('value');break;//0.1°C   // ---> loadlanguage.js 
            case 2:value=value/100;unit="µm";break;
            case 3:value=value/10;unit="mm";break;
            case 4:unit="µS/cm";break;
            case 5:unit="dB";break;
            case 6:unit=$('#F option:selected').attr('value');break;//internally stored in l/s
            case 7:value=value/10;unit="A";break;//0.1 A        
            case 8:unit="rpm";break;
            case 9: AI.unt = unit;  //cBar   // ---> loadlanguage.js
					AI.val = value;
					
					//AI_absATMpres in mbar, convert to mbar before calculation
					AI.val = AI.val*10
					AI = format_AbsRel_pressure(AI_pressuremeasurement,AI_absATMpres,AI); //only possible to return 1 value, we need 2 values, so place it into a struct

					value = AI.val;
					unit = AI.unt;
					
					value=value/1000; //convert to bar
					break;
            case 10:unit="%";break;
            case 11:value=value*10;unit="rpm";break;
            case 12:unit="ppm";break;
            case 13:value=value/100;unit="ppm";break;
            case 14:value=value/100;unit="% VV";break;
            case 15:unit="%";break;
            case 16:value=value/10;unit=$('#T option:selected').attr('value');break;//0.1°C   // ---> loadlanguage.js 
            case 17:value=value/1000;unit="";break;
            case 18:unit="";break;
            case 19:value=value/10;unit="kW";break;
            case 20:unit="kW";break;
            case 21:unit="J";break;
            case 22:unit="m³";break;
            case 23:unit="J/l";break;
            case 24:unit="";break;
            case 25:unit="Pa/s";break;
            case 26: unit = ""; break;
            case 27: value = value / 100; unit = "%"; break;
			case 29: value=value/10; unit="µm"; break;
			case 30: unit=$('#F option:selected').attr('value');//internally stored in l/min
					 value=value/60; break; //convert to l/s
			case 34: value=value/0.0277777777; //convert to l/s //internally stored in 0.1 m³/h
					 unit=$('#F option:selected').attr('value');break;
			case 35: AI.unt = unit; //dBar
					 AI.val = value;
					 
					 //AI_absATMpres in mbar, convert to mbar before calculation
					 AI.val = AI.val*100
					 AI = format_AbsRel_pressure(AI_pressuremeasurement,AI_absATMpres,AI); //only possible to return 1 value, we need 2 values, so place it into a struct
					 
					 value = AI.val;
					 unit = AI.unt;
					 
					 value=value/1000; //convert to bar
					 break;
        }
		
        switch(unit) {
			case "mbar(a)":
			case "mbar(g)":
			case "mbar":value=value*1000;break;
			case "psi(a)":
            case "psi":value=14.5038*value;break;
			case "MPa(a)":
            case "MPa":value=0.1*value;break;
			case "kPa(a)":
			case "kPa":value=100*value;break;
			case "kg/cm\u00b2(a)":
            case "kg/cm\u00b2":value=1.019716*value;break;
            case "\u00b0F":value=(9/5)*value+(AI_type==16?0:32);break;
            case "K":value=value+(AI_type==16?0:273.15);break;
            case "mmHg":value=value*750.061683;break;
			case "inHg":value=value*29.5301;break;
			case "Torr":value=value*750.063755;break;
			case "dbar(a)":
			case "dbar(g)":
			case "dbar": value=value*10;break;
			case "cfm":value=value/0.471951898662488;break;
			case "l/min":value=value*60;break;
			case "m³/h":value=value/0.277777778;break;
        }
		if ((unit!="mbar")&&(unit!="mbar(a)")&&(unit!="mbar(g)"))
			value = value.toFixed(AI_display);
        str = value.toString() + " " + unit; 
    }
    return str; 
}

function format_AbsRel_pressure (AI_pressuremeasurement, AI_absATMpres, AI) {
		if (AI_pressuremeasurement==1){ //sensor is measuring absolute pressure
						if (AI_absATMpres == null) //subindex was not present, show as relative value
							AI.unt=$('#P option:selected').attr('value'); 
						else if (AI_absATMpres==0)  //no value entered for atmospheric pressure
								AI.unt=$('#P option:selected').attr('value')+"(a)";
						else if (AI_absATMpres > 0) { //value entered for atmospheric pressure, subtract
								AI.val = AI.val-AI_absATMpres;
								AI.unt=$('#P option:selected').attr('value')+"(g)";
						}
					}
		else { //sensor is measuring relative pressure
						AI.unt=$('#P option:selected').attr('value');
		}
		
		return AI;
}

function format_AO_value(AO_value, AO_type, AO_display) {
    var value = AO_value;
    
    var unit;
    switch(AO_type) {
        case 27: value = value / 100; unit = "%"; break;
        case 28: value = value / 1000; unit = "mA"; break;
	}
	
    return value.toFixed(AO_display).toString() + " " + unit;
}

function format_CO_value(CO_value, CO_unit) {
    var str = "";
    var unit = "";
    var value = CO_value;
	
    switch(CO_unit) {
        case 0: 
			value = Math.floor(value / 3600); ((value == 1) ? unit = " " + language.get("HOURS", 2) : unit = " " + language.get("HOURS", 1)); 
			break;
        case 2:
			if(value == 0) 
				unit = " m3";
			else
				unit = "000 m3"; 
			break;
        case 3:
			unit=" %";
			break;
        case 4:
			value=(value/10).toFixed(1);
			unit=" kW";
			break;
        case 6:
			unit=" kWh";
			break;
		case 7:
			/*value=value/10;*/
			return value / 10;
			break;
		case 255:
			var hours   = Math.floor(value / 3600);
			var minutes = Math.floor((value - (hours * 3600)) / 60);
			var seconds = value - (hours * 3600) - (minutes * 60);

			if (hours   < 10)
				hours   = "0"+hours;
			if (minutes < 10)
				minutes = "0"+minutes;
			if (seconds < 10)
				seconds = "0"+seconds;
			value = hours+':'+minutes+':'+seconds;
			break;	
		default:
			break;
    }
	
    str = value.toString() + unit;
	
    return str;
}

function format_level(level) {
	return String.fromCharCode(64+level);	
}

function get_status_icon1(status, defaultIcon) {
	var src=defaultIcon;
	
    if((status&0x0008)==0x0008)
        src="images/Shutdown.gif";
    else if((status&0x0020)==0x0020)
        src="images/PermissiveStartFailure.gif";
    else if(((status&0x0002)==0x0002) || ((status&0x0004)==0x0004))
        src="images/Warning.gif";   
    else if((status&0x0010)==0x0010)
        src="images/Service.gif";    
    else if((status&0x0001)==0x0001)
        src="images/PreWarning.gif"; 
    else if((status&0x2000)==0x2000)
        src="images/PrePermissiveStartFailure.gif";
    else if((status&0x0040)==0x0040)
        src="images/SensorError.gif";          
		
    return src;
}

function get_status_icon2(status,defaultIcon)
{
    var src=defaultIcon;
    switch(status)
    {
        case 1:src="images/Stopped.gif";break;
        case 2:src="images/Unloaded.gif";break;
        case 4:src="images/Loaded.gif";break;
    }
    return src;
}

// debug purposes only
function dump(arr,level) {
	var dumped_text = "";
	if(!level) level = 0;
	
	//The padding given at the beginning of the line.
	var level_padding = "";
	for(var j=0;j<level+1;j++) level_padding += "    ";
	
	if(typeof(arr) == 'object') { //Array/Hashes/Objects 
		for(var item in arr) {
			var value = arr[item];
			
			if(typeof(value) == 'object') { //If it is an array,
				dumped_text += level_padding + "'" + item + "' ...\n";
				dumped_text += dump(value,level+1);
			} else {
				dumped_text += level_padding + "'" + item + "' => \"" + value + "\"\n";
			}
		}
	} else { //Stings/Chars/Numbers etc.
		dumped_text = "===>"+arr+"<===("+typeof(arr)+")";
	}
	return dumped_text;
}