import { User } from "../../authentication/models/user";

export class InviteRequestModel {
    public User: User;
    public IsReadOnly: boolean;
}