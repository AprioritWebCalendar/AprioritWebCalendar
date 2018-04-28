import { Event } from "../../event/models/event";
import { User } from "../../authentication/models/user";

export class Invitation {
    public Event: Event;
    public Invitator: User;
    public User: User;
    public IsReadOnly: boolean;
}