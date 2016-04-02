angular.module("WeChat.Services", [])
    .factory("registerService", ['$http', function ($http) {
        var _service = {};
        _service.doSave = function (code) {
            return $http({
                method: 'post',
                url: '/api/Register',
                data: {
                    code:code
                }
            });
        };
    }]);