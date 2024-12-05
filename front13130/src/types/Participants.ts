import EventC from "./Event";
import User from "./User";

export default interface Participant {
  userId: number;
  eventId: number;
  user?: User;
  evnt?: EventC;
  id: number;
}
