function CreateAlbumCtrl($scope, $modal, $log){
    $scope.uploadPhoto = function (){
        var modalInstance = $modal.open({
            templateUrl: 'partials/upload-photos.html',
            controller: ModalInstanceCtrl,
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

function ModalInstanceCtrl($scope, $modalInstance, $interval) {
    $scope.uploadForm = {
        upload: false,
        progress: 0,
        max: 100,
        complete: false
    };

    var intervalPromise;
    $scope.upload = function(){
        $scope.uploadForm.upload = true;
        intervalPromise = $interval(function(){
            if ($scope.uploadForm.progress < $scope.uploadForm.max)
                $scope.uploadForm.progress = $scope.uploadForm.progress + 20;
            else {
                if (intervalPromise) $interval.cancel(intervalPromise);
                $scope.uploadForm.complete = true;
            }
        }, 1000)
    };
    $scope.cancel = function () {
        if (intervalPromise) $interval.cancel(intervalPromise);
        $modalInstance.dismiss('cancel');
    };
    $scope.ok = function(){
        $modalInstance.close();
    }
}