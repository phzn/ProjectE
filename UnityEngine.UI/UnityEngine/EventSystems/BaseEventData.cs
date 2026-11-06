using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200004F RID: 79
	public class BaseEventData : AbstractEventData
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x0001810D File Offset: 0x0001630D
		public BaseEventData(EventSystem eventSystem)
		{
			this.m_EventSystem = eventSystem;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001811C File Offset: 0x0001631C
		public BaseInputModule currentInputModule
		{
			get
			{
				return this.m_EventSystem.currentInputModule;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00018129 File Offset: 0x00016329
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x00018136 File Offset: 0x00016336
		public GameObject selectedObject
		{
			get
			{
				return this.m_EventSystem.currentSelectedGameObject;
			}
			set
			{
				this.m_EventSystem.SetSelectedGameObject(value, this);
			}
		}

		// Token: 0x040001AF RID: 431
		private readonly EventSystem m_EventSystem;
	}
}
