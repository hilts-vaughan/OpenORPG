module AngularInterop {


    
    /**
     * Updates the AngularJS scope on this page
     */
    export function updateAngularScope() {
        var $body = angular.element(document.body);   // 1    

        var service = $body.injector().get('$timeout');
        var $rootScope: any = $body.scope();

        var phase = $rootScope.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
        } else {
            $rootScope.$apply();
        }
    }

    export function broadcastEvent(eventName: string) {
        var $body = angular.element(document.body);
        var $rootScope: any = $body.scope();

        // Send event
        $rootScope.$broadcast(eventName);
        this.updateAngularScope();
    }


}