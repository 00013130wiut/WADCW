import { Injectable } from "@angular/core";
import User from "../../types/User";

@Injectable({
  providedIn: "root",
})
export class UserService {
  user: User | null = null;
  constructor() {
    const user = localStorage.getItem("user");
    if (user) {
      this.user = JSON.parse(user);
    }
  }
  getUser() {
    return this.user;
  }
  setUser(user: User) {
    localStorage.setItem("user", JSON.stringify(user));
    this.user = user;
  }
}
