namespace NM
{
    /// <summary>
    /// You need to specify the Sprite in the inspector for each symbol you define here.
    /// </summary>
    public enum Symbol
    {
        Achievement,
        Message,
        Alert,
        Promo,
        Gift
    }

    /// <summary>
    /// You need to have normal state and reversed state in the controller with the name of the animation you define here.
    /// </summary>
    public enum Animation
    {
        SlideFromTop,
        SlideFromLeft,
        SlideFromRight
    }
}