var FILE_TYPES = {
	CSS: {index: 0, dir: "css", type: "text/css", dataType: "text"},
	IMAGE: {index: 2, dir: "images", type: "", dataType: "text"},
	JAVASCRIPT: {index: 1, dir: "js", type: "text/javascript", dataType: "script"}
};

var files = [];

// CSS files
var css_files = [];
css_files[0] = "style.css";

// Javascript files
var javascript_files = [];
javascript_files[0] = "generic.js";
javascript_files[1] = "Questions.js";
javascript_files[2] = "AnalogInputs.js";
javascript_files[3] = "AnalogOutputs.js";
javascript_files[4] = "CalculatedAnalogInputs.js";
javascript_files[5] = "DigitalInputs.js";
javascript_files[6] = "DigitalOutputs.js";
javascript_files[7] = "Converters.js";
javascript_files[8] = "Counters.js";
javascript_files[9] = "ServicePlan.js";
javascript_files[10] = "SpecialProtections.js";
javascript_files[11] = "SPMs.js";
javascript_files[12] = "MachineState.js";
javascript_files[13] = "checkbox.js";
javascript_files[14] = "table.js";
javascript_files[15] = "es.js";
javascript_files[16] = "IData.js";
javascript_files[17] = "DateFormat.js";

// Image files
var image_files = [];
image_files[0]="AccessKey.gif";
image_files[1]="Alarms.gif";
image_files[2]="AutomaticOperation.gif";
image_files[3]="Brandlogo.gif";
image_files[4]="CentralStopES.gif";
image_files[5]="CompressorShutdown.gif";
image_files[6]="EPCompressor.gif";
image_files[7]="FailedToLoad.gif";
image_files[8]="FSMotorStopped.gif";
image_files[9]="FSRunningLoaded.gif";
image_files[10]="FSRunningUnloaded.gif";
image_files[11]="GreenTriangle.gif";
image_files[12]="Loaded.gif";
image_files[13]="Manual.gif";
image_files[14]="MotorTripped.gif";
image_files[15]="NoAnswer.gif";
image_files[16]="NoCommunication.gif";
image_files[17]="NotAvailable.gif";
image_files[18]="NoValidCompressorType.gif";
image_files[19]="Ok.gif";
image_files[20]="PermissiveStartFailure.gif";
image_files[21]="PrePermissiveStartFailure.gif";
image_files[22]="PreWarning.gif";
image_files[23]="RedTriangle.gif";
image_files[24]="SensorError.gif";
image_files[25]="Service2.gif";
image_files[26]="Service.gif";
image_files[27]="Shutdown.gif";
image_files[28]="Stopped.gif";
image_files[29]="Transparent.gif";
image_files[30]="Unloaded.gif";
image_files[31]="VSCompressor.gif";
image_files[32]="Warning.gif";
image_files[33]="Whitespace.gif";

files[0] = css_files;
files[1] = javascript_files;
files[2] = image_files;

// Machine.txt
var machine = [];
/*machine_begin*/
machine['FileVer'] = "1.0";
machine['Model'] = "LARGO75_11";
machine['Generation'] = 1;
machine['Serial'] = "API176940";
machine['OS'] = "1900523111";
machine['App'] = "1900525735";
machine['WebApp'] = "1900524856";
machine['RegulationType'] = 1;
/*machine_end*/



// Languages
var languages = [];
/*languages_begin*/
languages["Russian"] = "\u0050\u0423\u0043\u0043\u004B\u0418\u0419\u0020\u0028\u0052\u0075\u0073\u0073\u0069\u0061\u006E\u0029";
languages["English"] = "English";
languages["Dutch"] = "\u004E\u0065\u0064\u0065\u0072\u006C\u0061\u006E\u0064\u0073\u0020\u0028\u0044\u0075\u0074\u0063\u0068\u0029";
/*languages_end*/


// URL.TXT
/*url_begin*/
var brand_url = "http://www.alup.com";
/*url_end*/

