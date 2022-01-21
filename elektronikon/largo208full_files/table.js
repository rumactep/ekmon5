// type, json, columns, element
function create_table() {
	var type = arguments[0];
	var json = arguments[1];
	var columns = arguments[2];
	
	var tableBody = $('#' + type + 'body');
	
	tableBody.html('');
	
	if(json.length > 0) {
		var titles = new Array(columns);
		for(var i = 0; i<columns; i++) {
			titles[i] = arguments[i+3];
		}
		
		// header
		var header = document.createElement('tr');
		header.className = 'table_header';
		
		for(var i = 0; i<columns; i++) {
			var cell = document.createElement('td');
			cell.id=type+"R-1C"+(i+1);
			cell.innerHTML = titles[i];
			header.appendChild(cell);
		}
		
		tableBody.append(header);
		
		switch(type) {
			case "MACHINESTATE":
				create_machine_state_table(type, json, tableBody);
				break;
			case "COUNTERS":
				create_counter_table(type, json, tableBody);
				break;
			case "SPECIALPROTECTIONS":
				create_special_protections_table(type, json, tableBody);
				break;
			case "SERVICEPLAN":
				create_serviceplan_table(type, json, tableBody);
				break;
			default:
				create_regular_table(type, json, tableBody);
				break;
		}
	}
}

function create_regular_table(type, json, tableBody) {
	for(var i in json) {
		var row = create_row(i);
		
		for(var j = 1; j<=3; j++) {
			var cell = create_cell(type, i, j);
			
			switch(j) {
				case 1:
					if(typeof json[i].MPL != 'undefined') {
						cell.innerHTML = language.get("MPL", json[i].MPL);
					} else {
						cell.innerHTML = '---';
					}
					break;
				case 2:
					cell.innerHTML = '---';	
					break;
				case 3:
					cell.className = 'icon';
					cell.innerHTML = '<img src="images/Transparent.gif" />';
					break;
				default:
					cell.innerHTML = '---';
					break;
			}
			
			row.appendChild(cell);
		}
		tableBody.append(row);
	}
	if((type=="CONVERTERS")&&(json[0].CONVERTERDEVICETYPE==1))
	{
		var cell1 = create_cell("FLOW", 1, 1);
		cell1.innerHTML = language.get("TABLETITLE",11);
		var row = create_row(json.length);
		row.appendChild(cell1);
		var cell2 = create_cell("FLOW", 1, 2);
		cell2.style.width = '60%';
		cell2.innerHTML='<table style="table-layout:fixed;padding:0;border-spacing:0;height:20px;border:solid 1px Black" cellpadding="0" cellspacing="0"><tr><td id="FLOWR1C1P0" style="width:50%;background-color:#0099CC;padding:0 0 0 0;margin:0 0 0 0"></td><td id="FLOWR1C1P1" style="width:50%;background-color:#FFFF00;padding:0 0 0 0;margin:0 0 0 0"></td></tr></table>';
		row.appendChild(cell2);
		var cell3 = create_cell("FLOW", 1, 3);
		row.appendChild(cell3);
		tableBody.append(row);
	}
}

function create_machine_state_table(type, json, tableBody) {
	for(var i in json) {
		var row = create_row(i);
		
		for(var j = 1; j<=2; j++) {
			var cell = create_cell(type, i, j);
			
			switch(j) {
				case 1:
					cell.innerHTML = language.get('MACHINESTATUS', 1);
					break;
				case 2:
					cell.innerHTML =language.get('MSTATE', json[0]);
					break;
			}
			
			row.appendChild(cell);
		}
		tableBody.append(row);
	}
}

function create_counter_table(type, json, tableBody) {
	for(var i in json) {
		var row = create_row(i);
		
		for(var j = 1; j<=2; j++) {
			var cell = create_cell(type, i, j);
			
			switch(j) {
				case 1:
					if(typeof json[i].MPL != 'undefined') {
						cell.innerHTML = language.get("MPL", json[i].MPL);
					} else {
						cell.innerHTML = '---';
					}
					break;
				case 2:
					cell.innerHTML = '---';	
					break;
			}
			
			row.appendChild(cell);
		}
		tableBody.append(row);
	}
}

function create_special_protections_table(type, json, tableBody) {
	for(var i in json) {
		var row = create_row(i);
		
		for(var j = 1; j<=2; j++) {
			var cell = create_cell(type, i, j);
			
			switch(j) {
				case 1:
					if(typeof json[i].MPL != 'undefined') {
						cell.innerHTML = language.get("MPL", json[i].MPL);
					} else {
						cell.innerHTML = '---';
					}
					break;
				case 2:
					cell.className = 'icon';
					cell.innerHTML = '<img src="images/Transparent.gif" />';
					break;
				default:
					cell.innerHTML = '---';
					break;
			}
			
			row.appendChild(cell);
		}
		tableBody.append(row);
	}
}

function create_serviceplan_table(type, json, tableBody) {
	for(var i in json) {
		var row = create_row(i);
		
		for(var j = 1; j<=4; j++) {
			var cell = create_cell(type, i, j);
						
			switch(j) {
				case 1:
					cell.style.width = 110;
					if(typeof json[i].STATICVALUE != 'undefined') {
						cell.innerHTML = json[i].STATICVALUE;
					} else {
						cell.innerHTML = '---';
					}
					break;
				case 2:
					cell.style.width = '300px';
					cell.style.padding = '3px';

					if(json[i].getType())
						cell.innerHTML = '<table style="table-layout:fixed;padding:0;border-spacing:0;height:20px;border:solid 1px Black" cellpadding="0" cellspacing="0"><tr><td id="' + type + 'R' + i + 'C2P0" style="background-color:#FFFFFF;padding:0 0 0 0;margin:0 0 0 0"></td><td id="' + type + 'R' + i + 'C2P1" style="background-color:#00FF00;padding:0 0 0 0;margin:0 0 0 0"></td></tr></table>';	
					else
						cell.innerHTML = '<table style="table-layout:fixed;padding:0;border-spacing:0;height:20px;border:solid 1px Black" cellpadding="0" cellspacing="0"><tr><td id="' + type + 'R' + i + 'C2P0" style="background-color:#FFFFFF;padding:0 0 0 0;margin:0 0 0 0"></td><td id="' + type + 'R' + i + 'C2P1" style="background-color:#0000FF;padding:0 0 0 0;margin:0 0 0 0"></td></tr></table>';	
					break;
				case 3:
					cell.style.width = 110;
					cell.innerHTML = '<div id="' + type + 'R' + i + 'C3LEVEL" style="width:90px;padding:5px"></div>';	
					break;
				case 4:
					cell.style.width = 30;
					if(typeof json[i].LEVEL != 'undefined') {
						cell.innerHTML = '<span id="' + type + 'R' + i + 'C2P2" style="padding:3">' + format_level(json[i].LEVEL) + '</span>';
					} else {
						cell.innerHTML = '---';
					}
					break;
			}
			
			row.appendChild(cell);
		}
		tableBody.append(row);
	}
}

function update_table() {
	var type = arguments[0];
	var json = arguments[1];

	// calculate the counter percentages if we are updating a counters table
	if(type == "COUNTERS") {
		var counter_percentages = calculate_counter_percentages(json);
	}
	
	if(json.length > 0 && $('#' + type + 'body').is(':visible')) {
		
	
		switch(type) {
			case "MACHINESTATE":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",5));
				break;
			case "ANALOGINPUTS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",1));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				break;
			case "ANALOGOUTPUTS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",2));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				break;
			case "COUNTERS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",3));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				break;
			case "CONVERTERS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",4));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				if(json[0].CONVERTERDEVICETYPE==1)
				{
					$('#FLOWR1C1').html(language.get("TABLETITLE",11));
					$('#FLOWR1C1P0').css('width',json[0].getFlow()+"%");
					$('#FLOWR1C1P1').css('width',(100-json[0].getFlow())+"%");
					$('#FLOWR1C3').html(json[0].getFlow()+" %");
				}
				break;
			case "DIGITALINPUTS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",6));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				break;
			case "DIGITALOUTPUTS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",7));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				break;
			case "SPECIALPROTECTIONS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",8));
				break;
			case "SERVICEPLAN":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",9));
				$('#' + type + 'R-1C4').html(language.get("TABLEVALUE",2));
				break;
			case "CALCULATEDANALOGINPUTS":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",10));
				$('#' + type + 'R-1C2').html(language.get("TABLEVALUE",1));
				break;
			case "SPM":
				$('#' + type + 'R-1C1').html(language.get("TABLETITLE",12));
				break;
		}

		for(var i in json) {
			var row = json[i];
			
			switch(type) {
				case "MACHINESTATE":
					$('#' + type + 'R' + i + 'C1').html(language.get('MACHINESTATUS', 1));
					$('#' + type + 'R' + i + 'C2').html(language.get('MSTATE', row));
					break;
				case "ANALOGINPUTS":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL)); 
					$('#' + type + 'R' + i + 'C2').html(format_AI_value(row.getValue(), row.INPUTTYPE, row.DISPLAYPRECISION)); 
					$('#' + type + 'R' + i + 'C3 img').attr('src', get_status_icon1(row.getStatus(), 'images/Transparent.gif')); 
					break;
				case "ANALOGOUTPUTS":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL));
					$('#' + type + 'R' + i + 'C2').html(format_AO_value(row.getValue(), row.OUTPUTTYPE, row.DISPLAYPRECISION)); 
					$('#' + type + 'R' + i + 'C3 img').attr('src', 'images/Transparent.gif'); 
					break;
				case "COUNTERS":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL));
					if(counter_percentages.length > 0 && row.MPL in counter_percentages) {
						$('#' + type + 'R' + i + 'C2').html(counter_percentages[row.MPL] + ' %');  
					} else {
						$('#' + type + 'R' + i + 'C2').html(format_CO_value(row.getValue(), row.COUNTERUNIT)); 
					}
					
					$('#' + type + 'R' + i + 'C3 img').attr('src', 'images/Transparent.gif');
					break;
				case "CONVERTERS":
					$('#' + type + 'R' + i + 'C1').html(language.get('CVNAME', row.CONVERTERTYPE)); 
					$('#' + type + 'R' + i + 'C2').html(row.getValue()+" rpm");
					break;
				case "DIGITALINPUTS":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL));
					$('#' + type + 'R' + i + 'C2').html(language.get('MPL', row.MPL, row.getValue()+1)); 
					$('#' + type + 'R' + i + 'C3 img').attr('src', get_status_icon1(row.getStatus(), 'images/Transparent.gif')); 
					break;
				case "DIGITALOUTPUTS":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL));
					$('#' + type + 'R' + i + 'C2').html(language.get('MPL', row.MPL, row.getValue()+1)); 
					$('#' + type + 'R' + i + 'C3 img').attr('src', 'images/Transparent.gif'); 
					break;
				case "SPECIALPROTECTIONS":
					$('#' + type + 'R' + i + 'C1').html(language.get("MPL", row.MPL)); 
					
					try {
						$('#' + type + 'R' + i + 'C2 img').attr('src', get_status_icon1(row.getStatus(), 'images/Ok.gif')); 
					} catch(e) {
						$('#' + type + 'R' + i + 'C2 img').attr('src', 'images/Question.png'); 
					}
					break;
				case "SERVICEPLAN":
					var current_value = Math.ceil(row.STATICVALUE - (row.getValue()/3600));
					
					if(current_value>0)
					{
						var percentage = Math.ceil(100 - (current_value / row.STATICVALUE) * 100);
						
						$('#' + type + 'R' + i + 'C2P0').css('width', percentage + '%');
						$('#' + type + 'R' + i + 'C2P1').css('width', (100-percentage) + '%');
						$('#' + type + 'R' + i + 'C3LEVEL').css('color', 'Black');
						$('#' + type + 'R' + i + 'C3LEVEL').css('background-color',$('#' + type + 'R' + i + 'C3LEVEL').parent().css('background-color'));
						$('#' + type + 'R' + i + 'C3LEVEL').html(current_value);
					}
					else
					{
						$('#' + type + 'R' + i + 'C2P0').css('width', '100%');
						$('#' + type + 'R' + i + 'C2P1').css('width', '0%');
						$('#' + type + 'R' + i + 'C3LEVEL').css('color', 'White');
						$('#' + type + 'R' + i + 'C3LEVEL').css('background-color', 'Red');
						$('#' + type + 'R' + i + 'C3LEVEL').html(-current_value+1);
					}

					if(row.getNext()) {
						if(row.getType())
							$('#' + type + 'R' + i + 'C2P2').css('background-color','#00FF00');
						else
							$('#' + type + 'R' + i + 'C2P2').css('background-color','#0000FF');
						$('#' + type + 'R' + i + 'C2P2').css('color','White');
					}
					else
					{
						$('#' + type + 'R' + i + 'C2P2').css('background-color',$('#' + type + 'R' + i + 'C2P2').parent().css('background-color'));
						$('#' + type + 'R' + i + 'C2P2').css('color','Black');
					}
					break;
				case "CALCULATEDANALOGINPUTS":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL));
					$('#' + type + 'R' + i + 'C2').html(format_AI_value(row.getValue(), row.INPUTTYPE, row.DISPLAYPRECISION)); 
					$('#' + type + 'R' + i + 'C3 img').attr('src', get_status_icon1(row.getStatus(), 'images/Transparent.gif'));
					break;
				case "SPM":
					$('#' + type + 'R' + i + 'C1').html(language.get('MPL', row.MPL)); 
					$('#' + type + 'R' + i + 'C2').html(row.getValue()); 
					break;
			}
		}
	}
}

function toggle_table(type) {
	$('#' + type + 'body').toggle();
}

function create_row(i) {
	var row = document.createElement('tr');
		
	if(i%2==0) {
		row.className = 'row';
	} else {
		row.className = 'row1';
	}
	
	return row;
}

function create_cell(type, row, column) {
	var cell = document.createElement('td');
	cell.id = type + "R" + row + "C" + column;
	
	return cell;
}