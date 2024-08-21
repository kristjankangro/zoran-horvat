// namespace Demo.Composition;
//
// static class Vehicles
// {
//     public static VehicleComposition Aircraft(MovingAbility lowAltFlying, MovingAbility highAltFlying) => new()
//     {
//         LowAltFlying = lowAltFlying,
//         HighAltFlying = highAltFlying
//     };
//
//     public static VehicleComposition SmallAircraft(MovingAbility lowAltFlying) => new()
//     {
//         LowAltFlying = lowAltFlying
//     };
//
//     public static VehicleComposition FloatingAircraft(MovingAbility floating, MovingAbility lowAltFlying) => new()
//     {
//         Floating = floating,
//         LowAltFlying = lowAltFlying
//     };
//
//     public static VehicleComposition Ground(MovingAbility driving) => new()
//     {
//         Driving = driving
//     };
//
//     public static VehicleComposition Boat(MovingAbility floating) => new()
//     {
//         Floating = floating
//     };
//
//     public static VehicleComposition Amphibia(MovingAbility driving, MovingAbility floating) => new()
//     {
//         Driving = driving,
//         Floating = floating
//     };
// }
//
