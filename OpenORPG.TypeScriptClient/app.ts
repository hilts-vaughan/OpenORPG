/// <reference path="jquery.d.ts" />
/// <reference path="underscore.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts"/>

/// <reference path="phaser.d.ts" />

/// <reference path="Game/CharacterState.ts" />
/// <reference path="Game/World/Zone.ts" />

/// <reference path="Game/Direction.ts" />

/// <reference path="OpenORPG/Interface.ts" />

module OpenORPG {
    class InventoryController implements IController {
        public get name(): string {
            return "inventoryController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
                $scope.gold = 4000;
            };
        }
    }

    class SettingsController implements IController {
        public get name(): string {
            return "SettingsController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
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
            };
        }
    }

    class QuestListController implements IController {
        public get name(): string {
            return "QuestListController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
                var formatter: RequirementFormatter = new RequirementFormatter();

                $scope.$on('QuestsChanged', (event, data) => {
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

                    _.each(requirements, (requirement: any, index: number) => {
                        formatter.getFormattedRequirement(requirement.type, requirement.info, $scope.selectedQuest.questInfo.requirementProgress[index].progress, (result) => {
                            $scope.currentTask += result;
                        });
                    });
                };
            };
        }
    }

    class SkillListController implements IController {
        public get name(): string {
            return "SkillListController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
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
            };
        }
    }

    class DialogController implements IController {
        public get name(): string {
            return "DialogController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
                // Respond to a dialog change
                $scope.$on('DialogChanged', (event, data) => {
                    $scope.selectQuest($scope.selectedIndex);
                });

                $scope.getIcon = function (index: number) {

                };
            };
        }
    }

    class CharacterStatusController implements IController {
        public get name(): string {
            return "CharacterStatusController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
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
            };
        }
    }

    class BottomBarController implements IController {
        public get name(): string {
            return "BottomBarController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
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
            };
        }
    }

    class CharacterWindowController implements IController {
        public get name(): string {
            return "CharacterWindowController";
        }

        public get angular(): ($scope: any, $rootScope: any) => any {
            return ($scope: any, $rootScope: any) => {
                // We can do some cool stuff here if we want to, but otherwise we're ok 
                $scope.getStatNameFromIndex = index => {
                    var names: string[] = [];
                    for (var n in StatTypes) {
                        if (typeof StatTypes[n] === 'number') names.push(n);
                    }
                    return names[index];
                };
            };
        }
    }

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

    Interface.game.controller(new InventoryController());
    Interface.game.controller(new SettingsController());
    Interface.game.controller(new QuestListController());
    Interface.game.controller(new SkillListController());
    Interface.game.controller(new DialogController());
    Interface.game.controller(new CharacterStatusController());
    Interface.game.controller(new BottomBarController());
    Interface.game.controller(new CharacterWindowController());
}

window.onload = () => {
    /* Setup underscore */
    var game = new OpenORPG.Game();
};