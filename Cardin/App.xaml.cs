using Cardin.Helper;
using Cardin.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cardin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public static RegistrationModel facilityRegistration = new RegistrationModel();
        public static IGlobalValueIndicator globalValueIndicator = new IGlobalValueIndicator();

    }
}
