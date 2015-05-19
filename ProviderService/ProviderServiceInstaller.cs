using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderService
{
    [RunInstaller(true)]
    public partial class ProviderServiceInstaller : System.Configuration.Install.Installer
    {
        public ProviderServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
