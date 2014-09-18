angular.module('SessionService', [])
    .service('Session', function () {
        this.username = null;
        this.password;
        this.name;
        this.token;

        this.clean = function(){
            delete this.username;
            delete this.password;
            delete this.name;
            delete this.token;
        }
    });