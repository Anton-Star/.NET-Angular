module app.common {
    'use strict';

    export interface ICommonFactory {
        $broadcast: (...args: any[]) => ng.IAngularEvent;
        $q: ng.IQService;
        $timeout: ng.ITimeoutService;
        activateController: (promises: ng.IPromise<any>[], controllerId: string)=> void;
        showSplash: () => void;
        hideSplash: () => void;
        isNumber: (val: string) => boolean;
        isValidPassword: (val: string) => boolean;
        textContains: (text: string, contains: string) => boolean;
        sortUsers(data: services.IUser[]): services.IUser[];
        currentUser: services.IUser;
        isUser: () => boolean;
        isDirector(orgName: string): boolean;
        isFact(): boolean;
        isConsultantCoordinator(): boolean;
        isReviewer(): boolean;
        isInspector(): boolean;
        inspectorHasAccess: boolean;
        setNextSection(root: services.IHierarchyData[], allSections: services.IHierarchyData[], parent: services.IHierarchyData): services.IHierarchyData[];
        findNextSection(id: string, root: services.IApplicationSection[], parent: services.IApplicationSection): services.IApplicationSection;
        loginRedirect(): void;
        goToTop(): void;
        onModalOpen(): void;
        onModalClose(): void;

        //Section RFI Related
        isQuestionRFI(question: services.IQuestion, includeNewStatus: boolean, appStatus: string): boolean;
        isQuestionRFIWithComments(appStatus: string, submittedDate: string, question: services.IQuestion): boolean;
        hideCommentFromUser(appStatus: string, submittedDate: string, comment: services.IApplicationResponseComment): boolean;

        containsRow(row: services.IHierarchyData, rowId: string, setFlag: boolean): boolean;
        //Section Status Views
        setCircleFromQuestions(row: services.IHierarchyData, rfis: services.IRfi[], isReviewer?: boolean): void;
        setCircleReviewer(row: services.IHierarchyData): void;
        setCircleReviewerFromChild(row: services.IHierarchyData): void;
        setCircleApplicantFromQuestions(row: services.IHierarchyData): void;
        setCircleApplicantFromChild(row: services.IHierarchyData): void;    
        getCircleColorForSections(rows: services.IHierarchyData[], isReviewer: boolean): string;  
        getCircleForSite(apps: services.IApplication[], isReviewer: boolean): string;  

        //Section Reviewer/Coordinator Views
        getResponseStatusTypes(): Array<services.IApplicationResponseStatusItem>;
        getCommentTypes(): Array<services.ICommentType>;
        getCommentType(i: number): services.ICommentType;
        processCommentType(ct: services.ICommentType): services.ICommentType;

        onResize(id?: string);
        resetApplication(): void;
        onOpenDate(e): void;
        onOpenCombo(e): void;
        checkItemValue(event: string, fromValue: Object, showSpinnerIfNotFound: boolean): ng.IPromise<{}>;

        applicationSettings: services.IApplicationSetting[];


        //application related values
        application: app.services.IApplication;
        compApplication: services.IComplianceApplication;
        compApp: services.ICompApplication;
        accessToken: app.services.IAccessToken;
        organization: app.services.IOrganization;
        isUserLeadInspector: boolean;
        rfis: app.services.IRfi[];
        applicationInspectors: services.IInspectionScheduleDetail[];
        compAppInspectors: services.IInspectionScheduleDetail[];
        applicationSections: services.IApplicationSection[];
        postInspectionDocuments: services.IDocument[];
        ctTotals: services.ICtTotal[];
        cbTotals: services.ITotal[];
        cbCategories: string[];
        documents: app.services.IDocument[];
        inspectionScheduleDetails: app.services.IInspectionScheduleDetail[];
        appSavedService: Function;
    }

    class Common implements ICommonFactory {
        $q: ng.IQService;
        $timeout: ng.ITimeoutService;
        currentUser: services.IUser;
        isShowing = false;
        counter = 0;
        documentService: services.IDocumentService;
        application: app.services.IApplication = null;
        compApplication: services.IComplianceApplication = null;
        compApp: services.ICompApplication = null;
        accessToken: app.services.IAccessToken = null;
        organization: app.services.IOrganization = null;
        isUserLeadInspector: boolean = null;
        rfis: services.IRfi[] = null;
        applicationSettings: app.services.IApplicationSetting[] = null;
        applicationSections: app.services.IApplicationSection[] = null;
        applicationInspectors: services.IInspectionScheduleDetail[] = null;
        compAppInspectors: services.IInspectionScheduleDetail[] = null;
        postInspectionDocuments: services.IDocument[] = null;
        ctTotals: services.ICtTotal[] = null;
        cbTotals: services.ITotal[] = null;
        cbCategories: string[] = null;
        documents: app.services.IDocument[] = null;
        inspectionScheduleDetails: app.services.IInspectionScheduleDetail[] = null;
        inspectorHasAccess = false;
        appSavedService: Function = null;

        constructor(
            private $anchorScroll: ng.IAnchorScrollService,
            private $window: ng.IWindowService,
            $q: ng.IQService,
            private $rootScope: ng.IRootScopeService,
            $timeout: ng.ITimeoutService,
            private config: IConfig,
            private $location: ng.ILocationService) {

            this.$q = $q;
            this.$timeout = $timeout;        
        }

        onResize(id?: string) {
            console.log('resize');

            var items;
            //if (id) {
            //    id = "#" + id;

            //    items = $(id + " .panel-heading");
            //} else {
            //    items = $(".panel-heading");
            //}

            items = $(".panel-heading");

            _.each(items, (i) => {
                var item: any = $(i).find(".col-lg-10");

                if (item && item.length > 0) {

                    var rightItem = null;
                    for (var j = 0; j < item.length; j++) {
                        var list = $(item[j]).attr('class').split(/\s+/);

                        for (var k = 0; k < list.length; k++) {
                            if (list[k].indexOf("hide") === -1) {
                                rightItem = item[j];
                                break;
                            }
                        }

                        if (rightItem) {
                            break;
                        }
                    }

                    if (rightItem) {
                        var classList = $(rightItem).attr('class').split(/\s+/);
                        var found = false;
                        var css = "";
                        for (var x = 0; x < classList.length; x++) {
                            if (classList[x].indexOf('height') > -1) {
                                css = classList[x].replace("height", "");
                                found = true;
                                break;
                            }
                        }

                        var height = $(rightItem).height();

                        if (!found) {
                            if (height !== 0) {
                                $(rightItem).addClass("height" + height.toString());
                                if (height > 35) {
                                    $(i).height(height + 15);
                                } else {
                                    $(i).height(50);
                                }
                            }


                        } 
                    }
                                    
                } else {
                    item = $(i).find(".col-lg-7");

                    if (item && item.length > 0) {
                        var rightItem = null;
                        for (var j = 0; j < item.length; j++) {
                            var list = $(item[j]).attr('class').split(/\s+/);

                            for (var k = 0; k < list.length; k++) {
                                if (list[k].indexOf("hide") === -1) {
                                    rightItem = item[j];
                                    break;
                                }
                            }

                            if (rightItem) {
                                break;
                            }
                        }

                        if (rightItem) {
                            var classList = $(rightItem).attr('class').split(/\s+/);
                            var found = false;
                            var css = "";
                            for (var x = 0; x < classList.length; x++) {
                                if (classList[x].indexOf('height') > -1) {
                                    css = classList[x].replace("height", "");
                                    found = true;
                                    break;
                                }
                            }

                            var height = $(rightItem).height();

                            if (!found) {
                                if (height !== 0) {
                                    $(rightItem).addClass("height" + height.toString());
                                    if (height > 35) {
                                        $(i).height(height + 15);
                                    } else {
                                        $(i).height(50);
                                    }
                                }

                            }
                        }

                        
                    }
                }

                
            });
        }

        resizeItems(items, text) {
            
        }

        goToTop() {
            this.$location.hash('start');
            this.$anchorScroll();
        }

        onModalOpen() {
            $("#divContainer").hide();
        }

        onModalClose() {
            $("#divContainer").show();
        }

        onOpenDate(e) {
            var id = e.sender.element[0].id;

            var posModal = $(".modal").offset().top;

            setTimeout(() => {
                var pos = $('#' + id).offset().top + 40;
                $(".k-animation-container").css("top", pos);
            },
                200);

        }

        onOpenCombo = (e) => {
            //this.goToTop();

            //var id = e.sender.element[0].id;

            //setTimeout(() => {
            //    var pos = $('#' + id).offset().top + 250;
            //    $(".k-animation-container").css("top", pos);
            //},
            //    200);

        }

        containsRow(row: services.IHierarchyData, rowId: string, setFlag: boolean): boolean {
            if (row.id === rowId) {
                if (setFlag) row.statusName = "Complete";
                return true;
            }

            if (row.children && row.children.length > 0) {
                var found = false;
                _.each(row.children, (child) => {
                    var f = this.containsRow(child, rowId, setFlag);

                    if (f) found = true;
                });

                return found;
            }

            return false;
        }

        checkItemValue(event: string, fromValue: Object, showSpinnerIfNotFound: boolean = false): ng.IPromise<{}> {
            var deferred = this.$q.defer();

            if (fromValue != null) {
                deferred.resolve(fromValue);
            } else {
                if (showSpinnerIfNotFound) {
                    this.showSplash();
                }
                var watcher = this.$rootScope.$on(event, (data: any, args: any) => {
                    deferred.resolve(args);
                    watcher();
                });
            }

            return deferred.promise;
        }

        resetApplication() {
            this.organization = null;
            this.application = null;
            this.compApplication = null;
            this.accessToken = null;
            this.isUserLeadInspector = null;
            this.rfis = null;
            this.applicationSections = null;
            this.applicationInspectors = null;
            this.compAppInspectors = null;
            this.postInspectionDocuments = null;
            this.cbTotals = null;
            this.ctTotals = null;
            this.cbCategories = null;
            this.documents = null;
            this.inspectionScheduleDetails = null;
            this.compApp = null;
            this.inspectorHasAccess = false;
        }

        loginRedirect() {
            this.$location.path('/').search({ x: 'u', url: this.$location.url() });
        }

        isDirector(orgName: string): boolean {
            if (!this.currentUser || !orgName) return false;

            var org = _.find(this.currentUser.organizations, (o) => {
                return o.organization.organizationName === orgName;
            });

            if (org && (org.jobFunction.name.indexOf("Director") > -1 || org.jobFunction.name.indexOf("Primary Contact") > -1)) {
                return true;
            }

            if (this.organization && this.organization.facilityDirectors) {
                var found = _.find(this.organization.facilityDirectors, (f) => {
                    return f === this.currentUser.emailAddress;
                });

                if (found) {
                    return true;
                }
            }

            return false;
        }

        isFact(): boolean {
            return this.currentUser && this.currentUser.role && (this.currentUser.role.roleName === this.config.roles.factAdministrator ||
                this.currentUser.role.roleName === this.config.roles.factCoordinator ||
                this.currentUser.role.roleName === this.config.roles.factQualityManager);
        }

        isConsultant(): boolean {
            return this.currentUser && this.currentUser.role && (this.currentUser.role.roleName === this.config.roles.factConsultant ||
                this.currentUser.role.roleName === this.config.roles.factConsultantCoordinator);
        }

        isConsultantCoordinator(): boolean {
            return this.currentUser && this.currentUser.role && this.currentUser.role.roleName === this.config.roles.factConsultantCoordinator;
        }

        isUser(): boolean {
            return this.currentUser && this.currentUser.role && (this.currentUser.role.roleName === this.config.roles.user ||
                this.currentUser.role.roleName === this.config.roles.factConsultant);
        }

        isConsultingCoordinator(): boolean {
            return this.currentUser &&
                this.currentUser.role &&
                this.currentUser.role.roleName === this.config.roles.factConsultantCoordinator;
        }

        isReviewer(): boolean {
            return this.currentUser && this.currentUser.role && this.currentUser.role.roleName === this.config.roles.factAdministrator ||
                this.currentUser.role.roleName === this.config.roles.inspector;
        }

        isInspector(): boolean {
            return this.currentUser && this.currentUser.role && this.currentUser.role.roleName === this.config.roles.inspector;
        }

        $broadcast(...args: any[]): ng.IAngularEvent {  
            return this.$rootScope.$broadcast.apply(this.$rootScope, args);
        }

        activateController(promises: ng.IPromise<any>[], controllerId: string): void {
            if (promises.length == 0) return;

            this.showSplash();
                
            this.$q.all(promises).finally(() => {
                this.hideSplash();
            });
        }

        showSplash(): void {
            if (!this.isShowing) {
                this.$broadcast(this.config.events.controllerActivating, null);
                this.isShowing = true;

                this.runSpinnerTimeout(true);
            }
        }

        private runSpinnerTimeout(isNew: boolean) {
            if (isNew) {
                this.counter = 0;
            }
            this.$timeout(() => {
                if (this.isShowing) {
                    if (this.counter > 60) {
                        this.hideSplash();
                        this.counter = 0;
                    } else {
                        this.counter++;
                    }

                    this.runSpinnerTimeout(false);
                } else {
                    this.$broadcast(this.config.events.controllerActivateSuccess, null);
                }
            }, 1000);
        }

        sortUsers(data: services.IUser[]): services.IUser[] {
            data.sort((a, b) => {
                var nameA = a.lastName.toLowerCase(), nameB = b.lastName.toLowerCase();
                if (nameA < nameB) //sort string ascending
                    return -1;
                if (nameA > nameB)
                    return 1;
                return 0; //default return value (no sorting)
            });

            return data;
        }

        getCurrentUser(): services.IUser {
            return this.currentUser;
        }

        hideSplash(): void {
            if (this.isShowing) {
                this.$broadcast(this.config.events.controllerActivateSuccess, null);
                this.isShowing = false;
            }
        }

        isNumber(val: string): boolean {
            return (/^[-]?\d+$/).test(val);
        }

        isValidPassword(password: string): boolean {           
            return !(/^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$/).test(password);
        }

        textContains(text: string, searchText: string) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }

        findNextSection(id: string, root: services.IApplicationSection[], parent: services.IApplicationSection): services.IApplicationSection {
            var index = -1;
            for (var i = 0; i < parent.children.length; i++) {
                if (parent.children[i].id === id) {
                    index = i + 1;
                    break;
                }
            }

            if (index > -1) {
                for (var j = index; j < parent.children.length; j++) {
                    if (parent.children[j].isVisible) {
                        return this.getAppSectionWithQuestions(parent.children[j]);
                    }
                }
            }

            if (parent.parent) {
                return this.findNextSection(parent.id, root, parent.parent);
            } else {
                index = -1;
                for (var k = 0; k < root.length; k++) {
                    if (root[k].id === parent.id) {
                        index = k + 1;
                        break;
                    }
                }

                if (index > -1) {
                    for (var l = index; l < root.length; l++) {
                        if (root[l].isVisible) {
                            return this.getAppSectionWithQuestions(root[l]);
                        }
                    }
                }
            }

            return null;
        }

        getAppSectionWithQuestions(section: services.IApplicationSection): services.IApplicationSection {
            if (!section) return null;

            if (section.children && section.children.length > 0) {
                for (var i = 0; i < section.children.length; i++) {
                    var sect = this.getAppSectionWithQuestions(section.children[i]);

                    if (sect != null) {
                        return sect;
                    }
                }
            }

            if (section.questions && section.questions.length > 0 && section.isVisible) return section;

            return null;
        }

        findNext(id: string, root: services.IHierarchyData[], parent: services.IHierarchyData): services.IHierarchyData {
            var index = -1;
            for (var i = 0; i < parent.children.length; i++) {
                if (parent.children[i].id === id) {
                    index = i + 1;
                    break;
                }
            }

            if (index > -1) {
                for (var j = index; j < parent.children.length; j++) {
                    if (parent.children[j].isVisible) {
                        return this.getSectionWithQuestions(parent.children[j]);
                    }
                }
            }

            if (parent.parent) {
                return this.findNext(parent.id, root, parent.parent);
            } else {
                index = -1;
                for (var k = 0; k < root.length; k++) {
                    if (root[k].id === parent.id) {
                        index = k + 1;
                        break;
                    }
                }

                if (index > -1) {
                    for (var l = index; l < root.length; l++) {
                        if (root[l].isVisible) {
                            return this.getSectionWithQuestions(root[l]);
                        }
                    }
                }
            }

            return null;
        }

        getSectionWithQuestions(section: services.IHierarchyData): services.IHierarchyData {
            if (!section) return null;

            if (section.children && section.children.length > 0) {
                for (var i = 0; i < section.children.length; i++) {
                    var sect = this.getSectionWithQuestions(section.children[i]);

                    if (sect != null) {
                        return sect;
                    }
                }
            }

            if (section.questions && section.questions.length > 0 && section.isVisible) return section;

            return null;
        }

        setNextSection(root: services.IHierarchyData[], allSections: services.IHierarchyData[], parent: services.IHierarchyData): services.IHierarchyData[] {
            for (var secIndex = 0; secIndex < allSections.length; secIndex++) {
                var section = allSections[secIndex];
                section.parent = parent;

                if (section.isVisible) {
                    section.children = section.children || [];

                    if (section.children && section.children.length > 0) {
                        section.children = this.setNextSection(root, section.children, section);
                    } else if (parent != null) {
                        section.nextSection = this.findNext(section.id, root, parent);
                    }

                    //for (var i = 0; i < section.children.length; i++) {
                    //    section.nextSection = null;

                    //    for (var j = i + 1; j < section.children.length; j++) {
                    //        if (section.children[j].isVisible) {
                    //            section.nextSection = section.children[j];
                    //            break;
                    //        }
                    //    }

                    //    if (section.nextSection == null && secIndex !== allSections.length - 1) {
                    //        var nextSection = allSections[secIndex + 1];
                    //        nextSection.children = nextSection.children || [];

                    //        for (var k = 0; k < nextSection.children.length; k++) {
                    //            if (nextSection.children[k].isVisible) {
                    //                section.nextSection = nextSection.children[k];
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            return allSections;
        }

        //START SECTION: RFI Related
        isQuestionRFI(question: services.IQuestion, includeNewStatus: boolean = true, appStatus: string = ""): boolean {
            if (question.text ===
                "Has a minimum average of one (1) marrow collection procedure per year been performed within the accreditation cycle?") {
                debugger;
            }
            if (this.isUser()) {

                var status = ((question.answerResponseStatusName === this.config.applicationSectionStatuses.rfi ||
                                question.visibleAnswerResponseStatusName === this.config.applicationSectionStatuses.rfi) &&
                                (appStatus === "Applicant Response" || appStatus === "RFI In Progress")) ||
                    question.visibleAnswerResponseStatusName === this.config.applicationSectionStatuses.rfi ||
                    question.visibleAnswerResponseStatusName === this.config.applicationSectionStatuses.rfiCompleted ||
                    (includeNewStatus && (question.visibleAnswerResponseStatusName === this.config.applicationSectionStatuses.new || question.visibleAnswerResponseStatusName === null));

                return status;
            } else {
                return question.answerResponseStatusName === this.config.applicationSectionStatuses.rfi ||
                    question.answerResponseStatusName === this.config.applicationSectionStatuses.rfiCompleted ||
                    (includeNewStatus && (question.answerResponseStatusName === this.config.applicationSectionStatuses.new || question.answerResponseStatusName === null));
            }
        }

        isQuestionRFIWithComments(appStatus: string, submittedDate: string, question: services.IQuestion): boolean {
            if (!question.responseCommentsRFI || question.responseCommentsRFI.length === 0) return false;
            if (appStatus === this.config.applicationSectionStatuses.inProgress) return false;
            if (appStatus === this.config.applicationSectionStatuses.rfi) return true;
            if (appStatus === "Applicant Response") return true;
            if (appStatus === "RFI In Progress") return true;

            var showComments = false;

            for (var i = 0; i < question.responseCommentsRFI.length; i++) {
                if (!this.hideCommentFromUser(appStatus, submittedDate, question.responseCommentsRFI[i])) {
                    showComments = true;
                    break;
                }
            }

            return showComments;
        }

        hideCommentFromUser(appStatus: string, submittedDate: string, comment: services.IApplicationResponseComment): boolean {
            if (comment && !comment.visibleToApplicant) {
                return true;
            }

            var updatedDate = new Date(comment.createdDate || comment.updatedDate);

            if (appStatus === this.config.applicationSectionStatuses.rfi || appStatus === "RFI In Progress" || appStatus === "Applicant Response") return false;


            if (this.isUser && submittedDate !== "" && submittedDate != null) {
                var submittedDateObj = moment(submittedDate);
                var subDate = submittedDateObj.add(1, "days").toDate();

                if (updatedDate > subDate)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }


        // -- START SECTION: Set Section/Requirement Status Values -- //
        setCircleFromQuestions(row: services.IHierarchyData, rfis: services.IRfi[], isReviewer: boolean = false) {

            for (var i = 0; i < row.questions.length; i++) {
                var q = row.questions[i];
                if (q.answerResponseStatusName === this.config.applicationSectionStatuses.rfiFollowUp) {
                    var comment = _.find(q.applicationResponseComments, (c: services.IApplicationResponseComment) => {
                        return c.commentType.name === this.config.applicationSectionStatuses.rfi;
                    });

                    if (comment) {
                        rfis.push({
                            requirementNumber: row.uniqueIdentifier,
                            questionNumber: i + 1,
                            comment: comment.comment
                        });
                    }
                }
            }

            //See [1459] for the order of precedence of statuses.
            if (isReviewer) this.setCircleReviewer(row);
            else this.setCircleApplicantFromQuestions(row);
            
        }
        setCircleReviewer(row: services.IHierarchyData) {
            row.circleStatusName = "";
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.rfi);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.new);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.rfiCompleted);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.forReview);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.rfiFollowUp);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.reviewed);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.notCompliant);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.compliant);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.notApplicable);
            this.setCircleReviewerInternal(row, this.config.applicationSectionStatuses.noResponseRequested);
        }
        setCircleReviewerInternal(row: services.IHierarchyData, status: string, all: boolean = false) {
            if (!row.circleStatusName) {
                var found = null;
                if (all) {
                    found = _.find(row.questions, (q: services.IQuestion) => {
                        if (this.isUser) {
                            return q.answerResponseStatusName !== status && !q.isHidden; //we shud use visibleReponseStatus if the reviewer sees applicant statuses on checklist view.
                        } else {
                            return q.answerResponseStatusName !== status && !q.isHidden;
                        }
                    });
                } else {
                    found = _.find(row.questions, (q: services.IQuestion) => {
                        return (q.answerResponseStatusName === status || (status === this.config.applicationSectionStatuses.forReview && q.answerResponseStatusName === "")) && !q.isHidden;
                    });
                }

                if (found) {
                    row.circleStatusName = status.replace(' ', '');
                    row.circle = status;
                }
            }

        }
        getCircleForSite(apps: services.IApplication[], isReviewer: boolean): string {
            var statuses;

            if (isReviewer) {

                statuses = [
                    this.config.applicationSectionStatuses.rfi,
                    this.config.applicationSectionStatuses.forReview,
                    this.config.applicationSectionStatuses.new,
                    this.config.applicationSectionStatuses.notCompliant,
                    this.config.applicationSectionStatuses.rfiFollowUp,
                    this.config.applicationSectionStatuses.rfiCompleted,
                    this.config.applicationSectionStatuses.reviewed,
                    this.config.applicationSectionStatuses.compliant,
                    this.config.applicationSectionStatuses.notApplicable,
                    this.config.applicationSectionStatuses.noResponseRequested
                ];
            } else {
                statuses = [
                    this.config.applicationSectionStatuses.rfi,
                    this.config.applicationSectionStatuses.rfiCompleted,
                    this.config.applicationSectionStatuses.inProgress
                ];
            }

            for (var i = 0; i < statuses.length; i++) {
                var status = this.getColorForApplication(apps, statuses[i]);

                if (status !== "") {
                    return status;
                }
            }

            return "";
        }

        getCircleColorForSections(rows: services.IHierarchyData[], isReviewer: boolean): string {
            var statuses;

            if (isReviewer) {

                statuses = [
                    this.config.applicationSectionStatuses.rfi,
                    this.config.applicationSectionStatuses.forReview,
                    this.config.applicationSectionStatuses.new,
                    this.config.applicationSectionStatuses.notCompliant,
                    this.config.applicationSectionStatuses.rfiFollowUp,
                    this.config.applicationSectionStatuses.rfiCompleted,
                    this.config.applicationSectionStatuses.reviewed,
                    this.config.applicationSectionStatuses.compliant,
                    this.config.applicationSectionStatuses.notApplicable,
                    this.config.applicationSectionStatuses.noResponseRequested
                ];
            } else {
                statuses = [
                    this.config.applicationSectionStatuses.rfi,
                    this.config.applicationSectionStatuses.rfiCompleted,
                    this.config.applicationSectionStatuses.inProgress
                ];
            }

            for (var i = 0; i < statuses.length; i++) {
                var status = this.getColorForSection(rows, statuses[i]);

                if (status !== "") {
                    return status;
                }
            }

            return "";        
        }

        getColorForApplication(apps: services.IApplication[], status: string): string {
            var found = _.find(apps, (child: services.IApplication) => {
                return child.circle === status;
            });

            return found ? status : "";
        }

        getColorForSection(rows: services.IHierarchyData[], status: string): string {
            var found = _.find(rows, (child: services.IHierarchyData) => {
                return child.circle === status && child.isVisible;
            });

            return found ? status : "";
        }

        setCircleReviewerFromChild(row: services.IHierarchyData)
        {
            row.circleStatusName = "";
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.rfi);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.new);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.rfiCompleted);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.forReview);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.rfiFollowUp);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.reviewed);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.notCompliant);
          
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.compliant);
            //this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.complete); //complete is only relevant for applicant
            //this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.notStarted); //complete is only relevant for applicant
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.notApplicable);
            this.setCircleReviewerFromChildInternal(row, this.config.applicationSectionStatuses.noResponseRequested);
        }
        setCircleReviewerFromChildInternal(row: services.IHierarchyData, status: string, all: boolean = false) {
            if (!row.circleStatusName) {
                var found = null;

                if (all) {
                    found = _.find(row.children, (child: services.IHierarchyData) => {
                        return child.circle !== status && child.isVisible; //if any one of them is false then return false.
                    });
                } else {
                    found = _.find(row.children, (child: services.IHierarchyData) => {
                        return child.circle === status && child.isVisible;
                    });
                }

                if (found) {
                    row.circleStatusName = status.replace(' ', ''); //this one is used for CSS class
                    row.circle = status; //this one is used for acctual name and comparisons.
                }
            }

        }

        setCircleApplicantFromQuestions(row: services.IHierarchyData) {
            row.circleStatusName = "";
            if (!row.circleStatusName) {
                var status = this.getApplicantAnswersStatusInternal(row.questions);
                row.circleStatusName = status.replace(' ', ''); //this one is used for CSS class
                row.circle = status; //this one is used for acctual name and comparisons.    
            }
        }
        setCircleApplicantFromChild(row: services.IHierarchyData) {
            row.circleStatusName = "";
            this.setCircleApplicantFromChildInternal(row, this.config.applicationSectionStatuses.rfi);
            this.setCircleApplicantFromChildInternal(row, this.config.applicationSectionStatuses.rfiCompleted);
            this.setCircleApplicantFromChildInternal(row, this.config.applicationSectionStatuses.inProgress);
            //For applicant, handling the completed and inprogress scenarios are tricky. so a seperate function.
            this.setCircleApplicantFromChild2Internal(row);
        }
        setCircleApplicantFromChildInternal(row: services.IHierarchyData, status: string) {
            var found = null;
            var visibleChildren = row.children.filter(function (c) { return c.isVisible });

            if (!row.circleStatusName) {
                found = _.find(visibleChildren, (child: services.IHierarchyData) => {
                    return child.circle === status;
                });
            }
            if (found) {
                row.circleStatusName = status.replace(' ', ''); //this one is used for CSS class
                row.circle = status; //this one is used for acctual name and comparisons.
            }
        }
        setCircleApplicantFromChild2Internal(row: services.IHierarchyData) {
            if (!row.circleStatusName) {

                var allComplete, anyComplete, allNotStarted = null;
                var visibleChildren = row.children.filter(function (c) { return c.isVisible });

                allComplete = _.every(visibleChildren, (child: services.IHierarchyData) => {
                    return child.circle === this.config.applicationSectionStatuses.complete;
                });
                anyComplete = _.find(visibleChildren, (child: services.IHierarchyData) => {
                    return child.circle === this.config.applicationSectionStatuses.complete;
                });
                allNotStarted = _.every(visibleChildren, (child: services.IHierarchyData) => {
                    return child.circle === this.config.applicationSectionStatuses.notStarted;
                });

                if (allNotStarted) {
                    row.circle = this.config.applicationSectionStatuses.notStarted;
                    row.circleStatusName = row.circle.replace(' ', '');
                } else if (allComplete) {
                    row.circle = this.config.applicationSectionStatuses.complete;
                    row.circleStatusName = row.circle.replace(' ', '');
                } else if (anyComplete) {
                    row.circle = this.config.applicationSectionStatuses.inProgress;
                    row.circleStatusName = row.circle.replace(' ', '');
                }
            }
        }
        getApplicantAnswersStatusInternal(questions: services.IQuestion[]): string {
            var answeredCount = 0;
            var notAnswered, anyRFICompleted, anyRFI = false;

            var visibleQuestions = questions.filter(function (c) { return !c.isHidden });

            _.each(visibleQuestions, (question: services.IQuestion) => {
                var answered = false;
                //if any question with visible status of RFI
                if (question.visibleAnswerResponseStatusName === this.config.applicationSectionStatuses.rfi) {
                    anyRFI = true;
                }
                //if any question with visible status of RFI Completed
                if (question.visibleAnswerResponseStatusName === this.config.applicationSectionStatuses.rfiCompleted) {
                    anyRFICompleted = true;
                }

                if (question.type == this.config.questionTypes.checkboxes || question.type == this.config.questionTypes.radioButtons) {
                    if (question.answers && question.answers.length > 0) {
                        //look for answer in checkboxes & radio buttons
                        var found = _.find(question.answers, (answer: any) => {
                            return answer.selected === true;
                        });
                        if (found) answered = true;
                    } else answered = false;
                } else {
                    //look for answer in all other question types
                    if (question.questionResponses && question.questionResponses[0]) {
                        answered = question.questionResponses[0].otherText ? true : false;
                        if (!answered) { answered = question.questionResponses[0].document ? true : false; }
                        if (!answered) { answered = question.questionResponses[0].userId ? true : false; }
                    }

                }
                if (answered) {
                    answeredCount++;
                } else {
                    notAnswered = true;
                }
            });

            if (anyRFI) {
                return this.config.applicationSectionStatuses.rfi;
            }
            else if (anyRFICompleted) {
                return this.config.applicationSectionStatuses.rfiCompleted;
            } else if (answeredCount > 0) {
                if (notAnswered) return this.config.applicationSectionStatuses.inProgress;
                else return this.config.applicationSectionStatuses.complete;
            }
            return this.config.applicationSectionStatuses.notStarted;

        }
        // -- END SECTION: Set Section/Requirement Status Values -- //
        
        getResponseStatusTypes() : Array<services.IApplicationResponseStatusItem> {
            return [{ "id": 2, "name": "Reviewed", "nameForApplicant": null }, { "id": 3, "name": "For Review", "nameForApplicant": null }, { "id": 4, "name": "Compliant", "nameForApplicant": null }, { "id": 5, "name": "Not Compliant", "nameForApplicant": null }, { "id": 6, "name": "N/A", "nameForApplicant": null }, { "id": 7, "name": "No Response Requested", "nameForApplicant": null }, { "id": 8, "name": "RFI", "nameForApplicant": null }, { "id": 9, "name": "RFI Completed", "nameForApplicant": null }, { "id": 10, "name": "RFI/Followup", "nameForApplicant": null }, { "id": 11, "name": "New", "nameForApplicant": null }];
        }
        getCommentTypes(): Array<services.ICommentType> {
            return [{ "id": '1', "name": "RFI" }, { "id": '2', "name": "Citation" }, { "id": '3', "name": "Suggestion" }, { "id": '4', "name": "Coordinator" }, { "id": '5', "name": "FACT Response" }, { "id": '6', "name": "FACT Only" }];
        }
        getCommentType(i: any): services.ICommentType {
            switch (i) {
                case 1:
                case '1':
                    return { "id": '1', "name": "RFI" };
                case 2: 
                case '2':
                    return { "id": '2', "name": "Citation" }; 
                case 3:
                case '3':
                    return { "id": '3', "name": "Suggestion" };
                case 4:
                case '4':
                    return { "id": '4', "name": "Coordinator" };
                case 5:
                case '5':
                    return { "id": '5', "name": "FACT Response" };
                case 6:
                case '6':
                    return { "id": '6', "name": "FACT Only" };
            }
        }
        processCommentType(ct: services.ICommentType): services.ICommentType {
            //We do this to accomodate a bug in angular. Sometimes the ng-model against ng-options is populated as a string value.
            if (typeof ct === 'string' || typeof ct === 'number') {
                return this.getCommentType(ct); //
            }
            return ct;
        }
        
    }

    factory.$inject = [
        '$anchorScroll',
        '$window',
        '$q',
        '$rootScope',
        '$timeout',
        'config',
        '$location'
    ];
    function factory(
        $anchorScroll: ng.IAnchorScrollService,
        $window: ng.IWindowService,
        $q: ng.IQService,
        $rootScope: ng.IRootScopeService,
        $timeout: ng.ITimeoutService,
        config: IConfig,
        $location: ng.ILocationService): ICommonFactory {

        return new Common($anchorScroll, $window, $q, $rootScope, $timeout, config, $location);
    }

    angular
        .module('app.common')
        .factory('common',
        factory);
}  