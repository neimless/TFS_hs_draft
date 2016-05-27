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
(function () {
    'use strict';

    angular
        .module('draftApp')
        .controller('CardsController', CardsController);
    
    CardsController.$inject = ['$scope', 'Drafts'];
    function CardsController($scope, Drafts) {
        $scope.cards = Drafts.query();
    }
})();

(function () {
    'use strict';

    var draftServices = angular.module('draftServices', ['ngResource']);

    draftServices.factory('Drafts', ['$resource',
        function ($resource) {
            return $resource('api/card', {}, {
                query: { method: 'GET', params: {}, isArray: true }
            });
        }]);
})();