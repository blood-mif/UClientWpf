using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UodClientWpf.Data
{
    public class MesureRegime
    {
        /// <summary>
        /// Измеренные данные на магнитном стенде
        /// </summary>
        /// <param name="polarity">Полярность стенда</param>
        /// <param name="ring">включенное кольцо</param>
        public MesureRegime(EnumUodCmds polarity, EnumUodCmds ring)
        {
            Polarity = polarity;
            Ring = ring;
        }
        /// <summary>
        /// Полярность стенда
        /// </summary>
        public EnumUodCmds Polarity { get; set; }
        /// <summary>
        /// Включенное кольцо
        /// </summary>
        public EnumUodCmds Ring { get; set; }

        /// <summary>
        /// Время измерения
        /// </summary>
        public double MesureTime{get;set;}

        /// <summary>
        /// Измеренные разности
        /// </summary>
        public double[] GyrRasn { get; set; } = new double[3];
        /// <summary>
        /// Измеренные суммы
        /// </summary>
        public double[] GyrSumm { get; set; } = new double[3];
    }

    
}
