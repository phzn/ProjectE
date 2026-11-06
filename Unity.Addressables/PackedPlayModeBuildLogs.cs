using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000002 RID: 2
[Serializable]
public class PackedPlayModeBuildLogs
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public List<PackedPlayModeBuildLogs.RuntimeBuildLog> RuntimeBuildLogs
	{
		get
		{
			return this.m_RuntimeBuildLogs;
		}
		set
		{
			this.m_RuntimeBuildLogs = value;
		}
	}

	// Token: 0x04000001 RID: 1
	[SerializeField]
	private List<PackedPlayModeBuildLogs.RuntimeBuildLog> m_RuntimeBuildLogs = new List<PackedPlayModeBuildLogs.RuntimeBuildLog>();

	// Token: 0x0200002C RID: 44
	[Serializable]
	public struct RuntimeBuildLog
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00009121 File Offset: 0x00007321
		public RuntimeBuildLog(LogType type, string message)
		{
			this.Type = type;
			this.Message = message;
		}

		// Token: 0x04000095 RID: 149
		public LogType Type;

		// Token: 0x04000096 RID: 150
		public string Message;
	}
}
