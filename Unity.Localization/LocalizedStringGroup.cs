using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class LocalizedStringGroup : ILocalizedStringList
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003A00 File Offset: 0x00001C00
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003A25 File Offset: 0x00001C25
		public List<LocalizedString> Strings
		{
			get
			{
				List<LocalizedString> list;
				if ((list = this.m_Strings) == null)
				{
					list = (this.m_Strings = new List<LocalizedString>());
				}
				return list;
			}
			set
			{
				this.m_Strings = value;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000C2 RID: 194 RVA: 0x00003A30 File Offset: 0x00001C30
		// (remove) Token: 0x060000C3 RID: 195 RVA: 0x00003A97 File Offset: 0x00001C97
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
					this.SubscribeChildren();
				}
				this.m_ListChanged = (LocalizedListChangeHandler)Delegate.Combine(this.m_ListChanged, value);
				if (this.m_CachedValues != null && this.m_LoadedCount == this.m_Strings.Count)
				{
					value(this.GetCurrentList());
				}
			}
			remove
			{
				this.m_ListChanged = (LocalizedListChangeHandler)Delegate.Remove(this.m_ListChanged, value);
				if (this.m_ListChanged == null)
				{
					this.UnsubscribeChildren();
				}
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public List<string> GetLocalizedList(List<string> target = null)
		{
			List<string> list = target;
			if (target == null)
			{
				List<LocalizedString> strings = this.m_Strings;
				list = new List<string>((strings != null) ? strings.Count : 0);
			}
			List<string> list2 = list;
			list2.Clear();
			if (this.m_Strings == null)
			{
				return list2;
			}
			for (int i = 0; i < this.m_Strings.Count; i++)
			{
				LocalizedString localizedString = this.m_Strings[i];
				list2.Add((localizedString == null || localizedString.IsEmpty) ? string.Empty : localizedString.GetLocalizedString());
			}
			return list2;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003B3C File Offset: 0x00001D3C
		public AsyncOperationHandle<List<string>> GetLocalizedListAsync(List<string> target = null)
		{
			LocalizedStringGroup.<>c__DisplayClass13_0 CS$<>8__locals1 = new LocalizedStringGroup.<>c__DisplayClass13_0();
			LocalizedStringGroup.<>c__DisplayClass13_0 CS$<>8__locals2 = CS$<>8__locals1;
			List<string> list = target;
			if (target == null)
			{
				List<LocalizedString> strings = this.m_Strings;
				list = new List<string>((strings != null) ? strings.Count : 0);
			}
			CS$<>8__locals2.resultList = list;
			CS$<>8__locals1.resultList.Clear();
			if (this.m_Strings == null || this.m_Strings.Count == 0)
			{
				return AddressablesInterface.ResourceManager.CreateCompletedOperation<List<string>>(CS$<>8__locals1.resultList, null);
			}
			List<AsyncOperationHandle> list2 = new List<AsyncOperationHandle>(this.m_Strings.Count);
			for (int i = 0; i < this.m_Strings.Count; i++)
			{
				LocalizedString localizedString = this.m_Strings[i];
				if (localizedString != null && !localizedString.IsEmpty)
				{
					list2.Add(localizedString.GetLocalizedStringAsync());
				}
			}
			if (list2.Count == 0)
			{
				for (int j = 0; j < this.m_Strings.Count; j++)
				{
					CS$<>8__locals1.resultList.Add(string.Empty);
				}
				return AddressablesInterface.ResourceManager.CreateCompletedOperation<List<string>>(CS$<>8__locals1.resultList, null);
			}
			AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle = AddressablesInterface.CreateGroupOperation(list2);
			CS$<>8__locals1.strings = this.m_Strings;
			return AddressablesInterface.ResourceManager.CreateChainOperation<List<string>, IList<AsyncOperationHandle>>(asyncOperationHandle, delegate(AsyncOperationHandle<IList<AsyncOperationHandle>> op)
			{
				int num = 0;
				for (int k = 0; k < CS$<>8__locals1.strings.Count; k++)
				{
					LocalizedString localizedString2 = CS$<>8__locals1.strings[k];
					if (localizedString2 == null || localizedString2.IsEmpty)
					{
						CS$<>8__locals1.resultList.Add(string.Empty);
					}
					else
					{
						AsyncOperationHandle asyncOperationHandle2 = op.Result[num++];
						CS$<>8__locals1.resultList.Add(asyncOperationHandle2.Convert<string>().Result ?? string.Empty);
					}
				}
				return AddressablesInterface.ResourceManager.CreateCompletedOperation<List<string>>(CS$<>8__locals1.resultList, null);
			});
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003C68 File Offset: 0x00001E68
		private void SubscribeChildren()
		{
			if (this.m_Strings == null)
			{
				return;
			}
			this.m_ChildHandlers = new List<LocalizedString.ChangeHandler>(this.m_Strings.Count);
			this.m_CachedValues = new string[this.m_Strings.Count];
			this.m_LoadedCount = 0;
			for (int i = 0; i < this.m_Strings.Count; i++)
			{
				int index = i;
				LocalizedString localizedString = this.m_Strings[i];
				if (localizedString == null)
				{
					this.m_ChildHandlers.Add(null);
					this.m_CachedValues[index] = string.Empty;
					this.m_LoadedCount++;
				}
				else
				{
					LocalizedString.ChangeHandler changeHandler = delegate(string v)
					{
						if (this.m_CachedValues == null || index >= this.m_CachedValues.Length)
						{
							return;
						}
						if (this.m_CachedValues[index] == null)
						{
							this.m_LoadedCount++;
						}
						this.m_CachedValues[index] = v ?? string.Empty;
						if (this.m_LoadedCount >= this.m_CachedValues.Length)
						{
							LocalizedListChangeHandler listChanged = this.m_ListChanged;
							if (listChanged == null)
							{
								return;
							}
							listChanged(this.GetCurrentList());
						}
					};
					this.m_ChildHandlers.Add(changeHandler);
					if (!localizedString.IsEmpty)
					{
						localizedString.StringChanged += changeHandler;
					}
					else
					{
						this.m_CachedValues[index] = string.Empty;
						this.m_LoadedCount++;
					}
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003D6C File Offset: 0x00001F6C
		private void UnsubscribeChildren()
		{
			if (this.m_Strings != null && this.m_ChildHandlers != null)
			{
				int num = 0;
				while (num < this.m_Strings.Count && num < this.m_ChildHandlers.Count)
				{
					LocalizedString localizedString = this.m_Strings[num];
					LocalizedString.ChangeHandler changeHandler = this.m_ChildHandlers[num];
					if (localizedString != null && changeHandler != null && !localizedString.IsEmpty)
					{
						localizedString.StringChanged -= changeHandler;
					}
					num++;
				}
			}
			this.m_ChildHandlers = null;
			this.m_CachedValues = null;
			this.m_LoadedCount = 0;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003DF4 File Offset: 0x00001FF4
		private List<string> GetCurrentList()
		{
			List<string> list;
			if ((list = this.m_CurrentList) == null)
			{
				list = (this.m_CurrentList = new List<string>());
			}
			List<string> list2 = list;
			list2.Clear();
			List<LocalizedString> strings = this.m_Strings;
			int num = ((strings != null) ? strings.Count : 0);
			for (int i = 0; i < num; i++)
			{
				list2.Add((this.m_CachedValues != null && i < this.m_CachedValues.Length && this.m_CachedValues[i] != null) ? this.m_CachedValues[i] : string.Empty);
			}
			return list2;
		}

		// Token: 0x04000024 RID: 36
		[SerializeField]
		private List<LocalizedString> m_Strings = new List<LocalizedString>();

		// Token: 0x04000025 RID: 37
		private List<LocalizedString.ChangeHandler> m_ChildHandlers;

		// Token: 0x04000026 RID: 38
		private string[] m_CachedValues;

		// Token: 0x04000027 RID: 39
		private int m_LoadedCount;

		// Token: 0x04000028 RID: 40
		private List<string> m_CurrentList;

		// Token: 0x04000029 RID: 41
		private LocalizedListChangeHandler m_ListChanged;
	}
}
