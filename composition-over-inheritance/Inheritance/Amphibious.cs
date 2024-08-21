namespace Demo.Inheritance;

class Amphibious : VehicleInheritance
{
    private int _rpm;
    private double _maxKnots;

    public Amphibious(double topSpeed, double maxKnots, int rpm) : base(topSpeed) =>
        (_rpm, _maxKnots) = (rpm, maxKnots);
    
    public override double GetAverageSpeed(double landKm, double waterKm, double airKm)
    {
        if (airKm > 0) return 0;
        double totalHours = landKm / base.TopSpeed + waterKm / _maxKnots * 1.852;
        return totalHours > 0 ? (landKm + waterKm) / totalHours : 0;
    }
    
    public override void StartEngine()
    {
        _rpm = 1200;
    }
}
