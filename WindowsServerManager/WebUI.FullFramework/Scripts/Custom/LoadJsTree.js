$(function () { $("#jstree").jstree(); });
$("#jstree").jstree({
    "core": {
        "data": [
            "Simple root node",
            {
                "text": "Root node 2",
                "state": {
                    "opened": true,
                    "selected": true
                },
                "children": [
                    { "text": "Child 1" },
                    "Child 2"
                ]
            }
        ]
    }
});
$("button").on("click", function () {
    $("#jstree").jstree(true).select_node("child_node_1");
    $("#jstree").jstree("select_node", "child_node_1");
    $.jstree.reference("#jstree").select_node("child_node_1");
});