function show_error(message) {
	$('#checks').hide();
	$('#tableleft_l1').hide();
	$('#tableleft_l12').hide();
	$('#tableright_l1').hide();
	$('#units').hide();
	$('#tablees_l3').hide();
	
	$('#error').html(message);
	$('#error').show();
}

function load_compressor() {
	page="machine";
	$('#page').html(machine['Model']);
		
	$('#units').hide();
	$('#tablees_l3').hide();
	$('#errors').hide();
	
	$('#checks').show();
    $('#TABLELEFT_L1').show();
    $('#TABLELEFT_L12').show();
    $('#TABLERIGHT_L1').show();
}

function load_es() {
	page="es";
	$('#page').html(language.get('LINK', 2));
	
	$('#units').hide();
	$('#checks').hide();
    $('#TABLELEFT_L1').hide();
    $('#TABLELEFT_L12').hide();
    $('#TABLERIGHT_L1').hide();
    $('#errors').hide();
	
	$('#tablees_l3').show();
}

function load_preferences() {
	page="preferences";
	$('#page').html(language.get('LINK', 3));
	
	$('#units').show();
	
	$('#checks').hide();
    $('#TABLELEFT_L1').hide();
    $('#TABLELEFT_L12').hide();
    $('#TABLERIGHT_L1').hide();
    $('#tablees_l3').hide();
    $('#errors').hide();
}

function format_AI_value(AI_value, AI_type, AI_display) {
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
        switch(AI_type) {
            case 0:value=value/1000;unit=$('#P option:selected').attr('value');break;//mbar   // ---> loadlanguage.js
            case 1:value=value/10;unit=$('#T option:selected').attr('value');break;//0.1°C   // ---> loadlanguage.js 
            case 2:value=value/100;unit="µm";break;
            case 3:value=value/10;unit="mm";break;
            case 4:unit="µS/cm";break;
            case 5:unit="dB";break;
            case 6:unit="l/s";break;
            case 7:value=value/10;unit="A";break;//0.1 A        
            case 8:unit="rpm";break;
            case 9:value=value/100;unit=$('#P option:selected').attr('value');break;//cBar   // ---> loadlanguage.js
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
			case 29: value=value*10; unit="Micron"; break;
        }
		
        switch(unit) {
            case "psi":value=14.5038*value;break;
            case "MPa":value=0.1*value;break;
            case "kg/cm\u00b2":value=1.019716*value;break;
            case "\u00b0F":value=(9/5)*value+32;break;
            case "K":value=value+273.15;break;
            case "mmHg":value=value*750.061683;break;
        }
        value = value.toFixed(AI_display);
        str = value.toString() + " " + unit; 
    }
    return str; 
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
        case 1:
			unit="";
			break;
        case 2:
			if(value == 0) {
				unit = " m3";
			} else {
				unit = "000 m3"; 
			}
			break;
        case 3:
			unit=" %";
			break;
        case 4:
			value=(value/10).toFixed(1);
			unit=" kW";
			break;
        case 5:
			unit="";
			break;
        case 6:
			unit="kWh";
			break;
		default:
			unit="";
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