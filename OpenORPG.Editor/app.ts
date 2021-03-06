﻿/// <reference path="Scripts/angular.d.ts" />
/// <reference path="Scripts/angular-route.d.ts" />

// script.js

// create the module and name it scotchApp
// also include ngRoute for all our routing needs
var editorApp = angular.module('editorApp', ['ngRoute', 'ngAnimate', 'mgcrea.ngStrap']);

// configure our routes
editorApp.config(function ($routeProvider) {
    $routeProvider

    // route for the dashboard page
        .when('/', {
            templateUrl: 'views/templates/dashboard.html',
            controller: 'mainController'
        })

    // route for the about page
        .when('/about', {
            templateUrl: 'views/templates/about.html',
            controller: 'aboutController'
        })

        .when('/skills', {
            templateUrl: 'views/templates/skills/skills.html',
            controller: 'Controllers.SkillIndexController'
        })

        .when('/skills/:id',
        {
            templateUrl: 'views/templates/skills/skill_details.html',
            controller: 'Controllers.SkillController'
        })

        .when('/items', {
            templateUrl: 'views/templates/items/items.html',
            controller: 'Items.ItemIndexController'
        })

        .when('/items/:itemId',
        {
            templateUrl: 'views/templates/items/items_details.html',
            controller: 'Items.ItemController'
        });


});


// create the controller and inject Angular's $scope
editorApp.controller('mainController', function ($scope) {
    // create a message to display in our view
    $scope.message = '(There is not anything interesting to look at right now.)';
});

editorApp.controller('headerController', function ($scope, $location) {

    $scope.isActive = function (viewLocation) {

        if (viewLocation == "/")
            if ($location.path() == viewLocation)
                return true;
            else
                return false;

        return $location.path().indexOf(viewLocation) == 0;

    };

});

editorApp.controller('aboutController', function ($scope) {
    $scope.message = 'Look! I am an about page.';
});

editorApp.controller('contactController', function ($scope) {
    $scope.message = 'Contact us! JK. This is just a demo.';
});