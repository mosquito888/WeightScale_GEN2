using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WeightScaleGen2.BGC.Models.DBModels;

namespace WeightScaleGen2.BGC.API.Common.Logger
{
    public class DatabaseLogger : ILogger
    {
        IDatabaseConnectionFactory _db;
        ISecurityCommon _securityCommon;
        private readonly IHttpContextAccessor _context;

        public DatabaseLogger(IDatabaseConnectionFactory db, ISecurityCommon securityCommon, IHttpContextAccessor context)
        {
            _db = db;
            _securityCommon = securityCommon;
            _context = context;
        }

        private enum Level
        {
            activity,
            info,
            warning,
            error
        };

        public void WriteError(int errorCode, string errorMessage, string additionalInfo, Exception exception,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(errorMessage, Level.error, memberName, sourceFilePath, sourceLineNumber);
            log.log_inner_exception = exception?.InnerException?.ToString();
            log.log_error_code = errorCode;
            log.log_exception_message = exception?.Message;
            log.log_additional_Info = additionalInfo;
            log.log_type = exception?.GetType().Name;
            log.log_stack_trace = exception?.StackTrace;
            WriteOutput(log);
            SaveLog(log);
        }

        public void WriteInfo(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(message, Level.info, memberName, sourceFilePath, sourceLineNumber);
            WriteOutput(log);
            SaveLog(log);
        }

        public void WriteActivity(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(message, Level.activity, memberName, sourceFilePath, sourceLineNumber);
            WriteOutput(log);
            SaveLog(log);
        }

        public void WriteWarning(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(message, Level.warning, memberName, sourceFilePath, sourceLineNumber);
            WriteOutput(log);
            SaveLog((log));
        }

        public void WriteError(Exception exception, string user, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void WriteError(int errorCode, string errorMessage, string additionalInfo, Exception exception, string user, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(errorMessage, Level.error, memberName, sourceFilePath, sourceLineNumber, user);
            log.log_inner_exception = exception?.InnerException?.ToString();
            log.log_error_code = errorCode;
            log.log_exception_message = exception?.Message;
            log.log_additional_Info = additionalInfo;
            log.log_type = exception?.GetType().Name;
            log.log_stack_trace = exception?.StackTrace;
            WriteOutput(log);
            SaveLog(log);
        }

        public void WriteInfo(string message, string user, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(message, Level.info, memberName, sourceFilePath, sourceLineNumber, user);
            WriteOutput(log);
            SaveLog(log);
        }

        public void WriteActivity(string message, string user, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(message, Level.activity, memberName, sourceFilePath, sourceLineNumber, user);
            WriteOutput(log);
            SaveLog(log);
        }

        public void WriteWarning(string message, string user, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var log = InitializeLog(message, Level.warning, memberName, sourceFilePath, sourceLineNumber, user);
            WriteOutput(log);
            SaveLog((log));
        }

        private LogData InitializeLog(string message, Level level, string memberName, string sourceFile, int lineNumber)
        {
            var remoteIpAddress = _context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var log = new LogData
            {
                log_message = message,
                log_additional_Info = message,
                log_caller_member_name = memberName,
                log_source_line_number = lineNumber.ToString(),
                log_caller_file_path = sourceFile,
                log_user = null,
                log_ip_address = remoteIpAddress,
                log_level = level.ToString(),
                log_date = DateTime.Now
            };

            return log;
        }

        private LogData InitializeLog(string message, Level level, string memberName, string sourceFile, int lineNumber, string user)
        {
            var remoteIpAddress = _context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var log = new LogData
            {
                log_message = message,
                log_additional_Info = message,
                log_caller_member_name = memberName,
                log_source_line_number = lineNumber.ToString(),
                log_caller_file_path = sourceFile,
                log_user = user,
                log_ip_address = remoteIpAddress,
                log_level = level.ToString(),
                log_date = DateTime.Now
            };

            return log;
        }

        private void SaveLog(LogData log)
        {
            WriteOutput(log);
            WriteDatbase(log);
        }

        private void WriteOutput(LogData log)
        {
            Debug.WriteLine($"[DATE]              : {log.log_date}");
            Debug.WriteLine($"[LEVEL]             : {log.log_level.ToUpper()}");
            Debug.WriteLine($"[MESSAGE]           : {log.log_message}");
            Debug.WriteLine($"[LINE]              : {log.log_source_line_number}");
            Debug.WriteLine($"[METHOD]            : {log.log_caller_member_name}");
            Debug.WriteLine($"[FILE]              : {log.log_caller_file_path}");

            if (log.log_level == Level.error.ToString())
            {
                Debug.WriteLine($"[ERROR CODE]         : {log.log_error_code}");
                Debug.WriteLine($"[EXCEPTION TYPE]     : {log.log_type}");
                Debug.WriteLine($"[EXCEPTION MESSAGE]  : {log.log_exception_message}");
                Debug.WriteLine($"[INNER EXCEPTION]    : {log.log_inner_exception}");
                Debug.WriteLine($"[ADDITIONAL INFO]    : {log.log_additional_Info}");
                Debug.WriteLine($"[STACKTRACE]         : {log.log_stack_trace}");
            }
            Debug.WriteLine("*------------------------------------------------------------------------------*\n");
        }

        private async void WriteDatbase(LogData log)
        {
            using var conn = await _db.CreateConnectionAsync();
            var p = new DynamicParameters();
            p.Add("@log_level", log.log_level);
            p.Add("@log_type", log.log_type);
            p.Add("@log_error_code", log.log_error_code);
            p.Add("@log_date", log.log_date);
            p.Add("@log_message", log.log_message);
            p.Add("@log_inner_exception", log.log_inner_exception);
            p.Add("@log_exception_message", log.log_exception_message);
            p.Add("@log_additional_Info", log.log_additional_Info);
            p.Add("@log_caller_member_name", log.log_caller_member_name);
            p.Add("@log_stack_trace", log.log_stack_trace);
            p.Add("@log_caller_file_path", log.log_caller_file_path);
            p.Add("@log_source_line_number", log.log_source_line_number);
            p.Add("@log_user", log.log_user);
            p.Add("@log_ip_address", log.log_ip_address);

            var query = @"
                            -- deleted before 7 day
                            DELETE sy_log WHERE CONVERT(DATE, log_date) < CONVERT(DATE, DATEADD(DAY, -7, GETDATE()))
                            
                            INSERT INTO dbo.sy_log
                            (
                              log_level
                             ,log_type
                             ,log_error_code
                             ,log_date
                             ,log_message
                             ,log_inner_exception
                             ,log_exception_message
                             ,log_additional_Info
                             ,log_caller_member_name
                             ,log_stack_trace
                             ,log_caller_file_path
                             ,log_source_line_number
                             ,log_user
                             ,log_ip_address
                            )
                            VALUES
                            (
                              @log_level                            -- log_level - nvarchar(50)
                             ,@log_type                             -- log_type - nvarchar(50)
                             ,@log_error_code                       -- log_error_code - int
                             ,@log_date                             -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- log_date - datetime
                             ,@log_message                          -- log_message - nvarchar(4000)
                             ,@log_inner_exception                  -- log_inner_exception - nvarchar(4000)
                             ,@log_exception_message                -- log_exception_message - nvarchar(4000)
                             ,@log_additional_Info                  -- log_additional_Info - nvarchar(4000)
                             ,@log_caller_member_name               -- log_caller_member_name - nvarchar(4000)
                             ,@log_stack_trace                      -- log_stack_trace - nvarchar(4000)
                             ,@log_caller_file_path                 -- log_caller_file_path - nvarchar(4000)
                             ,@log_source_line_number               -- log_source_line_number - nvarchar(50)
                             ,@log_user                             -- log_user - nvarchar(50)
                             ,@log_ip_address                       -- log_ip_address - nvarchar(50)
                            );
                        ";

            using (var trans = conn.BeginTransaction())
            {
                conn.Execute(query, p, trans);
                trans.Commit();
            }
            conn.Close();
        }

    }
}
