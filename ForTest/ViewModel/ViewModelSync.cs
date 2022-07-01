using BaseNetworkClient.Базовые_режимы;
using L_412_StrData;
using L_412_StrData.Regims;
using L_412_StrData.Regims._9B925.Reg308;
using L_412_StrData.Regims.IIbRegims.Answers;
using L_412_StrData.Regims.IIbRegims.Data;
using L_412_StrData.Regims.IIbRegims.Interface;
using L_412_StrData.Regims.Uod.Answers.L_412_StrData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UodClientWpf.ForTest.Regim;

namespace UodClientWpf.ForTest.ViewModel
{
    public class ViewModelSync : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelSync()
        {
            stopWatch = new Stopwatch();
            stopWatch.Stop();
        }

        public Stopwatch stopWatch;

        private void ViewTd(Reg308A reg)
        {

                        TKorp = Convert.ToInt32(reg.KorpKey.Value);
                        TXCold = Convert.ToInt32(reg.GyrTdKeyCold[0]);
                        TYCold = Convert.ToInt32(reg.GyrTdKeyCold[1]);
                        TZCold = Convert.ToInt32(reg.GyrTdKeyCold[2]);
                        UPkX = Convert.ToInt32(reg.Upk[0]);
                        UPkY = Convert.ToInt32(reg.Upk[1]);
                         UPkZ = Convert.ToInt32(reg.Upk[2]);
                        TXhot = Convert.ToInt32(reg.GyrTdKeyHot[0]);
                        TYhot = Convert.ToInt32(reg.GyrTdKeyHot[1]);
                        TZhot = Convert.ToInt32(reg.GyrTdKeyHot[2]);
        }

        public void UpdeateData(object o, EventArgs e)
        {

            stopWatch.Start();
            Time = ($"{stopWatch.Elapsed.Hours}:{stopWatch.Elapsed.Minutes}:{stopWatch.Elapsed.Seconds}");
            // RegUodSync data = (RegUodSync)o;
            // разобраться зачем использовать списо, если берётся последняя посылка
            //List<BaseReg> lst1 = (List<BaseReg>)o;
            //List<Reg308A> lst1 = (List<Reg308A>)o;
            var dataEvent = e as EventArgsData;
            if (dataEvent != null)
            {
                Skip = dataEvent.Skip;
                Error = dataEvent.Error;
                switch (dataEvent.CommandName)
                {
                    case "PowerOnWidthRsCtrl":
                    case "PowerOnWidthoutRsCtrl":
                        UpdateDateReg308(o);
                        break;
                    case "Reg33":
                        //var ff = ((ClassDataIib)o).FirstOrDefault(classdata => classdata.Cmd == 33).Regime;
                        UpdateDateReg33(o);
                        break;
                    case "Reg32":
                        UpdateDateReg32(o);
                        break;
                    case "Reg30":
                        UpdateDateReg30(o);
                        break;
                    default:
                        break;
                }
            }


        }
        public void UpdateDateReg308(object o)
            {
            Reg308A data = o as Reg308A;

            //var lst = lst1.Cast<RegUodSync>().ToList();
            //RegUodSync data = lst.Last();

            ViewTd(data);

                if (data.Rs == 0)
                {
                    RsTimeAns = ($"{stopWatch.Elapsed.Minutes}:{stopWatch.Elapsed.Seconds}:{stopWatch.Elapsed.Milliseconds}");
                }

                N = data.N;
                Moda = data.Moda;
                MmPhase = data.MmPhase;
                Ks = data.Ks;
                // Cap =       data.;
                Ksf = data.Ksf;
                // KeyValue =  data.key;
                // SensorNum = data.SensorNum;
                GirSummX = data.SummX;
                GirSummY = data.SummY;
                GirSummZ = data.SummZ;
                GirRasnX = data.RasnX;
                GirRasnY = data.RasnY;
                GirRasnZ = data.RasnZ;
            } 
       // Reg308A UpdeateDataReg308()
        public void UpdateDateReg33(object o)
        {
            
            Reg33A data = (Reg33A)o;
            AcpLineX = data.K_ACPx[0];
            AcpConstantX = data.K_ACPx[1];
            AcpLineY = data.K_ACPy[0];
            AcpConstantY = data.K_ACPy[1];
            AcpLineZ = data.K_ACPz[0];
            AcpConstantZ = data.K_ACPz[1];

            CapQuadraticX = data.K_CAPx[0];
            CapQuadraticY = data.K_CAPy[0];
            CapQuadraticZ = data.K_CAPz[0];
            CapLineX = data.K_CAPx[1];
            CapLineY = data.K_CAPy[1];
            CapLineZ = data.K_CAPz[1];
            CapConstantX = data.K_CAPx[2];
            CapConstantY = data.K_CAPy[2];
            CapConstantZ = data.K_CAPz[2];
        }
        public void UpdateDateReg32(object o)
        {

            Reg32A data = (Reg32A)o;
            KeyAcpX = data.KeyAcp[0];
            KeyAcpY = data.KeyAcp[1];
            KeyAcpZ = data.KeyAcp[2];
            KeyCapX = data.KeyCap[0];
            KeyCapY = data.KeyCap[1];
            KeyCapZ = data.KeyCap[2];
            MesureUX = data.MesureU[0];
            MesureUY = data.MesureU[1];
            MesureUZ = data.MesureU[2];
        }
        public void UpdateDateReg30(object o)
        {
            Reg30A data = (Reg30A)o;
            Cmd = data.Cmd;
            Crp = data.Crp;
            Status = data.Status;
        }
        private short cmd;
        public short Cmd
        {
            get { return cmd; }
            set { cmd = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cmd"));
            }
        }
        private short status;
        public short Status
        {
            get { return status; }
            set
            {
                status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
            }
        }
        private short crp;
        public short Crp
        {
            get { return crp; }
            set
            {
                crp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Crp"));
            }
        }

        private double keyAcpX;
        public double KeyAcpX
        {
            get { return keyAcpX; }
            set { keyAcpX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyAcpX"));
            }
        }

        private double keyAcpY;
        public double KeyAcpY
        {
            get { return keyAcpY; }
            set
            {
                keyAcpY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyAcpY"));
            }
        }

        private double keyAcpZ;
        public double KeyAcpZ
        {
            get { return keyAcpZ; }
            set
            {
                keyAcpZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyAcpZ"));
            }
        }
        private double keyCapX;
        public double KeyCapX
        {
            get { return keyCapX; }
            set
            {
                keyCapX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyCapX"));
            }
        }
        private double keyCapY;
        public double KeyCapY
        {
            get { return keyCapY; }
            set
            {
                keyCapY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyCapY"));
            }
        }
        private double keyCapZ;
        public double KeyCapZ
        {
            get { return keyCapZ; }
            set
            {
                keyCapZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyCapZ"));
            }
        }
        private double mesureUX;
        public double MesureUX
        {
            get { return mesureUX; }
            set
            {
                mesureUX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MesureUX"));
            }
        }
        private double mesureUZ;
        public double MesureUZ
        {
            get { return mesureUZ; }
            set
            {
                mesureUZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MesureUZ"));
            }
        }
        private double mesureUY;
        public double MesureUY
        {
            get { return mesureUY; }
            set
            {
                mesureUY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MesureUY"));
            }
        }

        private int skip;
        public int Skip
        {
            get { return skip; }
            set
            {
                skip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Skip"));
            }
        }
        private int error;
        public int Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }
        private float acpLineX;
        public float AcpLineX
        {
            get { return acpLineX; }
            set
            {
                acpLineX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpLineX"));
            }
        }
        private float acpLineY;
        public float AcpLineY
        {
            get { return acpLineY; }
            set
            {
                acpLineY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpLineY"));
            }
        }
        private float acpLineZ;
        public float AcpLineZ
        {
            get { return acpLineZ; }
            set
            {
                acpLineZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpLineZ"));
            }
        }

        private float acpConstantX;
        public float AcpConstantX
        {
            get { return acpConstantX; }
            set
            {
                acpConstantX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpConstantX"));
            }
        }
        private float acpConstantY;
        public float AcpConstantY
        {
            get { return acpConstantY; }
            set
            {
                acpConstantY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpConstantY"));
            }
        }
        private float acpConstantZ;
        public float AcpConstantZ
        {
            get { return acpConstantZ; }
            set
            {
                acpConstantZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpConstantZ"));
            }
        }

        private float capQuadraticX;
        public float CapQuadraticX
        {
            get { return capQuadraticX; }
            set
            {
                capQuadraticX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapQuadraticX"));
            }
        }
        private float capQuadraticY;
        public float CapQuadraticY
        {
            get { return capQuadraticY; }
            set
            {
                capQuadraticY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapQuadraticY"));
            }
        }
        private float capQuadraticZ;
        public float CapQuadraticZ
        {
            get { return capQuadraticZ; }
            set
            {
                capQuadraticZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapQuadraticZ"));
            }
        }
        private float capLineX;
        public float CapLineX
        {
            get { return capLineX; }
            set
            {
                capLineX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapLineX"));
            }
        }
        private float capLineY;
        public float CapLineY
        {
            get { return capLineY; }
            set
            {
                capLineY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapLineY"));
            }
        }
        private float capLineZ;
        public float CapLineZ
        {
            get { return capLineZ; }
            set
            {
                capLineZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapLineZ"));
            }
        }

        private float capConstantX;
        public float CapConstantX
        {
            get { return capConstantX; }
            set
            {
                capConstantX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapConstantX"));
            }
        }
        private float capConstantY;
        public float CapConstantY
        {
            get { return capConstantY; }
            set
            {
                capConstantY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapConstantY"));
            }
        }
        private float capConstantZ;
        public float CapConstantZ
        {
            get { return capConstantZ; }
            set
            {
                capConstantZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapConstantZ"));
            }
        }
        //struct sinchr401
        //{
        //    uint8_t beginByte;
        //    uint8_t tagSync;
        //    uint8_t stateDevice;        //struct DeviceState
        //    uint16_t GirRasnX;
        //    uint16_t GirSumX;
        //    uint16_t GirRasnY;
        //    uint16_t GirSumY;
        //    uint16_t GirRasnZ;
        //    uint16_t GirSumZ;

        //    uint8_t UpkOrTermodatchikiXYZ;      //struct UpkTdXYZ
        //    uint16_t KeyValueACP;
        //    uint16_t CAPSrp;
        //    uint8_t Ks;
        //};

        private int tKorp;
        public int TKorp
        {
            get { return tKorp; }
            set { tKorp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TKorp"));
            }
        }

        private int tXCold;
        public int TXCold
        {
            get { return tXCold; }
            set
            {
                tXCold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TXCold"));
            }
        }

        private int tYCold;
        public int TYCold
        {
            get { return tYCold; }
            set
            {
                tYCold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TYCold"));
            }
        }

        private int tZCold;
        public int TZCold
        {
            get { return tZCold; }
            set
            {
                tZCold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TZCold"));
            }
        }


        private int tXhot;
        public int TXhot
        {
            get { return tXhot; }
            set { tXhot = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TXhot"));
            }
        }

        private int tYhot;
        public int TYhot
        {
            get { return tYhot; }
            set
            {
                tYhot = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TYhot"));
            }
        }

        private int tZhot;
        public int TZhot
        {
            get { return tZhot; }
            set
            {
                tZhot = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TZhot"));
            }
        }

        private int uPkX;
        public int UPkX
        {
            get { return uPkX; }
            set { uPkX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UPkX"));
            }
        }

        private int uPkY;
        public int UPkY
        {
            get { return uPkY; }
            set
            {
                uPkY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UPkY"));
            }
        }
        private int uPkZ;
        public int UPkZ
        {
            get { return uPkZ; }
            set
            {
                uPkZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UPkZ"));
            }
        }

        private string time;

        public string Time
        {
            get { return time; }
            set { time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }

        private string rsTimeAns;

        public string RsTimeAns
        {
            get { return rsTimeAns; }
            set { rsTimeAns = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RsTimeAns"));
            }
        }

        private int n;
        public int N
        {
            get { return n; }
            set
            {
                n = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("N"));
            }
        }

        private int moda;
        public int Moda
        {
            get { return moda; }
            set
            {
                moda = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Moda"));
            }
        }

        private int mmPhase;
        public int MmPhase
        {
            get { return mmPhase; }
            set
            {
                mmPhase = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MmPhase"));
            }
        }

        private int ks;
        public int Ks
        {
            get { return ks; }
            set
            {
                ks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ks"));
            }
        }

        private double girRasnX;
        public double GirRasnX
        {
            get { return girRasnX; }
            set 
            {
                girRasnX = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("GirRasnX"));
            }
        }

        private double girRasnY;
        public double GirRasnY
        {
            get { return girRasnY; }
            set
            {
                girRasnY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GirRasnY"));
            }
        }

        private double girRasnZ;
        public double GirRasnZ
        {
            get { return girRasnZ; }
            set
            {
                girRasnZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GirRasnZ"));
            }
        }

        private double girSummX;
        public double GirSummX
        {
            get { return girSummX; }
            set
            {
                girSummX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GirSummX"));
            }
        }

        private double girSummY;
        public double GirSummY
        {
            get { return girSummY; }
            set
            {
                girSummY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GirSummY"));
            }
        }

        private double girSummZ;
        public double GirSummZ
        {
            get { return girSummZ; }
            set
            {
                girSummZ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GirSummZ"));
            }
        }

        private int sensorNum;
        public int SensorNum
        {
            get { return sensorNum; }
            set
            {
                sensorNum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SensorNum"));
            }
        }

        private int keyValue;
        public int KeyValue
        {
            get { return keyValue; }
            set
            {
                keyValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KeyValue"));
            }
        }

        private int cap;
        public int Cap
        {
            get { return cap; }
            set 
            {
                cap = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cap"));
            }
        }

        private int ksf;
        public int Ksf
        {
            get { return ksf; }
            set 
            {
                ksf = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ksf"));
            }
        }

    }
}
