using System;
using System.Diagnostics;

namespace System.Runtime
{
	// Token: 0x02000034 RID: 52
	internal class TraceLevelHelper
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00006890 File Offset: 0x00004A90
		internal static TraceEventType GetTraceEventType(byte level, byte opcode)
		{
			if (opcode <= 2)
			{
				if (opcode == 1)
				{
					return TraceEventType.Start;
				}
				if (opcode == 2)
				{
					return TraceEventType.Stop;
				}
			}
			else
			{
				if (opcode == 7)
				{
					return TraceEventType.Resume;
				}
				if (opcode == 8)
				{
					return TraceEventType.Suspend;
				}
			}
			return TraceLevelHelper.EtwLevelToTraceEventType[(int)level];
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000068C9 File Offset: 0x00004AC9
		internal static TraceEventType GetTraceEventType(TraceEventLevel level)
		{
			return TraceLevelHelper.EtwLevelToTraceEventType[(int)level];
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000068D2 File Offset: 0x00004AD2
		internal static TraceEventType GetTraceEventType(byte level)
		{
			return TraceLevelHelper.EtwLevelToTraceEventType[(int)level];
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000068DC File Offset: 0x00004ADC
		internal static string LookupSeverity(TraceEventLevel level, TraceEventOpcode opcode)
		{
			if (opcode <= TraceEventOpcode.Stop)
			{
				if (opcode == TraceEventOpcode.Start)
				{
					return "Start";
				}
				if (opcode == TraceEventOpcode.Stop)
				{
					return "Stop";
				}
			}
			else
			{
				if (opcode == TraceEventOpcode.Resume)
				{
					return "Resume";
				}
				if (opcode == TraceEventOpcode.Suspend)
				{
					return "Suspend";
				}
			}
			string text;
			switch (level)
			{
			case TraceEventLevel.Critical:
				text = "Critical";
				break;
			case TraceEventLevel.Error:
				text = "Error";
				break;
			case TraceEventLevel.Warning:
				text = "Warning";
				break;
			case TraceEventLevel.Informational:
				text = "Information";
				break;
			case TraceEventLevel.Verbose:
				text = "Verbose";
				break;
			default:
				text = level.ToString();
				break;
			}
			return text;
		}

		// Token: 0x040000F7 RID: 247
		private static TraceEventType[] EtwLevelToTraceEventType = new TraceEventType[]
		{
			TraceEventType.Critical,
			TraceEventType.Critical,
			TraceEventType.Error,
			TraceEventType.Warning,
			TraceEventType.Information,
			TraceEventType.Verbose
		};
	}
}
