




using System;

namespace com.F4A.MobileThird
{
    public class LevelReachedEventArgs : EventArgs
    {
        public int Number
        {
            get; private set;
        }

        public bool IsLevelExtraGem
        {
            get; private set;
        }

        public LevelReachedEventArgs(int number)
        {
            Number = number;
        }

        public LevelReachedEventArgs(int number, bool isLevelExtraGem)
        {
            this.Number = number;
            this.IsLevelExtraGem = isLevelExtraGem;
        }
    }
}
