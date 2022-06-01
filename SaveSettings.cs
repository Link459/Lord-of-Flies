using System;

namespace Lord_of_Flies
{
    [Serializable]
    public class SaveSettings
    {
        public BossStatue.Completion Completion = new()
        {
            isUnlocked = true
        };

        public bool AltStatue;
    }
}