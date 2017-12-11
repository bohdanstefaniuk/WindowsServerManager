function FeatureManager() {
    var changedFeatures = [];

    var debug = function () {
        console.log(changedFeatures);
    };

    var _searchInArray = function(item) {
        var found = false;
        for (var i = 0; i < changedFeatures.length; i++) {
            if (changedFeatures[i].id === item.id) {
                found = true;
                break;
            }
        }
        return found;
    };

    var _changeItem = function(item) {
        for (var i = 0; i < changedFeatures.length; i++) {
            if (changedFeatures[i].featureId === item.featureId) {
                changedFeatures[i] = item;
                break;
            }
        }
    }

    var initializeChangeEvents = function() {
        $(".featureControl").each(function() {
            var elem = $(this);

            elem.data("oldVal", elem.val());

            elem.bind("change",
                function(event) {
                    if (elem.data("oldVal") !== elem.val()) {
                        var featureId = elem.data("feature-id");
                        var featureNewValue = elem.val();

                        var feature = {
                            id: featureId,
                            code: "",
                            state: featureNewValue
                        };

                        if (_searchInArray(feature)) {
                            _changeItem(feature);
                        } else {
                            changedFeatures.push(feature);
                        }
                        elem.data("oldVal", elem.val());
                    }

                    debug();
                });
        });
    };

    var saveFeatures = function (url) {
        var jsonData = JSON.stringify({ "features": changedFeatures });

        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: url,
            type: "POST",
            data: jsonData,
            success: function() {
                alert("Сохранение успошно");
            }
        });
    }

    return {
        initializeChangeEvents: initializeChangeEvents,
        debug: debug,
        saveFeatures: saveFeatures
    }
}