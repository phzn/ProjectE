using System;
using System.Collections.Generic;
using UnityEngine.Localization.Events;

namespace UnityEngine.Localization.Components
{
	// Token: 0x020000D4 RID: 212
	[AddComponentMenu("Localization/Localize String List Event")]
	public class LocalizeStringListEvent : LocalizedMonoBehaviour
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00014B7A File Offset: 0x00012D7A
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x00014B82 File Offset: 0x00012D82
		public ILocalizedStringList ListReference
		{
			get
			{
				return this.m_ListReference;
			}
			set
			{
				this.ClearChangeHandler();
				this.m_ListReference = value;
				if (base.isActiveAndEnabled)
				{
					this.RegisterChangeHandler();
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00014B9F File Offset: 0x00012D9F
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x00014BA7 File Offset: 0x00012DA7
		public UnityEventStringList OnUpdateList
		{
			get
			{
				return this.m_UpdateList;
			}
			set
			{
				this.m_UpdateList = value;
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00014BB0 File Offset: 0x00012DB0
		private void OnEnable()
		{
			this.RegisterChangeHandler();
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00014BB8 File Offset: 0x00012DB8
		private void OnDisable()
		{
			this.ClearChangeHandler();
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00014BC0 File Offset: 0x00012DC0
		private void OnDestroy()
		{
			this.ClearChangeHandler();
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00014BC8 File Offset: 0x00012DC8
		private void UpdateList(List<string> value)
		{
			this.OnUpdateList.Invoke(value);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00014BD6 File Offset: 0x00012DD6
		private void RegisterChangeHandler()
		{
			if (this.m_ListReference == null)
			{
				return;
			}
			if (this.m_ChangeHandler == null)
			{
				this.m_ChangeHandler = new LocalizedListChangeHandler(this.UpdateList);
			}
			this.m_ListReference.ListChanged += this.m_ChangeHandler;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00014C0C File Offset: 0x00012E0C
		private void ClearChangeHandler()
		{
			if (this.m_ListReference != null && this.m_ChangeHandler != null)
			{
				this.m_ListReference.ListChanged -= this.m_ChangeHandler;
			}
		}

		// Token: 0x040001F9 RID: 505
		[SerializeReference]
		private ILocalizedStringList m_ListReference = new LocalizedStringList();

		// Token: 0x040001FA RID: 506
		[SerializeField]
		private UnityEventStringList m_UpdateList = new UnityEventStringList();

		// Token: 0x040001FB RID: 507
		private LocalizedListChangeHandler m_ChangeHandler;
	}
}
