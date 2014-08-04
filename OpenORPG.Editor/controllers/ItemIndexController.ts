module Items {
    export class ItemIndexScope {
        items: Models.Item[];
        newItem: Function;
        deleteItem: Function;
        itemTypes: Object;


    }



    export class ItemIndexController {
        private httpService: ng.IHttpService;

        constructor($scope: ItemIndexScope, $http: any, $location : ng.ILocationService) {
            this.httpService = $http;

            this.refreshProducts($scope);


            console.log($scope);

            var controller = this;

            $scope.newItem = function () {
       

                controller.addProduct(null, function (data) {
                    $location.path("/items/" + data.id);
                });
            };

            $scope.deleteItem = function (productId) {
                controller.deleteProduct(productId, function () {
             
                    controller.getAllProducts(function (data) {
                        $scope.items = data;                   
                    });
                });
            }


    }

    

        getAllProducts(successCallback: Function): void {
            this.httpService.get('/api/items').success((data, status) => {
                successCallback(data);
            });
        }

        addProduct(item: Models.Item, successCallback: Function): void {
            this.httpService.put('/api/items',null).success((data) => {
                successCallback(data);
            });
        }

        deleteProduct(itemId: string, successCallback: Function): void {
            this.httpService.delete('/api/items/' + itemId).success(() => {
                successCallback();
            });
        }

        refreshProducts(scope: ItemIndexScope) {
            scope.itemTypes = Models.ItemType;
            this.getAllProducts(data => {
                scope.items = data;
            });
        }


    }



}