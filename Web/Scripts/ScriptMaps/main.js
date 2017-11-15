var place1LatLong;
var place2LatLong;

google.maps.event.addDomListener(window, "load", () => {
    const user = new UserLocation(() => {
        const mapOptions = {
            zoom: 17,
            center: {
                lat: user.latitude,
                lng: user.longitude
            }
        };

        const mapa_element = document.getElementById('map');
        const map = new google.maps.Map(mapa_element, mapOptions);

        //Buscar una ubicación 1
        const search_input_1 = document.getElementById('search-place_1');
        const search_input_2 = document.getElementById('search-place_2');
        const autocomplete1 = new google.maps.places.Autocomplete(search_input_1);
        const autocomplete2 = new google.maps.places.Autocomplete(search_input_2);

        //Crear una chinche, marker 1
        var markerConfig1 = {
            map: map
        };
        const marker1 = new google.maps.Marker(markerConfig1);

        //Crear una chinche, marker 1
        var markerConfig2 = {
            map: map
        };
        const marker2 = new google.maps.Marker(markerConfig2);

        //enlazar el autocomplete con el mapa
        autocomplete1.bindTo("bounds", map);
        autocomplete2.bindTo("bounds", map);

        //Evento para obtener e lugar del input-search1
        google.maps.event.addListener(autocomplete1, "place_changed", () => {
            //console.log("Cambiamos de lugar");
            const place = autocomplete1.getPlace();
            place1LatLong = place.geometry.location;
            //console.log("DATOS PLACE 1 LAT" + place1LatLong.lat()+" Y LONG "+place1LatLong.lng());
            
            //localStorage.setItem("puntoA", JSON.stringify(place1LatLong));
            //Centrar el mapa en el lugar escogido en el input-search
            if(place.geometry.viewport)
            {                
                map.fitBounds(place.geometry.viewport);
            }
            else
            {                
                map.setCenter(place.geometry.location);
                map.zoom(17);
            }

            //marker1.setPlace({
            //    placeId: place.place_id,
            //    location: place.geometry.location
            //});
            //marker1.setVisible(true);

            
        });

        //Evento para obtener e lugar del input-search2
        google.maps.event.addListener(autocomplete2, "place_changed", () => {
            //console.log("Cambiamos de lugar");
            search_input_2.focus();

            const place2 = autocomplete2.getPlace();
            //Centrar el mapa en el lugar escogido en el input-search
            place2LatLong = place2.geometry.location;
            

            //localStorage.setItem("puntoB", JSON.stringify(place2LatLong));

            if (place2.geometry.viewport) {
                map.fitBounds(place2.geometry.viewport);
            }
            else {
                map.setCenter(place2.geometry.location);
                map.zoom(17);
            }

            //marker2.setPlace({
            //    placeId: place2.place_id,
            //    location: place2.geometry.location
            //});
            //marker2.setVisible(true);

            
            distanciaTiempo(place1LatLong.lat(), place1LatLong.lng(), place2LatLong.lat(), place2LatLong.lng());
            trazarRuta(map);
        });

        
    });
    
});


function distanciaTiempo(origen_lat, origen_lng, destino_lat, destino_lng) {
    //console.log("LAT ORIGEN: " + origen_lat + " LONG ORIGEN: " + origen_lng);
    //console.log("LAT DESTINO: " + destino_lat + " LONG DESTINO: " + destino_lng);

    var puntoA = new google.maps.LatLng(origen_lat, origen_lng);
    var puntoB = new google.maps.LatLng(destino_lat, destino_lng);
    var service = new google.maps.DistanceMatrixService();

    service.getDistanceMatrix({
        origins: [puntoA],
        destinations: [puntoB],
        travelMode: google.maps.TravelMode.DRIVING
    }, function (respuesta, status) {
        var info = respuesta.rows[0].elements[0];
        //console.log("INFO DOS PUNTOS FINAL: " + info);

        var distancia = info.distance.text;
        var duracion = info.duration.text;

        //console.log("DISTANCIA: " + distancia + " DURACION: " + duracion);

        $("#txtDistancia").val(distancia);
        $("#txtDuracion").val(duracion);
        //alert(distancia +" - "+ duracion);
    });
}

//$("#search-place_2").blur(function () {
//    $("#search-place_2").focus();
//    //distanciaTiempo(place1LatLong.lat(), place1LatLong.lng(), place2LatLong.lat(), place2LatLong.lng());
//});

//$("#search-place_2").blur(function () {
//    //console.log("LA DISTANCIA Y EL TIEMPO QUE SI SON: ");
//    distanciaTiempo(place1LatLong.lat(), place1LatLong.lng(), place2LatLong.lat(), place2LatLong.lng());
//    //alert("funcion");
//});
//function calcularDistancia(place,origen)
//{
//    var origin = new google.maps.LatLng(origen.latitude, origen.longitude);
//    // objeto para calcular la distancia
//    var service = new google.maps.DistanceMatrixService();
//    var configDistance = {
//        origins: [origin],
//        destinations: [place.geometry.location],
//        travelMode: google.maps.TravelMode.DRIVING
//    };
//    service.getDistanceMatrix(configDistance, (respuesta,status) => {
//        //Esta funcion se ejecuta cuando el servicio de distancia de Maps no responde
//        const info = respuesta.rows[0].elements[0];
//        //console.log(info);
//        const distancia = info.distance.text;
//        const duracion = info.duration.text;

//        //console.log("Distancia: "+distancia);
//        //console.log("Duracion: "+duracion);
//        document.getElementById('info').innerHTML = `
//            Estás a ${distancia} y tardas ${duracion} en llegar
//        `;
        
//    });    
//}

function trazarRuta(map)
{
    //como trazar una ruta
    var objConfigDR = {
        map: map
        //suppressMarkers: true
    };
    var origen = document.getElementById('search-place_1').value;//new google.maps.LatLng(lat, long);
    var destino = document.getElementById('search-place_2').value;
    //console.log("origen" + origen);
    //console.log("destino" + destino);

    var objConfigDS = {
        origin: origen,//gLatLong, //Lat o Long o String domicilio
        destination: destino,//objInformacion.address,//'Ingenio Madre Tierra,Santa Lucía Cotzumalguapa,Escuintla,Guatemala',
        travelMode: google.maps.TravelMode.DRIVING
        //tavelMode -> 
        //DRIVING = en carro , WALKING = caminando, TRANSIT = viajando en base a los servicios publicos de esa zona, BICYCLING = ir en bicicleta
    }
    var ds = new google.maps.DirectionsService(); //obtener coordenadas
    var dr = new google.maps.DirectionsRenderer(objConfigDR); //traduce coordenadas a la ruta visible

    ds.route(objConfigDS, fnRutear);

    function fnRutear(resultados, status) {
        //mostrar la linea entre A y B
        //console.log(resultados);
        if (status == 'OK') {
            dr.setDirections(resultados);
            resultados = "";
        }
        else
            alert("Hubo un error " + status);
    }
}
