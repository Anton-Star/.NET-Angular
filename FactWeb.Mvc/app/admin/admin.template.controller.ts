module app.admin {
    'use strict';

    interface ITemplateScope {
    }

    class TemplateController implements ITemplateScope {
        templates: Array<services.ITemplate>;
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
        selected: services.ITemplate = null;

        static $inject = [
            '$window',
            'templateService',
            'notificationFactory',
            'common',
            'config'
        ];
        constructor(
            private $window: ng.IWindowService,
            private templateService: services.ITemplateService,
            private notificationFactory: blocks.INotificationFactory,
            private common: common.ICommonFactory,
            private config: IConfig) {
            this.common.$broadcast(this.config.events.pageNameSet, {
                pageName: "Manage Templates",
                breadcrumbs: [
                    { url: '#/', name: 'Home', isActive: false },
                    { url: '', name: 'Admin', isActive: true },
                    { url: '', name: 'Manage Templates', isActive: true }
                ]
            });
            common.activateController([this.getOrganizations()], '');
        }

        getOrganizations(): ng.IPromise<void> {
            return this.templateService.getAll()
                .then((data: Array<services.ITemplate>) => {
                    this.templates = data;
                    this.gridOptions.dataSource.data(data);
                })
                .catch(() => {
                    this.notificationFactory.error("Error.");
                });
        }

        onAddNew(): void {
            this.selected = {
                id: null,
                name: "",
                text: ""
            };
        }

        onEdit(template: services.ITemplate): void {
            this.selected = template;
        }

        onDelete(template: services.ITemplate): void {
            if (!confirm("Are you sure you want to delete this template?")) {
                return;
            }

            this.common.showSplash();

            this.templateService.delete(template.id)
                .then((data: services.IServiceResponse) => {
                    if (data.hasError) {
                        this.notificationFactory.error(data.message);
                    } else {
                        var i = 0;
                        var found = false;

                        for (i = 0; i < this.templates.length; i++) {
                            if (this.templates[i].id === template.id) {
                                found = true;
                                break;
                            }
                        }

                        if (found) {
                            this.templates.splice(i, 1);
                            this.gridOptions.dataSource.data(this.templates);
                        }

                        this.notificationFactory.success("Template deleted successfully.");
                    }

                    this.common.hideSplash();
                })
                .catch(() => {
                    this.notificationFactory.error("Error saving data.");
                    this.common.hideSplash();
                });
        }

        isValid(): boolean {
            return this.selected.text !== "" && this.selected.name !== "";
        }

        onSave(): void {
            this.common.showSplash();
            if (this.selected.id == null) {
                this.templateService.add(this.selected)
                    .then((data: services.IGenericServiceResponse<services.ITemplate>) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            this.templates.push(data.item);
                            this.gridOptions.dataSource.data(this.templates);
                            this.notificationFactory.success("Template saved successfully.");
                        }
                        this.selected = null;
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error saving data.");
                        this.common.hideSplash();
                    });
            } else {
                this.templateService.update(this.selected)
                    .then((data: services.IServiceResponse) => {
                        if (data.hasError) {
                            this.notificationFactory.error(data.message);
                        } else {
                            var temp = _.find(this.templates, (t: services.ITemplate) => {
                                return t.id === this.selected.id;
                            });

                            if (temp) {
                                temp.name = this.selected.name;
                                temp.text = this.selected.text;
                            }

                            this.gridOptions.dataSource.data(this.templates);
                            this.selected = null;
                            this.notificationFactory.success("Template saved successfully.");
                        }
                        this.common.hideSplash();
                    })
                    .catch(() => {
                        this.notificationFactory.error("Error saving data.");
                        this.common.hideSplash();
                    });
            }
        }

        onCancel(): void {
            this.selected = null;
        }
    }

    angular
        .module('app.admin')
        .controller('app.admin.TemplateController',
        TemplateController);
} 