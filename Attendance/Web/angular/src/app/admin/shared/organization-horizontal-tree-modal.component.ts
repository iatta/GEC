import { IBasicOrganizationUnitInfo } from './../organization-units/basic-organization-unit-info';
import { ModalDirective } from 'ngx-bootstrap';
import { Component, Injector, Input, ViewChild, EventEmitter, Output, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { OrganizationUnitDto, OrganizationUnitServiceProxy, ListResultDtoOfOrganizationUnitDto } from '@shared/service-proxies/service-proxies';
import { ArrayToTreeConverterService } from '@shared/utils/array-to-tree-converter.service';
import { TreeDataHelperService } from '@shared/utils/tree-data-helper.service';
import { TreeNode } from 'primeng/api';
import * as _ from 'lodash';
import { Tree } from 'primeng/primeng';

export interface IOrganizationUnitsHorizontalTreeModalComponentData {
    allOrganizationUnits: OrganizationUnitDto[];
    selectedOrganizationUnits: string[];
}

@Component({
    selector: 'organizationUnitsHorizontalTreeModal',
    templateUrl: './organization-horizontal-tree-modal.component.html'
})
export class OrganizationUnitsHorizontalTreeModalComponent extends AppComponentBase {

    @Input() cascadeSelectEnabled = true;
    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) modal: ModalDirective;
    @Output() ouSelected = new EventEmitter<IBasicOrganizationUnitInfo>();

    treeData: any;
    active = false;
    loaded = false;
    selectedNode: TreeNode;

    constructor(
        private _arrayToTreeConverterService: ArrayToTreeConverterService,
        private _treeDataHelperService: TreeDataHelperService,
        private _organizationUnitService: OrganizationUnitServiceProxy,
        injector: Injector
    ) {
        super(injector);
    }

    show(): void {
        this.getTreeDataFromServer();
        // this.modal.show();
    }
    private getTreeDataFromServer(): void {
        let self = this;
        this._organizationUnitService.getOrganizationUnits().subscribe((result: ListResultDtoOfOrganizationUnitDto) => {
            this.treeData = this._arrayToTreeConverterService.createTree(result.items,
                'parentId',
                'id',
                null,
                'children',
                [
                    {
                        target: 'label',
                        targetFunction(item) {
                            return item.displayName;
                        }
                    }, {
                        target: 'expandedIcon',
                        value: 'fa fa-folder-open m--font-warning'
                    },
                    {
                        target: 'collapsedIcon',
                        value: 'fa fa-folder m--font-warning'
                    },
                    {
                        target: 'selectable',
                        value: true
                    },
                    {
                        target: 'memberCount',
                        targetFunction(item) {
                            return item.memberCount;
                        }
                    },
                    {
                        target: 'roleCount',
                        targetFunction(item) {
                            return item.roleCount;
                        }
                    }
                ]);

                this.setSelectedNodes();
                this.modal.show();

        });
    }



    nodeSelect(event) {
        if (!this.cascadeSelectEnabled) {
            return;
        }

        let parentNode = this._treeDataHelperService.findParent(this.treeData, { data: { id: event.node.data.id } });

        while (parentNode != null) {
            parentNode = this._treeDataHelperService.findParent(this.treeData, { data: { id: parentNode.data.id } });
        }

        this.ouSelected.emit(<IBasicOrganizationUnitInfo>{
            id: event.node.data.id,
            displayName: event.node.data.displayName
        });

    }


    onNodeUnselect(event) {
        let childrenNodes = this._treeDataHelperService.findChildren(this.treeData, { data: { name: event.node.data.name } });
        childrenNodes.push(event.node.data.name);
        // _.remove(this.selectedOus, x => childrenNodes.indexOf(x.data.name) !== -1);
    }

    setSelectedNodes() {
        debugger
        console.log(this.selectedNode);
        if(this.selectedNode){

            let item = this._treeDataHelperService.findNode(this.treeData, { data: { code: this.selectedNode.data.code } });
            if (item) {
                this.selectedNode = item;
                if(this.selectedNode.data.parentId)
                    this.expandNode(this.selectedNode.data.parentId);


            }

        }

    }



    expandNode(parentId:number){
        this.treeData.forEach(node => {
                if(node.data.id == parentId)
                    node.expanded = true;

                    if(node.children.length > 0){
                        node.children.forEach(children => {
                            if(children.data.id == parentId){
                                children.expanded = true;
                                this.expandNode(children.data.parentId)
                            }
                        });
                    }

        });
    }


    close(): void {
        this.active = false;
        this.modal.hide();
        // this.modalSave.emit(null);
    }
}
