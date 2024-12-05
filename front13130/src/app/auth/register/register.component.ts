import { NgFor } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Component } from "@angular/core";
import { NgModel, ReactiveFormsModule } from "@angular/forms";
import { Router } from "@angular/router";

@Component({
  selector: "app-register",
  imports: [ReactiveFormsModule],
  templateUrl: "./register.component.html",
  styleUrl: "./register.component.css",
  standalone: true,
})
export class RegisterComponent {
  constructor(private http: HttpClient, private router: Router) {}

  // Form submission logic
  onSubmit(event: SubmitEvent) {
    event.preventDefault();
    const form = event.target as HTMLFormElement;
    const registerData = {
      name: "",
      email: "",
      password: "",
    };
    for (let key of Object.keys(registerData)) {
      //@ts-ignore
      registerData[key] = form.elements[key].value.trim();
    }
    console.log(registerData);
    const url = "https://localhost:7021/api/Users/register"; // Replace with your backend URL

    // Send data to the backend
    this.http
      .post(url, registerData, {
        headers: {
          "Content-Type": "application/json",
        },
      })
      .subscribe({
        next: (response) => {
          console.log("Registration successful:", response);

          this.router.navigate(["/login"]); // Navigate to login page after registration
        },
        error: (error) => {
          console.error("Error during registration:", error);
        },
      });
  }
}
