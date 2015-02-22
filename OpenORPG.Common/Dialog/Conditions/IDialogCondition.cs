using OpenORPG.Common.Entity;

namespace OpenORPG.Common.Dialog.Conditions
{
    /// <summary>
    /// A public contract specifying in which cases a certain <see ref="DialogLink"/> may be available.
    /// </summary>
    public abstract class IDialogCondition
    {

        /// <summary>
        /// Performs a verification on the activating player to ensure they are qualified to interact with this
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public abstract bool Verify(ICharacterContract player);

    }
}
