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
        _service.doGet = function () {
            return $http({
                method: 'get',
                url: '/api/RegisterMember'
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
        _service.doSave = function (items) {
            return $http({
                method: 'post',
                url: '/api/Order',
                data: JSON.stringify(items),
            });
        };
        return _service;
    }])
    .factory("orderService", ['$http', function ($http) {
        var _service = {};
        _service.doGet = function () {
            return $http({
                method: 'get',
                url: '/api/Order'
            });
        };
        _service.doCheck = function (status) {
            return $http({
                method: 'get',
                url: '/api/Order',
                params: {
                    status: status
                }
            });
        };
        return _service;
    }]);