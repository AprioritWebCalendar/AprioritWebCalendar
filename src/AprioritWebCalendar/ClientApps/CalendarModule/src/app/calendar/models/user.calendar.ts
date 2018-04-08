import { User } from "../../authentication/models/user";

export class UserCalendar {
    public User: User;
    public IsReadOnly: boolean;
    public IsSubscribed: boolean = true;
}