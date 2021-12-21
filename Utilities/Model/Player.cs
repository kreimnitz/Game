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

        private double _glory;
        public double Glory
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

        private double _incomeRate;
        public double IncomeRate
        {
            get { return _incomeRate; }
            set { NotifyHelpers.SetProperty(this, ref _incomeRate, value); }
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
            _incomeRate = income;
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
            IncomeRate = playerStats.IncomeRate;
            GloryMax = playerStats.GloryMax;
        }

        public void ApplyIncome()
        {
            Glory = Math.Min(Glory + IncomeRate, GloryMax);
        }
    }
}
