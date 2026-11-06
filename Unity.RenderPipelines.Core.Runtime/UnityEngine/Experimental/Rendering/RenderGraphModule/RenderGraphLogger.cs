using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200001A RID: 26
	internal class RenderGraphLogger
	{
		// Token: 0x06000113 RID: 275 RVA: 0x000073FC File Offset: 0x000055FC
		public void Initialize(string logName)
		{
			StringBuilder stringBuilder;
			if (!this.m_LogMap.TryGetValue(logName, out stringBuilder))
			{
				stringBuilder = new StringBuilder();
				this.m_LogMap.Add(logName, stringBuilder);
			}
			this.m_CurrentBuilder = stringBuilder;
			this.m_CurrentBuilder.Clear();
			this.m_CurrentIndentation = 0;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007446 File Offset: 0x00005646
		public void IncrementIndentation(int value)
		{
			this.m_CurrentIndentation += Math.Abs(value);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000745B File Offset: 0x0000565B
		public void DecrementIndentation(int value)
		{
			this.m_CurrentIndentation = Math.Max(0, this.m_CurrentIndentation - Math.Abs(value));
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007478 File Offset: 0x00005678
		public void LogLine(string format, params object[] args)
		{
			for (int i = 0; i < this.m_CurrentIndentation; i++)
			{
				this.m_CurrentBuilder.Append('\t');
			}
			this.m_CurrentBuilder.AppendFormat(format, args);
			this.m_CurrentBuilder.AppendLine();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000074C0 File Offset: 0x000056C0
		public string GetLog(string logName)
		{
			StringBuilder stringBuilder;
			if (this.m_LogMap.TryGetValue(logName, out stringBuilder))
			{
				return stringBuilder.ToString();
			}
			return "";
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000074EC File Offset: 0x000056EC
		public string GetAllLogs()
		{
			string text = "";
			foreach (KeyValuePair<string, StringBuilder> keyValuePair in this.m_LogMap)
			{
				StringBuilder value = keyValuePair.Value;
				value.AppendLine();
				text += value.ToString();
			}
			return text;
		}

		// Token: 0x040000A4 RID: 164
		private Dictionary<string, StringBuilder> m_LogMap = new Dictionary<string, StringBuilder>();

		// Token: 0x040000A5 RID: 165
		private StringBuilder m_CurrentBuilder;

		// Token: 0x040000A6 RID: 166
		private int m_CurrentIndentation;
	}
}
