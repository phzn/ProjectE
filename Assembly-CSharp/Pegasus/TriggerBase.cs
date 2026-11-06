using System;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001C3 RID: 451
	public class TriggerBase : MonoBehaviour
	{
		// Token: 0x06000D61 RID: 3425 RVA: 0x0005C69A File Offset: 0x0005A89A
		public virtual void OnStart(PegasusPoi poi)
		{
			if (poi != null && this.m_triggerAtStart && poi.m_manager.m_displayDebug)
			{
				Debug.Log(string.Format("Started trigger on {0} - {1}", poi.m_manager.name, poi.name));
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0005C6DC File Offset: 0x0005A8DC
		public virtual void OnUpdate(PegasusPoi poi, float progress)
		{
			if (poi != null && this.m_triggerOnUpdate && poi.m_manager.m_displayDebug)
			{
				Debug.Log(string.Format("Udpated trigger on {0} - {1} {2:0.00}", poi.m_manager.name, poi.name, progress));
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0005C72D File Offset: 0x0005A92D
		public virtual void OnEnd(PegasusPoi poi)
		{
			if (poi != null && this.m_triggerAtEnd && poi.m_manager.m_displayDebug)
			{
				Debug.Log(string.Format("Ended trigger on {0} - {1}", poi.m_manager.name, poi.name));
			}
		}

		// Token: 0x04000DAA RID: 3498
		public bool m_triggerAtStart = true;

		// Token: 0x04000DAB RID: 3499
		public bool m_triggerOnUpdate;

		// Token: 0x04000DAC RID: 3500
		public bool m_triggerAtEnd = true;
	}
}
