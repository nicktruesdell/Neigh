var app = angular.module("app", ['ngResource']);

app.directive('ngAttr', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, controller) {
            var obj = scope.$eval(attrs.ngAttr);
            Object.keys(obj).forEach(function (key) {
                scope.$watch(obj[key], function (newValue) {
                    //console.log("Key:" + key, + " Value:" + newValue);
                    element.attr(key.toString(), newValue);
                });
            });
        }
    };
});