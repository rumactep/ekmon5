function create_checkbox(type, index, json) {
	if(json.length > 0) {
		$('#checkbox_container').append('<div class="checkbox"><input type="checkbox" name="' + type + '" checked="checked" onclick="toggle_table(\'' + type + '\');" /><span id="' + type + 'text">' + language.get('CHECK', index)  + '</span></div>');	
	} else {
		$('#' + type + 'body').remove();
	}
}