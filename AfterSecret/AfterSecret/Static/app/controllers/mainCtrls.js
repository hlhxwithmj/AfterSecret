angular.module("WeChat.Controllers", ["WeChat.Services"])
    .controller("MainCtrl", function ($scope, $http, httpRequestTracker) {
        $scope.hasPendingRequests = function () {
            return httpRequestTracker.hasPendingRequests();
        };
        $http.defaults.headers.common['token'] = window.sessionStorage["token"];
        $http.defaults.headers.common['openId'] = window.sessionStorage["openId"];
        $http.defaults.headers.common['path'] = window.sessionStorage["path"];
        $http.defaults.headers.common['openIdForPay'] = window.sessionStorage["openIdForPay"];
    })
    .controller('registerCtrl', function ($rootScope, $scope, $location, registerService) {
        $('form').validator();
        $scope.check = function () {
            if ($scope.code.toString().length == 12) {
                $scope.disabled = true;
                registerService.doSave($scope.code).success(function () {
                    $rootScope.agentCode = $scope.code;
                    $location.path("/registerMember");
                }).error(function () {
                    $scope.code = '';
                    $scope.disabled = false;
                });
            };
        };
    })
    .controller('termsCtrl', function () {

    })
    .controller('registerMemberCtrl', function ($rootScope, $scope, $location, registerMemberService) {
        $('form').validator();
        $("[name='gender']").bootstrapSwitch();
        $scope.model = {
            agentCode: $rootScope.agentCode
        };
        $scope.save = function () {
            $scope.model.gender = $("[name='gender']").bootstrapSwitch('state') == true ? "Male" : "Female";
            registerMemberService.doSave($scope.model).success(function () {
                $location.path("/items");
            }).error(function () {

            });
        };
    })
    .controller('itemsCtrl', function ($rootScope,$scope, $location, itemsService) {
        $rootScope.bg = "bg-star";
        $scope.model = [];
        itemsService.doGet().success(function (data) {
            angular.forEach(data, function (item, index) {
                $scope.model.push({
                    name: item.Name,
                    remark: item.Remark,
                    factor: item.Factor,
                    unitPrice: item.UnitPrice,
                    remain: item.Remain,
                    order: item.Order,
                    id: item.Id,
                    count: 0
                });
            });
        }).error(function () {

        });
        $scope.$watch(function () {
            var sum = 0;
            angular.forEach($scope.model, function (item, index) {
                sum = sum + item.unitPrice * item.factor * item.count;
            });
            return sum;
        }, function (sum) {
            $scope.total = sum;
        });

        $scope.skip = function () {
            $location.path("/purchaseList");
        };

        $scope.confirm = function () {

        };
    });