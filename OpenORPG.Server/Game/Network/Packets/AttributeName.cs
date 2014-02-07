namespace Server.Game.Network.Packets
{
    /// <summary>
    ///     This is a master listing of all possible attributes that can be associated with any given
    /// 
    ///     If you want to add an attribute, you simply need to add it to this enumeration.
    /// </summary>
    public enum AttributeName
    {
        Amount,

        Movable,

        Hitpoints,
        HitpointsMax,

        Intelligence,
        Vitality,
        Dexterity,
        Strength,
        ExperienceNeeded,
        Level,
        Experience,
        Gold
    }
}