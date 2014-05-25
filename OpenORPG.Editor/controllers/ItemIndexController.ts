module Items {
    export class ItemIndexScope {
        items: Models.Item[];
        addNewItem: Function;
        deleteItem: Function;
        itemTypes: Object;


    }



    export class ItemIndexController {
        private httpService: ng.IHttpService;

        constructor($scope: ItemIndexScope, $http: any) {
            this.httpService = $http;

            this.refreshProducts($scope);


            console.log($scope);

            var controller = this;

            $scope.addNewItem = function () {
                var newProduct = new Models.Item();

                controller.addProduct(newProduct, function () {
                    controller.getAllProducts(function (data) {
                        $scope.items = data;
                    });
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

        editItem(itemId: number) {
            alert("Edit " + itemId);
        }


        getAllProducts(successCallback: Function): void {
            this.httpService.get('/api/items').success((data, status) => {
                successCallback(data);
            });
        }

        addProduct(item: Models.Item, successCallback: Function): void {
            this.httpService.post('/api/items', item).success(() => {
                successCallback();
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