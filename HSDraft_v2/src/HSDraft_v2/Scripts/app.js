(function () {
    'use strict';

    config.$inject = ['$routeProvider', '$locationProvider'];
    
    angular.module('draftApp', [
        'ngRoute', 'draftServices'
    ]).config(config);

    function config($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: '/Views/lobby.html'
            })
            .when('/rules', {
                templateUrl: '/Views/rules.html'
            })
            .when('/cards', {
                templateUrl: '/Views/cards.html',
                controller: 'CardsController'
            });

        $locationProvider.html5Mode(true);
    }
})();