using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyLog
{
    public long LogId { get; set; }

    public string LogLevel { get; set; }

    public string LogType { get; set; }

    public int? LogErrorCode { get; set; }

    public DateTime? LogDate { get; set; }

    public string LogMessage { get; set; }

    public string LogInnerException { get; set; }

    public string LogExceptionMessage { get; set; }

    public string LogAdditionalInfo { get; set; }

    public string LogCallerMemberName { get; set; }

    public string LogStackTrace { get; set; }

    public string LogCallerFilePath { get; set; }

    public string LogSourceLineNumber { get; set; }

    public string LogUser { get; set; }

    public string LogIpAddress { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
