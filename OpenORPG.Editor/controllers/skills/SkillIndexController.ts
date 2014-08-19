module Controllers {
    export class SkillIndexScope {

        public itemContainer: IndexContainer<any>;
        newItem: Function;
        deleteItem: Function;
        itemTypes: Object;
    }



    export class SkillIndexController {
        private httpService: ng.IHttpService;

        constructor($scope: SkillIndexScope, $http: any, $location: ng.ILocationService) {
            this.httpService = $http;

            this.refreshProducts($scope);


            console.log($scope);

            var controller = this;

            $scope.itemContainer = new IndexContainer<Models.Item>();
            $scope.itemContainer.type = "skills";

            $scope.newItem = function () {


                controller.addProduct(null, function (data) {
                    $location.path("/items/" + data.id);
                });
            };

            $scope.deleteItem = function (productId) {
                controller.deleteProduct(productId, function () {

                    controller.getAllProducts(function (data) {
                        $scope.itemContainer.items = data;
                    });
                });
            }


    }



        getAllProducts(successCallback: Function): void {
            this.httpService.get('/api/skills').success((data, status) => {
                successCallback(data);
            });
        }

        addProduct(item: Models.Item, successCallback: Function): void {
            this.httpService.put('/api/skills', null).success((data) => {
                successCallback(data);
            });
        }

        deleteProduct(itemId: string, successCallback: Function): void {
            this.httpService.delete('/api/skills/' + itemId).success(() => {
                successCallback();
            });
        }

        refreshProducts(scope: SkillIndexScope) {
            scope.itemTypes = Models.ItemType;
            this.getAllProducts(data => {
                scope.itemContainer.items = data;
            });
        }


    }



}