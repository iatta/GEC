import { ActivatedRoute, Router, Params } from '@angular/router';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, AfterViewInit, ElementRef, NgZone  } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { LocationsServiceProxy, CreateOrEditLocationDto, LocationCredentialDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { MapLoaderService } from './map.loader';
declare var google: any;
@Component({
    selector: 'manageLocationModal',
    templateUrl: './manage-location.component.html'
})
export class ManageLocationComponent extends AppComponentBase implements OnInit{

    active = false;
    saving = false;
    latitude: number;
    longitude: number;
    zoom: number;
    address: string;
    private geoCoder;

    rectangle: any;
    @ViewChild('search',{static : true }) public searchElementRef: ElementRef;

    location: CreateOrEditLocationDto = new CreateOrEditLocationDto();
    map: any;
    drawingManager: any;
    selectedCredential:LocationCredentialDto = new LocationCredentialDto();
    selectedShape: any;

test:any;


    constructor(
        injector: Injector,
        private mapsAPILoader: MapsAPILoader,
        private _locationsServiceProxy: LocationsServiceProxy,
        private ngZone: NgZone,
        private _route: ActivatedRoute,
        private _router: Router,
    ) {
        super(injector);
    }

    drawPolygon() {
        MapLoaderService.load().then(()=>{
            this.map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 29.378586, lng:  47.990341 },
                zoom: 12
            });

            if(!this.location.id){
               this. populateDrawManager();
            }else{
                if(this.location.locationCredentials.length === 0)
                {
                    this.populateDrawManager();
                }else{
                    this.manuplateRecangle();
                }

            }

        });


  }

  populateDrawManager(){
    this.location.locationCredentials = [];

    this.drawingManager = new google.maps.drawing.DrawingManager({
      drawingMode: google.maps.drawing.OverlayType.POLYGON,
      drawingControl: true,
      drawingControlOptions: {
        position: google.maps.ControlPosition.TOP_CENTER,
        drawingModes: ['polygon']
      },
      markerOptions: {icon: 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png'},
      polygonOptions:{
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
            draggable: true,
            editable: true
        }
    });


   this.drawingManager.setMap(this.map);

    google.maps.event.addListener(this.drawingManager, 'overlaycomplete', (event) => {
      if (event.type === 'polygon') {
        let newShape = event.overlay;
        newShape.type = event.type;
        this.selectedShape = newShape;
        let ref = this;
        //this is the coordinate, you can assign it to a variable or pass into another function.
        this.addLocationCredential(event);

        google.maps.event.addListener(newShape, 'dragend', ()=>{
            this.updateCredentials();
        });


         google.maps.event.addListener(newShape, 'mouseout', ()=>{
            this.updateCredentials();
        });

        google.maps.event.addListener(newShape, 'mouseup', ()=>{
            this.updateCredentials();

        });



        this.drawingManager.setDrawingMode(null);
        this.drawingManager.setOptions({
            drawingControl : false,
            drawingControlOptions : {}
        });
        //Loading the drawn shape in the Map.
        this.drawingManager.setMap(this.map);
      }

    });
  }

  newLocationCredential(){
    this.drawPolygon();
  }

  updateCredentials(){
    this.location.locationCredentials = [];
    let paths = this.selectedShape.getPath();
    paths.forEach(path => {
        let newPolygonCredentialToAdd = new LocationCredentialDto();
        newPolygonCredentialToAdd.latitude = path.lat();
        newPolygonCredentialToAdd.longitude = path.lng();
        this.location.locationCredentials.push(newPolygonCredentialToAdd);
    });
  }


  addLocationCredential(event: any){
    let paths = event.overlay.getPath();
    paths.forEach(path => {
        let newPolygonCredentialToAdd = new LocationCredentialDto();
        newPolygonCredentialToAdd.latitude = path.lat();
        newPolygonCredentialToAdd.longitude = path.lng();
        this.location.locationCredentials.push(newPolygonCredentialToAdd);
    });
  }

  getAddress(latitude, longitude) {
    this.geoCoder.geocode({ 'location': { lat: latitude, lng: longitude } }, (results, status) => {
      console.log(results);
      console.log(status);
      if (status === 'OK') {
        if (results[0]) {
          this.zoom = 12;
          this.address = results[0].formatted_address;
        } else {
          window.alert('No results found');
        }
      } else {
        window.alert('Geocoder failed due to: ' + status);
      }

    });
  }


    ngOnInit(): void {
        this._route.params.subscribe((params: Params) => {
              this.show(params['id']);
         });
        this.show();
    }

    show(locationId?: number): void {

        if (!locationId) {
            this.location = new CreateOrEditLocationDto();
            this.location.id = locationId;
            this.active = true;
            this.drawPolygon();

        } else {
            this._locationsServiceProxy.getLocationForEdit(locationId).subscribe(result => {
                this.location = result.location;
                this.active = true;
                this.drawPolygon();
            });
        }
    }

    manuplateRecangle(){

        let paths = [];
        this.location.locationCredentials.forEach(crede => {

            paths.push({lat : parseFloat(crede.latitude.toString()) , lng :  parseFloat(crede.longitude.toString()) });
        });



        let polygon = new google.maps.Polygon({
            paths: paths,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
            draggable: true,
            editable: true
        });

        let bounds = new google.maps.LatLngBounds();
        polygon.getPath().forEach(function (path, index) {
            bounds.extend(path);
        });

        this.selectedShape = polygon;
        polygon.setMap(this.map);
         this.map.fitBounds(bounds);
        polygon.addListener('dragend', () => {
           this.updateCredentials();
        });

        polygon.addListener('mouseout', ()=>{
            this.updateCredentials();
        });

        polygon.addListener('mouseup', ()=>{
            this.updateCredentials();

        });

    }

    save(): void {
        this.saving = true;


        this._locationsServiceProxy.createOrEdit(this.location)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this._router.navigate(['app/main/setting/locations']);
            });
    }







    close(): void {
        this.active = false;
        this._router.navigate(['app/main/setting/locations']);

    }
}
