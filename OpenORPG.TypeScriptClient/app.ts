/// <reference path="jquery.d.ts" />
/// <reference path="underscore.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts"/>

/// <reference path="phaser.d.ts" />

/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />

/// <reference path="Game/Direction.ts" />


var app = angular.module('game', [])
    .controller('inventoryController', ['$scope', function ($scope) {
        $scope.gold = 4000;


    }])
    .controller('MyCtrl2', ['$scope', function ($scope) {

    }]);


app.directive('openDialog', function () {
    return {
        restrict: 'A',
        link: function (scope, elem, attr, ctrl) {
            var dialogId = '#' + attr.openDialog;
            elem.bind('click', function (e) {
                $(dialogId).dialog('open');
            });
        }
    };
});

window.onload = () => {




    // Setup underscore   

    var game = new OpenORPG.Game();
};