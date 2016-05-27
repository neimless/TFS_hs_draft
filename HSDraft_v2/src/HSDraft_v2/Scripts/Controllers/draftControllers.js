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
