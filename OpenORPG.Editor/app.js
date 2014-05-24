/// <reference path="Scripts/angular.d.ts" />
var Items;
(function (Items) {
    var Controller = (function () {
        function Controller($scope, $http) {
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
            };
        }
        Controller.prototype.getAllProducts = function (successCallback) {
            this.httpService.get('/api/products').success(function (data, status) {
                successCallback(data);
            });
        };

        Controller.prototype.addProduct = function (item, successCallback) {
            this.httpService.post('/api/products', item).success(function () {
                successCallback();
            });
        };

        Controller.prototype.deleteProduct = function (itemId, successCallback) {
            this.httpService.delete('/api/products/' + itemId).success(function () {
                successCallback();
            });
        };

        Controller.prototype.refreshProducts = function (scope) {
            this.getAllProducts(function (data) {
                scope.items = data;
            });
        };
        return Controller;
    })();
    Items.Controller = Controller;
})(Items || (Items = {}));
var Models;
(function (Models) {
    var Item = (function () {
        function Item() {
        }
        return Item;
    })();
    Models.Item = Item;

    (function (ItemType) {
        ItemType[ItemType["FieldItem"] = 0] = "FieldItem";
        ItemType[ItemType["Equipment"] = 1] = "Equipment";
    })(Models.ItemType || (Models.ItemType = {}));
    var ItemType = Models.ItemType;
})(Models || (Models = {}));
//# sourceMappingURL=app.js.map
