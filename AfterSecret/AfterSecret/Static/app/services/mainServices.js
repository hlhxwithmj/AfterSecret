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
        _service.doGetDetail = function (id) {
            return $http({
                method: 'get',
                url: '/api/OrderDetail',
                params: {
                    id: id
                }
            });
        };
        _service.doSave = function (items) {
            return $http({
                method: 'post',
                url: '/api/Order',
                data: JSON.stringify(items),
            });
        };
        _service.doDelete = function (id) {
            return $http({
                method: 'delete',
                url: '/api/Order',
                param: { id: id }
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
    }])
    .factory("inviteService", ['$http', function ($http) {
        var _service = {};
        _service.doGet = function () {
            return $http({
                method: 'get',
                url: '/api/Invite'
            });
        };
        _service.myseat = function (purchaseId) {
            return $http({
                method: 'post',
                url: '/api/Invite',
                data: purchaseId
            });
        };
        _service.cancel = function (registerMemberId) {
            return $http({
                method: 'get',
                url: '/api/Invite',
                params: { registerMemberId: registerMemberId }
            });
        };
        _service.share = function () {
            return $http({
                method: 'get',
                url: '/api/Share'
            });
        };
        return _service;
    }])
    .factory("ticketService", ['$http', function ($http) {
        var _service = {};
        _service.doGet = function () {
            return $http({
                method: 'get',
                url: '/api/Ticket'
            });
        };
        return _service;
    }]);