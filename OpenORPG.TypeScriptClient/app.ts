/// <reference path="jquery.d.ts" />
/// <reference path="underscore.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts"/>

/// <reference path="phaser.d.ts" />

/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />

/// <reference path="Game/Direction.ts" />

module OpenORPG {

    var app = angular.module('game', [])
        .controller('inventoryController', [
            '$scope', '$rootScope', function ($scope, $rootScope) {
                $scope.gold = 4000;





            }
        ])

        .controller('QuestListController', ['$scope', function($scope) {

            $scope.selectQuest = function(index) {
                $scope.selectedQuest = $scope.playerInfo.quests[index];
                $scope.selectedIndex = index;
            };


        }])

        .controller('CharacterWindowController', [
            '$scope', function ($scope) {

                // We can do some cool stuff here if we want to, but otherwise we're ok 

                $scope.getStatNameFromIndex = index => {

                    var names: string[] = [];
                    for (var n in StatTypes) {
                        if (typeof StatTypes[n] === 'number') names.push(n);
                    }

                    return names[index];
                };

            }
        ]);



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


}


window.onload = () => {




    // Setup underscore   

    var game = new OpenORPG.Game();
};