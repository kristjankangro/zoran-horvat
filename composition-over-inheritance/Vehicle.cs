namespace composition_over_inheritance;

public class Vehicle
{
    internal MovingAbility? Drive { get; init; }
    internal MovingAbility? Float { get; init; }
    internal MovingAbility? LowFly { get; init; }
    internal MovingAbility? HighFly { get; init; }

    public double GetAverageSpeed(double km, double waterKm, double airKm)
    {
        MovingAbility? land = Drive.Faster(LowFly);
        MovingAbility? water = Float.Faster(LowFly);
        MovingAbility? air = LowFly.Faster(HighFly);

        double? totalhours = km / land?.TopSpeed + waterKm / water.TopSpeed + airKm / air.TopSpeed;
        return (totalhours > 0 ? (km + waterKm + airKm) / totalhours : 0) ?? 0;
    }
}