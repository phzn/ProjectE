using System;
using System.Collections.Generic;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class LocalizedStringList : LocalizedString, ILocalizedStringList
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003E85 File Offset: 0x00002085
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003E8D File Offset: 0x0000208D
		public string Separator
		{
			get
			{
				return this.m_Separator;
			}
			set
			{
				this.m_Separator = value;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000CC RID: 204 RVA: 0x00003E98 File Offset: 0x00002098
		// (remove) Token: 0x060000CD RID: 205 RVA: 0x00003F18 File Offset: 0x00002118
		public event LocalizedListChangeHandler ListChanged
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.m_ListChanged == null)
				{
					base.StringChanged += this.OnStringChangedInternal;
				}
				this.m_ListChanged = (LocalizedListChangeHandler)Delegate.Combine(this.m_ListChanged, value);
				if (base.CurrentLoadingOperationHandle.IsValid() && base.CurrentLoadingOperationHandle.IsDone && this.m_CurrentList != null)
				{
					value(this.m_CurrentList);
				}
			}
			remove
			{
				this.m_ListChanged = (LocalizedListChangeHandler)Delegate.Remove(this.m_ListChanged, value);
				if (this.m_ListChanged == null)
				{
					base.StringChanged -= this.OnStringChangedInternal;
				}
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003F4B File Offset: 0x0000214B
		public LocalizedStringList()
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003F5E File Offset: 0x0000215E
		public LocalizedStringList(TableReference tableReference, TableEntryReference entryReference)
			: base(tableReference, entryReference)
		{
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003F73 File Offset: 0x00002173
		internal List<string> CurrentList
		{
			get
			{
				return this.m_CurrentList;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003F7C File Offset: 0x0000217C
		public List<string> GetLocalizedList(List<string> target = null)
		{
			List<string> list = target ?? new List<string>();
			this.SplitInto(base.GetLocalizedString(), list);
			return list;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003FA4 File Offset: 0x000021A4
		public AsyncOperationHandle<List<string>> GetLocalizedListAsync(List<string> target = null)
		{
			AsyncOperationHandle<string> localizedStringAsync = base.GetLocalizedStringAsync();
			List<string> resultList = target ?? new List<string>();
			return AddressablesInterface.ResourceManager.CreateChainOperation<List<string>, string>(localizedStringAsync, delegate(AsyncOperationHandle<string> op)
			{
				this.SplitInto(op.Result, resultList);
				return AddressablesInterface.ResourceManager.CreateCompletedOperation<List<string>>(resultList, null);
			});
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003FEC File Offset: 0x000021EC
		private void OnStringChangedInternal(string value)
		{
			if (this.m_CurrentList == null)
			{
				this.m_CurrentList = new List<string>();
			}
			this.SplitInto(value, this.m_CurrentList);
			LocalizedListChangeHandler listChanged = this.m_ListChanged;
			if (listChanged == null)
			{
				return;
			}
			listChanged(this.m_CurrentList);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004024 File Offset: 0x00002224
		private void SplitInto(string value, List<string> target)
		{
			target.Clear();
			if (value == null)
			{
				return;
			}
			string text = (string.IsNullOrEmpty(this.m_Separator) ? "," : this.m_Separator);
			target.AddRange(value.Split(new string[] { text }, StringSplitOptions.None));
		}

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private string m_Separator = ",";

		// Token: 0x0400002B RID: 43
		private List<string> m_CurrentList;

		// Token: 0x0400002C RID: 44
		private LocalizedListChangeHandler m_ListChanged;
	}
}
