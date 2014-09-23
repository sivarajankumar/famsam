/**
 * Created by ilaptop on 9/23/2014.
 */
function AllPhotosCtrl($scope, $modal, $log) {
    $scope.viewphoto = function () {
        var modalInstance = $modal.open({
            templateUrl: 'partials/view-photo.html',
            controller: PhotoCtrl,
            size: 'lg',
            backdrop: 'static'
            /*resolve: {
             session: function () {
             return $scope.session;
             }
             }*/
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
}

function PhotoCtrl($scope, $modalInstance) {
    $scope.myInterval = 5000;
    var slides = $scope.slides = [];
    $scope.addSlide = function(m) {
        if(m==1) {
            slides.push({
                image: 'img/avatar.jpg',
                text: 'demo hehe'
            });
        } else {
            slides.push({
                image: 'img/demoviewphoto.jpg',
                text: 'demo hehe'
            });
        }
    };
    $scope.addSlide(0);
    $scope.addSlide(1);
}