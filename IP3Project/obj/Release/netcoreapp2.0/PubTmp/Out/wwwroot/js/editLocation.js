/*

Author: Team16
Date: Trimester 2, 2018
Version: 1.0

Javascript functions that attend to the editing of and Location

Extensions and Tutorials used:
Google Maps API - Maps Positioning - https://developers.google.com/maps/documentation/ - Free License Plan

*/

///Prevents the form from trying to automatically submit due to enter being pressed
$("#pac-input").keypress(function (e) {
    var code = e.keyCode ? e.keyCode : e.which;
    if (code == 13) { //Enter keycode
        return false;
    }
});

///Google map API setup
function initMap() {

    var lat = parseFloat($(".lat").attr('id')); //Sets long and lat as variables
    var lng = parseFloat($(".lng").attr('id'));

    var markers = [];
    var map = new google.maps.Map(document.getElementById('map'), { //sets map
        center: { lat: lat, lng: lng },
        zoom: 5
    });

    addMarker(map.center); //adds marker to map

    map.addListener('click', function (event) { //If clicked on then a marker appears in position and sets location to its co-ordinates
        deleteMarkers();
        addMarker(event.latLng);
        $('#Location').val(event.latLng.lat() + "," + event.latLng.lng());
    });

    function addMarker(location) { //Adds marker to map
        var marker = new google.maps.Marker({
            position: location,
            map: map
        });
        markers.push(marker);
    }


    function setMapOnAll(map) { //Sets up marker maps in specified way
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    function deleteMarkers() { //Delete that calls clear markers for removing marker from map
        clearMarkers();
        markers = [];
    }

    function clearMarkers() { //clears a marker from map and closes any info windows
        marker.setVisible(false);
        infowindow.close();
        setMapOnAll(null);
    }

    var input = document.getElementById('pac-input'); //Sets up search box for searching positiongs in API

    var autocomplete = new google.maps.places.Autocomplete(
        input);
    autocomplete.bindTo('bounds', map);

    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input); //Places it top left of map

    var infowindow = new google.maps.InfoWindow(); //Sets up the info window
    var infowindowContent = document.getElementById('infowindow-content');
    infowindow.setContent(infowindowContent);
    var geocoder = new google.maps.Geocoder;
    var marker = new google.maps.Marker({
        map: map
    });
    marker.addListener('click', function () { //Brings up info window on click
        infowindow.open(map, marker);
    });

    autocomplete.addListener('place_changed', function () { //If places change then current marker deleted and new place set
        deleteMarkers();
        var place = autocomplete.getPlace();


        if (!place.place_id) {
            return;
        }
        geocoder.geocode({ 'placeId': place.place_id }, function (results, status) { //Geocodes the place

            if (status !== 'OK') {
                window.alert('Geocoder failed due to: ' + status);
                return;
            }
            map.setZoom(11);
            map.setCenter(results[0].geometry.location);
            // Set the position of the marker using the place ID and location.
            marker.setPlace({
                placeId: place.place_id,
                location: results[0].geometry.location
            });
            marker.setVisible(true);
            infowindowContent.children['place-name'].textContent = place.name; //Sets marker and info window with the place 
            infowindowContent.children['place-address'].textContent =
                results[0].formatted_address;

            infowindow.open(map, marker);
        });

        var lat = place.geometry.location.lat(); //Gets Lat and Long positions and sets location to them
        var lng = place.geometry.location.lng();
        $('#Location').val(lat + "," + lng);
    });
}