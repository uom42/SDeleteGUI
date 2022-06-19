#nullable enable

global using NLog;

using System.Linq;
using System.Xml.Linq;

using NLog.Targets;
using NLog.Targets.Wrappers;


#pragma warning disable IDE1006 // Naming Styles

namespace uom.Extensions

{
	internal static class Extensions_NLog
	{

		#region NLog.config file sample

		/*

		<?xml version="1.0" encoding="utf-8" ?>
		<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
			  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			  xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
			  autoReload="true"
			  throwExceptions="false"
			  internalLogLevel="Off"
			  internalLogFile="${tempdir}/UOM_PCAdminCompanion/nlog-internal.log">

			<!-- https://github.com/nlog/nlog/wiki/Configuration-file -->
			<targets async="true">
		 
				<target xsi:type="File" name="f" encoding="utf-8"
						fileName="${tempdir}/UOM/PCAdminCompanion/PCAdminCompanion_${shortdate}.log"
						layout="DATE=${longdate}|LEVEL=${uppercase:${level}}|PC=${machinename}|[PROCESS_ID=${processid}]|THREAD=${threadname}|LOCATION=${callsite:className=true:includeSourcePath=true:methodName=true}|${logger}|${message}|${exception:format=tostring,StackTrace}"
				
						maxArchiveFiles="20"
						/>
			</targets>

			<rules>
				<!-- Trace/Debug/Info/Warn/Error/Fatal -->
				<logger name="*" minlevel="Debug" writeTo="f" />
			</rules>
		</nlog>

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
			  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			  xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
			  autoReload="true"
			  throwExceptions="false"
			  internalLogLevel="Info" internalLogFile="C:\logs\nlog-internal.log"
			  keepVariablesOnReload="true" >

		  <extensions>
			<add assembly="NLog.Extended"/>
		  </extensions>

		  <variable name="SeqEndpointUrl" value="${appsetting:name=seq-endpoint-url}"/>

		  <targets async="true">
			<!-- some other targets here -->
			<target name="seq" type="Seq" serverUrl="${SeqEndpointUrl}" apiKey="some api key">
			  <property name="Source" value="${appsetting:name=seq-source}" />
			  <property name="Login" value="${identity:authType=false}" />
			  <property name="level" value="${level}" />
			  <property name="Guid" value="${event-context:Guid}" />
			  <property name="Type" value="${event-context:Type}" />
			  <property name="User" value="${event-context:User}" />
			  <property name="SQL" value="${event-context:SQL}" />
			  <property name="Parameters" value="${event-context:Parameters}" />
			  <property name="Elapsed" value="${event-context:Elapsed}" />
			</target>
		  </targets>

		  <rules>
			<logger name="Info" minlevel="Info" writeTo="fileInfo" />
			<logger name="Error" minlevel="Info" writeTo="fileError" />
			<logger name="Templates" minlevel="Info" writeTo="fileTemplates" />
			<logger name="Quartz.*" minlevel="Info" writeTo="fileQuartz" />
			<logger name="SQL" minlevel="Warn" writeTo="seq" />
			<logger name="SQL" minlevel="Info" writeTo="fileSQL" final="true" />
			<logger name="*" levels="Info,Warn,Error,Fatal" writeTo="seq" />
		  </rules>
		</nlog>

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		you can also use variables in you config XML. Details can be found here https://github.com/NLog/NLog/wiki/Var-Layout-Renderer

		Sample:

		Config file:
		<variable name="logDirectory" value="c:\temp" />
		<targets>
			<target xsi:type="File"
				name ="processInfo"
				fileName="${var:logDirectory}"
				layout="${longdate}  |  ProcessInfo: ${message}"
			/>
		</targets>
		
		Code:
		LogManager.Configuration.Variables["logDirectory"] = @"c:\temp\logs";


		 */
		#endregion


		private const string EMPTY_MESSAGE_PLACEHOLDER = "<-";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Log(this NLog.ILogger log, LogLevel level, string? message,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Log(level, $"->{callerName}(): {message ?? EMPTY_MESSAGE_PLACEHOLDER}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Log(this Lazy<ILogger> log, LogLevel level, string? message,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Value.e_Log(level, message, callerName, sourceFilePath, sourceLineNumber);



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Info(this ILogger log, string? message,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			string msg = $"->({sourceFilePath}:{sourceLineNumber}){callerName}(): INFO: {message ?? EMPTY_MESSAGE_PLACEHOLDER}";
			Debug.WriteLine(msg);
			log.Info(msg);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Info(this Lazy<ILogger> log, string? message = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Value.e_Info(message, callerName, sourceFilePath, sourceLineNumber);



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Debug(this ILogger log, string? message = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			string msg = $"->({sourceFilePath}:{sourceLineNumber}){callerName}(): DEBUG: {message ?? EMPTY_MESSAGE_PLACEHOLDER}";
			Debug.WriteLine(msg);
			log.Debug(msg);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Debug(this Lazy<ILogger> log, string? message = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Value.e_Debug(message, callerName, sourceFilePath, sourceLineNumber);



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Warn(this ILogger log, string? message,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Warn($"->({sourceFilePath}:{sourceLineNumber}){callerName}(): {message ?? EMPTY_MESSAGE_PLACEHOLDER}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Warn(this Lazy<ILogger> log, string? message = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Value.e_Warn(message, callerName, sourceFilePath, sourceLineNumber);



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Error(this ILogger log, Exception ex,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Error(ex, $"->({sourceFilePath}:{sourceLineNumber}){callerName}(): {ex.Message}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Error(this Lazy<ILogger> log, Exception ex,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Value.e_Error(ex, callerName, sourceFilePath, sourceLineNumber);



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Fatal(this ILogger log, Exception ex,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Fatal(ex, $"->({sourceFilePath}:{sourceLineNumber}){callerName}(): {ex.Message}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_Fatal(this Lazy<ILogger> log, Exception ex,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
			=> log.Value.e_Fatal(ex, callerName, sourceFilePath, sourceLineNumber);







		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_DumpObject<T>(
		this ILogger log,
		T? value = null,
		string? helperMessage = null,
		[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
		[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
		[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
		[System.Runtime.CompilerServices.CallerArgumentExpression("value")] string? valueName = null) where T : class
		{
			if (!helperMessage.e_IsNullOrWhiteSpace()) helperMessage = $" (Helper message:{helperMessage})";

			string arrayString = value.e_DumpObjectToString(valueName);
			log.e_Debug($"->DumpObject: {arrayString}{helperMessage}", callerName: callerName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber);

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_DumpObject<T>(
			this Lazy<ILogger> log,
			T? value = null,
			string? helperMessage = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
			[System.Runtime.CompilerServices.CallerArgumentExpression("value")] string? valueName = null) where T : class
			=> log.Value.e_DumpObject(value, helperMessage, callerName, sourceFilePath, sourceLineNumber, valueName);





		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_DumpArray<T>(this ILogger log,
			IEnumerable<T> arr,
			string elementsSeparator = "\n",
			string? helperMessage = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
			[System.Runtime.CompilerServices.CallerArgumentExpression("arr")] string? arrayName = null)
		{
			//string arrayString = string.Join(elementsSeparator, arr.Select(o => o!.ToString()).ToArray());
			string arrayString = arr.e_DumpArrayToString(elementsSeparator, arrayName: arrayName);
			log.Debug($"{arrayString}{helperMessage}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_DumpArray<T>(this Lazy<ILogger> log,
			IEnumerable<T> arr,
			string elementsSeparator = "\n",
			string? helperMessage = null,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
			[System.Runtime.CompilerServices.CallerArgumentExpression("arr")] string? arrayName = null)
			=> log.Value.e_DumpArray(arr, elementsSeparator, helperMessage, callerName, sourceFilePath, sourceLineNumber, arrayName);








		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void e_NLogError(
			this Exception ex,
			bool showError = true,
			ILogger? logger = null,
			string? errorDescriptionMessage = null,
			MessageBoxIcon icon = MessageBoxIcon.Error)
		{
			logger ??= LogManager.GetCurrentClassLogger();
			logger.Error(ex, errorDescriptionMessage);

			if (showError)
				MessageBox.Show((errorDescriptionMessage ?? "") + ex.Message, Application.ProductName, MessageBoxButtons.OK, icon);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void e_tryCatch(this ILogger log, Action a,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
			[System.Runtime.CompilerServices.CallerArgumentExpression("a")] string? actionName = null)
		{
			try { a.Invoke(); }
			catch (Exception ex) { ex.e_NLogError(false, log, $"{callerName}->{actionName}"); }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void e_tryCatch(this Lazy<ILogger> log, Action a,
			[System.Runtime.CompilerServices.CallerMemberName] string callerName = "",
			[System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
			[System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
			[System.Runtime.CompilerServices.CallerArgumentExpression("a")] string? actionName = null)
			=> log.Value.e_tryCatch(a, callerName, sourceFilePath, sourceLineNumber, actionName);




		private static FileTarget UnwrapFileTarget(string targetName)
		{
			if (LogManager.Configuration == null || !LogManager.Configuration!.ConfiguredNamedTargets!.Any())
				throw new Exception("LogManager.Configuration == null || !LogManager.Configuration.ConfiguredNamedTargets.Any()");

			Target? target = LogManager.Configuration.FindTargetByName(targetName) ?? throw new ArgumentOutOfRangeException(nameof(targetName));

			#region Unwrap async targets if necessary.
			/*
			WrapperTargetBase? wrapperTarget = target as WrapperTargetBase;
			// Unwrap the target if necessary.
			FileTarget fileTarget = ((wrapperTarget == null)
				? target as FileTarget
				: wrapperTarget.WrappedTarget as FileTarget) ?? throw new Exception($"Could not get a FileTarget from {target.GetType()}");
			 */


			//Targets can be wrapped multiple times (in my case I had a filter),
			//so the following snippet is a more generic approach to unwrapping that works for multiple levels and doesn't make assumptions about target names.
			while ((target != null) && (target is WrapperTargetBase))
			{
				target = (target as WrapperTargetBase)?.WrappedTarget;
			}

			#endregion

			FileTarget fileTarget = (target as FileTarget) ?? throw new Exception($"Could not get a FileTarget from {target!.GetType()}");
			return fileTarget;
		}



		internal static FileInfo NLogGetLogFile(string targetName)
		{
			FileTarget fileTarget = UnwrapFileTarget(targetName);

			// Need to set timestamp here if filename uses date. 
			// For example - filename="${basedir}/logs/${shortdate}/trace.log"
			var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
			string fileName = fileTarget.FileName.Render(logEventInfo);
			return new FileInfo(fileName);
		}


		/// <summary>Changing Log File Path Programmatically</summary>
		/// <param name="targetPathTemplate">"something like '${tempdir}/Company/Product/Product_${shortdate}.log'</param>
		/// <remarks>This also doesn't change the content of the NLog.config file, it just changes the runtime value.
		/// So you can also edit the NLog.config file to new value using the <see cref="NLogWriteLogFilePathTemplate()"/> </remarks>
		internal static void NLogSetLogFilePathTemplate(string targetName, string targetPathTemplate)
		{
			FileTarget fileTarget = UnwrapFileTarget(targetName);

			fileTarget.FileName = targetPathTemplate;
			LogManager.ReconfigExistingLoggers();

		}


		internal static void NLogSetLogPathToDefaultTemplate(string targetName, string? filePrefix = null)
		{
			Application.CompanyName.e_Assert_NullOrWhiteSpace();
			Application.ProductName.e_Assert_NullOrWhiteSpace();

			string elevation = uom.AppInfo.IsProcessElevated()
					? "Elevated_"
					: string.Empty;

			filePrefix = string.IsNullOrWhiteSpace(filePrefix)
				? string.Empty
				: (filePrefix + "_");

			string logPathTemplate = @"${tempdir}/" +
				Path.Combine(Application.CompanyName, Application.ProductName, Application.ProductName) +
				$"_{filePrefix}{elevation}" + @"${shortdate}.log";

			Extensions_NLog.NLogSetLogFilePathTemplate(targetName, logPathTemplate);
		}

		internal static void NLogWriteLogFilePathTemplate(DirectoryInfo nlogConfigFileDir, string targetName, string targetPathTemplate, string nlogConfigFile = "NLog.config")
		{
			var xdoc = XDocument.Load(Path.Combine(nlogConfigFileDir.FullName, nlogConfigFile));
			var ns = xdoc.Root.GetDefaultNamespace();
			var fTarget = xdoc.Descendants(ns + "target")
					 .FirstOrDefault(t => (string)t.Attribute("name") == targetName);
			fTarget.SetAttributeValue("fileName", targetPathTemplate);
			xdoc.Save(nlogConfigFile);
		}
	}
}

#pragma warning restore IDE1006 // Naming Styles
