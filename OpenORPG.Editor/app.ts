/// <reference path="Scripts/angular.d.ts" />
/// <reference path="Scripts/angular-route.d.ts" />

// script.js

// create the module and name it scotchApp
// also include ngRoute for all our routing needs
var editorApp = angular.module('editorApp', ['ngRoute', 'ngAnimate']);

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

    // route for the contact page
        .when('/contact', {
            templateUrl: 'pages/contact.html',
            controller: 'contactController'
        })
        .when('/items', {
            templateUrl: 'views/templates/items.html',
            controller: 'Items.ItemIndexController'
        })

        .when('/items/:itemId',
        {
            templateUrl: 'views/templates/items_details.html',
            controller: 'Items.ItemController'
        });


});

// create the controller and inject Angular's $scope
editorApp.controller('mainController', function ($scope) {
    // create a message to display in our view
    $scope.message = '(There is not anything interesting to look at right now.)';
});

editorApp.controller('aboutController', function ($scope) {
    $scope.message = 'Look! I am an about page.';
});

editorApp.controller('contactController', function ($scope) {
    $scope.message = 'Contact us! JK. This is just a demo.';
});