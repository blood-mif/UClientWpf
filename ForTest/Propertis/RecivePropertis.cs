using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UodClientWpf.Properties;

namespace UodClientWpf.ForTest.Propertis
{
    public class RecivePropertis : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> portNameCollection = new ObservableCollection<string>();

        public ObservableCollection<string> PortNameCollection
        {
            get
            {
                var pnList = SerialPort.GetPortNames();
                for (int i = 0; i < pnList.Length; i++)
                    portNameCollection.Add(pnList[i]);

                return portNameCollection;
            }
            set
            {
                portNameCollection = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PortNameCollection"));
            }
        }


        private ObservableCollection<int> baudRateCollection = new ObservableCollection<int>() { 9600, 19200, 38400, 57600, 115200, 921600 };

        public ObservableCollection<int> BaudRateCollection
        {
            get
            {
                return baudRateCollection;
            }
            set
            {

                baudRateCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BaudRateCollection"));
            }
        }


        private ObservableCollection<Parity> parityCollection = new ObservableCollection<Parity>() { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };

        public ObservableCollection<Parity> ParityCollection
        {
            get { return parityCollection; }
            set
            {
                parityCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ParityCollection"));
            }
        }

        private ObservableCollection<int> dataBitsCollection = new ObservableCollection<int>() { 8, 7, 6, 5 };

        public ObservableCollection<int> DataBitsCollection
        {
            get { return dataBitsCollection; }
            set
            {
                dataBitsCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataBitsCollection"));
            }
        }

        private ObservableCollection<StopBits> stopBitsCollection = new ObservableCollection<StopBits>() { StopBits.One, StopBits.None, StopBits.OnePointFive, StopBits.Two };

        public ObservableCollection<StopBits> StopBitsCollection
        {
            get
            {
                return stopBitsCollection;
            }
            set
            {
                stopBitsCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StopBitsCollection"));
            }
        }

        private ObservableCollection<Handshake> handshakesCollection = new ObservableCollection<Handshake>() { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };

        public ObservableCollection<Handshake> HandshakesCollection
        {
            get { return handshakesCollection; }
            set
            {
                handshakesCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HandshakesCollection"));
            }
        }

        private string portName = Settings.Default.PortName;

        public string PortName
        {
            get { return portName; }
            set
            {
                portName = value;
                Settings.Default.PortName = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PortName"));
            }
        }

        private int baudRate = Settings.Default.BaudRate;

        public int BaudRate
        {
            get { return baudRate; }
            set
            {
                baudRate = value;
                Settings.Default.BaudRate = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BaudRate"));
            }
        }

        private Parity parity = Settings.Default.Parity;

        public Parity Parity
        {
            get { return parity; }
            set
            {
                parity = value;
                Settings.Default.Parity = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Parity"));
            }
        }

        private int dataBits = Settings.Default.DataBits;

        public int DataBits
        {
            get { return dataBits; }
            set
            {
                dataBits = value;
                Settings.Default.DataBits = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataBits"));
            }
        }

        private StopBits stopBits = Settings.Default.StopBits;

        public StopBits StopBits
        {
            get { return stopBits; }
            set
            {
                stopBits = value;
                Settings.Default.StopBits = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StopBits"));
            }
        }

        private Handshake handshake = Settings.Default.Handshake;

        public Handshake Handshake
        {
            get { return handshake; }
            set
            {
                handshake = value;
                Settings.Default.Handshake = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Handshake"));
            }
        }

       // private string defaultPath = Settings.Default.DefaultPath;

        private string defaultPath = Directory.GetCurrentDirectory() + "\\dateFile.mki";

        public string DefaultPath
        {
            get { return defaultPath; }
            set { 
                defaultPath = value;
                Settings.Default.DefaultPath = value;
                Settings.Default.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DefaultPath"));

            }
        }


        private SerialPort serialPort;

        public SerialPort SerialPort
        {
            get
            {
                if (serialPort == null)
                    serialPort = new SerialPort();

                serialPort.PortName = Settings.Default.PortName;
                serialPort.BaudRate = Settings.Default.BaudRate;
                serialPort.DataBits = Settings.Default.DataBits;
                serialPort.StopBits = Settings.Default.StopBits;
                serialPort.Parity = Settings.Default.Parity;
                serialPort.Handshake = Settings.Default.Handshake;

                return serialPort;
            }
            set
            {
                serialPort = value;
            }
        }

    }
}
