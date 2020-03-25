import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly cookieTag = 'wordki-usr-tag';

  private token: { token: string };
  private subject: BehaviorSubject<boolean>;

  constructor(private cookiesService: CookieService) {
    this.subject = new BehaviorSubject<boolean>(false);
  }

  subscribe(): Observable<boolean> {
    return this.subject.asObservable();
  }

  getToken(): { token: string } {
    return this.token;
  }

  isLogin(): boolean {
    return this.token != null;
  }

  loginFromCookie(): void {
    if (!this.cookiesService.check(this.cookieTag)) {
      return;
    }
    const cookieValue = this.cookiesService.get(this.cookieTag);
    this.token = JSON.parse(cookieValue);
    this.sendToSubscribers();
  }

  logout(): void {
    this.token = null;
    if (this.cookiesService.check(this.cookieTag)) {
      this.cookiesService.delete(this.cookieTag);
    }
    this.sendToSubscribers();
  }

  refresh(newToken: any): void {
    if (newToken.length === 0) {
      return;
    }
    this.token = newToken;
    this.saveCookie();
    this.sendToSubscribers();
  }

  private sendToSubscribers(): void {
    this.subject.next(this.token !== null);
  }

  private saveCookie(): void {
    this.cookiesService.set(this.cookieTag, JSON.stringify(this.token));
    console.log(this.cookiesService.check(this.cookieTag) ? 'token w cookie' : 'blad w cookie');
  }

}
