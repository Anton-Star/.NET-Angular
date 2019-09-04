module app.services {
    'use strict';

    export interface IRequirementService {
        getRequirements(type: string): ng.IPromise<Array<IApplicationVersion>>;
        getRequirementsByVersion(type: string, versionId: string): ng.IPromise<IApplicationVersion>;
        getRequirementById(guid: string, reqId: string): ng.IPromise<IApplicationSection>;
        getRequirementsWithDocuments(appId: string, isComp: string): ng.IPromise<Array<ISectionDocument>>;
        save(item: IApplicationHierarchyData): ng.IPromise<IGenericServiceResponse<services.IApplicationSection>>;
        remove(id: string): ng.IPromise<IServiceResponse>;
        processSection(section: services.IApplicationSection, parentPart?: string, parentName?: string): IApplicationHierarchyData;
        questions: Array<IQuestion>;
        import(file, applicationType: string, version: string, versionNumber: string): ng.IPromise<IServiceResponse>;
        export(versionId: string): ng.IPromise<IExport[]>;
    }

    class RequirementService implements IRequirementService {
        questions: Array<IQuestion> = [];

        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private config: IConfig) { }

        export(versionId): ng.IPromise<IExport[]> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Requirement/Export?versionId=' + versionId)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IExport>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getRequirements(type: string): ng.IPromise<Array<IApplicationVersion>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Requirement?type=' + type)
                .success((response: ng.IHttpPromiseCallbackArg<Array<IApplicationVersion>>): void=> {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        } 

        getRequirementsByVersion(type: string, versionId: string): ng.IPromise<IApplicationVersion> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Requirement?type=' + type + '&versionId=' + versionId)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationVersion>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        getRequirementById(guid: string, reqId: string): ng.IPromise<IApplicationSection> {
            var deferred = this.$q.defer();
            this.$http
                .get('api/Requirement/GetByGuid?guid=' + guid + '&reqId=' + reqId)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSection>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }		


        getRequirementsWithDocuments(appId: string, isComp: string): ng.IPromise<Array<ISectionDocument>> {
            var deferred = this.$q.defer();
            this.$http
                .get('/api/Requirement/Documents?appId=' + appId + "&isComp=" + isComp)
                .success((response: ng.IHttpPromiseCallbackArg<Array<ISectionDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        import(file, applicationType: string, version: string, versionNumber: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            var data = new FormData();
            data.append("file", file);

            var request: ng.IRequestConfig = {
                method: 'POST',
                url: '/api/Requirement/Import?applicationType=' + applicationType + "&version=" + version + "&versionNumber=" + versionNumber,
                data: data,
                headers: {
                    'Content-Type': undefined
                }
            };

            this.$http(request)
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<IDocument>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        save(item: IApplicationHierarchyData): ng.IPromise<IGenericServiceResponse<services.IApplicationSection>> {
            var deferred = this.$q.defer();
            this.$http
                .post("/api/Requirement", {
                    id: item.id,
                    partNumber: item.partNumber,
                    name: item.name,
                    isVariance: item.isVariance,
                    helpText: item.helpText,
                    version: item.version,
                    children: item.children,
                    questions: item.questions,
                    order: item.order,
                    parentId: item.parentId || null,
                    applicationTypeName: item.applicationTypeName || null,
                    uniqueIdentifier: item.uniqueIdentifier || null,                    
                    scopeTypes: item.scopeTypes,
                    versionId: item.versionId || null
                })
                .success((response: ng.IHttpPromiseCallbackArg<IGenericServiceResponse<services.IApplicationSection>>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        remove(id: string): ng.IPromise<IServiceResponse> {
            var deferred = this.$q.defer();
            this.$http
                .delete("/api/Requirement/" + id)
                .success((response: ng.IHttpPromiseCallbackArg<IApplicationSetting>): void => {
                    deferred.resolve(response);
                })
                .error((e) => {
                    deferred.reject(e);
                });

            return deferred.promise;
        }

        processSection(section: services.IApplicationSection, parentPart?: string, parentName?: string): IApplicationHierarchyData {
            var row: IApplicationHierarchyData = {
                partNumber: section.partNumber.toString(),
                name: section.name,                
                hasChildren: false,
                id: section.id,
                helpText: section.helpText,
                isVariance: section.isVariance,
                version: section.version,
                order: section.order,
                questions: section.questions,
                uniqueIdentifier: section.uniqueIdentifier,
                scopeTypes: section.scopeTypes

            };

            if (section.questions && section.questions != null) {
                _.each(section.questions, (question: services.IQuestion) => {
                    var isAdded = _.find(this.questions, (q: services.IQuestion) => {
                        return q.id === question.id;
                    });

                    if (!isAdded) {
                        this.questions.push(question);
                    }
                });
            }
            
            if (parentPart) {
                row.partNumber = parentPart + "." + row.partNumber;
            }

            if (parentName) {
                row.parentName = parentName;
            }

            if (section.children && section.children.length > 0) {
                row.hasChildren = true;
                row.children = [];
                row.items = [];
                _.each(section.children,
                    (value: services.IApplicationSection) => {
                        row.children.push(this.processSection(value, row.partNumber, row.partNumber + ": " + row.name));
                        row.items.push(this.processSection(value, row.partNumber, row.partNumber + ": " + row.name));
                    });


            }
            
            return row;
        }
    }

    factory.$inject = [
        '$http',
        '$q',
        'config'
    ];
    function factory($http: ng.IHttpService,
        $q: ng.IQService,
        config: IConfig): IRequirementService {
        return new RequirementService($http, $q, config);
    }

    angular
        .module('app.services')
        .factory('requirementService',
        factory);
} 