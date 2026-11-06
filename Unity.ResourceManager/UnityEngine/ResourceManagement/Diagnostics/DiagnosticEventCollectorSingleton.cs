using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.Diagnostics
{
	// Token: 0x02000054 RID: 84
	public class DiagnosticEventCollectorSingleton : ComponentSingleton<DiagnosticEventCollectorSingleton>
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00007EE8 File Offset: 0x000060E8
		public static Guid PlayerConnectionGuid
		{
			get
			{
				if (DiagnosticEventCollectorSingleton.s_editorConnectionGuid == Guid.Empty)
				{
					DiagnosticEventCollectorSingleton.s_editorConnectionGuid = new Guid(1, 2, 3, new byte[] { 20, 1, 32, 32, 4, 9, 6, 44 });
				}
				return DiagnosticEventCollectorSingleton.s_editorConnectionGuid;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007F1E File Offset: 0x0000611E
		protected override string GetGameObjectName()
		{
			return "EventCollector";
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00007F25 File Offset: 0x00006125
		public static bool RegisterEventHandler(Action<DiagnosticEvent> handler, bool register, bool create)
		{
			if (register && (create || ComponentSingleton<DiagnosticEventCollectorSingleton>.Exists))
			{
				ComponentSingleton<DiagnosticEventCollectorSingleton>.Instance.RegisterEventHandler(handler);
				return true;
			}
			if (!register && ComponentSingleton<DiagnosticEventCollectorSingleton>.Exists)
			{
				ComponentSingleton<DiagnosticEventCollectorSingleton>.Instance.UnregisterEventHandler(handler);
			}
			return false;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00007F58 File Offset: 0x00006158
		internal void RegisterEventHandler(Action<DiagnosticEvent> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.s_EventHandlers.Add(handler);
			foreach (DiagnosticEvent diagnosticEvent in from evt in this.m_UnhandledEvents.Concat<DiagnosticEvent>(this.m_CreatedEvents.Values)
				orderby evt.Frame
				select evt)
			{
				handler(diagnosticEvent);
			}
			this.m_UnhandledEvents.Clear();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008000 File Offset: 0x00006200
		public void UnregisterEventHandler(Action<DiagnosticEvent> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.s_EventHandlers.Remove(handler);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000801C File Offset: 0x0000621C
		public void PostEvent(DiagnosticEvent diagnosticEvent)
		{
			if (diagnosticEvent.Stream == 1 && !this.m_CreatedEvents.ContainsKey(diagnosticEvent.ObjectId))
			{
				this.m_CreatedEvents.Add(diagnosticEvent.ObjectId, diagnosticEvent);
			}
			else if (diagnosticEvent.Stream == 5)
			{
				this.m_CreatedEvents.Remove(diagnosticEvent.ObjectId);
			}
			if (this.s_EventHandlers.Count > 0)
			{
				this.s_EventHandlers.Invoke(diagnosticEvent);
				return;
			}
			this.m_UnhandledEvents.Add(diagnosticEvent);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000080A0 File Offset: 0x000062A0
		private void Awake()
		{
			this.RegisterEventHandler(delegate(DiagnosticEvent diagnosticEvent)
			{
				PlayerConnection.instance.Send(DiagnosticEventCollectorSingleton.PlayerConnectionGuid, diagnosticEvent.Serialize());
			});
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000080C8 File Offset: 0x000062C8
		private void Update()
		{
			if (this.s_EventHandlers.Count > 0)
			{
				float num = Time.realtimeSinceStartup - this.m_lastTickSent;
				if (num > 0.25f)
				{
					float num2 = (float)(Time.frameCount - this.m_lastFrame) / num;
					this.m_lastFrame = Time.frameCount;
					this.fpsAvg = (this.fpsAvg + num2) * 0.5f;
					this.m_lastTickSent = Time.realtimeSinceStartup;
					int num3 = (int)(Profiler.GetMonoUsedSizeLong() / 1024L);
					this.PostEvent(new DiagnosticEvent("FrameCount", "FPS", 2, 1, Time.frameCount, (int)this.fpsAvg, null));
					this.PostEvent(new DiagnosticEvent("MemoryCount", "MonoHeap", 3, 2, Time.frameCount, num3, null));
				}
			}
		}

		// Token: 0x040000E1 RID: 225
		private static Guid s_editorConnectionGuid;

		// Token: 0x040000E2 RID: 226
		internal Dictionary<int, DiagnosticEvent> m_CreatedEvents = new Dictionary<int, DiagnosticEvent>();

		// Token: 0x040000E3 RID: 227
		internal List<DiagnosticEvent> m_UnhandledEvents = new List<DiagnosticEvent>();

		// Token: 0x040000E4 RID: 228
		internal DelegateList<DiagnosticEvent> s_EventHandlers = DelegateList<DiagnosticEvent>.CreateWithGlobalCache();

		// Token: 0x040000E5 RID: 229
		private float m_lastTickSent;

		// Token: 0x040000E6 RID: 230
		private int m_lastFrame;

		// Token: 0x040000E7 RID: 231
		private float fpsAvg = 30f;
	}
}
