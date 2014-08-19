 module Controllers {
     
     export class SkillController {
         private httpService: ng.IHttpService;
         private $scope: any;

         constructor($scope: any, $http: any, $routeParams: any, $location: any) {
             this.httpService = $http;
             this.$scope = $scope;

             this.getSkill($routeParams.id, (skill) => {
                 $scope.skill = skill;
                 $scope.id = $routeParams.id;
                 console.log($scope);
             });

             $scope.save = () => {

                 this.httpService.post('/api/skills/' + $scope.id, this.$scope.skill).success((data) => {
                     $location.path('/skills');
                 });

             };

         }
         getSkill(id: number, successCallback: Function): void {
             this.httpService.get('/api/skills/' + id).success((data, status) => {
                 successCallback(data);
             });
         }

     }


 }