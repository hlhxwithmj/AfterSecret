angular.module("WeChat.Controllers", ["WeChat.Services"])
    .controller("MainCtrl", function ($scope, $http) {

    })
    .controller('registerCtrl', function ($scope) {
        $('form').validator()
    });