using BaseNetworkClient.Connection;
using BaseNetworkClient.Базовые_режимы;
using L_412_StrData.Regims.Analyse;
using L_412_StrData.Regims.Uod;
using L_412_StrData.Regims.Uod.Answers;
using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
namespace UodClientWpf.Reg
{
    public class ASyncUodRegime : ClassSeparateThread
    {
        /// <summary>
        /// Конструктор класса асинхронного режжима УОД
        /// </summary>
        /// <param name="cmd">Комманда</param>
        /// <param name="conn">Узел с параметрами комманды</param>
        /// <param name="xmlNode">Узел с параметрами комманды</param>        
        public ASyncUodRegime(EnumUodCmds cmd, ClassBaseConnection conn, XElement xmlNode =null)
        {
            this.conn = conn;

            byte? parametr = null;
            byte tmpPar = 0;
            if (xmlNode!=null&&xmlNode.Attribute("value") != null && byte.TryParse(xmlNode.Attribute("value").Value, out tmpPar))
                parametr = tmpPar;

            quest = new ClassUodQuest(cmd, parametr);
            //analyse = new AnalyseDataStream<RegUodAsync>(quest.Cmd);
            analyse = new AnalyseDataStream<RegUodAsync>();
        }

        /// <summary>
        /// Класс подключения
        /// </summary>
        ClassBaseConnection conn;        

        /// <summary>
        /// Класс запроса
        /// </summary>
        private ClassUodQuest quest;

        /// <summary>
        /// Класс анализа 
        /// </summary>
        AnalyseDataStream<RegUodAsync> analyse ;
        
        public int CmdWaitTimeout = 1000;
        /// <summary>
        /// Потоковая функция
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected override void ExecuteFunc(object o, EventArgs e)
        {
            if (!conn.Connection)
                conn.Connect();
            int cnt = 0;


            //while (analyse.Lst.Count == 0 || (analyse.Lst.Where(dat=> ((RegUodAsync)dat).Cmd == quest.Cmd).Count() != 0 ))
            while (analyse.Lst.Where(dat => ((RegUodAsync)dat).Cmd == quest.Cmd).Count() == 0)
            {
                Console.WriteLine($"Запуск режима {quest.Cmd} Попытка {cnt++}");
                conn.WriteData(quest.ToByteMas);
                while (conn.AvalibleBytes== 0)
                {
                    int a = 0;
                }
                Thread.Sleep(CmdWaitTimeout);
                var buf = conn.ReadData();
                if (buf != null)
                {
                    analyse.AddData(buf.ToList());
                    if (analyse.Lst.Count == 0)
                    {
                        Console.WriteLine("Errrrrrrrr");
                    }
                }
            }
            Console.WriteLine($"Режима {quest.Cmd} завершен!\n");
        }
    }
}
