window.addEventListener("DOMContentLoaded",  function initMap()   
{   
    var coords = new google.maps.LatLng(51.108442, 17.039894);
    var mapOptions = {
      zoom: 10,
      center: coords,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
      disableDefaultUI: true,
      mapTypeControl: true,
      mapTypeControlOptions:  
     {  
        mapTypeIds: [  
            google.maps.MapTypeId.ROADMAP,  
            google.maps.MapTypeId.SATELLITE,  
            google.maps.MapTypeId.TERRAIN  
        ],
        style: google.maps.MapTypeControlStyle.DROPDOWN_MENU, //DROPDOWN_MENU,HORIZONTAL_BAR
    },
    scaleControl: true,  
    navigationControl: true,
    disableDoubleClickZoom: true,
    navigationControlOptions:  
    {  
        style: google.maps.NavigationControlStyle.SMALL // ANDROID, ZOOM_PAN, SMALL
    }
    };

    var mapa = new google.maps.Map(document.getElementById("map"), mapOptions); 

    var currentState = {
        activeMarker: null,
        activeMarkerListElement: null,
        activeInfoWindow: new google.maps.InfoWindow(),
    }
    currentState.activeInfoWindow.setContent('To tutaj');

    mapa.addListener('dblclick', function(e)
    {
        AddNewPosition(e, mapa, currentState)
    });

    document.getElementById("showButton").addEventListener("click", function()
    {
        if(!currentState.activeMarker)
        {
            return;
        }
        currentState.activeInfoWindow.open(mapa, currentState.activeMarker)
    });

    document.getElementById("deleteButton").addEventListener("click", function()
    {
        if(!currentState.activeMarker)
        {
            return;
        }
        currentState.activeMarkerListElement.remove();
        currentState.activeMarker.setMap(null);
    });

    function AddNewPosition(e, mapa, currentState)
    {
    
        var marker = new google.maps.Marker({
            position: e.latLng,
            map: mapa,
          });
    
          var liElement = document.createElement("li");
          liElement.innerHTML = e.latLng;
          liElement.classList.add("marker-list-element");
    
          //set marker to active, add active class
          liElement.addEventListener("click", function(){
            //state
            currentState.activeMarker = marker;
            currentState.activeMarkerListElement = liElement;
    
            //css
            for(var elem of document.getElementsByClassName("active-list-element"))
            {
                elem.classList.remove("active-list-element")
            }
            liElement.classList.add("active-list-element");
          });
    
          document.getElementsByTagName("ul")[0].appendChild(liElement)
    }
}) 

