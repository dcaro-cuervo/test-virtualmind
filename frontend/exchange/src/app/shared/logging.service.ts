import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class LoggingService {
    // TODO reemplace this for a tracking tool like slack.
    logError(message: string, stack: string) {
        console.log('LoggingService: ' + message);
    }
}
