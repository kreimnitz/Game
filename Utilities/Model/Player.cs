using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
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

        private int _monumentHealth;  
        public int MonumentHealth
        {
            get { return _monumentHealth; }
            set { NotifyHelpers.SetProperty(this, ref _monumentHealth, value); }
        }

        private int _swordsmanGarrisonCount;
        public int SwordsmanGarrisonCount
        {
            get { return _swordsmanGarrisonCount; }
            set { NotifyHelpers.SetProperty(this, ref _swordsmanGarrisonCount, value); }
        }

        private int _swordsmanAttackerCount;
        public int SwordsmanAttackerCount
        {
            get { return _swordsmanAttackerCount; }
            set { NotifyHelpers.SetProperty(this, ref _swordsmanAttackerCount, value); }
        }

        private int _swordsmanDefenderCount;
        public int SwordsmanDefenderCount
        {
            get { return _swordsmanDefenderCount; }
            set { NotifyHelpers.SetProperty(this, ref _swordsmanDefenderCount, value); }
        }

        private int _enemyAttackerCount;
        public int EnemyAttackerCount
        {
            get { return _enemyAttackerCount; }
            set { NotifyHelpers.SetProperty(this, ref _enemyAttackerCount, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Player(int id, int glory, int income)
        {
            ID = id;
            _glory = glory;
            _income = income;
            _monumentHealth = 100;
            _swordsmanAttackerCount = 0;
            _swordsmanDefenderCount = 0;
            _swordsmanGarrisonCount = 0;
            _enemyAttackerCount = 0;
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
            MonumentHealth = playerStats.MonumentHealth;
            SwordsmanAttackerCount = playerStats.SwordsmanAttackerCount;
            SwordsmanDefenderCount = playerStats.SwordsmanDefenderCount;
            EnemyAttackerCount = playerStats.EnemyAttackerCount;
            SwordsmanGarrisonCount = playerStats.SwordsmanGarrisonCount;
        }

        public byte[] ToByteArray()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        public static Player FromByteArray(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (Player)bf.Deserialize(ms);
            }
        }
    }
}
