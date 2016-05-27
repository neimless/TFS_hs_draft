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