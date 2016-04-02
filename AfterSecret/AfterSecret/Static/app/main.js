angular.module("WeChat", ["ngRoute", "WeChat.Controllers"])
       .config(function ($routeProvider) {
           $routeProvider.when("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
           $routeProvider.otherwise("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
       });