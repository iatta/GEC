import { Component, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { OrganizationUnitDto } from '@shared/service-proxies/service-proxies';
import { ArrayToTreeConverterService } from '@shared/utils/array-to-tree-converter.service';
import { TreeDataHelperService } from '@shared/utils/tree-data-helper.service';
import { TreeNode } from 'primeng/api';
import * as _ from 'lodash';

export interface IOrganizationUnitsTreeComponentData {
    allOrganizationUnits: OrganizationUnitDto[];
    selectedOrganizationUnits: string[];
    selectedOrganizationUnit: string;
}

@Component({
    selector: 'organization-unit-tree',
    templateUrl : './organization-unit-tree.component.html'
})
export class OrganizationUnitsTreeComponent extends AppComponentBase {

    @Input() cascadeSelectEnabled = true;

    set data(data: IOrganizationUnitsTreeComponentData) {
        this.setTreeData(data.allOrganizationUnits);
        this.setSelectedNodes(data.selectedOrganizationUnit);

        this._allOrganizationUnits = data.allOrganizationUnits;
        this._selectedOrganizationUnits = data.selectedOrganizationUnits;
    }

    treeData: any;
    selectedOus: TreeNode;

    private _allOrganizationUnits: OrganizationUnitDto[];
    private _selectedOrganizationUnits: string[];

    filter = '';

    constructor(
        private _arrayToTreeConverterService: ArrayToTreeConverterService,
        private _treeDataHelperService: TreeDataHelperService,
        injector: Injector
    ) {
        super(injector);
    }

    setTreeData(organizationUnits: OrganizationUnitDto[]) {
        this.treeData = this._arrayToTreeConverterService.createTree(organizationUnits, 'parentId', 'id', null, 'children',
            [{
                target: 'label',
                source: 'displayName'
            }, {
                target: 'expandedIcon',
                value: 'fa fa-folder-open m--font-warning'
            },
            {
                target: 'collapsedIcon',
                value: 'fa fa-folder m--font-warning'
            },
            {
                target: 'expanded',
                value: true
            }]);

    }

    // setSelectedNodes(selectedOrganizationUnits: string[]) {
    //     _.forEach(selectedOrganizationUnits, ou => {
    //         let item = this._treeDataHelperService.findNode(this.treeData, { data: { code: ou } });
    //         if (item) {
    //             this.selectedOus = item;
    //         }
    //     });
    // }

    setSelectedNodes(selectedOrganizationUnit: string) {

            let item = this._treeDataHelperService.findNode(this.treeData, { data: { code: selectedOrganizationUnit } });
            if (item) {
                this.selectedOus = item;
            }

        if(this.selectedOus){

            let item = this._treeDataHelperService.findNode(this.treeData, { data: { code: this.selectedOus.data.code } });
            if (item) {
                this.selectedOus = item;
                if(this.selectedOus.data.parentId)
                    this.expandNode(this.selectedOus.data.parentId);
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


    getSelectedOrganizations(): number {
        if (!this.selectedOus) {
            return null;
        }
        let organizationId = this.selectedOus.data.id;
        return organizationId;
    }

    filterOrganizationUnit(nodes, filterText): any {
        _.forEach(nodes, node => {
            if (node.data.displayName.toLowerCase().indexOf(filterText.toLowerCase()) >= 0) {
                node.styleClass =
                    this.showParentNodes(node);
            } else {
                node.styleClass = 'hidden-tree-node';
            }

            if (node.children) {
                this.filterOrganizationUnit(node.children, filterText);
            }
        });
    }

    showParentNodes(node): void {
        if (!node.parent) {
            return;
        }

        node.parent.styleClass = '';
        this.showParentNodes(node.parent);
    }

    filterOrganizationUnits(event): void {
        this.filterOrganizationUnit(this.treeData, this.filter);
    }

    nodeSelect(event) {

        // if (!this.cascadeSelectEnabled) {
        //     return;
        // }

        // let parentNode = this._treeDataHelperService.findParent(this.treeData, { data: { id: event.node.data.id } });

        // while (parentNode != null) {
        //     this.selectedOus.push(parentNode);
        //     parentNode = this._treeDataHelperService.findParent(this.treeData, { data: { id: parentNode.data.id } });
        // }
    }

    onNodeUnselect(event) {
        // let childrenNodes = this._treeDataHelperService.findChildren(this.treeData, { data: { name: event.node.data.name } });
        // childrenNodes.push(event.node.data.name);
        // _.remove(this.selectedOus, x => childrenNodes.indexOf(x.data.name) !== -1);
    }
}