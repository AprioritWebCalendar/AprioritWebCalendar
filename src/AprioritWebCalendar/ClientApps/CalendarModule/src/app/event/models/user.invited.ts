import { User } from "../../authentication/models/user";

export class UserInvited extends User {
    public IsAccepted: boolean;
}