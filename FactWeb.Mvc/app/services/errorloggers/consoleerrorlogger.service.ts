module app.services.errorLoggers {
    'use strict';

    class ConsoleErrorLogger implements enhancedErrorLogging.IErrorLogger {
        log(error: string): void { }
    }
}