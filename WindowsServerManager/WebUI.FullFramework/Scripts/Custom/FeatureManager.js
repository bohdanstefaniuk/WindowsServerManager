function FeatureManager() {
    var changedFeatures = [];
    var changedFeaturesElements = [];

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
                        var parentTr = elem.parents("tr");
                        changedFeaturesElements.push(parentTr);
                        parentTr.css("background-color", "#ddd");
                    }

                    debug();
                });
        });
    };

    var resetStyle = function() {
        changedFeaturesElements.forEach(function (item, i, arr) {
            item.css("background-color", "white");
        });
    };

    var saveFeatures = function (urlData, saveButton) {
        var jsonData = JSON.stringify({
            "features": changedFeatures,
            "db": urlData.db,
            "redisDb": urlData.redisDb
        });

        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: urlData.url,
            type: "POST",
            data: jsonData,
            success: function(response) {
                if (response != null && response.success) {
                    alert(response.responseText);
                } else {
                    alert(response.responseText);
                } 
            },
            error: function(response) {
                alert(response.responseText);
            },
            complete: function() {
                saveButton.prop("disabled", false);
                changedFeatures = [];
                resetStyle();
            }
        });
    }

    return {
        initializeChangeEvents: initializeChangeEvents,
        debug: debug,
        saveFeatures: saveFeatures
    }
}