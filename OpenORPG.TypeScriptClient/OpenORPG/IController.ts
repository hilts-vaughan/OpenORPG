module OpenORPG {
    export interface Scope extends ng.IScope {
        settings: Settings;
    }

    export interface IController {
        name: string;

        angular: ($scope: ng.IScope, $rootScope: ng.IScope) => any;
    }
}