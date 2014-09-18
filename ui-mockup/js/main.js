var app = angular.module("FAMSAM", ["ui.router", "SessionService", 'ui.bootstrap', 'LocalStorageModule']);

//route config
app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/home");

    $stateProvider
        .state('home',{
            url: '/home',
            templateUrl: 'partials/home.html',
            controller: LoginCtrl
        })
        .state('register',{
            url: '/register',
            templateUrl: 'partials/register.html',
            controller: RegisterCtrl
        })
        .state('allphotos', {
            url: '/allphotos',
            templateUrl: 'partials/viewallphotos.html',
            controller: AllPhotosCtrl
        })
        .state('create-story',{
            url: '/create-story',
            templateUrl: 'partials/create-story.html',
            controller: CreateStoryCtrl
        })
        .state('creat-album',{
            url: '/create-album',
            templateUrl: 'partials/create-album.html',
            controller: CreateAlbumCtrl
        })
    ;
});