module OpenORPG {
    export interface IController {
        name: string;

        angular: ($scope: ng.IScope, $rootScope: ng.IScope) => any;
    }
}