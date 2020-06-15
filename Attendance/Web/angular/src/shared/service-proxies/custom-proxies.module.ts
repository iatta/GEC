import { AbpHttpInterceptor, RefreshTokenService } from '@abp/abpHttpInterceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import * as ApiCustomServiceProxies from './custom-proxies';
import { ZeroRefreshTokenService } from '@account/auth/zero-refresh-token.service';

@NgModule({
    providers: [
        ApiCustomServiceProxies.CustomServiceProxy,
        { provide: RefreshTokenService, useClass: ZeroRefreshTokenService },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class CustomProxyModule { }
