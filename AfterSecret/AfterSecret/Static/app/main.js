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
           $routeProvider.when("/registerMember", {
               templateUrl: "/static/app/templates/registerMember.html",
               controller: 'registerMemberCtrl'
           });
           $routeProvider.when("/items", {
               templateUrl: "/static/app/templates/items.html",
               controller: 'itemsCtrl'
           });
           $routeProvider.otherwise("/register", {
               templateUrl: "/static/app/templates/register.html",
               controller: 'registerCtrl'
           });
       });