import { EventInput } from "@fullcalendar/core";

let eventGuid = 0;
const TODAY_STR = new Date().toISOString().replace(/T.*$/, ""); // YYYY-MM-DD of today

export const INITIAL_EVENTS: EventInput[] = [
  {
    id: createEventId(),
    title: "All-day event",
    start: TODAY_STR,
  },
  {
    id: createEventId(),
    title: "Timed event",
    start: TODAY_STR + "T00:00:00",
    end: TODAY_STR + "T03:00:00",
  },
  {
    id: createEventId(),
    title: "Timed event",
    start: TODAY_STR + "T12:00:00",
    end: TODAY_STR + "T15:00:00",
  },
  {
    id: createEventId(),
    title: "Event on December 27 New Year party",
    start: "2024-12-27" + "T12:00:00",
    end: "2024-12-27" + "T15:00:00",
  },
  {
    id: createEventId(),
    title: "2 Event on December 27 New Year party",
    start: "2024-12-27",
    end: "2024-12-27",
  },
];

export function createEventId() {
  return String(eventGuid++);
}
