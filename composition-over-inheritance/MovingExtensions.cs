namespace composition_over_inheritance;

static class MovingExtensions
{
    public static MovingAbility? Faster(this MovingAbility? a, MovingAbility? b) =>
        a is null || b?.TopSpeed > a.TopSpeed ? b : a;
}