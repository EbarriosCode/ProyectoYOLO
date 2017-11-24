class UserLocation {
    constructor(callback) {
        if (navigator.geolocation) {
            //Si tienen la API de geolocalización
            navigator.geolocation.getCurrentPosition((localizacion)=> {
                //Obtener la localización del usuario
                //console.log(localizacion);
                //console.log("Latitude: " + localizacion.coords.latitude);
                //console.log("Longitude: " + localizacion.coords.longitude);
                this.latitude = localizacion.coords.latitude;
                this.longitude = localizacion.coords.longitude;
                callback();
            });
        }
        else {
            alert("Tu navegador no soporta las funcionalidades de esta página");
        }
    }
}