angular.module("WeChat.Controllers", ["WeChat.Services"])
    .controller("MainCtrl", function ($scope, $http, httpRequestTracker) {
        $scope.hasPendingRequests = function () {
            return httpRequestTracker.hasPendingRequests();
        };
    })
    .controller('registerCtrl', function ($scope, $location, registerService) {
        $('form').validator();
        $scope.check = function () {
            registerService.doSave($scope.code).success(function () {
                $location.path("/member");
            }).error(function () {
                $scope.code = '';
            });
        };
    })
    .controller('termsCtrl', function () {

    })
    .controller('memberCtrl', function () {

    });