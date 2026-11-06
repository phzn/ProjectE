using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UnityEngine.Localization
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public abstract class LocalizedReference : ISerializationCallbackReceiver
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002C42 File Offset: 0x00000E42
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002C4A File Offset: 0x00000E4A
		public TableReference TableReference
		{
			get
			{
				return this.m_TableReference;
			}
			set
			{
				if (value.Equals(this.m_TableReference))
				{
					return;
				}
				this.m_TableReference = value;
				this.ForceUpdate();
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002C69 File Offset: 0x00000E69
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002C71 File Offset: 0x00000E71
		public TableEntryReference TableEntryReference
		{
			get
			{
				return this.m_TableEntryReference;
			}
			set
			{
				if (value.Equals(this.m_TableEntryReference))
				{
					return;
				}
				this.m_TableEntryReference = value;
				this.ForceUpdate();
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002C90 File Offset: 0x00000E90
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002C98 File Offset: 0x00000E98
		public FallbackBehavior FallbackState
		{
			get
			{
				return this.m_FallbackState;
			}
			set
			{
				this.m_FallbackState = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002CA1 File Offset: 0x00000EA1
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002CA9 File Offset: 0x00000EA9
		public Locale LocaleOverride
		{
			get
			{
				return this.m_LocaleOverride;
			}
			set
			{
				if (this.m_LocaleOverride == value)
				{
					return;
				}
				this.m_LocaleOverride = value;
				this.ForceUpdate();
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002CC7 File Offset: 0x00000EC7
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002CCF File Offset: 0x00000ECF
		public virtual bool WaitForCompletion
		{
			get
			{
				return this.m_WaitForCompletion;
			}
			set
			{
				this.m_WaitForCompletion = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000084 RID: 132
		internal abstract bool ForceSynchronous { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public bool IsEmpty
		{
			get
			{
				return this.TableReference.ReferenceType == TableReference.Type.Empty || this.TableEntryReference.ReferenceType == TableEntryReference.Type.Empty;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002D08 File Offset: 0x00000F08
		public void SetReference(TableReference table, TableEntryReference entry)
		{
			bool flag = false;
			if (!this.m_TableReference.Equals(table))
			{
				this.m_TableReference = table;
				flag = true;
			}
			if (!this.m_TableEntryReference.Equals(entry))
			{
				this.m_TableEntryReference = entry;
				flag = true;
			}
			if (flag)
			{
				this.ForceUpdate();
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002D50 File Offset: 0x00000F50
		public override string ToString()
		{
			return string.Format("{0}/{1}", this.TableReference, this.TableEntryReference.ToString(this.TableReference));
		}

		// Token: 0x06000088 RID: 136
		protected internal abstract void ForceUpdate();

		// Token: 0x06000089 RID: 137
		protected abstract void Reset();

		// Token: 0x0600008A RID: 138 RVA: 0x00002D86 File Offset: 0x00000F86
		public virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002D88 File Offset: 0x00000F88
		public virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private TableReference m_TableReference;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private TableEntryReference m_TableEntryReference;

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private FallbackBehavior m_FallbackState;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		private bool m_WaitForCompletion;

		// Token: 0x04000016 RID: 22
		internal Locale m_LocaleOverride;
	}
}
