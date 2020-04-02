import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ErrorService {

    errorMessage = '';
    error: any = null;

    constructor() {

    }

    setError(errorMessage: string, error: any): void {
        if (this.errorMessage.length > 0 || this.error !== null) {
            console.log('multiple error:', error);
            return;
        }
        console.log(error);
        this.errorMessage = errorMessage;
        this.error = error;
    }

    clearError(): void {
        this.errorMessage = '';
        this.error = null;
    }
}
