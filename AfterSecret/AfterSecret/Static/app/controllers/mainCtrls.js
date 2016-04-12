﻿angular.module("WeChat.Controllers", ["WeChat.Services"])
    .controller("MainCtrl", function ($scope, $location, $http, httpRequestTracker) {
        $scope.hasPendingRequests = function () {
            return httpRequestTracker.hasPendingRequests();
        };
        $http.defaults.headers.common['token'] = window.sessionStorage["token"];
        $http.defaults.headers.common['openId'] = window.sessionStorage["openId"];
        $http.defaults.headers.common['openIdForPay'] = window.sessionStorage["openIdForPay"];
        $location.path('/' + window.sessionStorage["path"]);
    })
    .controller('registerCtrl', function ($rootScope, $scope, $location, registerService) {
        $rootScope.bg = "welcome";
        $scope.error = false;
        $('form').validator();
        $scope.check = function () {
            $scope.isEmpty = $scope.code && $scope.code.toString().length > 0;
            if ($scope.code && $scope.code.toString().length == 12) {
                $scope.disabled = true;
                registerService.doSave($scope.code).success(function () {
                    $rootScope.agentCode = $scope.code;
                    $location.path("/registerMember");
                }).error(function () {
                    $scope.code = '';
                    $scope.isEmpty = false;
                    $scope.disabled = false;
                    $scope.error = true;
                });
            };
        };
    })
    .controller('termsCtrl', function () {

    })
    .controller('registerMemberCtrl', function ($rootScope, $scope, $location, registerMemberService) {
        $rootScope.bg = "bg-star";
        $('form').validator();
        $("[name='gender']").bootstrapSwitch();

        registerMemberService.doGet().success(function (data) {
            $scope.model = data || {};
            $scope.model.agentCode = $rootScope.agentCode;
        }).error(function () { });

        $scope.save = function () {
            $scope.model.gender = $("[name='gender']").bootstrapSwitch('state') == true ? "Male" : "Female";
            registerMemberService.doSave($scope.model).success(function () {
                $location.path("/items");
            }).error(function () {

            });
        };
    })
    .controller('itemsCtrl', function ($rootScope, $scope, $location, $interval, itemsService, orderService, inviteService) {
        $rootScope.bg = "bg-star";
        $scope.model = [];
        $scope.hasPermission = false;
        orderService.doCheck().success(function (data) {
            if (data > 0)
                $location.path('/orders');
        }).error(function () { });

        inviteService.share().success(function () {
            $scope.hasPermission = true;
        })
        .error(function () {
            $scope.hasPermission = false;
        });

        itemsService.doGet().success(function (data) {
            angular.forEach(data, function (item, index) {
                $scope.model.push({
                    name: item.name,
                    remark: item.remark,
                    factor: item.factor,
                    unitPrice: item.unitPrice,
                    remain: item.remain,
                    order: item.order,
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
            $location.path("/orders");
        };

        $scope.confirm = function () {
            orderService.doGet('created').success(function (data) {
                if (data > 0) {
                    alert('您有未完成订单，请先支付完再下单');
                    $location.path('/orders');
                }
            }).error(function () {

            });
            if ($scope.total > 0) {
                $scope.isConfirmed = true;
            }
        };
        $scope.greaterThan = function (prop, val) {
            return function (item) {
                return item[prop] > val;
            }
        }

        $scope.pay = function () {
            $rootScope.order = $scope.model;
            $location.path('/pay');
        };
    })
    .controller('payCtrl', function ($rootScope, $scope, $location, itemsService) {
        if ($rootScope.order) {
            itemsService.doSave($rootScope.order).success(function (charge) {
                pingpp.createPayment(charge, function (result, error) {
                    if (result == "success") {
                        // 只有微信公众账号 wx_pub 支付成功的结果会在这里返回，其他的 wap 支付结果都是在 extra 中对应的 URL 跳转。
                        $rootScope.$apply(function () {
                            $rootScope.order = undefined;
                            $location.path('/orders');
                        });
                    } else if (result == "fail") {
                        // charge 不正确或者微信公众账号支付失败时会在此处返回
                    } else if (result == "cancel") {
                        // 微信公众账号支付取消支付
                    }
                });
            }).error(function (error) {
                $location.path('/orders');
            });
        }
        else
            $location.path('/items');
    })
    .controller('ordersCtrl', function ($rootScope, $scope, $location, orderService) {
        $rootScope.bg = "bg-star";
        $scope.model = [];
        orderService.doGet().success(function (data) {
            $scope.model = data;
        }).error(function () { });

        $scope.invite = function () {
            $location.path('/invite');
        };
    })
    .controller('inviteCtrl', function ($rootScope, $scope, $location, inviteService) {
        $rootScope.bg = "bg-star";
        $scope.model = [];
        $scope.hasPermission = false;
        var getData = function () {
            inviteService.doGet().success(function (data) {
                $scope.model = data;
            }).error(function () { });
            inviteService.share().success(function () {
                $scope.hasPermission = true;
            }).error(function () {
                $scope.hasPermission = false;
            });
        };
        getData();

        $scope.myseat = function (purchaseId) {
            inviteService.myseat(purchaseId).then(function () {
                getData();
            });
        };
        $scope.arrange = function (ticketCode, purchaseId) {
            window.location.href = $location.protocol() + '://' + $location.host() + ':' + $location.port()
                + '/Static/invitation.html?ticketCode=' + ticketCode + '&purchaseId=' + purchaseId;
        };

        $scope.cancel = function (registerMemberId) {
            inviteService.cancel(registerMemberId).then(function () {
                getData();
            });
        };
    })
    .controller('ticketCtrl', function ($scope, ticketService) {
        ticketService.doGet().success(function (data) {
            $scope.model = data;
        }).error(function () { });
    });