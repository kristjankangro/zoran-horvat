namespace Demo.Inheritance;

abstract class VehicleInheritance
{
    protected double TopSpeed { get; }

    protected VehicleInheritance(double topSpeed) =>
        TopSpeed = topSpeed;
    
    public virtual void StartEngine() { }

    public abstract double GetAverageSpeed(double landKm, double waterKm, double airKm);
}
