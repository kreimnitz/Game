using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities.Model
{
    [Serializable]
    public class Player : IAutoNotifyPropertyChanged
    {
        public int ID { get; private set; }

        private int _glory;
        public int Glory
        {
            get { return _glory; }
            set { NotifyHelpers.SetProperty(this, ref _glory, value); }
        }

        private int _income;
        public int Income
        {
            get { return _income; }
            set { NotifyHelpers.SetProperty(this, ref _income, value); }
        }

        private int _attackPower;
        public int AttackPower
        {
            get { return _attackPower; }
            set { NotifyHelpers.SetProperty(this, ref _attackPower, value); }
        }

        private int _attackPowerMax;
        public int AttackPowerMax
        {
            get { return _attackPowerMax; }
            set { NotifyHelpers.SetProperty(this, ref _attackPowerMax, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Player(int id, int glory, int income)
        {
            ID = id;
            _glory = glory;
            _income = income;
            _attackPower = 20;
            _attackPowerMax = 20;
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
            AttackPower = playerStats.AttackPower;
            AttackPowerMax = playerStats.AttackPowerMax;
        }
    }
}
