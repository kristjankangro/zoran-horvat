namespace composition_over_inheritance;

static class Vehicles
{
    public static Vehicle CreateGround(MovingAbility driving) => new()
    {
        Drive = driving
    };
    
    public static Vehicle CreateBoat(MovingAbility flaoting) => new()
    {
        Float = flaoting
    };
    
    public static Vehicle CreateAmphibious(MovingAbility driving, MovingAbility flaoting) => new()
    {
        Drive = driving, Float = flaoting
    };
    
    public static Vehicle CreateSmallAircraft(MovingAbility flying) => new()
    {
        LowFly = flying
    };
}