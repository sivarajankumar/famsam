function CreateStoryCtrl($scope, $modal, $log){
    $scope.addAlbum = function () {
        var modalInstance = $modal.open({
            templateUrl: 'partials/add-album.html',
            controller: addAlbumCtrl,
            size: 'lg',
            backdrop: 'static'
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
}

function addAlbumCtrl($scope, $modalInstance) {
    $scope.addAlbumForm = {
        uploaded: false
    };
    $scope.cancel = function () {
        $modalInstance.close();
    };
    $scope.ok = function () {
        $modalInstance.close();
    };
}