import { LocationMachineMachineLookupTableModalComponent } from './../locationMachines/locationMachine-machine-lookup-table-modal.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, AfterViewInit, ElementRef, NgZone  } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { LocationsServiceProxy, CreateOrEditLocationDto, LocationCredentialDto, CreateOrEditLocationMachineDto, LocationMachineDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { MapLoaderService } from './map.loader';
declare var google: any;
// const input = document.getElementById("pac-input") as HTMLInputElement;
// const searchBox = new google.maps.places.SearchBox(input);
// let markers: google.maps.Marker[] = [];

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
    search:string;
    rectangle: any;
    @ViewChild('search',{static : true }) public searchElementRef: ElementRef;
    @ViewChild('locationMachineMachineLookupTableModal', { static: true }) locationMachineMachineLookupTableModal: LocationMachineMachineLookupTableModalComponent;
    locationMachine: CreateOrEditLocationMachineDto = new CreateOrEditLocationMachineDto();
    machineNameEn:string;
    location: CreateOrEditLocationDto = new CreateOrEditLocationDto();
    map: any;
    searchBox:any;
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
            this.location.machines = [];
            this.active = true;
            this.drawPolygon();

        } else {
            this._locationsServiceProxy.getLocationForEdit(locationId).subscribe(result => {
                this.location = result.location;
                if(!this.location.machines)
                    this.location.machines = [];

                this.active = true;
                this.drawPolygon();
            });
        }
    }

    ngAfterViewInit() {

    }

    drawPolygon() {
        MapLoaderService.load().then(()=>{
            this.map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 29.378586, lng:  47.990341 },
                zoom: 12
            });
            // this.searchBox = new google.maps.places.SearchBox(this.searchElementRef.nativeElement)
            // this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(this.searchElementRef.nativeElement);


              // Bias the SearchBox results towards current map's viewport.
            // this.map.addListener("bounds_changed", () => {
            //         this.searchBox.setBounds(this.map.getBounds() as google.maps.LatLngBounds);
            //     });

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
    openSelectMachineModal() {
        this.locationMachineMachineLookupTableModal.id = this.locationMachine.machineId;
        this.locationMachineMachineLookupTableModal.displayName = this.machineNameEn;
        this.locationMachineMachineLookupTableModal.show();
    }

    getNewMachineId() {
        if(this.isMachineExist(this.locationMachineMachineLookupTableModal.id)){
            this.message.warn("Machine Already Added To This Location");
        }else{
            if(this.locationMachineMachineLookupTableModal.id > 0){
                let locationMachineToAdd = new LocationMachineDto();
                locationMachineToAdd.machineId = this.locationMachineMachineLookupTableModal.id;
                locationMachineToAdd.machineName = this.locationMachineMachineLookupTableModal.displayName;
                this.location.machines.push(locationMachineToAdd);
            }

        }
    }

    isMachineExist(machineId:number){
        debugger
        if(this.location.machines.length  == 0)
            return false;

        let exist = this.location.machines.findIndex(x =>  x.machineId == machineId);

        return exist > -1;
    }


    removeMachine(machine: LocationMachineDto){
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {

                    let index = this.location.machines.indexOf(machine);
                    if(index > -1){
                        this.location.machines.splice(index,1);
                        this.notify.success(this.l('SuccessfullyDeleted'));
                    }

                }
            }
        );

    }

    close(): void {
        this.active = false;
        this._router.navigate(['app/main/setting/locations']);

    }
}
