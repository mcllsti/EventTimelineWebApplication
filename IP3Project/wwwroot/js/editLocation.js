$("#pac-input").keypress(function (e) {
    var code = (e.keyCode ? e.keyCode : e.which);
    if (code == 13) { //Enter keycode
        return false;
    }
});


function initMap() {

    var lat = parseFloat($(".lat").attr('id'));
    var lng = parseFloat($(".lng").attr('id'));

    var markers = [];
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: lat, lng: lng },
        zoom: 5
    });

    addMarker(map.center);

    map.addListener('click', function (event) {
        deleteMarkers()
        addMarker(event.latLng);
        $('#Location').val(event.latLng.lat() + "," + event.latLng.lng());
    });

    function addMarker(location) {
        var marker = new google.maps.Marker({
            position: location,
            map: map
        });
        markers.push(marker);
    }


    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    function deleteMarkers() {
        clearMarkers();
        markers = [];
    }

    function clearMarkers() {
        marker.setVisible(false);
        infowindow.close();
        setMapOnAll(null);
    }

    var input = document.getElementById('pac-input');

    var autocomplete = new google.maps.places.Autocomplete(
        input);
    autocomplete.bindTo('bounds', map);

    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    var infowindow = new google.maps.InfoWindow();
    var infowindowContent = document.getElementById('infowindow-content');
    infowindow.setContent(infowindowContent);
    var geocoder = new google.maps.Geocoder;
    var marker = new google.maps.Marker({
        map: map
    });
    marker.addListener('click', function () {
        infowindow.open(map, marker);
    });

    autocomplete.addListener('place_changed', function () {
        deleteMarkers()
        var place = autocomplete.getPlace();


        if (!place.place_id) {
            return;
        }
        geocoder.geocode({ 'placeId': place.place_id }, function (results, status) {

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
            infowindowContent.children['place-name'].textContent = place.name;
            infowindowContent.children['place-address'].textContent =
                results[0].formatted_address;

            infowindow.open(map, marker);
        });

        var lat = place.geometry.location.lat();
        var lng = place.geometry.location.lng();
        $('#Location').val(lat + "," + lng);
    });
}