module Items {
    export interface Scope {
        item: any;
        types: any;
    }

    export enum ItemType {
        FieldItem,
        Equipment,
        Skillbook
    }


    export class ItemController {
        private httpService: ng.IHttpService;

        constructor($scope: any, $http: any, $routeParams: any) {
            this.httpService = $http;

            this.getItem($routeParams.itemId, (item) => {
                $scope.item = item;




                $scope.types = [];
                for (var n in ItemType) {
                    if (typeof ItemType[n] === 'number')
                        $scope.types.push({ "name": n });
                };
                

                $scope.selectedType = $scope.types[$scope.item.type];


                console.log($scope);
            });

            var controller = this;


            console.log($scope.types);

        }




        getItem(id: number, successCallback: Function): void {
            this.httpService.get('/api/items/' + id).success((data, status) => {
                successCallback(data);
            });
        }

    }




}



