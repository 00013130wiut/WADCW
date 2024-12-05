import Participant from "./Participants";
import User from "./User";

export default interface EventC {
  title: string;
  date: string | Date;
  description: string;
  location: string;
  organizer?: User;
  id: string;
  participants?: Participant[];
  userId?: string;
}
