angular.module("WeChat.Controllers", ["WeChat.Services", "constantService"])
    .controller("MainCtrl", function ($scope, $location, $http, httpRequestTracker) {
        $scope.hasPendingRequests = function () {
            return httpRequestTracker.hasPendingRequests();
        };
        $http.defaults.headers.common['token'] = window.sessionStorage["token"];
        $http.defaults.headers.common['openId'] = window.sessionStorage["openId"];
        $http.defaults.headers.common['openIdForPay'] = window.sessionStorage["openIdForPay"];
        $location.path('/' + window.sessionStorage["path"]);
    })
    .controller('invitationCtrl', function ($scope, $rootScope, $routeParams, $location, registerMemberService, invitationService) {
        $rootScope.bg = "bg-img";
        $scope.ticketCode = $routeParams.code;
        $scope.inviter = $routeParams.inviter;
        registerMemberService.doGet().success(function () {

        }).error(function () {
            $location.path('/register');
        });
        invitationService.wxConfig();
        wx.ready(function () {
            wx.hideMenuItems({
                menuList: ['menuItem:share:timeline'] // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
            });
            if ($routeParams.code && $routeParams.code.substring(0,2) != '18')
                wx.onMenuShareAppMessage({
                    title: 'Invitation', // 分享标题
                    desc: 'You receive a ticket from your friend. Register and get your ticket to join the Secret After Party!', // 分享描述
                    link: $location.protocol() + '://' + $location.host() + ':' + $location.port()
                        + '/Static/invitation.html?ticketCode=' + $routeParams.code + '&inviter=' + $routeParams.inviter, // 分享链接
                    imgUrl: $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/static/image/invitation.png', // 分享图标
                    type: 'link', // 分享类型,music、video或link，不填默认为link
                    dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                    success: function () {
                        // 用户确认分享后执行的回调函数
                        $location.path('/invite');
                    },
                    cancel: function () {
                        // 用户取消分享后执行的回调函数
                    }
                });
            else
                wx.onMenuShareAppMessage({
                    title: 'The Secret After Party', // 分享标题
                    desc: 'Enter your agent code, register and come join me!', // 分享描述
                    link: $location.protocol() + '://' + $location.host() + ':' + $location.port()
                        + '/Static/invitation.html?ticketCode=' + $routeParams.code + '&inviter=' + $routeParams.inviter, // 分享链接
                    imgUrl: $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/static/image/share-sm.png', // 分享图标
                    type: 'link', // 分享类型,music、video或link，不填默认为link
                    dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                    success: function () {
                        // 用户确认分享后执行的回调函数
                        $location.path('/invite');
                    },
                    cancel: function () {
                        // 用户取消分享后执行的回调函数
                    }
                });
        });

        $scope.invite = function () {
            $scope.isShare = true;
        };

        $scope.back = function () {
            $location.path('/invite');
        };
    })
    .controller('registerCtrl', function ($rootScope, $scope, $location, registerService) {
        $rootScope.bg = "welcome";
        $scope.error = false;
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
    .controller('registerMemberCtrl', function ($rootScope, $scope, $location, registerMemberService, constantService) {
        $rootScope.bg = "bg-img";
        $scope.success = false;
        $scope.fail = false;
        $scope.showTerms = false;
        $scope.agreement = true;
        $scope.model = {};

        $scope.nationalityList = constantService.nationality;
        $scope.occupationList = constantService.occupation;
        registerMemberService.doGet().success(function (data) {
            $scope.model = data || {};
        }).error(function () { });

        $scope.save = function () {
            $scope.model.AgentCode = $rootScope.agentCode;
            $scope.model.gender = $("[name='gender']").bootstrapSwitch('state') == true ? "Male" : "Female";
            registerMemberService.doSave($scope.model).success(function (data) {
                if (data == 'success') {
                    $scope.success = true;
                }
                else
                    $location.path("/items");
            }).error(function (data) {
                if (data && data.Message == 'fail') {
                    $scope.fail = true;
                }
                else if (data && data.Message == 'ticket') {
                    $scope.alreadyHas = true;
                    $location.path("/register");
                }
            });
        };

        $scope.terms = function () {
            $scope.showTerms = true;
        }

        $scope.ticket = function () {
            $location.path('/ticket');
        }
        $scope.purchase = function () {
            $location.path('/items');
        }
    })
    .controller('itemsCtrl', function ($rootScope, $scope, $location, $routeParams, registerMemberService, itemsService, orderService, inviteGuestService) {
        $rootScope.bg = "bg-img";
        $scope.model = [];
        $scope.hasPermission = false;

        registerMemberService.doGet().success(function () {

        }).error(function () {
            $location.path('/register');
        });

        orderService.doCheck().success(function (data) {
            if (data > 0)
                $location.path('/orders');
        }).error(function () { });

        inviteGuestService.share().success(function () {
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
                    seats: item.seats,
                    unitPrice: item.unitPrice / 100,
                    remain: item.remain,
                    order: item.order,
                    imgSrc: item.imgSrc,
                    total: item.total,
                    id: item.Id,
                    count: undefined
                });
            });
            if ($routeParams.id) {
                var id = parseInt($routeParams.id);
                itemsService.doGetDetail(id).success(function (result) {
                    angular.forEach(result, function (r, i) {
                        angular.forEach($scope.model, function (m, j) {
                            if (m.id == r.id)
                                m.count = r.count;
                        });
                    });
                }).error(function () { });
            }
        }).error(function () {

        });

        $scope.$watch(function () {
            var sum = 0;
            angular.forEach($scope.model, function (item, index) {
                sum = sum + item.unitPrice * (item.count ? item.count : 0);
            });
            return sum;
        }, function (sum) {
            $scope.total = sum.toFixed(0);
        });

        $scope.skip = function () {
            $location.path("/orders");
        };

        $scope.confirm = function () {
            if ($scope.total == 0) {
                return;
            }
            if ($routeParams.id)
                itemsService.doDelete($routeParams.id).success(function () {
                    orderService.doCheck('unpaid').success(function (data) {
                        if (data > 0) {
                            alert('您有未完成订单，请先支付完再下单');
                            $location.path('/orders');
                        }
                    }).error(function () {

                    });
                }).error(function (data) {
                    if (data && data.Message == 'paid')
                        alert("订单已支付，不能修改");
                });
            else {
                orderService.doCheck('unpaid').success(function (data) {
                    if (data > 0) {
                        alert('您有未完成订单，请先支付完再下单');
                        $location.path('/orders');
                    }
                }).error(function () {

                });
            }
            if ($scope.total > 0) {
                $scope.isConfirmed = true;
                $scope.style = { 'border': 'none' };
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
    .controller('payCtrl', function ($rootScope, $scope, $location, itemsService, payService) {
        if ($rootScope.order) {
            itemsService.doSave($rootScope.order).success(function (charge) {
                pingpp.createPayment(charge, function (result, error) {
                    if (result == "success") {
                        // 只有微信公众账号 wx_pub 支付成功的结果会在这里返回，其他的 wap 支付结果都是在 extra 中对应的 URL 跳转。
                        payService.doProcessing(charge.id).then();
                    } else if (result == "fail") {
                        // charge 不正确或者微信公众账号支付失败时会在此处返回
                    } else if (result == "cancel") {
                        // 微信公众账号支付取消支付
                    }
                    $rootScope.$apply(function () {
                        $rootScope.order = undefined;
                        $location.path('/orders');
                    });
                });
            }).error(function (error) {
                if (error.Message == 'needtopay') {
                    alert('您有未完成订单，请先支付完再下单');
                }
                $location.path('/orders');
            });
        }
        else
            $location.path('/items');
    })
    .controller('ordersCtrl', function ($rootScope, $scope, $location, $interval, registerMemberService, orderService, inviteGuestService,shareService) {
        $rootScope.bg = "bg-img";
        $scope.hasPermission = false;

        inviteGuestService.share().success(function () {
            $scope.hasPermission = true;
        })
        .error(function () {
            $scope.hasPermission = false;
        });
        registerMemberService.doGet().success(function () {

        }).error(function () {
            $location.path('/register');
        });
        $scope.model = [];

        orderService.doGet().success(function (data) {
            $scope.model = data;
            angular.forEach($scope.model, function (item, index) {
                if (item.orderStatus == 10) {
                    $scope.expire = item.expireTime;
                    var a = $interval(function () {
                        var d = moment.utc(moment($scope.expire).diff(moment())).format("mm:ss")
                        if (d == "00:00")
                            $interval.cancel(a);
                        $scope.countdown = d;
                    }, 1000);
                }
            });
        }).error(function () { });

        $scope.invite = function () {
            $location.path('/invite');
        };
        $scope.purchase = function () {
            $location.path('/items');
        };

        $scope.edit = function (id) {
            $location.path('/items/' + id);
        };

        $scope.toggle = function (e) {
            $(e.target).parent().find('i.indicator').toggleClass('glyphicon-menu-down glyphicon-menu-up');
        };

        $scope.share = function () {
            shareService.doPost().success(function (data) {
                $location.path('/invitation/' + data.ticketCode + '/' + data.inviter);
            }).error(function () {

            });
        }
    })
    .controller('inviteCtrl', function ($rootScope, $scope, $location, registerMemberService, inviteService, ticketService) {
        $rootScope.bg = "bg-img";
        $scope.model = [];
        $scope.hasPermission = false;

        registerMemberService.doGet().success(function () {

        }).error(function () {
            $location.path('/register');
        });
        var getData = function () {
            inviteService.doGet().success(function (data) {
                $scope.model = data;
            }).error(function () { });
        };
        getData();

        $scope.invite = function (invitationType) {
            $location.path('/inviteGuest/' + invitationType);
        }
    })
    .controller('inviteGuestCtrl', function ($rootScope, $routeParams, $scope, $location, inviteGuestService, ticketService) {
        $rootScope.bg = "bg-img";
        $scope.invitationType = $routeParams.invitationType;
        $scope.title = $scope.invitationType == 10 ? 'Tickets' : 'Tables';
        $scope.hasMyTicket = false;
        $scope.takeMySeat = false;
        $scope.delete = false;
        $scope.inviteeId = 0;

        $scope.myseat = function () {
            inviteGuestService.myseat($scope.invitationType).success(function () {
                $scope.takeMySeat = false;
                getData();
            }).error(function () {
                getData();
            });
        };
        $scope.arrange = function () {
            if (!$scope.hasMyTicket) {
                $scope.takeMySeat = true;
                $scope.confirmMsg = $scope.invitationType == 10 ? 'Do you want to own this entrance ticket?' : 'Do you want to take a seat at this table?';
            }
            else
                $location.path('/invitation/' + $scope.model.invitationCode + '/' + $scope.model.inviter);
        };

        $scope.no = function () {
            $location.path('/invitation/' + $scope.model.invitationCode + '/' + $scope.model.inviter);
        }

        $scope.deleteConfirm = function (inviteeId, name) {
            $scope.deleteConfirmation = 'Are you sure you want to remove "' + name + '" from your guest lists?';
            $scope.delete = true;
            $scope.inviteeId = inviteeId;
        };

        $scope.cancel = function () {
            inviteGuestService.cancel($scope.inviteeId).then(function () {
                getData();
                if ($scope.inviteeId == $scope.model.inviterId)
                    $scope.hasMyTicket = false;
                $scope.deleteConfirmation = '';
                $scope.delete = false;
                $scope.inviteeId = 0;
            });
        };
        $scope.share = function () {
            shareService.doPost().success(function (data) {
                $scope.model = data;
                $location.path('/invitation/' + data.ticketCode + '/' + data.inviter);
            }).error(function () {

            });
        }

        var getData = function () {
            inviteGuestService.doGet($scope.invitationType).success(function (data) {
                $scope.model = data;
                $scope.model.left = data.total - data.invitees.length;
                angular.forEach($scope.model.invitees, function (item, index) {
                    if ($scope.model.inviterId == item.inviteeId)
                        $scope.model.invitees[index].src = $scope.invitationType == 10 ? '../static/image/ticketseat-gold.png' : '../static/image/seat-gold.png';
                    else
                        $scope.model.invitees[index].src = $scope.invitationType == 10 ? '../static/image/ticketseat-white.png' : '../static/image/seat-white.png';
                });
            }).error(function () { });
            ticketService.doGet().success(function () {
                $scope.hasMyTicket = true;
            }).error(function () {
                $scope.hasMyTicket = false;
            });
        };
        getData();
    })
    .controller('ticketCtrl', function ($scope, $rootScope, $location, registerMemberService, ticketService) {
        $rootScope.bg = "bg-img";
        registerMemberService.doGet().success(function () {

        }).error(function () {
            $location.path('/register');
        });
        leftPadding = function (str) {
            var pad = "000000";
            var ans = pad.substring(0, pad.length - str.length) + str;
            return ans;
        };
        ticketService.doGet().success(function (data) {
            $scope.model = data;
            $scope.model.ticketId = leftPadding($scope.model.ticketId + '');
        }).error(function (data) {
            if (data && data.Message == 'invite')
                $location.path('/invite');
        });
    });