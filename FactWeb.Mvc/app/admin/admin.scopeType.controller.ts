module app.admin {
    'use strict';

    interface IScopeTypeScope {
        scopeTypeId : number;
        scopeName: string;
        importName: string;
        isArchived: boolean;
        saveScope(): void;
        deleteScope: (organizationFacilityId) => void;
        editScope: (rowData) => void;        
        saveMode: boolean;
        
    }

class ScopeTypeController implements IScopeTypeScope {
        scopeTypeId = 0;
        scopeName: string;
        importName: string;
        isArchived: boolean;
        results: Array<services.IScopeType>;
        saveMode: boolean;
        gridOptions = {
            sortable: true,
            filterable: {
                operators: {
                    string: {
                        contains: "Contains"
                    }
                }
            },
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            pageable: {
                pageSize: 10
            }
        };

        static $inject = [
            '$window',
            'scopeTypeService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private scopeTypeService: app.services.IScopeTypeService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.saveMode = false;
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Scope Type",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Scope Type', isActive: true }
                ]
            });

            common.activateController([this.getScopeTypes()], 'scopeTypeController');
        }

        getScopeTypes(): ng.IPromise<void> {
            return this.scopeTypeService.get()
                .then((data: Array<app.services.IScopeType>) => {                        
                        if (data == null) {
                            this.notificationFactory.error('no items');
                        } else {
                            this.results = data;
                            this.gridOptions.dataSource.data(data);
                        }
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        saveScope(): void {
            this.common.showSplash();            
            this.scopeTypeService.save(this.scopeTypeId, this.scopeName, this.importName, this.isArchived, true)
                .then((data: app.services.IGenericServiceResponse<services.IScopeType>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (data.item != null) {                                                        
                            this.saveMode = false;                                        
                            if (this.scopeTypeId == 0) {
                                this.results.push(data.item);
                                this.gridOptions.dataSource.data(this.results);
                                if (data.message != '' && data.message != null) {
                                    this.notificationFactory.success(data.message);
                                }
                            }
                            else {                           
                                this.getScopeTypes();
                            }
                            this.clearForm();

                        } else {
                            this.notificationFactory.error("Error.");
                        }
                    }
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                    this.common.hideSplash();
                });
        }

        deleteScope(rowData): void {
            var confirmation = confirm("Are you sure you want to delete this relation ?");
            if (confirmation) {
                this.common.showSplash();                
                this.scopeTypeService.delete(rowData.scopeTypeId)
                    .then((data: app.services.IGenericServiceResponse<services.IScopeType>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (data.item != null) {                                
                                this.results= this.results.filter(function (el) {
                                    return el.scopeTypeId !== data.item.scopeTypeId;
                                    });
                                this.gridOptions.dataSource.data(this.results);
                                this.notificationFactory.success(data.message);
                            } else {
                                this.notificationFactory.error("Error.");
                            }
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error.");
                    });
            }
        }

        editScope(rowData): void {            
            this.scopeName = rowData.name;
            this.importName = rowData.importName;
            this.scopeTypeId = rowData.scopeTypeId;
            this.isArchived = rowData.isArchived;
            this.saveMode = true;
        }

        cancel(): void {
            this.clearForm();
            this.saveMode = false;
        }

        clearForm(): void {
            this.scopeTypeId = 0;
            this.scopeName = "";
            this.importName = "";
        }
        
    }

    angular
        .module('app.admin')
        .controller('app.admin.ScopeTypeController',
        ScopeTypeController);
} 