module app.modal.templates {
    'use strict';

    interface IValues {
        site: services.ISite;
        siteName: string;
    }

    class SiteTotalsController {
        isBusy = false;
        isCordBlood = true;
        cbUnitTypes: Array<services.ICBUnitType>;
        cbCategories: Array<services.ICbCategory>;
        transplantTypes: Array<services.ITransplantType>;
        transplantCellTypes: Array<services.ITransplantCellType>;
        clinicalPopulationTypes: Array<services.IClinicalPopulationType>;
        processingTypes: Array<services.IProcessingType>;
        collectionTypes: Array<services.ICollectionType>;
        editingCbRow: services.ISiteCordBloodTransplantTotal;
        editingCtTransplantRow: services.ISiteTransplantTotal;
        editingCollectionRow: services.ISiteCollectionTotal;
        editingProcessingRow: services.ISiteProcessingTotal;
        editingMethodologyRow: services.ISiteProcessingMethodologyTotal;
        selectedItem: any;
        cbGrid: any;
        transplantGrid: any;
        collectionGrid: any;
        processingGrid: any;
        methodologyGrid: any;
        selected: any;
        transplantMinDate: Date;
        transplantMaxDate: Date;
        collectionMinDate: Date;
        collectionMaxDate: Date;
        processingMinDate: Date;
        processingMaxDate: Date;
        methodologyMinDate: Date;
        methodologyMaxDate: Date;
        site: services.ISite;

        cordbloodGrid = {
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "cbUnitType.name", "title": "Unit Type" },
                { field: "cbCategory.name", "title": "Category" },
                { field: "numberOfUnits", "title": "Number of Units" },
                { field: "asOfDate", "title": "Date" },
                { template: "<button class=\"k-button\" ng-click=\"vm.onDeleteTotal(dataItem, 'CB')\">Delete</button>" }
            ],
            pageable: {
                pageSize: 10
            },
            change: (e) => {
                this.onCbEdit(e);
            }
        };
        transplantOptions = {
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "transplantCellType.name", "title": "Cell Type" },
                { field: "clinicalPopulationType.name", "title": "Population" },
                { field: "transplantType.name", "title": "Transplant Type" },
                { field: "isHaploid", "title": "Haploid" },
                { field: "numberOfUnits", "title": "Number of Units" },
                { field: "startDate", "title": "Start Date" },
                { field: "endDate", "title": "End Date" },
                { template: "<button class=\"k-button\" ng-click=\"vm.onDeleteTotal(dataItem, 'TO')\">Delete</button>" }
            ],
            pageable: {
                pageSize: 10
            },
            change: (e) => {
                this.onTransplantEdit(e);
            }
        };
        collectionOptions = {
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "collectionType.name", "title": "Collection Type" },
                { field: "clinicalPopulationType.name", "title": "Population" },
                { field: "numberOfUnits", "title": "Number of Units" },
                { field: "startDate", "title": "Start Date" },
                { field: "endDate", "title": "End Date" },
                { template: "<button class=\"k-button\" ng-click=\"vm.onDeleteTotal(dataItem, 'CO')\">Delete</button>" }
            ],
            pageable: {
                pageSize: 10
            },
            change: (e) => {
                this.onCollectionEdit(e);
            }
        };
        processingOptions = {
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "types", "title": "Cell Types" },
                { field: "numberOfUnits", "title": "Number of Units" },
                { field: "startDate", "title": "Start Date" },
                { field: "endDate", "title": "End Date" },
                { template: "<button class=\"k-button\" ng-click=\"vm.onDeleteTotal(dataItem, 'PO')\">Delete</button>" }
            ],
            pageable: {
                pageSize: 10
            },
            change: (e) => {
                this.onProcessingEdit(e);
            }
        };
        methodologyOptions = {
            selectable: "row",
            dataSource: new kendo.data.DataSource({
                data: [],
                pageSize: 10
            }),
            columns: [
                { field: "processingType.name", "title": "Processing Type" },
                { field: "platformCount", "title": "Number of Platforms" },
                { field: "protocolCount", "title": "Number of Protocols" },
                { field: "startDate", "title": "Start Date" },
                { field: "endDate", "title": "End Date" },
                { template: "<button class=\"k-button\" ng-click=\"vm.onDeleteTotal(dataItem, 'MO')\">Delete</button>" }
            ],
            pageable: {
                pageSize: 10
            },
            change: (e) => {
                this.onMethodologyEdit(e);
            }
        };

        static $inject = [
            '$scope',
            '$q',
            'cacheService',
            'siteService',
            'notificationFactory',
            'common',
            'config',
            '$uibModalInstance',
            'values'
        ];

        constructor(
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private cacheService: services.ICacheService,
            private siteService: services.ISiteService,
            private notificationFactory: app.blocks.INotificationFactory,
            private common: app.common.ICommonFactory,
            private config: IConfig,
            private $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
            private values: IValues) {

            if (!values.site && values.siteName) {
                this.getSite();
            } else {
                this.site = values.site;
                this.loadData();
            }
                      
        }

        loadData() {
            _.each(this.site.siteCordBloodTransplantTotals, (total) => {
                total.asOfDateUnformatted = moment(total.asOfDate);
            });

            _.each(this.site.siteProcessingTotals, (processing) => {
                processing.types = "";

                _.each(processing.siteProcessingTotalTransplantCellTypes, (celltype) => {
                    processing.types += celltype.transplantCellType.name + ", ";
                });
                if (processing.types.length > 0) {
                    processing.types = processing.types.substr(0, processing.types.length - 2);
                }
            });


            this.cordbloodGrid.dataSource.data(this.site.siteCordBloodTransplantTotals);
            this.transplantOptions.dataSource.data(this.site.siteTransplantTotals);
            this.collectionOptions.dataSource.data(this.site.siteCollectionTotals);
            this.processingOptions.dataSource.data(this.site.siteProcessingTotals);
            this.methodologyOptions.dataSource.data(this.site.siteProcessingMethodologyTotals);

            if (this.site.facilities && this.site.facilities.length > 0) {
                this.isCordBlood = this.site.facilities[0].serviceType && this.site.facilities[0].serviceType.name.indexOf("CB") > -1;
            }

            this.common.activateController([
                this.getCbUnitTypes(),
                this.getCbCategories(),
                this.getTransplantTypes(),
                this.getTransplantCellTypes(),
                this.getClinicalPopulationTypes(),
                this.getProcessingTypes(),
                this.getCollectionTypes()], '');  
        }

        getSite() {

            this.common.showSplash();
            this.siteService.getByName(this.values.siteName)
                .then((data: services.ISite) => {
                    this.site = data;

                    this.common.hideSplash();
                    this.loadData();
                });
        }

        getCbUnitTypes(): ng.IPromise<void> {
            return this.cacheService.getCbUnitTypes()
                .then((data) => {
                    this.cbUnitTypes = data;
                });
        }

        getCbCategories(): ng.IPromise<void> {
            return this.cacheService.getCbCategories()
                .then((data) => {
                    this.cbCategories = data;
                });
        }

        getTransplantTypes(): ng.IPromise<void> {
            return this.cacheService.getTransplantTypes()
                .then((data) => {
                    this.transplantTypes = data;
                });
        }

        getTransplantCellTypes(): ng.IPromise<void> {
            return this.cacheService.getTransplantCellTypes()
                .then((data) => {
                    this.transplantCellTypes = data;
                });
        }

        getClinicalPopulationTypes(): ng.IPromise<void> {
            return this.cacheService.getClinicalPopulationTypes()
                .then((data) => {
                    this.clinicalPopulationTypes = data;
                });
        }

        getProcessingTypes(): ng.IPromise<void> {
            return this.cacheService.getProcessingTypes()
                .then((data) => {
                    this.processingTypes = data;
                });
        }

        getCollectionTypes(): ng.IPromise<void> {
            return this.cacheService.getCollectionTypes()
                .then((data) => {
                    this.collectionTypes = data;
                });
        }

        onCbEdit(event) {
            var grid = event.sender;
            this.selected = grid.select();
            this.selectedItem = grid.dataItem(this.selected);
            this.editingCbRow = this.selectedItem;
            this.$scope.$apply();
        }

        onTransplantEdit(event) {
            var grid = event.sender;
            this.selected = grid.select();
            this.selectedItem = grid.dataItem(this.selected);
            this.editingCtTransplantRow = this.selectedItem;
            this.transplantMinDate = new Date(this.editingCtTransplantRow.startDate);
            this.transplantMaxDate = new Date(this.editingCtTransplantRow.endDate);
            this.$scope.$apply();
        }

        onCollectionEdit(event) {
            var grid = event.sender;
            this.selected = grid.select();
            this.selectedItem = grid.dataItem(this.selected);
            this.editingCollectionRow = this.selectedItem;
            this.collectionMinDate = new Date(this.editingCollectionRow.startDate);
            this.collectionMaxDate = new Date(this.editingCollectionRow.endDate);
            this.$scope.$apply();
        }

        onProcessingEdit(event) {
            var grid = event.sender;
            this.selected = grid.select();
            this.selectedItem = grid.dataItem(this.selected);
            this.editingProcessingRow = this.selectedItem;
            if (this.editingProcessingRow && this.editingProcessingRow.startDate) {
                this.processingMinDate = new Date(this.editingProcessingRow.startDate);
            }

            if (this.editingProcessingRow && this.editingProcessingRow.endDate) {
                this.processingMaxDate = new Date(this.editingProcessingRow.endDate);
            }

            if (this.editingProcessingRow && this.editingProcessingRow.siteProcessingTotalTransplantCellTypes) {
                if (!this.editingProcessingRow.selectedTypes) { this.editingProcessingRow.selectedTypes = []; }
                _.each(this.editingProcessingRow.siteProcessingTotalTransplantCellTypes, (cellType) => {
                    this.editingProcessingRow.selectedTypes.push(cellType.transplantCellType.name);
                });
            }
            

            //this.editingProcessingRow.selectedTypes = [this.editingProcessingRow.transplantCellType.name];
            this.$scope.$apply();
        }

        onMethodologyEdit(event) {
            var grid = event.sender;
            this.selected = grid.select();
            this.selectedItem = grid.dataItem(this.selected);
            this.editingMethodologyRow = this.selectedItem;
            this.methodologyMinDate = new Date(this.editingMethodologyRow.startDate);
            this.methodologyMaxDate = new Date(this.editingMethodologyRow.endDate);
            this.$scope.$apply();
        }

        onFromDateChanged(type: string) {
            switch (type) {
                case "transplant":
                    this.transplantMinDate = new Date(this.editingCtTransplantRow.startDate);
                    break;
                case "collection":
                    this.collectionMinDate = new Date(this.editingCollectionRow.startDate);
                    break;
                case "processing":
                    this.processingMinDate = new Date(this.editingProcessingRow.startDate);
                    break;
                case "methodology":
                    this.methodologyMinDate = new Date(this.editingMethodologyRow.startDate);
                    break;
            }
        }

        onToDateChanged(type: string) {
            switch (type) {
                case "transplant":
                    this.transplantMaxDate = new Date(this.editingCtTransplantRow.endDate);
                    break;
                case "collection":
                    this.collectionMaxDate = new Date(this.editingCollectionRow.endDate);
                    break;
                case "processing":
                    this.processingMaxDate = new Date(this.editingProcessingRow.endDate);
                    break;
                case "methodology":
                    this.methodologyMaxDate = new Date(this.editingMethodologyRow.endDate);
                    break;
            }
        }

        onAdd() {
            this.editingCbRow = {
                siteId: this.site.siteId,
                cbUnitType: {
                    id: 0,
                    name: ""
                },
                cbCategory: {
                    id: 0,
                    name: ""
                },
                asOfDate: ""
            };
        }

        onAddTransplant() {
            this.editingCtTransplantRow = {
                siteId: this.site.siteId,
                transplantCellType: { id: "", name: "" },
                clinicalPopulationType: { id: 0, name: "" },
                transplantType: { id: 0, name: "" },
                isHaploid: false,
                startDate: "",
                endDate: ""
            }
        }

        onAddCollection() {
            this.editingCollectionRow = {
                siteId: this.site.siteId,
                collectionType: { id: "", name: "" },
                clinicalPopulationType: { id: 0, name: "" },
                startDate: "",
                endDate: ""
            };
        }

        onAddProcessing() {
            this.editingProcessingRow = {
                siteId: this.site.siteId,
                siteProcessingTotalTransplantCellTypes: [],
                selectedTypes: [],
                startDate: "",
                endDate: ""
            };
        }

        onAddMethodology() {
            this.editingMethodologyRow = {
                siteId: this.site.siteId,
                processingType: { id: 0, name: "" },
                startDate: "",
                endDate: ""
            };
        }

        onCbSave() {
            this.isBusy = true;

            if (this.isCordBlood) {
                this.siteService.saveCordBloodTotals(this.editingCbRow)
                .then((data: services.IGenericServiceResponse<services.ISiteCordBloodTransplantTotal>) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        if (!this.editingCbRow.id) {
                            this.site.siteCordBloodTransplantTotals.push(data.item);
                            this.cordbloodGrid.dataSource.data(this.site.siteCordBloodTransplantTotals);
                        }
                        this.onCbCancel();
                        
                        this.notificationFactory.success("Total saved successfully");
                    }

                    this.isBusy = false;
                })
                .catch(() => {
                    this.notificationFactory.error("Error trying to save. Please contact support.");
                    this.isBusy = false;
                });
            }
        }

        onCbCancel() {
            this.editingCbRow = null;
            setTimeout(() => {
                this.cbGrid.clearSelection();
            }, 100);
        }

        onTransplantCancel() {
            this.editingCtTransplantRow = null;
            setTimeout(() => {
                this.transplantGrid.clearSelection();
            }, 100);
        }

        onDeleteTotal(item, type: string) {
            console.log(item, type);

            if (!confirm("Are you sure you want to delete this record?")) {
                return;
            }

            this.common.showSplash();

            this.siteService.deleteTotal(item.id, type)
                .then(data => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        this.notificationFactory.success('Record deleted successfully.');

                        switch (type) {
                            case "CB":
                                for (let i = 0; i < this.site.siteCordBloodTransplantTotals.length; i++)
                                {
                                    if (this.site.siteCordBloodTransplantTotals[i].id === item.id) {
                                        this.site.siteCordBloodTransplantTotals.splice(i, 1);
                                        break;
                                    }
                                }

                                this.cordbloodGrid.dataSource.data(this.site.siteCordBloodTransplantTotals);
                                break;
                            case "TO":
                                for (let i = 0; i < this.site.siteTransplantTotals.length; i++) {
                                    if (this.site.siteTransplantTotals[i].id === item.id) {
                                        this.site.siteTransplantTotals.splice(i, 1);
                                        break;
                                    }
                                }
                                
                                this.transplantOptions.dataSource.data(this.site.siteTransplantTotals);
                                break;
                            case "CO":
                                for (let i = 0; i < this.site.siteCollectionTotals.length; i++) {
                                    if (this.site.siteCollectionTotals[i].id === item.id) {
                                        this.site.siteCollectionTotals.splice(i, 1);
                                        break;
                                    }
                                }

                                this.collectionOptions.dataSource.data(this.site.siteCollectionTotals);
                                break;
                            case "PO":
                                for (let i = 0; i < this.site.siteProcessingTotals.length; i++) {
                                    if (this.site.siteProcessingTotals[i].id === item.id) {
                                        this.site.siteProcessingTotals.splice(i, 1);
                                        break;
                                    }
                                }

                                this.processingOptions.dataSource.data(this.site.siteProcessingTotals);
                                break;
                            case "MO":
                                for (let i = 0; i < this.site.siteProcessingMethodologyTotals.length; i++) {
                                    if (this.site.siteProcessingMethodologyTotals[i].id === item.id) {
                                        this.site.siteProcessingMethodologyTotals.splice(i, 1);
                                        break;
                                    }
                                }

                                this.methodologyOptions.dataSource.data(this.site.siteProcessingMethodologyTotals);
                                break;
                        }
                    }

                    this.common.hideSplash();
                })
                .catch(e => {
                    this.notificationFactory.error('Error saving data. ' + e);
                    this.common.hideSplash();
                });
        }

        onTransplantSave() {
            this.isBusy = true;

            if (!this.isCordBlood) {
                this.siteService.saveTransplantTotals(this.editingCtTransplantRow)
                    .then((data: services.IGenericServiceResponse<services.ISiteTransplantTotal>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (!this.editingCtTransplantRow.id) {
                                this.site.siteTransplantTotals.push(data.item);
                                this.transplantOptions.dataSource.data(this.site.siteTransplantTotals);
                            }
                            this.onTransplantCancel();

                            this.notificationFactory.success("Total saved successfully");
                        }

                        this.isBusy = false;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to save. Please contact support.");
                        this.isBusy = false;
                    });
            }
        }

        onCollectionCancel() {
            this.editingCollectionRow = null;
            setTimeout(() => {
                this.collectionGrid.clearSelection();
            }, 100);
        }

        onCollectionSave() {
            this.isBusy = true;

            if (!this.isCordBlood) {
                this.siteService.saveCollectionTotals(this.editingCollectionRow)
                    .then((data: services.IGenericServiceResponse<services.ISiteCollectionTotal>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (!this.editingCollectionRow.id) {
                                this.site.siteCollectionTotals.push(data.item);
                                this.collectionOptions.dataSource.data(this.site.siteCollectionTotals);
                            }
                            this.onCollectionCancel();

                            this.notificationFactory.success("Total saved successfully");
                        }

                        this.isBusy = false;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to save. Please contact support.");
                        this.isBusy = false;
                    });
            }
        }

        onProcessingCancel() {
            this.editingProcessingRow = null;
            setTimeout(() => {
                this.processingGrid.clearSelection();
            }, 100);
        }

        onProcessingSave() {
            this.isBusy = true;

            if (!this.isCordBlood) {
                this.siteService.saveProcessingTotals(this.editingProcessingRow)
                    .then((data: services.IGenericServiceResponse<services.ISiteProcessingTotal>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (!this.editingProcessingRow.id) {
                                data.item.types = "";
                                _.each(data.item.siteProcessingTotalTransplantCellTypes, (cellType) => {
                                    data.item.types += cellType.transplantCellType.name + ", ";
                                });
                                if (data.item.types.length > 0) {
                                    data.item.types = data.item.types.substr(0, data.item.types.length - 2);
                                }

                                this.site.siteProcessingTotals.push(data.item);

                                this.processingOptions.dataSource.data(this.site.siteProcessingTotals);
                            }
                            this.onProcessingCancel();

                            this.notificationFactory.success("Total saved successfully");
                        }

                        this.isBusy = false;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to save. Please contact support.");
                        this.isBusy = false;
                    });
                
            }
        }

        onMethodologyCancel() {
            this.editingMethodologyRow = null;
            setTimeout(() => {
                this.methodologyGrid.clearSelection();
            }, 100);
        }

        onMethodologySave() {
            this.isBusy = true;

            if (!this.isCordBlood) {
                this.siteService.saveMethodologyTotals(this.editingMethodologyRow)
                    .then((data: services.IGenericServiceResponse<services.ISiteProcessingMethodologyTotal>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            if (!this.editingMethodologyRow.id) {
                                this.site.siteProcessingMethodologyTotals.push(data.item);
                                this.methodologyOptions.dataSource.data(this.site.siteProcessingMethodologyTotals);
                            }
                            this.onMethodologyCancel();

                            this.notificationFactory.success("Total saved successfully");
                        }

                        this.isBusy = false;
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error trying to save. Please contact support.");
                        this.isBusy = false;
                    });
            }
        }

        onClose(): void {
            this.$uibModalInstance.close(this.site);
        }
        
    }

    angular
        .module('app.modal.templates')
        .controller('app.modal.templates.SiteTotalsController',
        SiteTotalsController);
} 