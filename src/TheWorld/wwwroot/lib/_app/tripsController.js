!function(){"use strict";function i(i){var t=this;t.trips=[],t.newTrip={},t.errorMessage="",t.isBusy=!0,i.get("/api/trips").then(function(i){angular.copy(i.data,t.trips)},function(i){t.errorMessage="Failed to load data: "+i})["finally"](function(){t.isBusy=!1}),t.addTrip=function(){t.isBusy=!0,t.errorMessage="",i.post("/api/trips",t.newTrip).then(function(i){t.trips.push(i.data),t.newTrip={}},function(i){t.errorMessage="Failed to save new trip."})["finally"](function(){t.isBusy=!1})}}i.$inject=["$http"],angular.module("app-trips").controller("tripsController",i)}();