using System;
using NLog;

namespace Payments.Common.NLog
{
    public static class NLog
    {
        public static void LogTrace(Type declaringType, string message)
        {
            LogManager.GetLogger(declaringType.FullName).Trace(message);
        }

        public static void LogDebug(Type declaringType, string message)
        {
            LogManager.GetLogger(declaringType.FullName).Debug(message);
        }

        public static void LogInfo(Type declaringType, string message)
        {
            LogManager.GetLogger(declaringType.FullName).Info(message);
        }

        public static void LogWarn(Type declaringType, string message)
        {
            LogManager.GetLogger(declaringType.FullName).Warn(message);
        }

        public static void LogError(Type declaringType, string message)
        {
            LogManager.GetLogger(declaringType.FullName).Error(message);
        }

        public static void LogFatal(Type declaringType, string message)
        {
            LogManager.GetLogger(declaringType.FullName).Fatal(message);
        }
    }
}