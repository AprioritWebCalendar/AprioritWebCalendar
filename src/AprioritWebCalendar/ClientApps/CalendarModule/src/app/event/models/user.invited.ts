import { User } from "../../authentication/models/user";

export class UserInvited {
    public User: User;
    public IsAccepted: boolean;
    public IsReadOnly: boolean;
}