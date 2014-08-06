/// <reference path="jquery.d.ts" />
/// <reference path="underscore.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts"/>

/// <reference path="phaser.d.ts" />

/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />

/// <reference path="Game/Direction.ts" />


var app = angular.module('game', [])
    .controller('inventoryController', [
        '$scope', '$rootScope', function ($scope, $rootScope) {
            $scope.gold = 4000;

            $scope.die = () => {
                debugger;
            };

        }
    ])

    .controller('HudNetworkController', ['$scope', '$rootScope', function ($scope, $rootScope) {


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