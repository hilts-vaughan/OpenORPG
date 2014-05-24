module Items {
    export interface Scope {
        items: Models.Item[];
        addNewItem: Function;
        deleteItem: Function;
    }

 

    export class Controller {
        private httpService: ng.IHttpService;

        constructor($scope: Scope, $http: any) {
            this.httpService = $http;

            this.refreshProducts($scope);

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

        getAllProducts(successCallback: Function): void {           
            this.httpService.get('/api/products').success((data, status) => {
                successCallback(data);
            });
        }

        addProduct(item: Models.Item, successCallback: Function): void {
            this.httpService.post('/api/products', item).success(() => {
                successCallback();
            });
        }

        deleteProduct(itemId: string, successCallback: Function): void {
            this.httpService.delete('/api/products/' + itemId).success(() => {
                successCallback();
            });
        }

        refreshProducts(scope: Scope) {
            this.getAllProducts(data => {
                scope.items = data;
            });
        }


    }



}