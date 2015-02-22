using OpenORPG.Common.Entity;

namespace OpenORPG.Common.Dialog.Conditions
{
    public class MinimumLevelDialogCondition : IDialogCondition
    {
        private int _level;

        public MinimumLevelDialogCondition(int level)
        {
           _level = level;
        }

        public bool Verify(ICharacterContract player)
        {
            return player.Level >= _level;
        }
    }
}
