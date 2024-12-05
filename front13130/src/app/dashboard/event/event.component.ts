import { Component } from "@angular/core";
import EventC from "../../../types/Event";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute } from "@angular/router";
import { DatePipe, NgFor, NgIf } from "@angular/common";
import User from "../../../types/User";

@Component({
  selector: "app-event",
  imports: [DatePipe, NgFor],
  templateUrl: "./event.component.html",
  styleUrl: "./event.component.css",
})
export class EventComponent {
  event: EventC | null = null; // Event details object
  users: User[] = [];
  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit() {
    this.fetchUsers();
    // Get the event ID from the route
    const eventId = this.route.snapshot.paramMap.get("id");
    if (eventId) {
      this.fetchEventDetails(eventId);
    }
  }

  fetchEventDetails(eventId: string) {
    this.http.get<EventC>(`/Events/${eventId}`).subscribe({
      next: (data) => {
        this.event = data; // Store the fetched event data
      },
      error: (err) => {
        console.error("Error fetching event details:", err);
      },
    });
  }
  fetchUsers() {
    this.http.get<User[]>("/Users").subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        console.error("Error fetching users:", err);
      },
    });
  }

  addParticipant(event: SubmitEvent) {
    event.preventDefault(); // @ts-ignore

    //@ts-ignore
    const participan = event.target!.participant!.value;
    console.log(participan);
    this.http
      .post<any>(`/Events/${this.event!.id}/participants/${participan}`, {})
      .subscribe({
        next: (newParticipant) => {
          // Update the participants list locally
          this.event!.participants!.push(newParticipant);
        },
        error: (err) => {
          console.error("Error adding participant:", err);
        },
      });
  }

  // Remove a participant from the event
  removeParticipant(participantId: number) {
    this.http.delete(`/Participants/${participantId}`).subscribe({
      next: () => {
        this.fetchEventDetails(this.event!.id);
      },
      error: (err) => {
        console.error("Error removing participant:", err);
      },
    });
  }
}
