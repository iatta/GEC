import { MachineUsersServiceProxy, ReadAllUsersOutput, Person, UploadMachineUserInput, MachineData, UserFlatDto, DownloadImageInput } from './../../../../shared/service-proxies/service-proxies';
import { style } from '@angular/animations';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MachinesServiceProxy, MachineDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditMachineModalComponent } from './create-or-edit-machine-modal.component';
import { ViewMachineModalComponent } from './view-machine-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './manage-machine-users.component.html',
    animations: [appModuleAnimation()],
    styles:[`
    .ui-dropdown .ui-dropdown-label{
        display: block !important;
    }`]
})


export class ManageMachineUsersComponent extends AppComponentBase implements OnInit {
    lodaing=false;
    machines:MachineDto[]=[];
    selectedDestinationMachine:MachineDto[]=[];
    selectedSourceMachine:MachineDto[]=[];
    selectedMachineToDelete:MachineDto;
    allUsersResponse:ReadAllUsersOutput;
    selectedUsers:Person[] = [];
    selectedUsersToDelete:Person[]=[];

    input:UploadMachineUserInput[]=[];
    personList:Person[]=[];
    personListToDelete:Person[]=[];
    users:UserFlatDto[]=[];
    selectedUser
    show=false;
    readAllUsersOutput:ReadAllUsersOutput[]=[]
    momentDate:moment.Moment = moment();
    constructor(
        injector: Injector,
        private _machinesServiceProxy : MachinesServiceProxy,
        private _userServiceProxy : UserServiceProxy,
        private _machineUsersServiceProxy: MachineUsersServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.lodaing = true;
       this._machinesServiceProxy.getAllFlat().subscribe((result)=>{
        this.machines = result;
        this._userServiceProxy.getUsersFlat().subscribe((result)=>{
            this.users = result;
            this.show = true;
            this.lodaing = false;
        })

       });

    }

    loadUsers(){
        this.lodaing = true;
        this.personList=[];
        let input:UploadMachineUserInput[] = [];

        this.selectedSourceMachine.forEach(machine => {
            let uploadMachineUserInput = new UploadMachineUserInput();
            uploadMachineUserInput.person= new Person();
            uploadMachineUserInput.person.userCode = 11;
            uploadMachineUserInput.machineData = new MachineData();
            
            uploadMachineUserInput.machineData.ip = machine.ipAddress;
            uploadMachineUserInput.machineData.port = machine.port;
            uploadMachineUserInput.machineData.sn = machine.subNet;
            input.push(uploadMachineUserInput);
        });
        this._machineUsersServiceProxy.readAllUsers(input).subscribe((result)=>{
            result.forEach(res => {
                res.personList.forEach(person => {
                    this.personList.push(person);
                });
                this.lodaing = false;
            });

         })
    }

    loadUsersToDelete(){
        this.lodaing = true;
        this.personListToDelete=[];
        let input:UploadMachineUserInput[] = [];

       
            let uploadMachineUserInput = new UploadMachineUserInput();
            uploadMachineUserInput.person= new Person();
            uploadMachineUserInput.person.userCode = 11;
            uploadMachineUserInput.machineData = new MachineData();
            
            uploadMachineUserInput.machineData.ip = this.selectedMachineToDelete.ipAddress;
            uploadMachineUserInput.machineData.port = this.selectedMachineToDelete.port;
            uploadMachineUserInput.machineData.sn = this.selectedMachineToDelete.subNet;
            input.push(uploadMachineUserInput);
     
        this._machineUsersServiceProxy.readAllUsers(input).subscribe((result)=>{
            result.forEach(res => {
                res.personList.forEach(person => {
                    this.personListToDelete.push(person);
                });

            });
            this.lodaing = false;
         })
    }

    deleteUsers(){
        if(this.selectedUsersToDelete.length == 0)
            return this.notify.error("Please Select users ");

        this.lodaing = true;
        let input:DownloadImageInput[]=[];
        this.selectedUsersToDelete.forEach(user => {
            let deleteUserInput = new DownloadImageInput();
            deleteUserInput.userCode = user.userCode;
            deleteUserInput.machineData = new MachineData();

            deleteUserInput.machineData.ip = this.selectedMachineToDelete.ipAddress;
            deleteUserInput.machineData.port = this.selectedMachineToDelete.port;
            deleteUserInput.machineData.sn = this.selectedMachineToDelete.subNet;

            input.push(deleteUserInput);
        });

        this._machineUsersServiceProxy.deleteUser(input).subscribe(result=>{
            console.log(result);
            this.loadUsersToDelete();
            this.lodaing = false;
        });
    }

    moveUsers(){
        if(this.selectedUsers.length == 0)
            return this.notify.error("Please Select users ");

            this.lodaing =true;

        this.selectedUsers.forEach(user => {
            this.selectedDestinationMachine.forEach(machine => {
                let uploadMachineUserInput = new UploadMachineUserInput();
            uploadMachineUserInput.person = new Person();
            uploadMachineUserInput.machineData = new MachineData();
            uploadMachineUserInput.person.userCode = user.userCode;
            uploadMachineUserInput.person.pName = user.pName;
            uploadMachineUserInput.machineData.ip = machine.ipAddress;
            uploadMachineUserInput.machineData.port = machine.port;
            uploadMachineUserInput.machineData.sn = machine.subNet;
            this.input.push(uploadMachineUserInput);
            });
        });
        this._machineUsersServiceProxy.uploadUsers(this.input).subscribe((result)=>{
            console.log(result);

            this.lodaing =false;
            this.loadUsers();
        })
    }
}
