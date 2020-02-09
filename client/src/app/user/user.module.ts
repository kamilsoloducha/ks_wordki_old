import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TokenInterceptorService } from '../authorization/services/token.interceptor/token-interceptor.service';
import { CookieService } from 'ngx-cookie-service';
import { UserProvider, UserProviderBase, UserProviderMock } from './services/user.provider/user.provider';
import { environment } from 'src/environments/environment';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { UserRoutingModule } from './user-routing.module';
import { AuthorizationModule } from '../authorization/authorization.module';

@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    UserRoutingModule,
    AuthorizationModule
  ],
  providers: [
    { provide: UserProviderBase, useClass: environment.production ? UserProvider : UserProviderMock },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true
    },
    CookieService,
  ]
})
export class UserModule { }