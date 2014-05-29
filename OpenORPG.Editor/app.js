/// <reference path="Scripts/angular.d.ts" />
/// <reference path="Scripts/angular-route.d.ts" />
// script.js
// create the module and name it scotchApp
// also include ngRoute for all our routing needs
var editorApp = angular.module('editorApp', ['ngRoute', 'ngAnimate']);

// configure our routes
editorApp.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'views/templates/dashboard.html',
        controller: 'mainController'
    }).when('/about', {
        templateUrl: 'views/templates/about.html',
        controller: 'aboutController'
    }).when('/contact', {
        templateUrl: 'pages/contact.html',
        controller: 'contactController'
    }).when('/items', {
        templateUrl: 'views/templates/items.html',
        controller: 'Items.ItemIndexController'
    }).when('/items/:itemId', {
        templateUrl: 'views/templates/items_details.html',
        controller: 'Items.itemDetailsController'
    });
});

// create the controller and inject Angular's $scope
editorApp.controller('mainController', function ($scope) {
    // create a message to display in our view
    $scope.message = 'Everyone come and see how good I look!';
});

editorApp.controller('aboutController', function ($scope) {
    $scope.message = 'Look! I am an about page.';
});

editorApp.controller('contactController', function ($scope) {
    $scope.message = 'Contact us! JK. This is just a demo.';
});
var Items;
(function (Items) {
    var ItemController = (function () {
        function ItemController($scope, $http) {
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
        ItemController.prototype.getAllProducts = function (successCallback) {
            this.httpService.get('/api/items').success(function (data, status) {
                successCallback(data);
            });
        };

        ItemController.prototype.addProduct = function (item, successCallback) {
            this.httpService.post('/api/items', item).success(function () {
                successCallback();
            });
        };

        ItemController.prototype.deleteProduct = function (itemId, successCallback) {
            this.httpService.delete('/api/items/' + itemId).success(function () {
                successCallback();
            });
        };

        ItemController.prototype.refreshProducts = function (scope) {
            this.getAllProducts(function (data) {
                scope.items = data;
            });
        };
        return ItemController;
    })();
    Items.ItemController = ItemController;
})(Items || (Items = {}));
var Items;
(function (Items) {
    var ItemIndexScope = (function () {
        function ItemIndexScope() {
        }
        return ItemIndexScope;
    })();
    Items.ItemIndexScope = ItemIndexScope;

    var ItemIndexController = (function () {
        function ItemIndexController($scope, $http) {
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
            };
        }
        ItemIndexController.prototype.getAllProducts = function (successCallback) {
            this.httpService.get('/api/items').success(function (data, status) {
                successCallback(data);
            });
        };

        ItemIndexController.prototype.addProduct = function (item, successCallback) {
            this.httpService.post('/api/items', item).success(function () {
                successCallback();
            });
        };

        ItemIndexController.prototype.deleteProduct = function (itemId, successCallback) {
            this.httpService.delete('/api/items/' + itemId).success(function () {
                successCallback();
            });
        };

        ItemIndexController.prototype.refreshProducts = function (scope) {
            scope.itemTypes = Models.ItemType;
            this.getAllProducts(function (data) {
                scope.items = data;
            });
        };
        return ItemIndexController;
    })();
    Items.ItemIndexController = ItemIndexController;
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
