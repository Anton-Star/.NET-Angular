//common view for both reviewer and coordinator

module app.widgets {
    'use strict';

    class AnswerComments implements ng.IDirective {

        //local variables which are not instance specific for this directives
        private editIndex: number = 0;
        private currentUserId: string;
        
        constructor(
            private $compile: ng.ICompileService,
            private $templateRequest: ng.ITemplateRequestService,
            private common: common.ICommonFactory) {

            this.currentUserId = this.common.currentUser.userId;
        }

        static factory(): ng.IDirectiveFactory {
            var directive = (
                $compile: any,
                $templateRequest,
                common) =>
                new AnswerComments($compile, $templateRequest, common);

            directive.$inject = [
                '$compile',
                '$templateRequest',
                'common'
            ];

            return directive;
        }

        terminal = true;
        restrict = 'E';

        scope = {
            question: "=question",
            comment: "=comment",
            status: "=status",
            commentTypes: "=commenttypes",
            submitted: "=submitted",
            saveComment: "=",
            isCoordinatorView: "=iscoordinatorview",
            editComment: "=",
            delete: "=",
            onDocumentDownload: "=",
            showDocumentLibrary: "="
        };

        link = (scope: any, element: any, attributes: any) => {
            scope.editIndex = ++this.editIndex;
            scope.currentUserId = this.currentUserId;
            scope.created = new Date(scope.comment.createdDate);
            scope.common = this.common;
            if (scope.submitted) {
                scope.submitted = moment(scope.submitted).toDate();    
            }
            
            scope.question.documents = scope.comment.commentDocuments;

            scope.onDelete = (question: services.IQuestion,
                comment: services.IApplicationResponseComment,
                editIndex: number) => {

                scope.delete(question, comment, editIndex);
            }

            scope.$watch('submitted', (newVal, oldVal) => {
                if (scope.submitted) {
                    scope.sub = moment(scope.submitted).toDate();
                }
            });
            
            var _thisScope = this;
            this.$templateRequest("/app/widgets/answerComments.html").then((html) => {
                var template = angular.element(html);
                element.append(template);
                _thisScope.$compile(template)(scope);
            });
            
        }
    }


    angular
        .module('app.widgets')
        .directive('answerComments', AnswerComments.factory());
}