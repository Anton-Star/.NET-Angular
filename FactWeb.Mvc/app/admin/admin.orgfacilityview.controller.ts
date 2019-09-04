module app.admin {
    'use strict';

    interface IRelationType {
        name: string;
        title: string;
    }


    interface IOrganizationFacilityScope {
        selectedOrganization: string;
        selectedFacility: string;
        results: Array<services.IOrganization>;
        search(): void;        
    }

    class OrgFacilityViewController implements IOrganizationFacilityScope {

        organizations: Array<services.IOrganization>;
        facilities: Array<services.IFacility>;
        selectedOrganization = "";
        selectedFacility = "";
        results: services.IOrganization[];
        data = [{}];
        options = {
            zoom: .6,
            dataSource: new kendo.data.HierarchicalDataSource({
                data: this.results,
                schema: {
                    model: {
                        children: "facilities"
                    }
                }
            }),
            layout: {
                type: "tree",
                subtype: "down",
                width: 800
            },
            shapeDefaults: {
                visual: this.visualTemplate,
                content: {
                    fontSize: 12
                }
            },
            connectionDefaults: {
                stroke: {
                    color: "#979797",
                    width: 2
                }
            }
        };
        

        static $inject = [
            '$window',
            '$scope',
            'cacheService',
            'organizationService',
            'facilityService',
            'notificationFactory',
            'currentUser',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private $scope: ng.IScope,
            private cacheService: services.ICacheService,
            private organizationService: services.IOrganizationService,
            private facilityService: services.IFacilityService,
            private notificationFactory: app.blocks.INotificationFactory,
            private currentUser: app.services.IUser,
            private common: app.common.ICommonFactory,
            private config: IConfig) {
            this.results = [];

            common.activateController([this.getOrganizations(), this.getFacilities()], 'OrgFacilityViewController');
        }

        getOrganizations(): ng.IPromise<void> {

            return this.organizationService.getFlatOrganizations()
                .then((data: Array<services.IOrganization>) => {
                    this.organizations = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting organizations. Please contact support.");
                });
        }

        getFacilities(): ng.IPromise<void> {
            return this.facilityService.getAllFlat()
                .then((data: Array<services.IFacility>) => {
                    this.facilities = data;
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting facilities. Please contact support.");
                });
        }

        savePdf(): void {
            var diagram: any = $("#diagram");
            diagram.getKendoDiagram().saveAsPDF();
        }

        search(): ng.IPromise<void> {
            this.common.showSplash();
            var org = _.find(this.organizations, (o: services.IOrganization) => {
                return o.organizationName.trim() === this.selectedOrganization.trim(); 
            });

            var fac = _.find(this.facilities, (f: services.IFacility) => {
                return f.facilityName === this.selectedFacility;
            });

            return this.organizationService.searchByOrgFacility((org ? org.organizationId : null), fac ? fac.facilityId : null)
                .then((data: Array<services.IOrganization>) => {
                    this.results = data;
                    this.options.dataSource.data(data);
                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error getting facilities. Please contact support.");
                    this.common.hideSplash();
                });
        }

        clearForm(){
            this.selectedOrganization = "";
            this.selectedFacility = "";
            this.results = new Array<services.IOrganization>();
            this.options.dataSource.data(new Array<services.IOrganization>());
        }

        visualTemplate(options): kendo.dataviz.diagram.Group {
            var dataviz = kendo.dataviz;
            var g = new dataviz.diagram.Group();
            var dataItem = options.dataItem;

            var text = dataItem.facilityName || dataItem.organizationName;
            //text = text ? text.length > 40 ? text.substring(0, 40) : text : "";
            var width = (7.5 * text.length) + 10;
            if (width < 300) width = 300;


            g.append(new dataviz.diagram.Rectangle({
                width: width,
                height: 50,
                stroke: {
                    width: 1
                },
                fill: dataItem.colorScheme
            }));
            try {
                g.append(new dataviz.diagram.TextBlock({
                    text: text,
                    x: 20,
                    y: 20
                    //fill: "#fff"
                }));
            } catch(err) {
            }

            if (dataItem.relation) {
                g.append(new dataviz.diagram.Rectangle({
                    width: 10,
                    height: 50,
                    fill: dataItem.relation === "Strong" ? "#ff0000" : "#00ff00",
                    stroke: {
                        width: 0
                    }
                }));
            }

            return g;
        }
        
    }

    angular
        .module('app.admin')
        .controller('app.admin.OrgFacilityViewController',
        OrgFacilityViewController);
} 