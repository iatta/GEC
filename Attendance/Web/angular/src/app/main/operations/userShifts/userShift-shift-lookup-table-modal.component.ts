import { PagedResultDtoOfUserShiftShiftLookupTableDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { UserShiftsServiceProxy, UserShiftShiftLookupTableDto, GetShiftForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

@Component({
    selector: 'userShiftShiftLookupTableModal',
    styleUrls: ['./userShift-shift-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './userShift-shift-lookup-table-modal.component.html'
})
export class UserShiftShiftLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    selectedShiftIds:number[];
    selectedDay:string;
    selectedUserId:number;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    constructor(
        injector: Injector,
        private _userShiftsServiceProxy: UserShiftsServiceProxy
    ) {
        super(injector);
    }

    show(shiftIds:number[],selectedDay:string,selectedUserId:number): void {
        this.selectedDay = selectedDay;
        this.selectedUserId = selectedUserId;
        this.selectedShiftIds = shiftIds;
        this.active = true;
        this.paginator.rows = 5;
        this.getAll();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._userShiftsServiceProxy.getAllShiftForLookupTable(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
            this.modal.show();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave() {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
