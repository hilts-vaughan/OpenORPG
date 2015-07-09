module OpenORPG {
    class Providers {
        public $controllerProvider: any;
        public $compileProvider: any;
        public $provide: any;
    }

    export class Module {
        private _angular: ng.IModule = null;
        private _providers: Providers = null;
        private _queueLen: number = -1;

        constructor(name: string) {
            this._providers = new Providers();

            var instance = this;
            this._angular = angular.module(name, [],
                function ($controllerProvider: any, $compileProvider: any, $provide: any): void {
                    instance._providers.$controllerProvider = $controllerProvider;
                    instance._providers.$compileProvider = $compileProvider;
                    instance._providers.$provide = $provide;
                });
            
            this._queueLen = this._angular['_invokeQueue'].length;
        }

        /* TODO: Fully wrap ng.IModule and remove angular and register() */
        public get angular(): ng.IModule {
            return this._angular;
        }

        public get name(): string {
            return this.angular.name;
        }

        public bootstrap(): ng.auto.IInjectorService {
            return angular.bootstrap(document, [this.name]);
        }

        public controller(controller: IController): Module {
            this._angular.controller(controller.name, ['$scope', '$rootScope',
                ($scope: any, $rootScope: any) => {
                    $scope.controller = controller;
                    $scope.settings = $.extend({}, Settings.getInstance());

                    controller.angular($scope, $rootScope);
                }
            ]);

            return this.register();
        }

        public register(): Module {
            var queue = this._angular['_invokeQueue'];

            for (var i = this._queueLen; -1 < i && i < queue.length; i++) {
                var call = queue[i];

                var provider = this._providers[call[0]];
                if (provider) {
                    provider[call[1]].apply(provider, call[2]);
                }
            }

            this._queueLen = queue.length;

            return this;
        }
    }
}