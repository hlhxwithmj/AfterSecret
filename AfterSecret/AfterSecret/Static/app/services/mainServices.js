angular.module("WeChat.Services", [])
    .factory('httpRequestTracker', ['$http', function ($http) {
        var httpRequestTracker = {};
        httpRequestTracker.hasPendingRequests = function () {
            return $http.pendingRequests.length > 0;
        };
        return httpRequestTracker;
    }])
    .factory("registerService", ['$http', function ($http) {
        var _service = {};
        _service.doSave = function (code) {
            return $http({
                method: 'post',
                url: '/api/Register',
                data: {
                    code: code
                }
            });
        };
        return _service;
    }]);