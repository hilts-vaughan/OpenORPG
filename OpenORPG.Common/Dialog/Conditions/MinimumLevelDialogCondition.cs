using OpenORPG.Common.Entity;

namespace OpenORPG.Common.Dialog.Conditions
{
    public class MinimumLevelDialogCondition : IDialogCondition
    {

        public int Level { get; set; }


        public MinimumLevelDialogCondition(int level)
        {
            Level = level;
        }

        public MinimumLevelDialogCondition()
        {
            
        }

        public override bool Verify(ICharacterContract player)
        {
            return player.Level >= Level;
        }

        public override string ToString()
        {
           return "Minimum Level Requirement: " + Level;

        }
    }
}
