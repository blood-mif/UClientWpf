using BaseNetworkClient.Connection;
using BaseNetworkClient.Базовые_режимы;
using L_412_StrData;
using L_412_StrData.Regims;
using L_412_StrData.Regims._9B925.Reg308;
using L_412_StrData.Regims.Analyse;
using L_412_StrData.Regims.IIbRegims.Answers;
using L_412_StrData.Regims.IIbRegims.Data;
using L_412_StrData.Regims.Uod;
using L_412_StrData.Regims.Uod.Answers.L_412_StrData;
using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UodClientWpf.ForTest.Propertis;
using UodClientWpf.ForTest.View;
using UodClientWpf.ForTest.ViewModel;
using UodClientWpf.Reg;

namespace UodClientWpf.ForTest.Regim
{
    public class RegSync : ClassSeparateThread 
    {

        public RegSync(ClassBaseConnection conn,SerialPort serialPort, EnumUodCmds enumCmd,bool syncFlag = true, string path = null, bool godsMode = false)
        {
            sp = serialPort;
            cmd = new ClassUodQuest(enumCmd);
            _path = path;
            var vm = new ViewModelSync();
            _godsMode = godsMode;
            EventSendSyncData += vm.UpdeateData;
            ViewModel = vm;
            SyncFlag = syncFlag;
            basePath = path;
            EventStartReciveData += StartRecive;
            EventSaveData += SaveData;    

        }
        public EventHandler EventStartReciveData;
        public RegSync()
        {

        }
        bool _godsMode;
        bool work = true;
        string basePath;
        private ClassUodQuest cmd;
        SerialPort sp;
        private string _path;
       // ClassBaseConnection _conn;
        public bool SyncFlag;
        public EventHandler EventSendSyncData;
        public EventHandler EventSendReg33Data;
        public EventHandler EventStopSyncData;
        public EventHandler EventSaveData;
        AutoResetEvent waitEndCycle = new AutoResetEvent(false);
        AnalyseDataStream<Reg308A> analyse;

        public bool reciveData = false;
        public bool saveDate = false;
        public int Skip;
        public int Error;

        public void StartRecive(object o, EventArgs e)
        {
            reciveData = true;
        }      
        public void SaveData(object o, EventArgs e)
        {
            saveDate = true;
        }
        public void StopAnalyse(object o, EventArgs e)
        {
            work = false;
        }
        private void chooseTypeReg<Reg>(object o,EventArgs e) where Reg : BaseReg, new()
        {

            var analyse = new AnalyseDataStream<Reg>();
            if (_godsMode == true)
            {
                reciveData = true;

            }
            //else
            //{
            //analyse = new AnalyseDataStream<RegUodSync>();

            //}

            DateTime lastViewTimer = DateTime.Now;

            List<BaseReg> CurrLst = new List<BaseReg>();
            work = true;
            while (work)
            {
                if (!sp.IsOpen)
                    sp.Open();
                DateTime beginData = DateTime.Now;
                DateTime currData = DateTime.Now;
                //while ( sp.BytesToRead > 0 && (currData - beginData).TotalSeconds < 1)
                {
                    //while (analyse.Lst.IsEmpty == true && (currData - beginData).TotalSeconds < 1)
                    //{
                    if (reciveData)
                    {
                        analyse = new AnalyseDataStream<Reg>(basePath, FileMode.Append);
                        reciveData = false;
                    }
                    if (saveDate)
                    {
                        analyse.CloseStream();
                        analyse = new AnalyseDataStream<Reg>();
                        saveDate = false;
                    }
                    currData = DateTime.Now;
                    int size = sp.BytesToRead;
                    if (size != 0)
                        Console.WriteLine(size);
                    byte[] massByte = new byte[size];
                    sp.Read(massByte, 0, massByte.Length);
                    analyse.AddData(massByte);
                    //((ViewModelSync)ViewModel).UpdeateData(analyse.LastData,null);
                    if (analyse.Lst.Count == 0)
                        continue;
                    var aa = analyse.LastData;
                    Skip = analyse.Skip;
                    Error = analyse.ErrKs;
                    //CurrLst.Add(aa);
                    //analyse.Lst.TryDequeue(out BaseReg regAns);

                    if ((currData - lastViewTimer).TotalMilliseconds > 100)
                    {
                        //EventSendSyncData?.Invoke(CurrLst.Last(), null);

                        EventSendSyncData?.Invoke(aa, new EventArgsData(cmd.Cmd.ToString(), aa, Skip, Error));
                        lastViewTimer = DateTime.Now;
                        CurrLst = new List<BaseReg>();
                    }
                    analyse.WriteToStream();
                }
            }
            analyse.CloseStream();

        }
        protected override void ExecuteFunc(object o, EventArgs e)
        {
            switch (cmd.Cmd)
            {
                case EnumUodCmds.PowerOnWidthRsCtrl:
                case EnumUodCmds.PowerOnWidthoutRsCtrl:
                case EnumUodCmds.StartSyncro501:
                case EnumUodCmds.StartSyncro401:
                case EnumUodCmds.StartSyncro601:
                case EnumUodCmds.StartSyncro601LowSpeed:
                    chooseTypeReg <Reg308A>(o, e);
                    break;
                case EnumUodCmds.Reg30:
                    chooseTypeReg<Reg30A>(o, e);
                    break;
                case EnumUodCmds.Reg32:
                    chooseTypeReg<Reg32A>(o, e);
                    break;
                case EnumUodCmds.Reg33:
                    chooseTypeReg<Reg33A>(o, e);
                    break;
                default:
                    break;
            }



            //if (cmd.Cmd == EnumUodCmds.Reg30 || cmd.Cmd == EnumUodCmds.Reg32 || cmd.Cmd == EnumUodCmds.Reg33)
            //{
            //    while (true)
            //    {
            //        ClassDataIib classDataIib = new ClassDataIib();
            //        int size = sp.BytesToRead;
            //        byte[] massByte = new byte[size];
            //        sp.Read(massByte, 0, massByte.Length);
            //        classDataIib.AddData(massByte);
            //        //classDataIib.Where(classdata => classdata.Cmd == 32).Last();
            //        if (classDataIib.Length == 0)
            //            continue;
            //        EventSendSyncData?.Invoke(classDataIib, new EventArgsData(cmd.Cmd.ToString()));
            //    }
            //}

            // rp.Close();
            //analyse = new AnalyseDataStream<RegUodSync>(_path, FileMode.Create);
            //var startSyncRegime = new ASyncUodRegime(cmd.Cmd, _conn, null) { CmdWaitTimeout = 2000 };
            //startSyncRegime.Start();
            //startSyncRegime.Stop();
            //rp.SerialPort.Write
        }



    }
}
