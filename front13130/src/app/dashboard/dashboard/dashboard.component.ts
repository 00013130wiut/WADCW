import {
  Component,
  signal,
  ChangeDetectorRef,
  WritableSignal,
  OnInit,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router, RouterOutlet } from "@angular/router";
import { FullCalendarModule } from "@fullcalendar/angular";
import {
  CalendarOptions,
  DateSelectArg,
  EventClickArg,
  EventApi,
  EventDropArg,
} from "@fullcalendar/core";
import interactionPlugin from "@fullcalendar/interaction";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import { INITIAL_EVENTS, createEventId } from "./event-utils";
import listPlugin from "@fullcalendar/list";
import { HttpClient } from "@angular/common/http";
import EventC from "../../../types/Event";
import { UserService } from "../../auth/user.service";

@Component({
  selector: "app-dashboard",
  standalone: true,
  imports: [CommonModule, FullCalendarModule],
  templateUrl: "./dashboard.component.html",
  styleUrl: "./dashboard.component.css",
})
export class DashboardComponent implements OnInit {
  calendarVisible = signal(true);
  selectedDate: Date | null = null;
  events = [];
  calendarOptions = signal<CalendarOptions>({
    plugins: [interactionPlugin, dayGridPlugin, timeGridPlugin, listPlugin],
    headerToolbar: {
      left: "prev,next today",
      center: "title",
      right: "dayGridMonth,timeGridWeek,timeGridDay,listWeek",
    },
    events: this.events,
    initialView: "dayGridMonth",
    // alternatively, use the `events` setting to fetch from a feed
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    select: this.handleDateSelect.bind(this),
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this),
    eventDrop: this.handleEventDrop.bind(this),
    /* you can update a remote database when these fire:
    eventAdd:
    eventChange:
    eventRemove:
    */
  });
  currentEvents = signal<EventApi[]>([]);

  constructor(
    private changeDetector: ChangeDetectorRef,
    private httpClient: HttpClient,
    private userService: UserService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.httpClient.get<EventC[]>("/Events").subscribe({
      next: (res) => {
        this.calendarOptions.update((opts) => {
          return {
            ...opts,
            events: res.map((event) => ({
              id: event.id,
              title: event.title,
              start: event.date, // Assuming 'date' is in 'YYYY-MM-DD' or ISO8601 format
              description: event.description,
              extendedProps: {
                location: event.location,
                organizer: event.organizer,
              },
            })),
          };
        });
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  handleDateSelect(selectInfo: DateSelectArg) {
    const user = this.userService.getUser();
    if (!user) {
      this.router.navigate(["/login"]);
      return;
    }
    if (user.role === "User") return;
    this.selectedDate = selectInfo.start;
    this.openModal();
  }

  handleEventClick(clickInfo: EventClickArg) {
    this.router.navigate(["/event", clickInfo.event.id]);
  }

  handleEvents(events: EventApi[]) {
    this.currentEvents.set(events);
    this.changeDetector.detectChanges(); // workaround for pressionChangedAfterItHasBeenCheckedError
  }

  openModal() {
    const modal: HTMLDialogElement | null =
      document.querySelector("#my_modal_1");
    if (modal) {
      modal.showModal(); // Open the modal
    }
  }
  closeModal() {
    const modal: HTMLDialogElement | null =
      document.querySelector("#my_modal_1");
    if (modal) {
      modal.close(); // Open the modal
    }
  }
  handleNewEvent(event: SubmitEvent) {
    event.preventDefault();
    const form = event.target as HTMLFormElement;
    const fd = new FormData(form);
    this.selectedDate?.setHours(
      parseInt(fd.get("date")!.toString().split(":")[0])
    );
    this.selectedDate?.setMinutes(
      parseInt(fd.get("date")!.toString().split(":")[1])
    );

    const newEvent: Partial<EventC> = {
      title: fd.get("title")!.toString(),
      description: fd.get("description")!.toString(),
      date: "",
      location: fd.get("location")!.toString(),
      // id: createEventId(),
      userId: fd.get("userId")!.toString(),
    };

    newEvent.date = this.selectedDate!.toISOString();

    this.httpClient.post<EventC>("/Events", newEvent).subscribe({
      next: (event) => {
        this.calendarOptions.update((opts) => {
          return {
            ...opts,
            events: opts.events
              ? [
                  //@ts-ignore
                  ...opts.events,
                  {
                    title: event.title,
                    start: event.date, // Assuming 'date' is in 'YYYY-MM-DD' or ISO8601 format
                    description: event.description,
                    extendedProps: {
                      location: event.location,
                      organizer: event.organizer,
                    },
                  },
                ]
              : [newEvent],
          };
        });
        this.closeModal();
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  handleEventDrop(arg: EventDropArg) {
    const updatedEvent = {
      //@ts-ignore
      ...arg.event._def,
      //@ts-ignore
      ...arg.event._def.extendedProps,
      date: arg.event.start?.toISOString(),
    };
    //@ts-ignore
    console.log(updatedEvent);

    //@ts-ignore
    this.httpClient
      .put(`/Events/${updatedEvent.publicId}`, updatedEvent)
      .subscribe({
        next: () => {
          console.log("Event updated successfully");
        },
        error: (err) => {
          console.error("Error updating event:", err);
          arg.revert();
        },
      });
  }
}
