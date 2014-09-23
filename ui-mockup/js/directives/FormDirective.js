angular.module("FormDirective",[])
.directive('ngAutofocus', function(){
        return {
            link: function (scope, elem, attr){
                console.log(angular.element(elem)[0]);
                angular.element(elem)[0].focus();
            }
        }
    })