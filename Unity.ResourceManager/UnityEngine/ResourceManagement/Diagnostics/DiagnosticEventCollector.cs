using System;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.Diagnostics
{
	// Token: 0x02000055 RID: 85
	public class DiagnosticEventCollector : MonoBehaviour
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000210 RID: 528 RVA: 0x000081BC File Offset: 0x000063BC
		public static Guid PlayerConnectionGuid
		{
			get
			{
				return DiagnosticEventCollectorSingleton.PlayerConnectionGuid;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000081C4 File Offset: 0x000063C4
		public static DiagnosticEventCollector FindOrCreateGlobalInstance()
		{
			if (DiagnosticEventCollector.s_Collector == null)
			{
				GameObject gameObject = new GameObject("EventCollector", new Type[] { typeof(DiagnosticEventCollector) });
				DiagnosticEventCollector.s_Collector = gameObject.GetComponent<DiagnosticEventCollector>();
				gameObject.hideFlags = HideFlags.DontSave;
			}
			return DiagnosticEventCollector.s_Collector;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008212 File Offset: 0x00006412
		public static bool RegisterEventHandler(Action<DiagnosticEvent> handler, bool register, bool create)
		{
			return DiagnosticEventCollectorSingleton.RegisterEventHandler(handler, register, create);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000821C File Offset: 0x0000641C
		public void UnregisterEventHandler(Action<DiagnosticEvent> handler)
		{
			ComponentSingleton<DiagnosticEventCollectorSingleton>.Instance.UnregisterEventHandler(handler);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008229 File Offset: 0x00006429
		public void PostEvent(DiagnosticEvent diagnosticEvent)
		{
			ComponentSingleton<DiagnosticEventCollectorSingleton>.Instance.PostEvent(diagnosticEvent);
		}

		// Token: 0x040000E8 RID: 232
		private static DiagnosticEventCollector s_Collector;
	}
}
