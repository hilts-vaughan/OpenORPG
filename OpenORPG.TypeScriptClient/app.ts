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

module OpenORPG {
    export class Indexer<T> {
        [key: string]: T;
    }

    export class Dictionary<T> {
        private _indexer: Indexer<T>;

        constructor() {
            this._indexer = new Indexer<T>();
        }

        protected get indexer(): Indexer<T> {
            return this._indexer;
        }

        public get(key: string): T {
            return this.indexer[key];
        }

        public set(key: string, value: T): T {
            this.indexer[key] = value;

            return value;
        }

        public has(key: string): boolean {
            return !(this.indexer[key] === undefined || this.indexer[key] === null);
        }

        public del(key: string): boolean {
            if (!this.has(key)) {
                return false;
            }

            this.indexer[key] = undefined;
            return true;
        }
    }

    export class Angular {
        private static _modules: Dictionary<Angular.Module> = null;

        public static get modules(): Dictionary<Angular.Module> {
            if (this._modules === undefined || this._modules === null) {
                this._modules = new Dictionary<Angular.Module>();
            }

            return this._modules;
        }

        public static safeGet(name: string): Angular.Module {
            if (!this.modules.has(name)) {
                return this.modules.set(name, new Angular.Module(name));
            }

            return this.modules.get(name);
        }

        public static get Game(): Angular.Module {
            return this.safeGet('game');
        }

        public static initialize(): void {
            Angular.Game.module.directive('openDialog', function () {
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

            Angular.Game.module.filter("skillFormatter",() => {
                return (input: number) => {
                    return Math.max(Math.ceil(input), 0) + "s";
                };
            });
        }
    }

    export module Angular {
        export class Providers {
            public $controllerProvider: any;
            public $compileProvider: any;
            public $provide: any;
        }

        export class Module {
            private providers: Providers;
            private ngModule: ng.IModule;
            private queueLength: number;

            constructor(name: string) {
                this.providers = new Angular.Providers();

                var instance = this;
                this.ngModule = angular.module(name, [],
                    function ($controllerProvider: any, $compileProvider: any, $provide: any): void {
                        instance.providers.$controllerProvider = $controllerProvider;
                        instance.providers.$compileProvider = $compileProvider;
                        instance.providers.$provide = $provide;
                    });
            }

            public get module(): ng.IModule {

                return this.ngModule;
            }

            public flushQueueLength(): void {
                this.queueLength = this.ngModule['_invokeQueue'].length;
            }

            public register(): void {
                var queue = this.ngModule['_invokeQueue'];

                for (var i = this.queueLength; i < queue.length; i++) {
                    var call = queue[i];

                    var provider = this.providers[call[0]];
                    if (provider) {
                        provider[call[1]].apply(provider, call[2]);
                    }
                }

                this.queueLength = queue.length;
            }
        }
    }

    Angular.initialize();

    Angular.Game.module.controller('inventoryController', [
        '$scope', '$rootScope', function ($scope, $rootScope) {
            $scope.gold = 4000;
        }
    ]);

    Angular.Game.module.controller('SettingsController', [
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

    Angular.Game.module.controller('QuestListController', [
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

    Angular.Game.module.controller('SkillListController', [
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

    Angular.Game.module.controller('DialogController', [
        '$scope', function ($scope) {
            // Respond to a dialog change
            $scope.$on('DialogChanged',(event, data) => {
                $scope.selectQuest($scope.selectedIndex);
            });

            $scope.getIcon = function (index: number) {

            };
        }
    ]);

    Angular.Game.module.controller('CharacterStatusController', [
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

    Angular.Game.module.controller('BottomBarController', [
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

    Angular.Game.module.controller('CharacterWindowController', [
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

    Angular.Game.flushQueueLength();
}

window.onload = () => {
    // Setup underscore
    var game = new OpenORPG.Game();
};