export class User {
    public Id : Number;
    public UserName : String;
    public Email : String;
    public IsEmailConfirmed : Boolean;
    public Roles : Array<String>;
    public TimeZone: string;

    public TelegramId?: number;
    public IsTelegramNotificationEnabled?: boolean;
}