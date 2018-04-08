import { User } from "../../authentication/models/user";

export class Calendar {
    public Id: Number;
    public Name: string;
    public Description: string;
    public Color: string;
    public Owner: User;

    public IsReadOnly?: boolean;
    public IsSubscribed?: boolean;
}