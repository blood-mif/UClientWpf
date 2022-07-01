using BaseNetworkClient.Connection;
using BaseNetworkClient.Базовые_режимы;
using L_412_StrData;
using L_412_StrData.Regims.Analyse;
using L_412_StrData.Regims.Uod;
using L_412_StrData.Regims.Uod.Answers.L_412_StrData;
using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using UodClientWpf.View;
using UodClientWpf.ViewModel;

namespace UodClientWpf.Reg
{
    /// <summary>
    /// Класс синхронного режима
    /// </summary>
    /// <typeparam name="T">Window для отображения</typeparam>
    public class SyncUodRegime : ClassSeparateThread
    {
        /// <summary>
        /// Конструктор класса асинхронного режжима УОД
        /// </summary>
        /// <param name="cmd">Комманда</param>
        /// <param name="conn">Узел с параметрами комманды</param>
        public SyncUodRegime(EnumUodCmds cmd, ClassBaseConnection conn, string path=null)
        {
            quest = new ClassUodQuest(cmd);
            this.conn = conn;
            this.path = path;
            ViewModel = new ViewModelSyncRegime() { WindowTitle = $"Данные режима SyncRegime" };            
        }


        /// <summary>
        /// Класс запроса
        /// </summary>
        private ClassUodQuest quest;
        /// <summary>
        /// Класс подключения
        /// </summary>
        ClassBaseConnection conn;
        /// <summary>
        /// Путь для сохранения
        /// </summary>
        private string path;

        /// <summary>
        /// Класс анализа 
        /// </summary>
        AnalyseDataStream<RegUodSync> analyse;


        
        /// <summary>
        /// Потоковая функция
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected override void ExecuteFunc(object o, EventArgs e)
        {
            analyse = new AnalyseDataStream<RegUodSync>(path, FileMode.Create);
            #region Запуск синхронного режима
            var startSyncRegime = new ASyncUodRegime(quest.Cmd, conn, null) { CmdWaitTimeout = 2000 };
            startSyncRegime.Start();
            startSyncRegime.Stop();
            #endregion
            BegTime = DateTime.Now;




            while (!FlagAbort)
            {
                Thread.Sleep(1);
                var buf = conn.ReadData();
                if (buf != null)
                    analyse.AddData(buf.ToList());

                if (analyse.Lst.Count != 0)
                {                  
                    ((ViewModelSyncRegime)ViewModel).SyncRegime = (RegUodSync)analyse.LastData;
                    ((ViewModelSyncRegime)ViewModel).Time = DateTime.Now - BegTime;
                }
                analyse.WriteToStream();
            }
            analyse.CloseStream();
            //analyse.SaveMki(path);            
            
            
            #region Остановка синхронного режима
            var stopSyncRegime = new ASyncUodRegime(EnumUodCmds.StopSyncro, conn, null) { CmdWaitTimeout = 4000 };
            stopSyncRegime.Start();
            stopSyncRegime.Stop();
            #endregion

           
        }

       





    }
}
