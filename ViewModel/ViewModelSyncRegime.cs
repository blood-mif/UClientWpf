using L_412_StrData.Regims;
using L_412_StrData.Regims.Uod.Answers.L_412_StrData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UodClientWpf.ViewModel
{
    public class ViewModelSyncRegime : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BaseReg syncRegime;
        /// <summary>
        /// Данные синхронного режима
        /// </summary>
        public BaseReg SyncRegime
        {
            get { return syncRegime; }
            set
            {
                syncRegime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SyncRegime"));
            }
        }

        private TimeSpan time;
        /// <summary>
        /// Время режима
        /// </summary>
        public TimeSpan Time
        {
            get { return time; }
            set
            {
                time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WindowTitleWithTime"));

            }
        }

        /// <summary>
        /// Подпись окна измерения напряжения
        /// </summary>
        public string WindowTitle { get; set; }

        private string _windowTitleWithTime;

        public string WindowTitleWithTime
        {
            get
            {                
                _windowTitleWithTime = $"{WindowTitle} [{Time:g}]";
                return _windowTitleWithTime;

            }
            set
            {
                _windowTitleWithTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WindowTitleWithTime"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WindowTitle"));


            }
        }








    }
}
