using System;
using System.Collections.Generic;
using UnityEngine.Localization.Events;

namespace UnityEngine.Localization.Components
{
	// Token: 0x020000D3 RID: 211
	[AddComponentMenu("Localization/Localize String Event")]
	public class LocalizeStringEvent : LocalizedMonoBehaviour
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000149E6 File Offset: 0x00012BE6
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x000149EE File Offset: 0x00012BEE
		public LocalizedString StringReference
		{
			get
			{
				return this.m_StringReference;
			}
			set
			{
				this.ClearChangeHandler();
				this.m_StringReference = value;
				if (base.isActiveAndEnabled)
				{
					this.RegisterChangeHandler();
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00014A0B File Offset: 0x00012C0B
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x00014A13 File Offset: 0x00012C13
		public UnityEventString OnUpdateString
		{
			get
			{
				return this.m_UpdateString;
			}
			set
			{
				this.m_UpdateString = value;
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00014A1C File Offset: 0x00012C1C
		public void RefreshString()
		{
			LocalizedString stringReference = this.StringReference;
			if (stringReference == null)
			{
				return;
			}
			stringReference.RefreshString();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00014A2F File Offset: 0x00012C2F
		public void SetTable(string tableReference)
		{
			if (this.StringReference == null)
			{
				this.StringReference = new LocalizedString();
			}
			this.StringReference.TableReference = tableReference;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00014A55 File Offset: 0x00012C55
		public void SetEntry(string entryName)
		{
			if (this.StringReference == null)
			{
				this.StringReference = new LocalizedString();
			}
			this.StringReference.TableEntryReference = entryName;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00014A7B File Offset: 0x00012C7B
		protected virtual void OnEnable()
		{
			this.RegisterChangeHandler();
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00014A83 File Offset: 0x00012C83
		protected virtual void OnDisable()
		{
			this.ClearChangeHandler();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00014A8B File Offset: 0x00012C8B
		private void OnDestroy()
		{
			this.ClearChangeHandler();
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00014A93 File Offset: 0x00012C93
		protected virtual void UpdateString(string value)
		{
			this.OnUpdateString.Invoke(value);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00014AA1 File Offset: 0x00012CA1
		private void OnValidate()
		{
			this.RefreshString();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00014AAC File Offset: 0x00012CAC
		internal virtual void RegisterChangeHandler()
		{
			if (this.StringReference == null)
			{
				return;
			}
			if (this.m_FormatArguments.Count > 0)
			{
				this.StringReference.Arguments = this.m_FormatArguments.ToArray();
				if (PlaymodeState.IsPlaying)
				{
					Debug.LogWarningFormat("LocalizeStringEvent({0}) is using the deprecated Format Arguments field which will be removed in the future. Consider upgrading to use String Reference Local Variables instead.", new object[] { base.name, this });
				}
			}
			if (this.m_ChangeHandler == null)
			{
				this.m_ChangeHandler = new LocalizedString.ChangeHandler(this.UpdateString);
			}
			this.StringReference.StringChanged += this.m_ChangeHandler;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014B36 File Offset: 0x00012D36
		internal virtual void ClearChangeHandler()
		{
			if (this.StringReference != null)
			{
				this.StringReference.StringChanged -= this.m_ChangeHandler;
			}
		}

		// Token: 0x040001F5 RID: 501
		[SerializeField]
		private LocalizedString m_StringReference = new LocalizedString();

		// Token: 0x040001F6 RID: 502
		[SerializeField]
		private List<Object> m_FormatArguments = new List<Object>();

		// Token: 0x040001F7 RID: 503
		[SerializeField]
		private UnityEventString m_UpdateString = new UnityEventString();

		// Token: 0x040001F8 RID: 504
		private LocalizedString.ChangeHandler m_ChangeHandler;
	}
}
