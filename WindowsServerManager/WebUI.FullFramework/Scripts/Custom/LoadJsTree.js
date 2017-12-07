// Initialize Menu
function Refresh() {
    $('#jstree').jstree({
        'core': {
            'data': {
                'url': "/IIS/GetIISMenuModel",
                'data': function(node) {
                    return { 'id': node.id };
                }
            }
        }
        //"json_data": {
        //    "ajax": {
        //        "url": "/IISController/GetIISMenuModel",
        //        "type": "POST",
        //        "dataType": "json",
        //        "contentType": "application/json charset=utf-8"
        //    }
        //},
        ////"themes": {
        ////    "theme": "default",
        ////    "dots": false,
        ////    "icons": true,
        ////    "url": "/content/themes/default/style.css"
        ////},

        ////"plugins": ["themes", "json_data", "dnd", "contextmenu", "ui", "crrm"]
        //"plugins": ["json_data"]

    });
}
