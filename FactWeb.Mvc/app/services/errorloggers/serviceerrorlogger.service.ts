module app.services.errorLoggers {
    'use strict';

    class ServerErrorLogger implements enhancedErrorLogging.IErrorLogger {
        log(error: string): void { }
    }
}