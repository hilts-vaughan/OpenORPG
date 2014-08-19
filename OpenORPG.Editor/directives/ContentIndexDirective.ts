

/*
 * We place our actual directive code first, the rest is our controller for this directive
 */
editorApp.directive("contentIndexDirective",
    () => {
        return {
            restrict: 'E',
            scope: {
                'container': '=indexContainer',
                'search': '=filter'
            },
            templateUrl: "views/templates/index_table.html"

        }
    }
    );


class ContentIndexDirective {
    private scope: any;

    constructor($scope: any, $http: any, $routeParams: any, $location: any) {

        // Do something fancy here if required; although all that should be required is a container list, and objects within it
        this.scope = $scope;

        /*
         * You can implement some special stuff to do here if you have to
         */
        $scope.doSomethingDumb = () => {

        };


    }



}