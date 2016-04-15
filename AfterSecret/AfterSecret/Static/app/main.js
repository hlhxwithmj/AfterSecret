angular.module("WeChat", ["ngRoute", "WeChat.Controllers"])
       .config(function ($routeProvider) {
           $routeProvider.when("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
           $routeProvider.when("/registerMember", {
               templateUrl: "/static/app/templates/registerMember.html",
               controller: 'registerMemberCtrl'
           });
           $routeProvider.when("/items/:id", {
               templateUrl: "/static/app/templates/items.html",
               controller: 'itemsCtrl'
           });
           $routeProvider.when("/items/", {
               templateUrl: "/static/app/templates/items.html",
               controller: 'itemsCtrl'
           });
           $routeProvider.when("/orders", {
               templateUrl: "/static/app/templates/orders.html",
               controller: 'ordersCtrl'
           });
           $routeProvider.when("/pay", {
               templateUrl: "/static/app/templates/pay.html",
               controller: 'payCtrl'
           });
           $routeProvider.when("/invite", {
               templateUrl: "/static/app/templates/invite.html",
               controller: 'inviteCtrl'
           });
           $routeProvider.when("/invitation/:code/:inviter", {
               templateUrl: "/static/app/templates/invitation.html",
               controller: 'invitationCtrl'
           });
           $routeProvider.when("/ticket", {
               templateUrl: "/static/app/templates/ticket.html",
               controller: 'ticketCtrl'
           });
           $routeProvider.when("/share", {
               templateUrl: "/static/app/templates/share.html",
               controller: 'shareCtrl'
           });
           $routeProvider.otherwise("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
       });
//.config(['$httpProvider', function ($httpProvider) {
//    $httpProvider.interceptors.push(function ($q, $location) {
//        return {
//            'responseError': function (rejection) {
//                var defer = $q.defer();
//                if (rejection.status == 401) {
//                    $location.path("/unauthorized");
//                }
//                defer.reject(rejection);
//                return defer.promise;
//            }
//        };
//    });
//}]);
