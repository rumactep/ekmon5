var root = $(location).attr('href');
var vJSON = {
    DEVICE: [],
    ANALOGINPUTS: [],
    DIGITALINPUTS: [],
    COUNTERS: [],
    DIGITALOUTPUTS: [],
    CALCULATEDANALOGINPUTS: [],
    CONVERTERS: [],
    SPECIALPROTECTIONS: [],
    SPM2: [],
    SERVICEPLAN: [],
    ANALOGOUTPUTS: [],
    ES: {
        ACTIVE: false,
        NRCOMPRESSORS: null,
        NRDRYERS: null,
        STATE: null,
        REGULATIONPRESSURE: null,
        CONTROLVSD: null,
        COMPRESSORS: [],
        DRYERS: [],
        COMPRESSORMASTERBAR: null,
        DRYERMASTERBAR: null
    }
};
var language = new language();
var es = new es();
var page = "machine";

// generic method that executes an ajax request and calls the appropriate callback function depending on the outcome	
function request(url, method, callbackSuccess, callbackError, dataType) {
    if (dataType == null) {
        dataType = 'text';
    }

    $.ajax({
        url: root + url,
        type: method,
        dataType: dataType,
        success: function (data, textStatus, xhr) {
            callbackSuccess(data, textStatus, xhr);
        },
        error: function (xhr, textStatus, errorThrown) {
            // callbackError(xhr, textStatus, errorThrown);
        }
    });
}

$(document).ready(function () {
    $.ajaxSetup({
        async: false,
        timeout: 10000
    });
    jQuery.support.cors = true;

    init_interface();

    load_files();
});

function init_interface() {
    $('#error').hide();
    $('#units').hide();
    $('#tablees_l3').hide();
    $('#link2str').hide();
    $('#page').html(machine['Model']);
    $('#logo_link').attr('href', brand_url);
}

function load_files() {
    for (var typeIndex in FILE_TYPES) {
        var fileType = FILE_TYPES[typeIndex];
        for (var fileIndex in files[fileType.index]) {
            var path = fileType.dir + '/' + files[fileType.index][fileIndex];
            request_file(path, typeIndex, fileType);
        }
    }
    populate_language_dropdown();
    // 4: set timer
    window.setInterval(refresh_data, 10000);
}

function request_file(path, typeIndex, fileType) {
    path = path + '?_=' + new Date().getTime();
    request(path,
        'GET',
        // success
        function (data) {
            switch (typeIndex) {
                case "CSS":
                    var fileref = document.createElement('link');
                    fileref.type = fileType.type;
                    fileref.rel = 'stylesheet';
                    fileref.href = path;
                    document.getElementsByTagName('head')[0].appendChild(fileref);
                    //$('head').append(fileref);
                    break;
                case "JAVASCRIPT":
                    var fileref = document.createElement('script');
                    fileref.type = fileType.type;
                    fileref.src = path;
                    document.getElementsByTagName('head')[0].appendChild(fileref);
                    //$('head').append(fileref);
                    break;
                case "IMAGE":
                    var fileref = new Image();
                    fileref.src = path;

                    if (path.indexOf('Brandlogo') != -1) {
                        $('#logo_link img').attr('src', path);
                    }

                    fileref.onerror = function () {
                        request_file(path, typeIndex, fileType);
                    }

                    break;
            }
        },
        // error
        function (xhr, textStatus, errorThrown) {
            request_file(path, typeIndex, fileType);
        },
        fileType.dataType
    );
}

function populate_language_dropdown() {
    $('#language_select').empty();

    for (var i in languages) {
        $('#language_select')
            .append($('<option>', {
                value: i
            })
                .text(languages[i]));
    }

    //var stored_language = $.cookies.get('LangName');
    //if(stored_language != null) {
    //	$('#language_select').val(stored_language).attr('selected', true);
    //}

    load_language(true);
}

function load_settings() {
    $('#elektronikon').html(language.get('HEAD', 1));

    var vQuestions = new Questions();

    Q_2000_AI(vQuestions);
    Q_2000_DI(vQuestions);
    Q_2000_CNT(vQuestions);
    Q_2000_CNV(vQuestions);
    Q_2000_DO(vQuestions);
    Q_2000_CAI(vQuestions);

    //Q_2000_CNV(vQuestions);

    Q_2000_SPR(vQuestions);
    Q_2000_AO(vQuestions);
    Q_2000_SPM(vQuestions);
    Q_3000_ES(vQuestions);
    Q_2000_SPL(vQuestions);
    Q_2000_MMT(vQuestions);

    vQuestions.SendReceive(); /*********************************************************************************/

    A_2000_AI(vQuestions, vJSON.ANALOGINPUTS);
    A_2000_DI(vQuestions, vJSON.DIGITALINPUTS);
    A_2000_CNT(vQuestions, vJSON.COUNTERS);
    A_2000_CNV(vQuestions, vJSON.CONVERTERS);
    A_2000_DO(vQuestions, vJSON.DIGITALOUTPUTS);
    A_2000_CAI(vQuestions, vJSON.CALCULATEDANALOGINPUTS);

    A_2000_SPR(vQuestions, vJSON.SPECIALPROTECTIONS);
    A_2000_AO(vQuestions, vJSON.ANALOGOUTPUTS);
    A_2000_SPM(vQuestions, vJSON.SPM2);
    A_3000_ES(vQuestions, vJSON.ES); //!!! нет в чек боксах
    A_2000_SPL(vQuestions, vJSON.SERVICEPLAN);
    A_2000_MMT(vQuestions, vJSON.DEVICE); // !!! нет в чек боксах

    create_checkbox('ANALOGINPUTS', 1, vJSON.ANALOGINPUTS);
    create_checkbox('ANALOGOUTPUTS', 2, vJSON.ANALOGOUTPUTS);
    create_checkbox('COUNTERS', 3, vJSON.COUNTERS);
    create_checkbox('CONVERTERS', 4, vJSON.CONVERTERS);
    create_checkbox('DIGITALINPUTS', 6, vJSON.DIGITALINPUTS);
    create_checkbox('DIGITALOUTPUTS', 7, vJSON.DIGITALOUTPUTS);
    create_checkbox('SPECIALPROTECTIONS', 8, vJSON.SPECIALPROTECTIONS);
    create_checkbox('SERVICEPLAN', 9, vJSON.SERVICEPLAN);
    create_checkbox('CALCULATEDANALOGINPUTS', 10, vJSON.CALCULATEDANALOGINPUTS);
    create_checkbox('SPM', 11, vJSON.SPM2);

    create_tables();
    //refresh_data();
}

function refresh_data() {
    var vQuestions = new Questions();

    Q_3000_AI(vQuestions, vJSON.ANALOGINPUTS);
    Q_3000_DI(vQuestions, vJSON.DIGITALINPUTS);
    Q_3000_CNT(vQuestions, vJSON.COUNTERS);
    Q_3000_CNV(vQuestions, vJSON.CONVERTERS);
    Q_3000_DO(vQuestions, vJSON.DIGITALOUTPUTS);
    Q_3000_CAI(vQuestions, vJSON.CALCULATEDANALOGINPUTS);

    Q_3000_SPR(vQuestions, vJSON.SPECIALPROTECTIONS);
    Q_3000_AO(vQuestions, vJSON.ANALOGOUTPUTS);
    Q_3000_SPM(vQuestions, vJSON.SPM2);
    Q_3000_ES(vQuestions, vJSON.ES);
    Q_3000_SPL(vQuestions, vJSON.SERVICEPLAN);
    Q_3000_MS(vQuestions);

    try {
        vQuestions.SendReceive(); /*********************************************************************************/

        A_3000_AI(vQuestions, vJSON.ANALOGINPUTS);
        A_3000_DI(vQuestions, vJSON.DIGITALINPUTS);
        A_3000_CNT(vQuestions, vJSON.COUNTERS);
        A_3000_CNV(vQuestions, vJSON.CONVERTERS);
        A_3000_DO(vQuestions, vJSON.DIGITALOUTPUTS);
        A_3000_CAI(vQuestions, vJSON.CALCULATEDANALOGINPUTS);

        A_3000_SPR(vQuestions, vJSON.SPECIALPROTECTIONS);
        A_3000_AO(vQuestions, vJSON.ANALOGOUTPUTS);
        A_3000_SPM(vQuestions, vJSON.SPM2);
        A_3000_ES(vQuestions, vJSON.ES);
        A_3000_SPL(vQuestions, vJSON.SERVICEPLAN);
        A_3000_MS(vQuestions, vJSON.DEVICE);

        fill_tables();
    } catch (ee) { }
    var now = new Date();
    //var str23 = ee.get;
    $('#load_time').html(language.get('FOOT', 1) + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds() + "  " + now.getDate() + "/" + (now.getMonth() + 1) + "/" + now.getFullYear());
}

function create_tables() {
    create_table('MACHINESTATE', vJSON.DEVICE, 2, language.get('TABLETITLE', 5), '');
    create_table('ANALOGINPUTS', vJSON.ANALOGINPUTS, 3, language.get('TABLETITLE', 1), language.get('TABLEVALUE', 1), '');
    create_table('ANALOGOUTPUTS', vJSON.ANALOGOUTPUTS, 3, language.get('TABLETITLE', 2), language.get('TABLEVALUE', 1), '');
    create_table('COUNTERS', vJSON.COUNTERS, 2, language.get('TABLETITLE', 3), language.get('TABLEVALUE', 1));
    create_table('CONVERTERS', vJSON.CONVERTERS, 3, language.get('TABLETITLE', 4), language.get('TABLEVALUE', 1), '');
    create_table('DIGITALINPUTS', vJSON.DIGITALINPUTS, 3, language.get('TABLETITLE', 6), language.get('TABLEVALUE', 1), '');

    create_table('DIGITALOUTPUTS', vJSON.DIGITALOUTPUTS, 3, language.get('TABLETITLE', 7), language.get('TABLEVALUE', 1), '');
    create_table('SPECIALPROTECTIONS', vJSON.SPECIALPROTECTIONS, 2, language.get('TABLETITLE', 8), '');
    create_table('SERVICEPLAN', vJSON.SERVICEPLAN, 4, language.get('TABLETITLE', 9), '', '', language.get('TABLEVALUE', 2));
    create_table('CALCULATEDANALOGINPUTS', vJSON.CALCULATEDANALOGINPUTS, 3, language.get('TABLETITLE', 10), language.get('TABLEVALUE', 2), '');
    create_table('SPM', vJSON.SPM2, 3, language.get('TABLETITLE', 12), '', ''); // mist nog index in TABLETITLE

    es.clean();
    es.update(vJSON.ES);
}

function fill_tables() {
    update_table('ANALOGINPUTS', vJSON.ANALOGINPUTS);
    update_table('ANALOGOUTPUTS', vJSON.ANALOGOUTPUTS);
    update_table('COUNTERS', vJSON.COUNTERS);
    update_table('CONVERTERS', vJSON.CONVERTERS);
    update_table('DIGITALINPUTS', vJSON.DIGITALINPUTS);
    update_table('DIGITALOUTPUTS', vJSON.DIGITALOUTPUTS);

    update_table('SPECIALPROTECTIONS', vJSON.SPECIALPROTECTIONS);
    update_table('SERVICEPLAN', vJSON.SERVICEPLAN);
    update_table('MACHINESTATE', vJSON.DEVICE);
    update_table('CALCULATEDANALOGINPUTS', vJSON.CALCULATEDANALOGINPUTS);
    update_table('SPM', vJSON.SPM2);

    es.update(vJSON.ES);
}

function load_language() {
    var firstRun = arguments[0];

    $('#errors').hide();

    var selected_language = $('#language_select option:selected').attr('value');

    // load the selected language
    request('languages/' + selected_language + '.txt',
        'GET',
        // success
        function (data) {
            language.read(data);

            $('#serial').html(language.get('SERIAL', 1) + ' ' + machine['Serial']);
            $('#link1str').html(language.get('LINK', 1));
            $('#link3str').html(language.get('LINK', 3));

            $('#langs_description').html(language.get('LANGS', 1));

            $('#PRESSUREUNITstr').html(language.get('UNIT', 1));
            $('#TEMPERATUREUNITstr').html(language.get('UNIT', 2));

            $('#ANALOGINPUTStext').html(language.get('CHECK', 1));
            $('#ANALOGOUTPUTStext').html(language.get('CHECK', 2));
            $('#COUNTERStext').html(language.get('CHECK', 3));
            $('#CONVERTERStext').html(language.get('CHECK', 4));
            $('#DIGITALINPUTStext').html(language.get('CHECK', 6));
            $('#DIGITALOUTPUTStext').html(language.get('CHECK', 7));
            $('#SPECIALPROTECTIONStext').html(language.get('CHECK', 8));
            $('#SERVICEPLANtext').html(language.get('CHECK', 9));
            $('#CALCULATEDANALOGINPUTStext').html(language.get('CHECK', 10));
            $('#SPMtext').html(language.get('CHECK', 11));

            switch (page) {
                case "machine":
                    load_compressor();
                    break;
                case "es":
                    load_es();
                    break;
                case "preferences":
                    load_preferences();
                    break;
            }

            if (firstRun == true) {
                load_settings();
            } else {
                //create_tables();
                fill_tables();
            }
        },
        // error
        function () {
            show_error('An error occured while retrieving the language file');
        }
    );

    if (firstRun == true) {
        load_settings();
    } else {
        //create_tables();
        fill_tables();
    }


    // set cookie
    var now = new Date();
    now.setTime(now.getTime() + 365 * 24 * 60 * 60 * 1000);
    //$.cookies.set('LangName', selected_language, {expiresAt: now});
}

function language() {
    this.language = new Array();

    this.read = function (data) {
        this.language = new Array();

        // split each line
        var lines = data.split('\r\n');
        for (var i in lines) {
            // split key value pairs using delimiter
            var parts = lines[i].split('$$');

            if (parts.length > 1) {
                var info = parts[0].split('_');
                var type = info[0];
                var idx = parseInt(info[1], 10);

                var argc = info.length;
                var strings = new Array(argc - 1);

                strings[0] = parts[1];
                if (argc == 4) {
                    strings[1] = info[2];
                    strings[2] = info[3];
                }

                if (typeof this.language[type] == 'undefined') {
                    // the type array has not been defined
                    // create a new one
                    this.language[type] = new Array();
                }

                if (argc != 4) {
                    this.language[type][idx] = strings[0];
                } else {
                    this.language[type][idx] = new Array();
                    for (var i in strings) {
                        this.language[type][idx][i] = strings[i];
                    }
                }
            }
        }
    }

    this.get = function (type, key, index) {
        if (typeof this.language[type] != 'undefined') {
            if (typeof key != 'undefined' && typeof this.language[type][key] != 'undefined') {
                if (this.language[type][key].length > 1 && typeof this.language[type][key][index] != 'undefined') {
                    return this.language[type][key][index];
                } else {
                    if (typeof this.language[type][key] == 'object') {
                        return this.language[type][key][0];
                    }

                    return this.language[type][key];
                }
            }

            return "*** " + type + " " + key + " ***";
        }

        return "*** " + type + " ***";
    }
}

function es() {
    this.clean = function () {
        $('#tableCompressors').html('');
        $('#tableDryers').html('');
        this.CompressorCount = 0;
        this.DryerCount = 0;
    }

    this.display = function () {
        var isESActive = arguments[0];
        var nrOfCompressors = arguments[1];
        var nrOfDryers = arguments[2];
        var totalAvailableWidth = $('#tables').width() - 250;

        if (isESActive && this.CompressorCount != nrOfCompressors) {

            $('#tableCompressors').html('<tr style="text-align:center"><td colspan="7" style="font-size:28px">Compressors</td></tr>');
            var row = document.createElement("tr");
            for (var j = 0; j < 6; j++) {
                var cell = document.createElement("td");
                cell.style.visibility = (j < nrOfCompressors ? "visible" : "hidden");
                cell.width = Math.round(totalAvailableWidth / 6) + "px";
                cell.style.verticalAlign = 'top';
                cell.style.margin = "0px";

                cell.innerHTML = "<div class='compressor_container'>" +
                    "<h3>" + (j + 1) + "</h3>" +
                    "<div id='Compressor" + j + "BOX' class='compressor_box'>" +
                    "<div id='Compressor" + j + "U' class='compressor_u'><table></table></div>" +
                    "<div id='Compressor" + j + "D' class='compressor_d'><table></table></div>" +
                    "</div>" +
                    "<div class='compressor_bottom'><table></table></div>" +
                    "<img id='Compressor" + j + "EXCEPTIONSTATUS' src='images/Transparent.gif' style='position:relative; top:-185px; left: -8px;' />" +
                    "<img id='Compressor" + j + "SERVICE' src='images/Transparent.gif' style='position:relative; top:-175px; left: -8px;'/>" +
                    "</div>";
                row.appendChild(cell);
            }
            var cell = document.createElement("td");
            cell.width = "220px";
            cell.style.verticalAlign = 'top';
            cell.style.paddingLeft = "30px";

            cell.innerHTML = "<div style='width:275px'>" +
                "<h3 style='width:75px;text-align:center'>ESi</h3>" +
                "<div style='float:left;width:75px;height:200px;border:1px solid #000000;background-color:#0000FF;margin:0 0 0 0'>" +
                "<div id='ESU' style='background-color:#FFFFFF;margin:0 0 0 0'><table></table></div>" +
                "<div id='ESD' style='background-color:#FF0000;margin:0 0 0 0'><table></table></div>" +
                "</div>" +
                "<table style='height:202px;float:left'>" +
                //MAXIMAL VALUE
                "<tr>" +
                "<td>" +
                "<div id='ESMAX_TOT' style='position:relative;width:100px;height:30px'>" +
                "<div id='ESMAX' style='float:left;border-color:Black;border-width:1px;border-style:solid;width:25px;background-color:Black; margin:7px 5px 7px 0px'><table></table></div>" +
                "<div id='ESMAXVALUE' style='float:left'>x.xxx</div>" +
                "</div>" +
                "</td>" +
                "</tr>" +
                //MINIMAL VALUE
                "<tr>" +
                "<td>" +
                "<div id='ESMIN_TOT' style='position:relative;width:100px;height:30px'>" +
                "<div id='ESMIN' style='float:left;border-color:Black;border-width:1px;border-style:solid;width:25px;background-color:Black; margin:7px 5px 7px 0px'><table></table></div>" +
                "<div id='ESMINVALUE' style='float:left'>x.xxx</div>" +
                "</div>" +
                "</td>" +
                "</tr>" +
                "</table>" +
                "</div>" +
                "<div style='position:relative;top:0px;width:75px;text-align:center'>" +
                "<img id='ESSTATUS' src='images/Alarms.gif' />" +
                "</div>" +
                //ACTUAL VALUE
                "<div id='ESUNIT' style='position:relative;top:5px;font-size:14px'>bar</div>";

            row.appendChild(cell);
            $('#tableCompressors').append(row);

            this.CompressorCount = nrOfCompressors;
        }

        if (isESActive && this.DryerCount != nrOfDryers) {
            $('#tableDryers').html('<tr style="text-align:center"><td colspan="6" style="font-size:28px">Dryers</td></tr>');
            var row = document.createElement("tr");
            for (var j = 0; j < 6; j++) {
                var cell = document.createElement("td");
                cell.style.visibility = (j < nrOfDryers ? "visible" : "hidden");
                cell.width = Math.round(totalAvailableWidth / nrOfCompressors) + "px";
                cell.style.verticalAlign = 'top';
                cell.style.margin = "0px";

                cell.innerHTML = "<div class='compressor_container'>" +
                    "<h3>" + (j + 1) + "</h3>" +
                    "<div id='Dryer" + j + "BOX' class='compressor_box'>" +
                    "<div id='Dryer" + j + "U' class='compressor_u'><table></table></div>" +
                    "<div id='Dryer" + j + "D' class='compressor_d'><table></table></div>" +
                    "</div>" +
                    "<div class='compressor_bottom'><table></table></div>" +
                    "<img id='Dryer" + j + "EXCEPTIONSTATUS' src='images/Transparent.gif' style='position:relative; top:-185px; left: -8px;' />" +
                    "<img id='Dryer" + j + "SERVICE' src='images/Transparent.gif' style='position:relative; top:-175px; left: -8px;'/>" +
                    "</div>";
                row.appendChild(cell);
            }
            var cell = document.createElement("td");
            cell.width = "220px";
            cell.style.verticalAlign = 'top';
            cell.style.paddingLeft = "30px";

            cell.innerHTML = "<div style='width:275px'>" +
                "<div style='float:left;width:75px;height:200px;border:1px solid #000000;background-color:#0000FF;margin:0 0 0 0'>" +
                "<div id='ESU_Dryer' style='background-color:#FFFFFF;margin:0 0 0 0'><table></table></div>" +
                "<div id='ESD_Dryer' style='background-color:#FF0000;margin:0 0 0 0'><table></table></div>" +
                "</div>" +
                "<table style='height:202px;float:left'>" +
                //MAXIMAL VALUE
                "<tr>" +
                "<td>" +
                "<div id='ESMAX_TOT_Dryer' style='position:relative;width:100px;height:30px'>" +
                "<div id='ESMAX_Dryer' style='float:left;border-color:Black;border-width:1px;border-style:solid;width:25px;background-color:Black; margin:7px 5px 7px 0px'><table></table></div>" +
                "<div id='ESMAXVALUE_Dryer' style='float:left'>x.xxx</div>" +
                "</div>" +
                "</td>" +
                "</tr>" +
                //MINIMAL VALUE
                "<tr>" +
                "<td>" +
                "<div id='ESMIN_TOT_Dryer' style='position:relative;width:100px;height:30px'>" +
                "<div id='ESMIN_Dryer' style='float:left;border-color:Black;border-width:1px;border-style:solid;width:25px;background-color:Black; margin:7px 5px 7px 0px'><table></table></div>" +
                "<div id='ESMINVALUE_Dryer' style='float:left'>x.xxx</div>" +
                "</div>" +
                "</td>" +
                "</tr>" +
                "</table>" +
                "</div>" +
                "<div style='position:relative;top:0px;width:75px;text-align:center'>" +
                "<img id='ESSTATUS_Dryer' src='images/Alarms.gif' />" +
                "</div>" +
                //ACTUAL VALUE
                "<div id='ESUNIT_Dryer' style='position:relative;top:5px;font-size:14px'>bar</div>";

            row.appendChild(cell);
            $('#tableDryers').append(row);

            this.DryerCount = nrOfDryers;
        }
    }

    this.update = function () {
        var json = arguments[0];

        if (json.ACTIVE) {
            $('#link2str').show();
            if ((this.CompressorCount != json.NRCOMPRESSORS) || (this.DryerCount != json.NRDRYERS)) {
                this.clean();
                this.display(json.ACTIVE, json.NRCOMPRESSORS, json.NRDRYERS);
            }
        } else
            $('#link2str').hide();

        if (typeof json != 'undefined' && json.ACTIVE) { //$('#tablees_l3').is(':visible') &&
            es.set_master_state(json.STATE);
            if (json.COMPRESSORMASTERBAR != null)
                es.set_master_value(json.COMPRESSORMASTERBAR);
            if (json.DRYERMASTERBAR != null)
                es.set_master_value_Dryer(json.DRYERMASTERBAR);


            // slaves
            for (var j = 0; j < json.NRCOMPRESSORS; j++) {
                if (typeof json.COMPRESSORS[j] != 'undefined') {
                    es.set_slave_status(j, json.COMPRESSORS[j].SUMMARY1, json.CONTROLVSD);
                    es.set_slave_service(j, json.COMPRESSORS[j].SUMMARY2);
                    es.set_slave_type_value(j, json.COMPRESSORS[j]);
                }
            }
            for (var j = 0; j < json.NRDRYERS; j++) {
                if (typeof json.DRYERS[j] != 'undefined') {
                    es.set_dryer_value(j, json.DRYERS[j]);
                    es.set_dryer_status(j, json.DRYERS[j].LOWERICON);
                    es.set_dryer_service(j, json.DRYERS[j].UPPERICON);
                }
            }
        }
    }

    this.set_slave_status = function () {
        var nr = arguments[0];
        var exception = arguments[1];
        var controlVSD = arguments[2];

        var image = "images/Transparent.gif";

        if (exception == 0 || exception == 11 || exception == 60 || (exception > 30 && exception < 36)) {
            switch (exception) {
                case 0:
                    image = "images/Transparent.gif";
                    break;
                case 11:
                    image = "images/AccessKey.gif";
                    break;
                case 31:
                    image = "images/Alarms.gif";
                    break;
                case 32:
                    image = "images/Service2.gif";
                    break;
                case 33:
                    image = "images/FailedToLoad.gif";
                    break;
                case 34:
                    image = "images/MotorTripped.gif";
                    break;
                case 35:
                    image = "images/Manual.gif";
                    break;
                case 60:
                    var image = "images/Local.gif";
                    break;
            }

            $('#Compressor' + nr + 'BOX').css('border-style', 'solid');

            if (nr + 1 == controlVSD) {
                $('#Compressor' + nr + 'D').css('background-color', '#F8FC80');
            } else {
                $('#Compressor' + nr + 'D').css('background-color', '#B8ECF8');
            }
        } else {
            switch (exception) {
                case 10:
                    var image = "images/NoValidCompressorType.gif";
                    break;
                case 20:
                    var image = "images/NoCommunication.gif";
                    break;
                case 30:
                    var image = "images/CompressorShutdown.gif";
                    break;
                case 40:
                    var image = "images/NoAnswer.gif";
                    break;
                case 50:
                    var image = "images/NotAvailable.gif";
                    break;
            }

            $('#Compressor' + nr + 'BOX').css('border-style', 'dashed');
            $('#Compressor' + nr + 'D').css('background-color', '#FFFFFF');
        }

        $('#Compressor' + nr + 'EXCEPTIONSTATUS').attr('src', image);
    }
    this.set_dryer_status = function () {
        var nr = arguments[0];
        var exception = arguments[1];

        var image = "images/Transparent.gif";

        if (exception == 0 || exception == 11 || exception == 60 || (exception > 30 && exception < 36) || exception == 72) {
            switch (exception) {
                case 0:
                    image = "images/Transparent.gif";
                    break;
                case 11:
                    image = "images/AccessKey.gif";
                    break;
                case 31:
                    image = "images/Alarms.gif";
                    break;
                case 32:
                    image = "images/Service2.gif";
                    break;
                case 33:
                    image = "images/FailedToLoad.gif";
                    break;
                case 34:
                    image = "images/MotorTripped.gif";
                    break;
                case 35:
                    image = "images/Manual.gif";
                    break;
                case 60:
                    image = "images/Local.gif";
                    break;
                case 72:
                    image = "images/WaterDrop.png";
                    break;
            }

            $('#Dryer' + nr + 'BOX').css('border-style', 'solid');

        } else {
            switch (exception) {
                case 10:
                    image = "images/NoValidCompressorType.gif";
                    break;
                case 20:
                    image = "images/NoCommunication.gif";
                    break;
                case 30:
                    image = "images/CompressorShutdown.gif";
                    break;
                case 40:
                    image = "images/NoAnswer.gif";
                    break;
                case 50:
                    image = "images/NotAvailable.gif";
                    break;
                case 70:
                    image = "images/ErrorPressure.png";
                    break;
                case 71:
                    image = "images/ErrorDewpoint.png";
                    break;
            }

            $('#Dryer' + nr + 'BOX').css('border-style', 'dashed');
        }

        $('#Dryer' + nr + 'EXCEPTIONSTATUS').attr('src', image);
    }

    this.set_dryer_service = function () {
        var nr = arguments[0];
        var service = arguments[1];

        var image = "images/Transparent.gif";
        switch (service) {
            case 0:
                var image = "images/Transparent.gif";
                break;
            case 10:
                var image = "images/NoValidCompressorType.gif";
                break;
            case 11:
                var image = "images/AccessKey.gif";
                break;
            case 20:
                var image = "images/NoCommunication.gif";
                break;
            case 30:
                var image = "images/CompressorShutdown.gif";
                break;
            case 31:
                var image = "images/Alarms.gif";
                break;
            case 32:
                var image = "images/Service2.gif";
                break;
            case 33:
                var image = "images/FailedToLoad.gif";
                break;
            case 34:
                var image = "images/MotorTripped.gif";
                break;
            case 35:
                var image = "images/Manual.gif";
                break;
            case 40:
                var image = "images/NoAnswer.gif";
                break;
            case 50:
                var image = "images/NotAvailable.gif";
                break;
        }

        $('#Dryer' + nr + 'SERVICE').attr('src', image);
    }
    this.set_slave_service = function () {
        var nr = arguments[0];
        var service = arguments[1];

        var image = "images/Transparent.gif";
        switch (service) {
            case 0:
                var image = "images/Transparent.gif";
                break;
            case 10:
                var image = "images/NoValidCompressorType.gif";
                break;
            case 11:
                var image = "images/AccessKey.gif";
                break;
            case 20:
                var image = "images/NoCommunication.gif";
                break;
            case 30:
                var image = "images/CompressorShutdown.gif";
                break;
            case 31:
                var image = "images/Alarms.gif";
                break;
            case 32:
                var image = "images/Service2.gif";
                break;
            case 33:
                var image = "images/FailedToLoad.gif";
                break;
            case 34:
                var image = "images/MotorTripped.gif";
                break;
            case 35:
                var image = "images/Manual.gif";
                break;
            case 40:
                var image = "images/NoAnswer.gif";
                break;
            case 50:
                var image = "images/NotAvailable.gif";
                break;
        }

        $('#Compressor' + nr + 'SERVICE').attr('src', image);
    }


    this.set_dryer_value = function () {
        var nr = arguments[0];
        var dryer = arguments[1];

        $('#Dryer' + nr + 'U').height(2 * (100 - dryer.BARVALUE));
        $('#Dryer' + nr + 'D').height(2 * dryer.BARVALUE);
    }
    this.set_slave_type_value = function () {
        var nr = arguments[0];
        var slave = arguments[1];

        switch (slave.TYPE) {
            case 1: //LU
            case 3: //Pump
                if ((slave.MS & 0x01) == 0x01) { //stop  
                    $('#Compressor' + nr + 'U').height(200);
                    $('#Compressor' + nr + 'D').height(0);
                }
                if ((slave.MS & 0x02) == 0x02) { //unload
                    $('#Compressor' + nr + 'U').height(160);
                    $('#Compressor' + nr + 'D').height(40); //20%
                }
                if ((slave.MS & 0x04) == 0x04) { //loaded
                    $('#Compressor' + nr + 'U').height(0);
                    $('#Compressor' + nr + 'D').height(200);
                }
                break;
            case 2: //VSD
                $('#Compressor' + nr + 'U').height(200 - Math.round((200 / slave.MAX) * slave.ACT));
                $('#Compressor' + nr + 'D').height(Math.round((200 / slave.MAX) * slave.ACT));
                break;

            default: //same as LU and Pump
                if ((slave.MS & 0x01) == 0x01) { //stop  
                    $('#Compressor' + nr + 'U').height(200);
                    $('#Compressor' + nr + 'D').height(0);
                }
                if ((slave.MS & 0x02) == 0x02) { //unload
                    $('#Compressor' + nr + 'U').height(160);
                    $('#Compressor' + nr + 'D').height(40); //20%
                }
                if ((slave.MS & 0x04) == 0x04) { //loaded
                    $('#Compressor' + nr + 'U').height(0);
                    $('#Compressor' + nr + 'D').height(200);
                }
                break;
        }
    }

    this.set_master_state = function () {
        var image = "";
        switch (arguments[0]) {
            case 0x2011:
                image = "images/NotAvailable.gif";
                break;
            case 0x2021:
                image = "images/NoCommunication.gif";
                break;
            case 0x2032:
                image = "images/CentralStopES.gif";
                break;
            case 0x2091:
                image = "images/CompressorShutdown.gif";
                break;
            case 0x6012:
                image = "images/AutomaticOperation.gif";
                break;
        }

        $('#ESSTATUS').attr('src', image);
        $('#ESSTATUS_Dryer').attr('src', image);
    }

    this.set_master_value_Dryer = function (json) {
        if (json.METHODOFFILLING) //top down
        {
            $('#ESU_Dryer').css('height', json.PERCENTAGE + '%');
            $('#ESD_Dryer').css('height', (100 - json.PERCENTAGE) + '%');
            $('#ESU_Dryer').css('background-color', (json.INRANGE ? 'Green' : 'Red'));
            $('#ESD_Dryer').css('background-color', 'White');
        } else //bottom up
        {
            $('#ESU_Dryer').css('height', (100 - json.PERCENTAGE) + '%');
            $('#ESD_Dryer').css('height', json.PERCENTAGE + '%');
            $('#ESU_Dryer').css('background-color', 'White');
            $('#ESD_Dryer').css('background-color', (json.INRANGE ? 'Green' : 'Red'));
        }
        var unit = $('#P option:selected').attr('value');
        var value1 = json.LEVEL1 / 1000;
        var value2 = json.LEVEL2 / 1000;
        switch (unit) {
            case "psi":
                value1 = 14.5038 * value1;
                value2 = 14.5038 * value2;
                break;
            case "MPa":
                value1 = 0.1 * value1;
                value2 = 0.1 * value2;
                break;
            case "kg/cm\u00b2":
                value1 = 1.019716 * value1;
                value2 = 1.019716 * value2;
                break;
            case "mmHg":
                value1 = value1 * 750.061683;
                value2 = value2 * 750.061683;
                break;
        }

        $('#ESMAXVALUE_Dryer').text((value1 > value2 ? value1 : value2).toFixed(3));
        $('#ESMINVALUE_Dryer').text((value1 < value2 ? value1 : value2).toFixed(3));
        if (json.TYPE) {
            $('#ESMAX_TOT_Dryer').css('top', '-43px');
            $('#ESMIN_TOT_Dryer').css('top', '57px');
        } else {
            $('#ESMAX_TOT_Dryer').css('top', '6px');
            $('#ESMIN_TOT_Dryer').css('top', '6px');
        }

        var str = "";
        var value = 0;

        if (json.ACT >>> 15) {
            value = -32767;
        }
        var value = value + (json.ACT & 0x7FFF);

        if (value == 32767)
            str = language.get('SENSORERROR', 1);
        else {
            value = value / 1000;
            var unit = $('#P option:selected').attr('value');
            switch (unit) {
                case "psi":
                    value = 14.5038 * value;
                    break;
                case "MPa":
                    value = 0.1 * value;
                    break;
                case "kg/cm\u00b2":
                    value = 1.019716 * value;
                    break;
                case "°F":
                    value = (9 / 5) * value + 32;
                    break;
                case "K":
                    value = value + 273.15;
                    break;
                case "mmHg":
                    value = value * 750.061683;
                    break;
            }
            value = value.toFixed(3);
            str = value.toString() + " " + unit;
        }
        $('#ESUNIT_Dryer').text(str);
    }
    this.set_master_value = function (json) {
        if (json.METHODOFFILLING) //top down
        {
            $('#ESU').css('height', json.PERCENTAGE + '%');
            $('#ESD').css('height', (100 - json.PERCENTAGE) + '%');
            $('#ESU').css('background-color', (json.INRANGE ? 'Green' : 'Red'));
            $('#ESD').css('background-color', 'White');
        } else //bottom up
        {
            $('#ESU').css('height', (100 - json.PERCENTAGE) + '%');
            $('#ESD').css('height', json.PERCENTAGE + '%');
            $('#ESU').css('background-color', 'White');
            $('#ESD').css('background-color', (json.INRANGE ? 'Green' : 'Red'));
        }
        var unit = $('#P option:selected').attr('value');
        var value1 = json.LEVEL1 / 1000;
        var value2 = json.LEVEL2 / 1000;
        switch (unit) {
            case "psi":
                value1 = 14.5038 * value1;
                value2 = 14.5038 * value2;
                break;
            case "MPa":
                value1 = 0.1 * value1;
                value2 = 0.1 * value2;
                break;
            case "kg/cm\u00b2":
                value1 = 1.019716 * value1;
                value2 = 1.019716 * value2;
                break;
            case "mmHg":
                value1 = value1 * 750.061683;
                value2 = value2 * 750.061683;
                break;
        }

        $('#ESMAXVALUE').text((value1 > value2 ? value1 : value2).toFixed(3));
        $('#ESMINVALUE').text((value1 < value2 ? value1 : value2).toFixed(3));
        if (json.TYPE) {
            $('#ESMAX_TOT').css('top', '-43px');
            $('#ESMIN_TOT').css('top', '57px');
        } else {
            $('#ESMAX_TOT').css('top', '6px');
            $('#ESMIN_TOT').css('top', '6px');
        }

        var str = "";
        var value = 0;

        if (json.ACT >>> 15) {
            value = -32767;
        }
        var value = value + (json.ACT & 0x7FFF);

        if (value == 32767)
            str = language.get('SENSORERROR', 1);
        else {
            value = value / 1000;
            var unit = $('#P option:selected').attr('value');
            switch (unit) {
                case "psi":
                    value = 14.5038 * value;
                    break;
                case "MPa":
                    value = 0.1 * value;
                    break;
                case "kg/cm\u00b2":
                    value = 1.019716 * value;
                    break;
                case "°F":
                    value = (9 / 5) * value + 32;
                    break;
                case "K":
                    value = value + 273.15;
                    break;
                case "mmHg":
                    value = value * 750.061683;
                    break;
            }
            value = value.toFixed(3);
            str = value.toString() + " " + unit;
        }
        $('#ESUNIT').text(str);
    }
}