using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006F1 RID: 1777
	public static class BicycleDiagnosticLog
	{
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x000F95AF File Offset: 0x000F77AF
		public static string SessionId
		{
			get
			{
				return BicycleDiagnosticLog.sessionId;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000F95B6 File Offset: 0x000F77B6
		public static string CurrentPath
		{
			get
			{
				if (BicycleDiagnosticLog.writer == null)
				{
					return BicycleDiagnosticLog.lastPath;
				}
				return BicycleDiagnosticLog.writer.CurrentPath;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x000F95CF File Offset: 0x000F77CF
		public static string DirectoryPath
		{
			get
			{
				return BicycleDiagnosticLog.directoryPath;
			}
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000F95D6 File Offset: 0x000F77D6
		static BicycleDiagnosticLog()
		{
			BicycleDiagnosticLog.ResetSession();
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000F95E8 File Offset: 0x000F77E8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSession()
		{
			BicycleDiagnosticLog.Close();
			BicycleDiagnosticLog.writer = null;
			BicycleDiagnosticLog.lastPath = string.Empty;
			BicycleDiagnosticLog.sessionId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString("N");
			BicycleDiagnosticLog.directoryPath = Path.Combine(Application.isEditor ? Path.GetDirectoryName(Application.dataPath) : Application.persistentDataPath, "BicycleDiagnostics");
			BicycleDiagnosticLog.lastFlushTime = 0.0;
			BicycleDiagnosticLog.retryAfter = 0.0;
			BicycleDiagnosticLog.failureReported = false;
			BicycleDiagnosticLog.shuttingDown = false;
			Application.quitting -= BicycleDiagnosticLog.OnShutdown;
			Application.quitting += BicycleDiagnosticLog.OnShutdown;
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000F96B4 File Offset: 0x000F78B4
		public static void Write(object record, bool flush = false)
		{
			if (record == null || BicycleDiagnosticLog.shuttingDown)
			{
				return;
			}
			double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
			if (realtimeSinceStartupAsDouble < BicycleDiagnosticLog.retryAfter)
			{
				return;
			}
			try
			{
				string text = JsonUtility.ToJson(record, false);
				if (BicycleDiagnosticLog.writer == null)
				{
					BicycleDiagnosticLog.writer = new BicycleDiagnosticSessionWriter(BicycleDiagnosticLog.directoryPath, BicycleDiagnosticLog.sessionId, 33554432L);
				}
				BicycleDiagnosticLog.writer.WriteJsonLine(text);
				BicycleDiagnosticLog.lastPath = BicycleDiagnosticLog.writer.CurrentPath;
				if (flush || realtimeSinceStartupAsDouble - BicycleDiagnosticLog.lastFlushTime >= 1.0)
				{
					BicycleDiagnosticLog.writer.Flush();
					BicycleDiagnosticLog.lastFlushTime = realtimeSinceStartupAsDouble;
				}
			}
			catch (Exception ex)
			{
				BicycleDiagnosticLog.HandleFailure(ex, realtimeSinceStartupAsDouble);
			}
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000F9760 File Offset: 0x000F7960
		public static void Flush()
		{
			if (BicycleDiagnosticLog.writer == null)
			{
				return;
			}
			double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
			try
			{
				BicycleDiagnosticLog.writer.Flush();
				BicycleDiagnosticLog.lastFlushTime = realtimeSinceStartupAsDouble;
			}
			catch (Exception ex)
			{
				BicycleDiagnosticLog.HandleFailure(ex, realtimeSinceStartupAsDouble);
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000F97A8 File Offset: 0x000F79A8
		public static void Close()
		{
			if (BicycleDiagnosticLog.writer == null)
			{
				return;
			}
			BicycleDiagnosticSessionWriter bicycleDiagnosticSessionWriter = BicycleDiagnosticLog.writer;
			BicycleDiagnosticLog.lastPath = bicycleDiagnosticSessionWriter.CurrentPath;
			BicycleDiagnosticLog.writer = null;
			try
			{
				bicycleDiagnosticSessionWriter.Dispose();
			}
			catch (Exception ex)
			{
				BicycleDiagnosticLog.HandleFailure(ex, Time.realtimeSinceStartupAsDouble);
			}
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000F97F8 File Offset: 0x000F79F8
		private static void OnShutdown()
		{
			BicycleDiagnosticLog.shuttingDown = true;
			BicycleDiagnosticLog.Close();
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000F9808 File Offset: 0x000F7A08
		private static void HandleFailure(Exception exception, double now)
		{
			BicycleDiagnosticLog.retryAfter = now + 10.0;
			BicycleDiagnosticSessionWriter bicycleDiagnosticSessionWriter = BicycleDiagnosticLog.writer;
			BicycleDiagnosticLog.writer = null;
			if (bicycleDiagnosticSessionWriter != null)
			{
				BicycleDiagnosticLog.lastPath = bicycleDiagnosticSessionWriter.CurrentPath;
				try
				{
					bicycleDiagnosticSessionWriter.Dispose();
				}
				catch (Exception)
				{
				}
			}
			if (BicycleDiagnosticLog.failureReported)
			{
				return;
			}
			BicycleDiagnosticLog.failureReported = true;
			Debug.LogWarning("[BicycleDiagnosticLog] Scrittura diagnostica sospesa: " + exception.Message + ". Nuovo tentativo tra almeno 10 secondi. Cartella: " + BicycleDiagnosticLog.directoryPath);
		}

		// Token: 0x04002C1E RID: 11294
		private const long MaximumPartBytes = 33554432L;

		// Token: 0x04002C1F RID: 11295
		private static BicycleDiagnosticSessionWriter writer;

		// Token: 0x04002C20 RID: 11296
		private static string sessionId;

		// Token: 0x04002C21 RID: 11297
		private static string directoryPath;

		// Token: 0x04002C22 RID: 11298
		private static string lastPath = string.Empty;

		// Token: 0x04002C23 RID: 11299
		private static double lastFlushTime;

		// Token: 0x04002C24 RID: 11300
		private static double retryAfter;

		// Token: 0x04002C25 RID: 11301
		private static bool failureReported;

		// Token: 0x04002C26 RID: 11302
		private static bool shuttingDown;
	}
}
