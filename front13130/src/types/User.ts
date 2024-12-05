export default interface User {
  id: number;
  email: string;
  name: string;
  token?: string;
  role: "Admin" | "User" | "Organizer";
}
