using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UodClientWpf.Data;

namespace UodClientWpf.ViewModel
{
    public class ViewModelMesureMagnitSensyty:INotifyPropertyChanged

    {
        public ViewModelMesureMagnitSensyty()
        {
            
        }
        /// <summary>
        /// Список данных по прогреву
        /// </summary>
        ObservableCollection<MesureRegime> HeatingLst { get; set; }

        /// <summary>
        /// Список данных для оси X
        /// </summary>
        ObservableCollection<MesureRegime> SensityAxeX { get; set; }
        /// <summary>
        /// Список данных для оси Y
        /// </summary>
        ObservableCollection<MesureRegime> SensityAxeY { get; set; }
        /// <summary>
        /// Список данных для оси Z
        /// </summary>
        ObservableCollection<MesureRegime> SensityAxeZ { get; set; }

        /// <summary>
        /// Событие обновления данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
