function IISPageComponentLoader() {
    // Объект, который соделжит отношения между названием параметра и методом на стороне сервера
    var components = {
        "InformationComponent": "GetFeaturesComponent",
        "ConnectionStringsComponent": "GetConnectionStringsComponent",
        "ConfigFileComponent": "GetConfigurationFileComponent",
        "FeaturesComponent": "GetFeaturesComponent"
    }

    var componentContainerId = "#IISPageComponentContainer";

    var loadComponent = function (componentName) {
        var methodName = components[componentName];

        $(componentContainerId).load("/IIS/" + methodName, { }, function () {});
    }

    return {
        LoadComponent: loadComponent
    }
}