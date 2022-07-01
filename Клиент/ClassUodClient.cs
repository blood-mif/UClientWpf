using System;
using System.IO;
using System.Linq;
using BaseNetworkClient.Connection;
using L_412_BaseNetworkClient;
using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using L_412_StrData;
using System.Xml;
using UodClientWpf.Reg;
using UodClientWpf.View;
using BaseNetworkClient.Базовые_режимы;
using UodClientWpf.ViewModel;
using System.Xml.Linq;
using System.Collections.Generic;

namespace UodClientWpf.Клиент
{
    public class ClassUodClient : ClassBaseClient
    {
        public ClassUodClient(XElement initNode) : base(initNode)
        {
        }
        public ClassUodClient(String InitStr)
        {
            var str = InitStr.Split(new[] { '\t' });
            var par = str[1].Split(new[] { ':' });

            #region Инициализация клиента по TCP-IP
            if (par.Length == 4)
            {
                _conn = new ClassTcpIpConnection(par[2], Convert.ToInt32(par[3]));
                return;
            }
            #endregion

            #region Инициализация клиента по COM
            //_conn = new ClassSerialConnection(InitStr); 
            //_conn = new ClassSerialConnection(str[1]);
            #endregion
        }

        public ClassUodClient()
        {

        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="ip">Ip адрес</param>
        /// <param name="port">Порт</param>
        public ClassUodClient(String ip, int port)
        {
            _conn = new ClassTcpIpConnection(ip, port);
        }

        /// <summary>
        /// Класс соединения
        /// </summary>
        private ClassBaseConnection _conn;

        public override bool VerifyConnection()
        {
            return _conn.VerifyConnection();
        }

        
        ASyncUodRegime reg;

        ClassRegimeWidthWindow<SyncRegime> syncWithWindow;

        ClassOpenWindow<ViewMagneticSensity> _viewMagnitSensity;

        public override void StartCommand(XElement xmlNode)
        {
            SyncUodRegime regSyncro;
            string path1 = null;
            string path2 = null;
            string path = null;
            string pathResult = null;
            if (xmlNode.Attribute("file") != null)          
               path = xmlNode.Attribute("dir") != null ? $"{xmlNode.Attribute("dir").Value}\\{xmlNode.Attribute("file").Value}" : xmlNode.Attribute("file").Value;
            if (xmlNode.Attribute("file1") != null)
                path1 = xmlNode.Attribute("dir") != null ? $"{xmlNode.Attribute("dir").Value}\\{xmlNode.Attribute("file1").Value}" : xmlNode.Attribute("file1").Value;
            if (xmlNode.Attribute("file2") != null)
                path2 = xmlNode.Attribute("dir") != null ? $"{xmlNode.Attribute("dir").Value}\\{xmlNode.Attribute("file2").Value}" : xmlNode.Attribute("file2").Value;
            if (xmlNode.Attribute("pathResult") != null)
                pathResult = xmlNode.Attribute("dir") != null ? $"{xmlNode.Attribute("dir").Value}\\{xmlNode.Attribute("pathResult").Value}" : xmlNode.Attribute("pathResult").Value;


            switch (xmlNode.Attribute("cmd").Value.ToLower())
            {

                case "magnitsensity":
                    var viewMod = new ViewModelMesureMagnitSensyty();
                    _viewMagnitSensity = new ClassOpenWindow<ViewMagneticSensity>(viewMod);
                    _viewMagnitSensity.Start();
                    break;
                case "endmagnitsensity":
                    if (_viewMagnitSensity != null)
                        _viewMagnitSensity.Stop();
                    _viewMagnitSensity = null;
                    break;
                case "magneticstendpolarynormal":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendPolaryNormal, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "magneticstendpolaryreverse":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendPolaryReverse, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "magneticstendpoweron":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendPowerOn, Conn, xmlNode);
                    reg.StartAndStop();
                    break;

                case "endmagneticstendpoweron":
                case "magneticstendpoweroff":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendPowerOff, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "magneticstendringsoff":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendRingsOff, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "magneticstendringx":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendRingX, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "magneticstendringy":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendRingY, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "magneticstendringz":
                    reg = new ASyncUodRegime(EnumUodCmds.MagneticStendRingZ, Conn, xmlNode);
                    reg.StartAndStop();
                    break;

                case "modareverson":
                    reg = new ASyncUodRegime(EnumUodCmds.ModaReverseOn, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "modaduration":
                    reg = new ASyncUodRegime(EnumUodCmds.ModaDuration, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "setdelitelmu":
                    reg = new ASyncUodRegime(EnumUodCmds.SetDelitelMu, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "poweronwidthrsctrl":
                    reg = new ASyncUodRegime(EnumUodCmds.PowerOnWidthRsCtrl, Conn, xmlNode);
                    reg.StartAndStop();
                    break;
                case "endpoweronwidthrsctrl":
                    reg = new ASyncUodRegime(EnumUodCmds.PowerOff, Conn, xmlNode);
                    reg.StartAndStop();
                    break;

                case "startsyncro501":
                    if (syncWithWindow == null)
                    {
                        regSyncro = new SyncUodRegime(EnumUodCmds.StartSyncro501, Conn, path);
                        syncWithWindow = new ClassRegimeWidthWindow<SyncRegime>(regSyncro);
                        syncWithWindow.Start();
                    }
                    break;
                case "startsyncro401":
                    if (syncWithWindow == null)
                    {
                        regSyncro = new SyncUodRegime(EnumUodCmds.StartSyncro401, Conn,path);
                        syncWithWindow = new ClassRegimeWidthWindow<SyncRegime>(regSyncro);
                        syncWithWindow.Start();
                    }
                    break;
                case "stopsyncro":
                case "endstartsyncro401":
                case "endstartsyncro501":
                    if (syncWithWindow != null)
                    {
                        syncWithWindow.Stop();
                        syncWithWindow = null;
                    }
                    break;
                case "compoundfiles":                 

                    //if (path != null)
                    CompoundFiles(path1, path2, pathResult);

                    break;

            }
        }

        public void AbortCommand()
        {
        }

        public delegate String DelGetPath();

        public DelGetPath EventGetPath;

        private String _pathToFile;

        public void FileSavePath()
        {
            if (EventGetPath != null)
                _pathToFile = EventGetPath();
        }

        /// <summary>
        /// Функция склеивания двух файлов
        /// </summary>
        /// <param name="path1">Путь к файлу</param>
        /// <param name="path2">Путь к файлу</param>
        /// <param name="pathResult">Результирующий путь сохранения</param>
        public void CompoundFiles(String path1, String path2, String pathResult)
        {

            #region Old
            //if (EventStartRegime != null)
            //    EventStartRegime(null, null);

            //String path1 = $"{path}\\MagmitSencity_Moda0_0\\report.txt";
            //String path2 = $"{path}\\MagmitSencity_Moda1_0\\report.txt";
            //string[] SummText = new string[0];
            //if (File.Exists(path1) && File.Exists(path2))
            //{
            //    var text1 = File.ReadAllLines(path1);
            //    string[] Mass = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
            //    var text01 = text1.Concat(Mass).ToArray();
            //    var text2 = File.ReadAllLines(path2);
            //    SummText = text01.Concat(text2).ToArray();
            //}
            //else
            //{
            //    if (!File.Exists(path1))
            //        Console.WriteLine("Файл не существует " + path1);
            //    if (!File.Exists(path2))
            //        Console.WriteLine("Файл не существует " + path2);
            //}
            //File.WriteAllLines($"{path}\\FileReport.txt", SummText);

            //if (EventStopRegime != null)
            //    EventStopRegime(null, null);
            #endregion

            #region NewSave
            if (EventStartRegime != null)
                EventStartRegime(null, null);

            if (File.Exists(path1) && File.Exists(path2))
            {

                var list1 = File.ReadAllBytes(path1).ToList();
                var list2 = File.ReadAllBytes(path2).ToList();

                //var list1 = ReaderF.ReadLstData23(path1);
                //var list2 = ReaderF.ReadLstData23(path2); //SyncUOD list data

                //var list1 = ReaderF.ReadLstData23Pp(path1);
                //var list2 = ReaderF.ReadLstData23Pp(path2); //SyncUOD list data

                //var list1 = ReaderF.ReadLstData10(path1); //Чтение файлов с УОД как 10 режим
                //var list2 = ReaderF.ReadLstData10(path2);

                list1.AddRange(list2); //Добавление к списку1 данных списка2
                File.WriteAllBytes(pathResult, list1.ToArray());  //RegUod23
                Console.WriteLine("Файл сохранен! " + pathResult);
                //List<RegUod23> combined = list1.Concat(list2).ToList(); //Объединяет две последовательности. Concat returns a new sequence without modifying the original list.
                //WriterF.WriteLstData(combined, pathResult);  //RegUod23
            }
            else
            {
                if (!File.Exists(path1))
                    Console.WriteLine("Файл не существует " + path1);
                if (!File.Exists(path2))
                    Console.WriteLine("Файл не существует " + path2);
            }
            #endregion
        }

        public void BrowsDialog()
        {
        }

        void MesureMagnitSencity(String cmd, String path = null)
        {
            var moda = ReaderF.GetParametrInt(cmd, "moda");

        }

        void TestAlgoritmMagnitSens(String cmd, String path = null)
        {
            if (EventStartRegime != null)
                EventStartRegime(null, null);
            var path1 = ReaderF.GetParametrString(cmd, "Path");
            EventStopRegime(null, null);
            //meas.StartMesure(iibType, heartingTime, measureTime, moda, cycles, path);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SelectModa(int moda)
        {
            //var par = new RegSelectModa(moda);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };

            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }

        /// <summary>
        /// Включение МТ
        /// </summary>
        private void PushOn()
        {
            //var par = new RegPushOnPushOff(EnumPush.PushOn);
            //_asyncRegim=new ClassUodAsyncRegime(AnalyseProgramParams,_conn)
            //                {EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime};

            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Выключение МТ
        /// </summary>
        private void PushOff()
        {
            //var par = new RegPushOnPushOff(EnumPush.PushOff);
            //_asyncRegim=new ClassUodAsyncRegime(AnalyseProgramParams,_conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };
            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }

        /// <summary>
        /// Установка делителя МУ
        /// </summary>
        /// <param name="del"></param>
        private void SetDelitelMu(int del)
        {
            //var par = new RegSetDelitelMu(del);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };
            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Управление магнитным стендом
        /// </summary>
        /// <param name="StendState">Состояние стенда</param>
        private void MagneticStendState(EnumUodCmds StendState)
        {
            //var par = new RegMagneticStendState(StendState);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };

            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Управление полярностью стенда
        /// </summary>
        /// <param name="polary">Полярность стенда</param>
        private void MagneticReversPolarity(EnumUodCmds polary)
        {
            //var par = new RegMagneticStendPolary(polary);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };

            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Включение колец стенда
        /// </summary>
        /// <param name="ring">Кольцо стенда</param>
        private void MagneticRing(EnumUodCmds ring)
        {
            // var par = new RegMagneticStendRing(ring);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };
            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }

        /// <summary>
        /// Включение/отключение реверса
        /// </summary>
        /// <param name="revState">состояние реверса</param>
        private void SetReversOnOff(bool revState)
        {
            //var par = new RegUodReversOnOff(revState);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };
            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Установка времени работы на одной моде
        /// </summary>
        /// <param name="time">Время работы на моде</param>
        private void SetReversTime(int time)
        {
            //var par = new RegHalfPerTime(time);
            //_asyncRegim = new ClassUodAsyncRegime(AnalyseProgramParams, _conn) { EventStartRegime = EventStartRegime, EventStopRegime = EventStopRegime };
            //_regime = _asyncRegim;
            //_asyncRegim.StartRegime(par);
            //_asyncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Режим синхронной передачи данных
        /// </summary>
        /// <param name="iib">Режим ИИБ 501/401</param>
        /// <param name="time">Время съема данных</param>
        /// <param name="path">Путь для сохранения файла</param>
        /// <param name="flagForm">открывать или нет форму визуализации</param>
        private void SyncRegime(int iib, double time, String path, bool flagForm)
        {
            //var mt = iib == 401 ? EnumSyncRegim.StartSyncRegimeMt401 : EnumSyncRegim.StartSyncRegimeMt501;

            //var par = new object[] { new RegSyncRegimeStartStop(mt), Convert.ToInt32(time / AnalyseProgramParams.ConStruct.TauSi),path };

            //if (_syncRegim == null)
            //    _syncRegim = new ClassUodSyncRegime(AnalyseProgramParams, _conn, flagForm)
            //                     {
            //                         EventStartRegime = EventStartRegime,
            //                         EventStopRegime = EventStopRegime,
            //                         EventViewGraph = EventViewGraph,
            //                         EventViewTable = EventViewTable
            //                     };
            //_regime = _syncRegim;
            //_syncRegim.StartRegime(par);
            //_syncRegim.WaitEndRegime();
        }
        /// <summary>
        /// Удаление накопленных данных
        /// </summary>
        private void DeleteData()
        {
            if (EventStartRegime != null)
                EventStartRegime(null, null);
            EventStopRegime(null, null);
        }


    }
}
