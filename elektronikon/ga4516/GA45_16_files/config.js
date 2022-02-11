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
machine['Model'] = "GA45_16";
machine['Generation'] = 1;
machine['Serial'] = "API587025";
machine['OS'] = "1900523093";
machine['App'] = "1900525591";
machine['WebApp'] = "1900524854";
machine['RegulationType'] = 1;
/*machine_end*/



// Languages
var languages = [];
/*languages_begin*/
languages["Russian"] = "\u0050\u0423\u0043\u0043\u004B\u0418\u0419\u0020\u0028\u0052\u0075\u0073\u0073\u0069\u0061\u006E\u0029";
languages["English"] = "English";
languages["Dutch"] = "\u004E\u0065\u0064\u0065\u0072\u006C\u0061\u006E\u0064\u0073\u0020\u0028\u0044\u0075\u0074\u0063\u0068\u0029";
languages["Brazilian_Portuguese"] = "\u0050\u006F\u0072\u0074\u0075\u0067\u0075\u00EA\u0073\u0020\u0042\u0052\u0020\u0028\u0042\u0072\u0061\u007A\u0069\u006C\u0069\u0061\u006E\u0020\u0050\u006F\u0072\u0074\u0075\u0067\u0075\u0065\u0073\u0065\u0029";
languages["Bulgarian"] = "\u0411\u042A\u041B\u0413\u0041\u0050\u0043\u004B\u0418\u0020\u0028\u0042\u0075\u006C\u0067\u0061\u0072\u0069\u0061\u006E\u0029";
languages["Chinese"] = "\u4E2D\u6587\u0020\u0028\u0043\u0068\u0069\u006E\u0065\u0073\u0065\u0029";
languages["Croatian"] = "\u0048\u0072\u0076\u0061\u0074\u0073\u006B\u0069\u0020\u0028\u0043\u0072\u006F\u0061\u0074\u0069\u0061\u006E\u0029";
languages["Czech"] = "\u010C\u0065\u0161\u0074\u0069\u006E\u0061\u0020\u0028\u0043\u007A\u0065\u0063\u0068\u0029";
languages["Danish"] = "\u0044\u0061\u006E\u0073\u006B\u0020\u0028\u0044\u0061\u006E\u0069\u0073\u0068\u0029";
languages["Estonian"] = "\u0045\u0065\u0073\u0074\u0069\u0020\u0028\u0045\u0073\u0074\u006F\u006E\u0069\u0061\u006E\u0029";
languages["Finnish"] = "\u0053\u0075\u006F\u006D\u0069\u0020\u0028\u0046\u0069\u006E\u006E\u0069\u0073\u0068\u0029";
languages["French"] = "\u0046\u0072\u0061\u006E\u00E7\u0061\u0069\u0073\u0020\u0028\u0046\u0072\u0065\u006E\u0063\u0068\u0029";
languages["German"] = "\u0044\u0065\u0075\u0074\u0073\u0063\u0068\u0020\u0028\u0047\u0065\u0072\u006D\u0061\u006E\u0029";
languages["Greek"] = "\u0045\u039B\u039B\u0048\u004E\u0049\u004B\u0041\u0020\u0028\u0047\u0072\u0065\u0065\u006B\u0029";
languages["Hungarian"] = "\u004D\u0061\u0067\u0079\u0061\u0072\u0020\u0028\u0048\u0075\u006E\u0067\u0061\u0072\u0069\u0061\u006E\u0029";
languages["Italian"] = "\u0049\u0074\u0061\u006C\u0069\u0061\u006E\u006F\u0020\u0028\u0049\u0074\u0061\u006C\u0069\u0061\u006E\u0029";
languages["Japanese"] = "\u65E5\u672C\u8A9E\u0020\u0028\u004A\u0061\u0070\u0061\u006E\u0065\u0073\u0065\u0029";
languages["Korean"] = "\uD55C\uAD6D\uC5B4\u0020\u0028\u004B\u006F\u0072\u0065\u0061\u006E\u0029";
languages["Macedonian"] = "\u004D\u0061\u006B\u0065\u0064\u006F\u006E\u0073\u006B\u0069\u0020\u0028\u004D\u0061\u0063\u0065\u0064\u006F\u006E\u0069\u0061\u006E\u0029";
languages["Norwegian"] = "\u004E\u006F\u0072\u0073\u006B\u0020\u0028\u004E\u006F\u0072\u0077\u0065\u0067\u0069\u0061\u006E\u0029";
languages["Polish"] = "\u0050\u006F\u006C\u0073\u006B\u0069\u0020\u0028\u0050\u006F\u006C\u0069\u0073\u0068\u0029";
languages["Portuguese"] = "\u0050\u006F\u0072\u0074\u0075\u0067\u0075\u0065\u0073\u0020\u0028\u0050\u006F\u0072\u0074\u0075\u0067\u0075\u0065\u0073\u0065\u0029";
languages["Romanian"] = "\u0052\u006F\u006D\u0061\u006E\u0061\u0020\u0028\u0052\u006F\u006D\u0061\u006E\u0069\u0061\u006E\u0029";
languages["Serbian"] = "\u0053\u0072\u0070\u0073\u006B\u0069\u0020\u0028\u0053\u0065\u0072\u0062\u0069\u0061\u006E\u0029";
languages["Slovak"] = "\u0053\u006C\u006F\u0076\u0065\u006E\u0063\u0069\u006E\u0061\u0020\u0028\u0053\u006C\u006F\u0076\u0061\u006B\u0029";
languages["Slovenian"] = "\u0053\u006C\u006F\u0076\u0065\u006E\u0073\u006B\u006F\u0020\u0028\u0053\u006C\u006F\u0076\u0065\u006E\u0069\u0061\u006E\u0029";
languages["Spanish"] = "\u0045\u0073\u0070\u0061\u00F1\u006F\u006C\u0020\u0028\u0053\u0070\u0061\u006E\u0069\u0073\u0068\u0029";
languages["Swedish"] = "\u0053\u0076\u0065\u006E\u0073\u006B\u0061\u0020\u0028\u0053\u0077\u0065\u0064\u0069\u0073\u0068\u0029";
languages["Turkish"] = "\u0054\u00FC\u0072\u006B\u00E7\u0065\u0020\u0028\u0054\u0075\u0072\u006B\u0069\u0073\u0068\u0029";
languages["Latvian"] = "\u004C\u0061\u0074\u0076\u0069\u0065\u0161\u0075\u0020\u0028\u004C\u0061\u0074\u0076\u0069\u0061\u006E\u0029";
languages["Lithuanian"] = "\u004C\u0069\u0065\u0074\u0075\u0076\u0069\u0173\u0020\u006B\u0061\u006C\u0062\u0061\u0020\u0028\u004C\u0069\u0074\u0068\u0075\u0061\u006E\u0069\u0061\u006E\u0029";
languages["Hindi"] = "\u0045\u006E\u0067\u006C\u0069\u0073\u0068\u0020\u0028\u0048\u0069\u006E\u0064\u0069\u0029";
/*languages_end*/


// URL.TXT
/*url_begin*/
var brand_url = "http://www.atlascopco.com";
/*url_end*/

