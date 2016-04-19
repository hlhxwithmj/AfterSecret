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
                method: 'get',
                url: '/api/OrderDelete',
                params: {
                    id: id
                }
            });
        };
        _service.doUpdate = function (id) {
            return $http({
                method: 'post',
                url: '/api/OrderPaid',
                data: {
                    id: id
                }
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
    }])
    .factory("invitationService", ['$http', '$location', function ($http, $location) {
        var _service = {};
        _service.wxConfig = function () {
            $http({
                method: 'get',
                url: '/api/WxConfig',
                params: {
                    url: $location.absUrl().substring(0, $location.absUrl().indexOf('#'))
                }
            }).success(function (data) {
                wx.config({
                    //debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: data.appId, // 必填，公众号的唯一标识
                    timestamp: data.timestamp, // 必填，生成签名的时间戳
                    nonceStr: data.nonceStr, // 必填，生成签名的随机串
                    signature: data.signature,// 必填，签名，见附录1
                    jsApiList: ['onMenuShareAppMessage', 'hideMenuItems'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });
            });
        };
        return _service;
    }])
    .factory("shareService", ['$http', function ($http) {
        var _service = {};
        _service.doPost = function () {
            return $http({
                method: 'post',
                url: '/api/Share'
            });
        };
        return _service;
    }])
    .factory("payService", ['$http', function ($http) {
        var _service = {};
        _service.doProcessing = function (chargeId) {
            return $http({
                method: 'get',
                url: '/api/OrderProcessing',
                params: {
                    chargeId: chargeId
                }
            })
        };
        return _service;
    }]);