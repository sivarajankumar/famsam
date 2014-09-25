angular.module("FormDirective",[])
.directive('ngAutofocus', function(){
        return {
            link: function (scope, elem, attrs){
                angular.element(elem)[0].focus();
            }
        }
    });