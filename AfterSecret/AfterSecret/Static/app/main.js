angular.module("WeChat", ["ngRoute", "WeChat.Controllers"])
       .config(function ($routeProvider) {
           $routeProvider.when("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
           $routeProvider.when("/terms", {
               templateUrl: "/static/app/templates/terms.html",
               controller: 'termsCtrl'
           });
           $routeProvider.when("/member", {
               templateUrl: "/static/app/templates/member.html",
               controller: 'memberCtrl'
           });
           $routeProvider.otherwise("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
       });