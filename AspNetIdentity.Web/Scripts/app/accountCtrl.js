(function () {
    'use strict';

    var app = angular.module('app', []);
    app.controller('accountController', ['$scope', '$http', accountController]);

    function accountController($scope, $http) {
        $scope.loading = true;
        $scope.addMode = false;
        $scope.apiHome = 'http://localhost:51624';

        $http.get($scope.apiHome + '/api/accounts/users').success(function(data) {
                $scope.accounts = data;
                $scope.loading = false;
            })
            .error(function() {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

        $scope.toggleEdit = function() {
            $scope.editMode = !$scope.editMode;
        };

        $scope.toggleAdd = function() {
            $scope.addMode = !$scope.addMode;
        };

        //Create Account
        $scope.add = function() {
            $scope.loading = true;
            $http.post($scope.apiHome + '/api/accounts/create', this.newaccount).success(function (data) {
                alert("Account Created Successfully!");
                $scope.addMode = false;
                $scope.accounts.push(data);
                $scope.loading = false;
            }).error(function(data) {
                $scope.error = "An Error has occured while Adding account! " + data;
                $scope.loading = false;
            });
        };

        //Edit Account
        $scope.save = function() {
            alert("Edit");
            $scope.loading = true;
            var frien = this.account;
            alert(frien);
            $http.put($scope.apiHome + '/api/account/' + friend.Id, frien).success(function (data) {
                alert("account updated!!");
                frien.editMode = false;
                $scope.loading = false;
            }).error(function(data) {
                $scope.error = "Unable to save account!" + data;
                $scope.loading = false;
            });
        };

        $scope.deleteaccount = function() {
            $scope.loading = true;
            var Id = this.account.id;
            $http.delete($scope.apiHome + '/api/account/' + Id).success(function (data) {
                alert("Account successfully deleted!!");
                $.forEach($scope.accounts, function(i) {
                    if ($scope.accounts[i].id === Id) {
                        $scope.accounts.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
            }).error(function(data) {
                $scope.error = "An Error has occured while deleteing account!" + data;
                $scope.loading = false;
            });
        };
    }
})();