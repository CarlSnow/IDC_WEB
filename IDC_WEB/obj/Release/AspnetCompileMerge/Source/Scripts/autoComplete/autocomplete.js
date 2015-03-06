$(document).ready(function () {
    $("#search_input").focus(InitializeWizardInfo);
    InitializeAutoCompleteSelect();
});

function InitializeWizardInfo() {
    $("#search_input").autocomplete(
        {
            source: function (request, response) {
                $.ajax({
                    url: "/Home/InitializeWizardInfo",
                    data: "searchKey=" + $("#search_input").val(),
                    success: function (data) {
                        var obj = eval("(" + data + ")");
                        response($.map(obj, function (item, index) {
                            return "[" + item.prod_code + "]" + item.prod_name;
                        }))
                    }
                })
            },
            minChars: 1,
            width: 260,
            dataType: "json",
            matchContains: true,
        })
}


function InitializeAutoCompleteSelect() {
    $("#search_input").on("autocompleteselect", function (event, ui) {
        var selectedItem = ui.item.value;
        var index = selectedItem.indexOf(']');
        var searchKey = selectedItem.substring(1, index);
        Search(searchKey);
    });
}