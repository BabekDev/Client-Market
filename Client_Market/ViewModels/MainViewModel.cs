using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Market.ViewModels
{
    class MainViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainViewModel()
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Debug("");
        }
    }
}
