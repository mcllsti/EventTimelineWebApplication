
var map;
var markers = [];
var markerAPI;
var markerSearch;
var mapCenter = { lat: 30, lng: 0 };
var zoomLevel = 2;
var apiAvailable = false;
var apiLat;
var apiLng;

function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: mapCenter,
        zoom: zoomLevel
    });
    var card = document.getElementById('pac-card');
    var input = document.getElementById('pac-input');
    var types = document.getElementById('type-selector');
    var strictBounds = document.getElementById('strict-bounds-selector');

    // click on the map to add a marker
    google.maps.event.addListener(map, "click", function (event) {

        addMarker(event.latLng);

        console.log(event.latLng + ', ' + markers[0].getPosition().lat() + ', ' + markers[0].getPosition().lng());
    });

    map.controls[google.maps.ControlPosition.TOP_RIGHT].push(card);

    var autocomplete = new google.maps.places.Autocomplete(input);

    // Bind the map's bounds (viewport) property to the autocomplete object,
    // so that the autocomplete requests use the current map bounds for the
    // bounds option in the request.
    autocomplete.bindTo('bounds', map);

    var infowindow = new google.maps.InfoWindow();
    var infowindowContent = document.getElementById('infowindow-content');
    infowindow.setContent(infowindowContent);
    markerSearch = new google.maps.Marker({
        map: map,
        anchorPoint: new google.maps.Point(0, -29)
    });

    autocomplete.addListener('place_changed', function () {
        infowindow.close();
        markerSearch.setVisible(false);
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            // User entered the name of a Place that was not suggested and
            // pressed the Enter key, or the Place Details request failed.
            window.alert("No details available for input: '" + place.name + "'");
            return;
        }

        // If the place has a geometry, then present it on a map.
        if (place.geometry.viewport) {
            map.fitBounds(place.geometry.viewport);
            addMarker(place.geometry.location);
        } else {
            map.setCenter(place.geometry.location);
            map.setZoom(17);  // Why 17? Because it looks good.
            addMarker(place.geometry.location);
        }
        markerSearch.setPosition(place.geometry.location);

        markerSearch.setVisible(true);

        //                    var address = '';
        //                    if (place.address_components) {
        //                        address = [
        //                            (place.address_components[0] && place.address_components[0].short_name || ''),
        //                            (place.address_components[1] && place.address_components[1].short_name || ''),
        //                            (place.address_components[2] && place.address_components[2].short_name || '')
        //                        ].join(' ');
        //                    }
        //
        //                    infowindowContent.children['place-icon'].src = place.icon;
        //                    infowindowContent.children['place-name'].textContent = place.name;
        //                    infowindowContent.children['place-address'].textContent = address;
        //                    infowindow.open(map, markerSearch);
    });

    // Sets a listener on a radio button to change the filter type on Places
    // Autocomplete.
    function setupClickListener(id, types) {
        var radioButton = document.getElementById(id);
        radioButton.addEventListener('click', function () {
            autocomplete.setTypes(types);
        });
    }

    setupClickListener('changetype-all', []);
    //                setupClickListener('changetype-address', ['address']);
    //                setupClickListener('changetype-establishment', ['establishment']);
    //                setupClickListener('changetype-geocode', ['geocode']);

    document.getElementById('use-strict-bounds')
        .addEventListener('click', function () {
            console.log('Checkbox clicked! New state=' + this.checked);
            autocomplete.setOptions({ strictBounds: this.checked });
        });
}

// FUNCTIONS FOR MARKER PLACMENT
// Adds a marker to the map and to the array.
function addMarker(location) {
    //clearMarkers();
    deleteMarkers();
    marker = new google.maps.Marker({
        position: location,
        map: map,
        animation: google.maps.Animation.DROP
    });
    markers[0] = marker;
    getPoint_Lat(markers[0].getPosition().lat());
    getPoint_Lng(markers[0].getPosition().lng());

    //**************************************************************
    //UPLOAD 'location' to API HERE, 'location' = (00.00000, 00.000)
    //**************************************************************
}

// Sets the map on all markers in the array.
function setMapOnAll(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
    setMapOnAll(null);
    markers = [];
    markerSearch.setVisible(false);
    getPoint_Lat("None");
    getPoint_Lng("None");
    //removes the marker that is set by the api
    //markerAPI.setMap(null);
}

function getPoint_Lat(lat) {
    document.getElementById('pointLat').innerHTML = [lat];
}

function getPoint_Lng(lng) {
    document.getElementById('pointLng').innerHTML = [lng];
}