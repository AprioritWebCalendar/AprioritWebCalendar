import { PeriodType } from "./period.type";
import { Period } from "./period";

export class PeriodRequestModel {
    public Type: PeriodType;
    public PeriodStart: string;
    public PeriodEnd: string;
    
    public Cycle?: number;

    public ToPeriod() : Period {
        var period = new Period();

        period.Type = this.Type;
        period.Cycle = this.Cycle;
        period.PeriodStart = new Date(this.PeriodStart);
        period.PeriodEnd = new Date(this.PeriodEnd);

        return period;
    }
}