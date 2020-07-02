import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Injectable()
export class ServerErrorInterceptor implements HttpInterceptor {

    constructor() { }

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(
            retry(1),
            catchError((error: HttpErrorResponse) => {
                if (error.status === 404) {
                    // Bad request / object not found
                    if (error.error.message) {
                        return throwError(error.error.message);
                    }

                    return throwError('Bad request / object not found');
                }
                if (error.status === 500) {
                    // server error
                    return throwError('Backend server error');
                }
                return throwError(error);
            })
        );
    }
}
