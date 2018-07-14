public class FlagEffect : IEffect
{
    public bool DefaultActive { get; private set; }
    public bool Active { get; set; }

    public FlagEffect(bool defaultActive)
    {
        DefaultActive = defaultActive;
        Active = defaultActive;
    }
}
