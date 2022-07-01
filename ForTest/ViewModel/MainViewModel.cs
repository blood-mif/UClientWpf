using BaseNetworkClient.Connection;
using BaseNetworkClient.Базовые_режимы;
using L_412_StrData;
using L_412_StrData.Regims;
using L_412_StrData.Regims.Analyse;
using L_412_StrData.Regims.IIbRegims.Quests;
using L_412_StrData.Regims.Uod;
using L_412_StrData.Regims.Uod.Answers;
using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using Microsoft.Win32;
using PpdControls.Commands;
using PpdControls.UserControlsPropertis;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UodClientWpf.ForTest.Propertis;
using UodClientWpf.ForTest.Regim;
using UodClientWpf.ForTest.View;
using UodClientWpf.Properties;
using UodClientWpf.Reg;

namespace UodClientWpf.ForTest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            CmdRecivePropertis = new ClassCommand(SaveResivePropertis);
            CmdStartReg = new ClassCommand(UniversalFunc);
            RPropertis = new RecivePropertis();
            VMS = new ViewModelSync();
        }
        public List<EnumUodCmds> LstEnumCommand = new List<EnumUodCmds>();
        public ViewModelSync VMS { get; set; }
        public RecivePropertis RPropertis { get; set; }
        public ClassCommand CmdRecivePropertis { get; set; }
        public ClassCommand CmdStartReg { get; set; }
        RegSync startSyncReg;
        AnalyseDataStream<RegUodAsync> analyse = new AnalyseDataStream<RegUodAsync>();
        SerialPort sp = new SerialPort();
        public void UniversalFunc(object o)
        {
            if (!sp.IsOpen)
            {
                sp = RPropertis.SerialPort;

                var pnList = SerialPort.GetPortNames();
                if (!pnList.Contains(sp.PortName))
                {
                    MessageBox.Show("Ошибка, выбранный вами " + sp.PortName + " отсутствует");
                    return;
                }
                sp.Open();
            }

            int commandParametr = Convert.ToInt32(o);
            switch (commandParametr)
            {
                case 101101:
                    if (!flagVisibleLogBar)
                        flagVisibleLogBar = true;
                    else
                        flagVisibleLogBar = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagVisibleLogBar"));
                    return;
                case 202202:
                    startSyncReg.EventStartReciveData?.Invoke(null, null);
                    FlagWriteDate = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagWriteDate"));
                    return;
                case 303303:
                    SaveData();
                    FlagWriteDate = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagWriteDate"));
                    return;

                case 404404:
                    if (!flagVisibleLogBar)
                        flagVisibleLogBar = true;
                    else
                        flagVisibleLogBar = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagVisibleLogBar"));
                    return;
                default:
                    break;
            }

            string nameEnumCmd = Enum.GetName(typeof(EnumUodCmds), commandParametr);

            LstEnumCommand.Add((EnumUodCmds)Enum.Parse(typeof(EnumUodCmds), nameEnumCmd));
            EnumUodCmds enumCmd = new EnumUodCmds();
            enumCmd = (EnumUodCmds)Enum.Parse(typeof(EnumUodCmds), nameEnumCmd);

            ClassBaseConnection cbc = new ClassSerialConnection(RPropertis.PortName, RPropertis.BaudRate.ToString(), RPropertis.Parity.ToString(), RPropertis.DataBits.ToString(), RPropertis.StopBits.ToString(), RPropertis.Handshake.ToString());
            startSyncReg = new RegSync(cbc, sp, enumCmd, true, RPropertis.DefaultPath, godsMode);

            switch (nameEnumCmd)
            {
                case "Reg30":
                    flagEnableButtonSyncroReg = true;
                    reg30 = true;
                    reg32 = false;
                    reg33 = false;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg33"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg32"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg30"));
                    enabledButton = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnabledButton"));
                    break;
                case "Reg32":
                    flagEnableButtonSyncroReg = true;
                    reg32 = true;
                    reg33 = false;
                    reg30 = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg33"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg32"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg30"));
                    enabledButton = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnabledButton"));
                    break;
                case "Reg33":
                    flagEnableButtonSyncroReg = true;
                    reg33 = true;
                    reg32 = false;
                    reg30 = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg33"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg32"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg30"));
                    enabledButton = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnabledButton"));
                    break;
                case "StopSyncro":
                    enabledButton = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnabledButton"));
                    EventStopReciveData?.Invoke(null, null);
                    VMS.stopWatch.Reset();
                    startSyncReg.Stop();
                    //if (sp.IsOpen)
                    //    sp.Close();
                    flagEnableButtonSyncroReg = false;
                    flagChooseTypeDevice = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseTypeDevice"));
                    SendCommand(enumCmd, sp);
                    LstEnumCommand = new List<EnumUodCmds>();
                    return;
                case "StartSyncro401":
                case "StartSyncro501":
                case "StartSyncro601":
                case "StartSyncro601LowSpeed":
                    reg308 = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg308"));
                    File.Delete(RPropertis.DefaultPath);
                    flagChooseTypeDevice = false;
                    flagChooseReversModa = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseTypeDevice"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseReversModa"));
                    return;
                case "ModaReverseOn":
                    flagChooseReversModa = false;
                    flagChooseDurationModa = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseReversModa"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseDurationModa"));
                    return;
                case "ModaReverseOff":
                    flagChooseReversModa = false;
                    flagChooseFistModa = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseReversModa"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseFistModa"));
                    return;
                case "ModaDuration":
                    flagChooseDurationModa = false;
                    flagChooseFistModa = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseDurationModa"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseFistModa"));
                    return;
                case "ModaSelectOne":
                case "ModaSelectZero":
                    flagChooseFistModa = false;
                    flagChoosePower = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChoosePower"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseFistModa"));
                    return;
                case "PowerOff":
                    flagWriteDate = false;
                    flagEnableButtonSyncroReg = false;
                    flagChooseTypeDevice = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagWriteDate"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseTypeDevice"));
                    SendCommand(enumCmd, sp);
                    LstEnumCommand = new List<EnumUodCmds>();
                    return;

                case "PowerOnWidthRsCtrl":
                case "PowerOnWidthoutRsCtrl":
                    flagChoosePower = false;
                    // flagWriteDate = true;
                    flagEnableButtonSyncroReg = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChoosePower"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagWriteDate"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                    break;
                default:
                    break;
            }








            if (enumCmd == EnumUodCmds.PowerOnWidthoutRsCtrl || enumCmd == EnumUodCmds.PowerOnWidthRsCtrl)
            {
                for (int i = LstEnumCommand.Count - 1; i >= 0; i--)
                {
                    enumCmd = LstEnumCommand[i];
                    SendCommand(enumCmd, sp);
                    if (i == 0)
                    {
                        startSyncReg.EventSendSyncData += VMS.UpdeateData;
                        EventStopReciveData += startSyncReg.StopAnalyse;
                        startSyncReg.Start();
                        flagEnableButtonSyncroReg = true;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
                        LstEnumCommand = new List<EnumUodCmds>();
                    }
                }
            }
            else if (enumCmd == EnumUodCmds.Reg33 || enumCmd == EnumUodCmds.Reg32 ||  enumCmd == EnumUodCmds.Reg30 )
            {
                    SendCommand(enumCmd, sp);
                        startSyncReg.EventSendSyncData += VMS.UpdeateData;
                        EventStopReciveData += startSyncReg.StopAnalyse;
                        startSyncReg.Start();
                        LstEnumCommand = new List<EnumUodCmds>();

            }
            else
            {
                SendCommand(enumCmd,sp);
            }

        }

        void SendCommand(EnumUodCmds enumCmd, SerialPort sp)
        {
            if (!sp.IsOpen)
                sp.Open();

            string nameEnumCmd = enumCmd.ToString();
            //string number = $"0x{(int)Enum.Parse(typeof(EnumUodCmds), nameEnumCmd):X}";
            double? ParamsItem = null;

            if (nameEnumCmd == "ModaDuration")
                ParamsItem = ModaDuration;
            if (nameEnumCmd == "SetDelitelMu")
                ParamsItem = SetDelitelMu;

            BaseReg uodQuest;
            uodQuest = new ClassUodQuest(enumCmd, ParamsItem);
            switch (enumCmd)
            {
                case EnumUodCmds.Reg30:
                   uodQuest = new Reg30Q(Srp);
                    break;
                case EnumUodCmds.Reg32:
                    uodQuest = new Reg32Q(uCapX,UCapY,UCapZ);
                    break;
                case EnumUodCmds.Reg33:
                    uodQuest = new Reg33Q();
                    break;
                default:
                    break;
            }

           // ClassUodQuest uodQuest = new ClassUodQuest(enumCmd, ParamsItem);
          //  Reg33Q dd = new Reg33Q();
            if (HistoryLoge != null)
                HistoryLoge += "\n";
            for (int i = 0; i < uodQuest.ToByteMas.Length; i++)
            {
                HistoryLoge += uodQuest.ToByteMas[i] + " ";
            }
            sp.Write(uodQuest.ToByteMas, 0, uodQuest.ToByteMas.Length);
           // HistoryLoge += "( hex = " + number + " )";
           // HistoryLoge += "\nYou send " + nameEnumCmd;


            DateTime beginData = DateTime.Now;
            DateTime currData = DateTime.Now;
            Console.WriteLine("Start");
            var size = sp.BytesToRead;
            while (size !=4  && (currData - beginData).TotalSeconds < 0.5)
            {
                currData = DateTime.Now;
                size = sp.BytesToRead;
                //if (size!=0)
                //   Console.WriteLine(size);


                byte[] massByte = new byte[size];
                sp.Read(massByte, 0, massByte.Length);

                //analyse.Lst.IsEmpty == true
                analyse.AddData(massByte);
                

                // GetPiezoVoltage  0x0B,
                if (nameEnumCmd == "GetMtInfo")
                {
                    if (!flagVisibleLogBar)
                        flagVisibleLogBar = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagVisibleLogBar"));
                    if (analyse.Lst.Count != 0)
                    {
                        RegUodAsync sss = (RegUodAsync)analyse.LastData;
                        HistoryLoge += "\n";
                        // HistoryLoge += "\nFirstbyte: " + sss.FirstByte.ToString();
                        // HistoryLoge += "\nCMD: " + sss.Cmd.ToString();
                        HistoryLoge += "\n-Состояние блока БС и Фс (1 байт): " + sss.Status.ToString();
                        HistoryLoge += "\n-Состояние реле 1 = " + ((sss.Status & 0x1) == 0 ? "5 Вольт " : "27 Вольт ");
                        HistoryLoge += "\n-Состояние реле 2 = " + (((sss.Status & 0x2) >> 1) == 0 ? "5 Вольт " : "27 Вольт ");
                        HistoryLoge += "\n-Состояние реле магнитный стенд = " + (((sss.Status & 0x4) >> 2) == 0 ? "Выкл" : "Вкл");
                        HistoryLoge += "\n-Обратная полярность магнитного стендa = " + (((sss.Status & 0x8) >> 4) == 0 ? "Выкл" : "Вкл");
                        HistoryLoge += "\n-Номер кольца магнитного стендa = ";
                        switch ((sss.Status & 0x60) >> 5)
                        {
                            case 0:
                                HistoryLoge += "Кольцо Х";
                                break;
                            case 1:
                                HistoryLoge += "Кольцо Y";
                                break;
                            case 2:
                                HistoryLoge += "Кольцо Z";
                                break;
                            case 3:
                                HistoryLoge += "Отключены все кольца";
                                break;
                            default:
                                break;
                        }
                        HistoryLoge += "\n-Режим работы = " + (((sss.Status & 0x80) >> 7) == 0 ? "Без переключения моды" : "С переключением моды");

                        HistoryLoge += "\n-Время появления сигнала РС: " + ((sss.DeviceInfo & 0x3F) * 50 / 1000).ToString() + "сек";
                        HistoryLoge += "\n-Состояние сигнала РС: " + (((sss.DeviceInfo & 0x80) >> 7) == 0 ? "Нет РС" : "Есть Рс");
                        HistoryLoge += "\n-Информация о приборе: " + sss.DeviceInfo.ToString();
                        HistoryLoge += "\n-Значение делителя MU: " + sss.MU.ToString();
                        HistoryLoge += "\n-Мода = " + (sss.Moda == 0 ? "0" : "1");
                        HistoryLoge += "\n-Полуволновое напряжение по X: " + sss.Ul[0].ToString();
                        HistoryLoge += "\n-Полуволновое напряжение по Y: " + sss.Ul[1].ToString();
                        HistoryLoge += "\n-Полуволновое напряжение по Z: " + sss.Ul[2].ToString();
                        HistoryLoge += "\n-Время переключения моды: " + sss.ModaTime.ToString();
                        HistoryLoge += "\n-Ks: " + sss.Ks.ToString();
                        HistoryLoge += "\n";
                    }
                }
                //if (massByte[0]!=null)

            }
            //for (int i = 0; i < 1000000; i++)
            //{

            //}
            Console.WriteLine("ok\n");
            analyse = new AnalyseDataStream<RegUodAsync>();
            //HistoryLoge += "\nAnswer: ";
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    HistoryLoge += arr[i].ToString() + " ";
            //    if (i > 100)
            //        return;
            //}
            //if (sp.IsOpen)
            //    sp.Close();

        }

        public EventHandler EventStopReciveData;



        private void SaveData()
        {
            startSyncReg.EventSaveData?.Invoke(null, null);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == false)
            {
                MessageBoxResult mbr = MessageBox.Show("Вы уверены что не хотите сохранять файл?", "", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    return;
                }
                SaveData();
            }
            if (saveFileDialog.FileName != "")
            {
                if (saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length - 4) == ".uod")
                    File.Copy(RPropertis.DefaultPath, saveFileDialog.FileName,true);
                else
                    File.Copy(RPropertis.DefaultPath, saveFileDialog.FileName + ".uod",true);
                File.Delete(RPropertis.DefaultPath);
            }
        }

        public void SaveResivePropertis(object o)
        {
            WindowRecivePropertis wp = new WindowRecivePropertis();
            wp.DataContext = new RecivePropertis();
            wp.ShowDialog();
            RPropertis = (RecivePropertis)wp.DataContext;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private int srp =0;
        public int Srp
        {
            get { return srp; }
            set { srp = value;
                if (srp < 0)
                    srp = 0;
                if (srp > 1)
                    srp = 1;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Srp"));
            }
        }
        private double uCapX =0;
        public double UCapX
        {
            get { return uCapX; }
            set
            {
                uCapX = value;
                if (uCapX < 0)
                    uCapX = 0;
                if (uCapX > 4095)
                    uCapX = 4095;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UCapX"));
            }
        }
        private double uCapY =0;
        public double UCapY
        {
            get { return uCapY; }
            set
            {
                uCapY = value;
                if (uCapY < 0)
                    uCapY = 0;
                if (uCapY > 4095)
                    uCapY = 4095;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UCapY"));
            }
        }
        private double uCapZ =0;
        public double UCapZ
        {
            get { return uCapZ; }
            set
            {
                uCapZ = value;
                if (uCapZ < 0)
                    uCapZ = 0;
                if (uCapZ > 4095)
                    uCapZ = 4095;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UCapZ"));
            }
        }


        #region VisibleFlags
        private bool enabledButton = true;
        public bool EnabledButton
        {
            get { return enabledButton; }
            set { enabledButton = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnabledButton"));
            }
        }


        private bool reg308 = false;

        public bool Reg308
        {
            get { return reg308; }
            set { reg308 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg308"));
            }
        }
        private bool reg33 = false;
        public bool Reg33
        {
            get { return reg33; }
            set
            {
                reg33 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg33"));
            }
        }
        private bool reg32 = false;
        public bool Reg32
        {
            get { return reg32; }
            set
            {
                reg32 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg32"));
            }
        }
        private bool reg30 = false;
        public bool Reg30
        {
            get { return reg30; }
            set
            {
                reg30 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Reg30"));
            }
        }


        private bool flagEnableButtonSyncroReg = false;
        public bool FlagEnableButtonSyncroReg
        {
            get { return flagEnableButtonSyncroReg; }
            set
            {
                flagEnableButtonSyncroReg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagEnableButtonSyncroReg"));
            }
        }

        private bool flagVisibleLogBar = false;
        public bool FlagVisibleLogBar
        {
            get { return flagVisibleLogBar; }
            set
            {
                flagVisibleLogBar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagVisibleLogBar"));
            }
        }
        private bool flagChooseTypeDevice = true;
        public bool FlagChooseTypeDevice
        {
            get { return flagChooseTypeDevice; }
            set
            {
                flagChooseTypeDevice = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseTypeDevice"));
            }
        }

        private bool flagChooseReversModa = false;
        public bool FlagChooseReversModa
        {
            get { return flagChooseReversModa; }
            set
            {
                flagChooseReversModa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseReversModa"));
            }
        }
        private bool flagChooseDurationModa = false;
        public bool FlagChooseDurationModa
        {
            get { return flagChooseDurationModa; }
            set
            {
                flagChooseDurationModa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseDurationModa"));
            }
        }
        private bool flagChooseFistModa = false;
        public bool FlagChooseFistModa
        {
            get { return flagChooseFistModa; }
            set
            {
                flagChooseFistModa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChooseFistModa"));
            }
        }
        private bool flagChoosePower = false;
        public bool FlagChoosePower
        {
            get { return flagChoosePower; }
            set
            {
                flagChoosePower = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagChoosePower"));
            }
        }
        private bool flagShowDate = false;
        public bool FlagShowDate
        {
            get { return flagShowDate; }
            set
            {
                flagWriteDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagShowDate"));
            }
        }

        private bool flagWriteDate = false;
        public bool FlagWriteDate
        {
            get { return flagWriteDate; }
            set
            {
                flagWriteDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagWriteDate"));
            }
        }

        #endregion

        private bool godsMode = false;

        public bool GodsMode
        {
            get { return godsMode; }
            set {
                godsMode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GodsMode"));
                FlagWriteDate = GodsMode;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlagWriteDate"));
            }
        }

        private string historyLoge;
        public string HistoryLoge
        {
            get { return historyLoge; }
            set
            {
                historyLoge = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HistoryLoge"));
            }

        }

        private double? modaDuration = 30;
        public double? ModaDuration
        {
            get { return modaDuration; }
            set
            {
                modaDuration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ModaDuration"));
            }

        }

        private double? setDelitelMu = 1;
        public double? SetDelitelMu
        {
            get { return setDelitelMu; }
            set
            {
                setDelitelMu = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SetDelitelMu"));
            }

        }


        //       private StringCollection historyLoge = Settings.Default.HistoryLoge;
        //public StringCollection HistoryLoge
        //{
        //    get { return historyLoge; }
        //    set
        //    {
        //        historyLoge = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HistoryLoge"));
        //    }

        //}
    }

}
