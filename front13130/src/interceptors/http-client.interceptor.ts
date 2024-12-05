// default.interceptor.ts
import { inject } from "@angular/core";
import { HttpInterceptorFn, HttpHeaders } from "@angular/common/http";

import { Router } from "@angular/router";
import { UserService } from "../app/auth/user.service";

export const requestInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = "https://localhost:7021/api";
  const userService = inject(UserService);
  const user = userService.getUser();

  if (!user) {
    return next(
      req.clone({
        url: `${baseUrl}${req.url}`,
        headers: new HttpHeaders()
          .set("Content-Type", "application/json")
          .set("Accept", "application/json"),
      })
    );
  }
  const headers = new HttpHeaders()
    .set("Content-Type", "application/json")
    .set("Accept", "application/json")
    .set("Authorization", `Bearer ${user.token}`);

  let modifiedReq = req.clone({
    url: `${baseUrl}${req.url}`,
    headers,
  });

  return next(modifiedReq);
};
