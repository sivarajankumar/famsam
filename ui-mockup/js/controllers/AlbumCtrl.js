function CreateAlbumCtrl($scope, $modal, $log) {
    $scope.uploadPhoto = function () {
        var modalInstance = $modal.open({
            templateUrl: 'partials/upload-photos.html',
            controller: AddPhotosCtrl,
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

function AddPhotosCtrl($scope, $modalInstance, $interval) {
    $scope.uploadForm = {
        upload: false,
        progress: 0,
        max: 100,
        complete: false,
        files: []
    };

    var intervalPromise;
    $scope.upload = function () {
        $scope.uploadForm.upload = true;
        intervalPromise = $interval(function () {
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
    $scope.ok = function () {
        $modalInstance.close();
    };
    $scope.selectFile = function (files) {
        console.log('xxxx');
        $scope.invalidFile = false;
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var reader = new FileReader();
            reader.onload = (function (theFile) {
                return function (e) {
                    var aFile = {
                        src: e.target.result,
                        filename: file.name
                    };
                    $scope.uploadForm.files.push(aFile);
                    $scope.$apply();
                }
            })(file);

            reader.readAsDataURL(file);
        }

    };
}