import { HttpClient } from "@angular/common/http";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import User from "../../../types/User";
import { UserService } from "../user.service";

@Component({
  selector: "app-login",
  imports: [],
  templateUrl: "./login.component.html",
  styleUrl: "./login.component.css",
})
export class LoginComponent {
  constructor(
    private http: HttpClient,
    private router: Router,
    private userService: UserService
  ) {}

  // Form submission logic
  onSubmit(event: SubmitEvent) {
    event.preventDefault();
    const form = event.target as HTMLFormElement;
    const registerData = {
      email: "",
      password: "",
    };

    for (let key of Object.keys(registerData)) {
      //@ts-ignore
      registerData[key] = form.elements[key].value.trim();
    }
    console.log(registerData);
    const url = "/Users/login"; // Replace with your backend URL

    // Send data to the backend
    this.http.post<User>(url, registerData).subscribe({
      next: (response) => {
        this.userService.setUser(response); // Store the user in the user service
        localStorage.setItem("token", response.token!); // Store the token in local storage
        this.router.navigate(["/dashboard"]); // Navigate to login page after registration
      },
      error: (error) => {
        console.error("Error during registration:", error);
      },
    });
  }
}
