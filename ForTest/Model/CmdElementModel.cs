using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using PpdControls.Commands;
using System.Collections.Generic;

namespace UodClientWpf.ForTest.Model
{
    public class CmdElementModel
    {
        public List<CmdElementModel> NextElemtnts = new List<CmdElementModel>();
        public CmdElementModel PrefElement;
        public EnumUodCmds EnumUodCmds;
        public ClassCommand Command;
        public CmdElementModel(EnumUodCmds enumUodCmds)
        {
            EnumUodCmds = enumUodCmds;
        }
        public CmdElementModel()
        {

        }
        public void AddElement(EnumUodCmds enumUodCmds)
        {
            var fff = new CmdElementModel(enumUodCmds);
            fff.PrefElement = this;
            NextElemtnts.Add(fff);
        }
        public void AddElement(CmdElementModel cmdElementModel)
        {
            cmdElementModel.PrefElement = this;
            NextElemtnts.Add(cmdElementModel);

        }
    }
}
