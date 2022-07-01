using L_412_StrData.Regims.Analyse;
using L_412_StrData.Regims.Uod;
using L_412_StrData.Regims.Uod.Answers;
using L_412_StrData.Regims.Uod.Quests.MagneticCommands;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UodClientWpf.ForTest;
using UodClientWpf.ForTest.ViewModel;
using UodClientWpf.View;

namespace UodClientWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            //   TestParameters tp = new TestParameters();
        }

    }
}
