using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Market.ViewModels
{
    class LoggerDebug
    {
        private delegate void LoggerDelegate(string value);
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static LoggerDelegate loggerDelegate;
        private static LoggerDebug logger;
        private LoggerDebug() { log4net.Config.XmlConfigurator.Configure(); }

        public static void getInstance(string value)
        {
            if (logger == null) { logger = new LoggerDebug(); }
            log4net.Config.XmlConfigurator.Configure();
            loggerDelegate = log.Debug;
            loggerDelegate?.Invoke(value);
        }
    }
}
