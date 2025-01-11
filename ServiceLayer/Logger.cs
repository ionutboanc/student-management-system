using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace ServiceLayer
{
    public static class Logger
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static Logger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }

        public static void LogError(string message, Exception ex)
        {
            logger.Error(message, ex);
        }

        public static void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public static void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}