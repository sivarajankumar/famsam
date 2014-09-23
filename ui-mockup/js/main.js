var app = angular.module("FAMSAM", ["ui.router", "SessionService", 'ui.bootstrap', 'LocalStorageModule', 'FormDirective']);

//route config
app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/index");

    $stateProvider
        .state('index',{
            url: '/index',
            templateUrl: 'partials/index.html',
            controller: LoginCtrl
        })
        .state('home', {
            url: '/home',
            templateUrl: 'partials/home-master.html'
        })
        .state('register',{
            url: '/register',
            templateUrl: 'partials/register.html',
            controller: RegisterCtrl
        })
        .state('home.allphotos', {
            url: '/allphotos',
            templateUrl: 'partials/viewallphotos.html',
            controller: AllPhotosCtrl
        })
        .state('home.create-story',{
            url: '/create-story',
            templateUrl: 'partials/create-story.html',
            controller: CreateStoryCtrl
        })
        .state('home.creat-album',{
            url: '/create-album',
            templateUrl: 'partials/create-album.html',
            controller: CreateAlbumCtrl
        })
        .state('home.family-home',{
            url: '/family-home',
            templateUrl: 'partials/family-home.html'
        })
        .state('home.personal-info',{
            url: '/personal-info',
            templateUrl: 'partials/personal-info.html'
        })
        .state('home.album-detail',{
            url: '/album-detail',
            templateUrl: 'partials/album-detail.html'
        })
    ;
});
