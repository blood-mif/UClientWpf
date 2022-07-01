using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UodClientWpf.ForTest.Propertis;

namespace UodClientWpf.ForTest
{
    public class Connect
    {
        public Connect(RecivePropertis rp)
        {
            _rp = rp;
        }
        RecivePropertis _rp;



        public void ConnectOn()
        {
            _rp.SerialPort.Open();
        }
        public void ConnectOff()
        {
            _rp.SerialPort.Close();
        }
    }
}
