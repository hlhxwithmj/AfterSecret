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
                method: 'get',
                url: '/api/Register',
                params: {
                    code: code
                }
            });
        };
        return _service;
    }])
    .factory("registerMemberService", ['$http', function ($http) {
        var _service = {};
        _service.doSave = function (model) {
            return $http({
                method: 'post',
                url: '/api/RegisterMember',
                data: model
            });
        };
        return _service;
    }])
    .factory("itemsService", ['$http', function ($http) {
        var _service = {};
        _service.doGet = function () {
            return $http({
                method: 'get',
                url: '/api/Items'
            });
        };
        return _service;
    }]);