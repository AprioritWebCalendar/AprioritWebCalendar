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

    public static FromPeriod(period: Period) : PeriodRequestModel {
        var model = new PeriodRequestModel;

        model.Type = period.Type;
        model.PeriodStart = period.PeriodStart.toDateString();
        model.PeriodEnd = period.PeriodEnd.toDateString();
        model.Cycle = period.Cycle;

        return model;
    }
}