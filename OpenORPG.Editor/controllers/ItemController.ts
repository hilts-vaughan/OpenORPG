module Items {
    export interface Scope {
        item: any;
        id : number;
        types: any;
    }

    export enum ItemType {
        FieldItem,
        Equipment,
        Skillbook
    }


    export class ItemController {
        private httpService: ng.IHttpService;
        private $scope: any;

        constructor($scope: any, $http: any, $routeParams: any, $location : any) {
            this.httpService = $http;
            this.$scope = $scope;

            this.getItem($routeParams.itemId, (item) => {
                $scope.item = item;
                $scope.id = $routeParams.itemId;



                $scope.types = [];
                for (var n in ItemType) {
                    console.log(n);
                    if (typeof ItemType[n] === 'number')
                        $scope.types.push({ "name": n });
                };


                $scope.selectedType = $scope.types[$scope.item.type];


                console.log($scope);
            });

            var controller = this;

            // Setup collapse

            console.log($scope.types);

            $scope.changedType = () => {
                console.log(this.$scope.selectedType);
                console.log($scope.item);
            };

            $scope.save = () => {

                this.httpService.post('/api/items/' + $scope.id, this.$scope.item).success((data) => {
                    $location.path('/items');
                });

            };

        }




        getItem(id: number, successCallback: Function): void {
            this.httpService.get('/api/items/' + id).success((data, status) => {
                successCallback(data);
            });
        }

    }




}



