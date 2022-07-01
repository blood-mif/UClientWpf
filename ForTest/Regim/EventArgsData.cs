using L_412_StrData.Regims;
using System;

namespace UodClientWpf.ForTest.Regim
{
    public class EventArgsData : EventArgs
    {
        public string CommandName;
        public BaseReg Reg;
        public int Skip;
        public int Error;
        public EventArgsData( string commandName, BaseReg reg,int skip,int error)
        {
            CommandName = commandName;
            Reg = reg;
        }
        public EventArgsData(string commandName)
        {
            CommandName = commandName;
        }
    } 
}
