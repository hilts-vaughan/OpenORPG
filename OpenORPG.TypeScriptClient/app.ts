var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};

/// <reference path="jquery.d.ts" />
/// <reference path="underscore.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts"/>

/// <reference path="phaser.d.ts" />

/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />

/// <reference path="Game/Direction.ts" />

/// <reference path="OpenORPG/Interface.ts" />

module OpenORPG {
    Interface.game.angular.directive('openDialog', function () {
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

    Interface.game.angular.filter("skillFormatter", () => {
        return (input: number) => {
            return Math.max(Math.ceil(input), 0) + "s";
        };
    });

    Interface.game.angular.controller('inventoryController', [
        '$scope', '$rootScope', function ($scope, $rootScope) {
            $scope.gold = 4000;
        }
    ]);

    Interface.game.angular.controller('SettingsController', [
        '$scope', '$rootScope', function ($scope, $rootScope) {
            $scope.settings = $.extend({}, Settings.getInstance());

            $scope.save = function () {
                $.extend(Settings.getInstance(), $scope.settings);
                Settings.getInstance().flush();
                Settings.getInstance().save();
            }

            $scope.discard = function () {
                $scope.settings = $.extend({}, Settings.getInstance());
            }

            $scope.reset = function () {
                Settings.getInstance().reset();
            }
        }
    ]);

    Interface.game.angular.controller('QuestListController', [
        '$scope', function ($scope) {
            var formatter: RequirementFormatter = new RequirementFormatter();

            $scope.$on('QuestsChanged',(event, data) => {
                $scope.selectQuest($scope.selectedIndex);
            });

            $scope.selectQuest = function (index) {
                $scope.selectedQuest = $scope.playerInfo.quests[index];
                $scope.selectedIndex = index;

                //TODO: We need to make this take all requirements into account; not just the first
                if ($scope.selectedQuest.state == 1) {
                    $scope.localizeRequirements($scope.selectedQuest.currentStep.requirements);
                } else {
                    $scope.currentTask = LocaleManager.getInstance().getString("QuestCompletedFormatter", []);
                }
            };

            $scope.localizeRequirements = function (requirements) {
                $scope.currentTask = " ";

                _.each(requirements,(requirement: any, index: number) => {
                    formatter.getFormattedRequirement(requirement.type, requirement.info, $scope.selectedQuest.questInfo.requirementProgress[index].progress,(result) => {
                        $scope.currentTask += result;
                    });
                });
            };
        }
    ]);

    Interface.game.angular.controller('SkillListController', [
        '$scope', function ($scope) {
            $scope.getIcon = function (index: number) {
                var skill = $scope.playerInfo.characterSkills[index];
                var icon = GraphicsUtil.getIconCssFromId(skill.iconId);

                return icon;
            };

            $scope.useSkill = (skill: Skill) => {
                if (skill.cooldown <= 0) {
                    var packet = PacketFactory.createSkillUsePacket(skill.id, -1);
                    NetworkManager.getInstance().sendPacket(packet);
                }
            };
        }
    ]);

    Interface.game.angular.controller('DialogController', [
        '$scope', function ($scope) {
            // Respond to a dialog change
            $scope.$on('DialogChanged',(event, data) => {
                $scope.selectQuest($scope.selectedIndex);
            });

            $scope.getIcon = function (index: number) {

            };
        }
    ]);

    Interface.game.angular.controller('CharacterStatusController', [
        '$scope', function ($scope) {
            $scope.getVitalPercent = function (type: number) {
                var vital = this.playerInfo.characterStats[type];

                if (!vital)
                    return 0;

                var percent = (vital.currentValue / vital.maximumValue) * 100;

                return percent;
            }

            $scope.getVitalLabel = function (type: number) {
                var vital = this.playerInfo.characterStats[type];

                if (!vital)
                    return "0";

                return vital.currentValue.toString() + "/" + vital.maximumValue.toString();
            }
        }
    ]);

    Interface.game.angular.controller('BottomBarController', [
        '$scope', function ($scope) {
            $scope.getExpPercent = function (type: number) {
                if (!this.playerInfo.player)
                    return 0;

                var exp = this.playerInfo.player.experience;
                if (!exp)
                    return 0;

                var percent = (exp / (this.playerInfo.player.level * 500)) * 100;

                return percent;
            }

            $scope.getExpLabel = function () {
                if (!this.playerInfo.player)
                    return "";

                return this.playerInfo.player.experience + "/" + this.playerInfo.player.level * 500;
            }
        }
    ]);

    Interface.game.angular.controller('CharacterWindowController', [
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
}

window.onload = () => {
    // Setup underscore
    var game = new OpenORPG.Game();
};