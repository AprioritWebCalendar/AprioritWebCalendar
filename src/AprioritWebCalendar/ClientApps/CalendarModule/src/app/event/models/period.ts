import { PeriodType } from "./period.type";

export class Period {
    public Type: PeriodType;
    public PeriodStart: Date;
    public PeriodEnd: Date;
    
    public Cycle?: number;
}