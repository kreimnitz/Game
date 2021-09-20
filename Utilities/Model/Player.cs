using System;
using System.ComponentModel;

namespace Utilities.Model
{
    [Serializable]
    public class Player : IAutoNotifyPropertyChanged
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            private set { NotifyHelpers.SetProperty(this, ref _id, value); }
        }

        private int _glory;
        public int Glory
        {
            get { return _glory; }
            set { NotifyHelpers.SetProperty(this, ref _glory, value); }
        }

        private int _gloryMax;
        public int GloryMax
        {
            get { return _gloryMax; }
            set { NotifyHelpers.SetProperty(this, ref _gloryMax, value); }
        }

        private int _income;
        public int Income
        {
            get { return _income; }
            set { NotifyHelpers.SetProperty(this, ref _income, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Player() : this(-1, 0, 0, 0)
        {
        }

        public Player(int id) : this(id, GameConstants.StartingGlory, GameConstants.StartingIncome, GameConstants.StartingMaxGlory)
        {
        }

        public Player(int id, int glory, int income, int gloryMax)
        {
            ID = id;
            _glory = glory;
            _gloryMax = gloryMax;
            _income = income;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CopyFrom(Player playerStats)
        {
            if (ID == -1)
            {
                ID = playerStats.ID;
            }
            Glory = playerStats.Glory;
            Income = playerStats.Income;
            GloryMax = playerStats.GloryMax;
        }

        public void ApplyIncome()
        {
            Glory = Math.Min(Glory + Income, GloryMax);
        }
    }
}
