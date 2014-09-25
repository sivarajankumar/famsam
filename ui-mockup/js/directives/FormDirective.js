angular.module("FormDirective",[])
.directive('ngAutofocus', function(){
        return {
            link: function (scope, elem, attr){
                angular.element(elem)[0].focus();
            }
        }
    })