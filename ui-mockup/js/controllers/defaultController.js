function BaseCtrl($scope, Session, localStorageService) {
    if (!Session.username) {
        var localSession = localStorageService.get('Session');
        if (localSession) {
            Session.username = localSession.username;
            Session.password = localSession.password;
            Session.name = localSession.name;
            Session.token = localSession.token;
        }
    }
    $scope.session = Session;//ViewBag
}

function LoginCtrl($scope, Session, $state, localStorageService) {
    $scope.user = {loginFail: false};
    $scope.signin = function (valid) {
        if (valid) {
            if ($scope.user.username == 'mrbean' && $scope.user.password == 'mrbean') {
                //save user info to Session Service
                Session.username = $scope.user.username;
                Session.name = 'Mr. Bean';
                Session.password = $scope.user.password;
                Session.token = 'TOKEN_SAMPLE';

                //save session to local storage
                localStorageService.set('Session', Session);

                //save session to scope
                $scope.session = Session;
                $state.go('allphotos');
            } else {
                $scope.user.loginFail = true;
            }
        } else {

        }
    }
}
function RegisterCtrl($scope) {

}
function NewFeedCtrl($scope) {

}