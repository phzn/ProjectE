using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class PersistentVariablesSource : ISource, IDictionary<string, VariablesGroupAsset>, ICollection<KeyValuePair<string, VariablesGroupAsset>>, IEnumerable<KeyValuePair<string, VariablesGroupAsset>>, IEnumerable, ISerializationCallbackReceiver
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000B6CB File Offset: 0x000098CB
		public static bool IsUpdating
		{
			get
			{
				return PersistentVariablesSource.s_IsUpdating != 0;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000B6D5 File Offset: 0x000098D5
		public int Count
		{
			get
			{
				return this.m_Groups.Count;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000B6E2 File Offset: 0x000098E2
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000B6E5 File Offset: 0x000098E5
		public ICollection<string> Keys
		{
			get
			{
				return this.m_GroupLookup.Keys;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000B6F2 File Offset: 0x000098F2
		public ICollection<VariablesGroupAsset> Values
		{
			get
			{
				return this.m_GroupLookup.Values.Select<PersistentVariablesSource.NameValuePair, VariablesGroupAsset>((PersistentVariablesSource.NameValuePair k) => k.group).ToList<VariablesGroupAsset>();
			}
		}

		// Token: 0x17000099 RID: 153
		public VariablesGroupAsset this[string name]
		{
			get
			{
				return this.m_GroupLookup[name].group;
			}
			set
			{
				this.Add(name, value);
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002F9 RID: 761 RVA: 0x0000B748 File Offset: 0x00009948
		// (remove) Token: 0x060002FA RID: 762 RVA: 0x0000B77C File Offset: 0x0000997C
		public static event Action EndUpdate;

		// Token: 0x060002FB RID: 763 RVA: 0x0000B7AF File Offset: 0x000099AF
		public PersistentVariablesSource(SmartFormatter formatter)
		{
			formatter.Parser.AddOperators(".");
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000B7DD File Offset: 0x000099DD
		public static void BeginUpdating()
		{
			PersistentVariablesSource.s_IsUpdating++;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B7EB File Offset: 0x000099EB
		public static void EndUpdating()
		{
			PersistentVariablesSource.s_IsUpdating--;
			if (PersistentVariablesSource.s_IsUpdating != 0)
			{
				if (PersistentVariablesSource.s_IsUpdating < 0)
				{
					Debug.LogWarning("Incorrect number of Begin and End calls to PersistentVariablesSource. BeginUpdating must be called before EndUpdating.");
					PersistentVariablesSource.s_IsUpdating = 0;
				}
				return;
			}
			Action endUpdate = PersistentVariablesSource.EndUpdate;
			if (endUpdate == null)
			{
				return;
			}
			endUpdate();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000B828 File Offset: 0x00009A28
		public static IDisposable UpdateScope()
		{
			PersistentVariablesSource.BeginUpdating();
			return default(PersistentVariablesSource.ScopedUpdate);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000B848 File Offset: 0x00009A48
		public bool TryGetValue(string name, out VariablesGroupAsset value)
		{
			PersistentVariablesSource.NameValuePair nameValuePair;
			if (this.m_GroupLookup.TryGetValue(name, out nameValuePair))
			{
				value = nameValuePair.group;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000B874 File Offset: 0x00009A74
		public void Add(string name, VariablesGroupAsset group)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name", "Name must not be null or empty.");
			}
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			PersistentVariablesSource.NameValuePair nameValuePair = new PersistentVariablesSource.NameValuePair
			{
				name = name,
				group = group
			};
			name = name.ReplaceWhiteSpaces("-");
			this.m_GroupLookup[name] = nameValuePair;
			this.m_Groups.Add(nameValuePair);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000B8E7 File Offset: 0x00009AE7
		public void Add(KeyValuePair<string, VariablesGroupAsset> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000B900 File Offset: 0x00009B00
		public bool Remove(string name)
		{
			PersistentVariablesSource.NameValuePair nameValuePair;
			if (this.m_GroupLookup.TryGetValue(name, out nameValuePair))
			{
				this.m_Groups.Remove(nameValuePair);
				this.m_GroupLookup.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000B93A File Offset: 0x00009B3A
		public bool Remove(KeyValuePair<string, VariablesGroupAsset> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000B949 File Offset: 0x00009B49
		public void Clear()
		{
			this.m_GroupLookup.Clear();
			this.m_Groups.Clear();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000B961 File Offset: 0x00009B61
		public bool ContainsKey(string name)
		{
			return this.m_GroupLookup.ContainsKey(name);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000B970 File Offset: 0x00009B70
		public bool Contains(KeyValuePair<string, VariablesGroupAsset> item)
		{
			VariablesGroupAsset variablesGroupAsset;
			return this.TryGetValue(item.Key, out variablesGroupAsset) && variablesGroupAsset == item.Value;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		public void CopyTo(KeyValuePair<string, VariablesGroupAsset>[] array, int arrayIndex)
		{
			foreach (KeyValuePair<string, PersistentVariablesSource.NameValuePair> keyValuePair in this.m_GroupLookup)
			{
				array[arrayIndex++] = new KeyValuePair<string, VariablesGroupAsset>(keyValuePair.Key, keyValuePair.Value.group);
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000BA10 File Offset: 0x00009C10
		IEnumerator<KeyValuePair<string, VariablesGroupAsset>> IEnumerable<KeyValuePair<string, VariablesGroupAsset>>.GetEnumerator()
		{
			foreach (KeyValuePair<string, PersistentVariablesSource.NameValuePair> keyValuePair in this.m_GroupLookup)
			{
				yield return new KeyValuePair<string, VariablesGroupAsset>(keyValuePair.Key, keyValuePair.Value.group);
			}
			Dictionary<string, PersistentVariablesSource.NameValuePair>.Enumerator enumerator = default(Dictionary<string, PersistentVariablesSource.NameValuePair>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000BA1F File Offset: 0x00009C1F
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, PersistentVariablesSource.NameValuePair> keyValuePair in this.m_GroupLookup)
			{
				yield return new KeyValuePair<string, VariablesGroupAsset>(keyValuePair.Key, keyValuePair.Value.group);
			}
			Dictionary<string, PersistentVariablesSource.NameValuePair>.Enumerator enumerator = default(Dictionary<string, PersistentVariablesSource.NameValuePair>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000BA30 File Offset: 0x00009C30
		public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
		{
			string selectorText = selectorInfo.SelectorText;
			IVariableGroup variableGroup = selectorInfo.CurrentValue as IVariableGroup;
			if (variableGroup != null && PersistentVariablesSource.EvaluateLocalGroup(selectorInfo, variableGroup))
			{
				return true;
			}
			if (selectorInfo.SelectorOperator == "")
			{
				FormatCache formatCache = selectorInfo.FormatDetails.FormatCache;
				if (PersistentVariablesSource.EvaluateLocalGroup(selectorInfo, (formatCache != null) ? formatCache.LocalVariables : null))
				{
					return true;
				}
				VariablesGroupAsset variablesGroupAsset;
				if (this.TryGetValue(selectorText, out variablesGroupAsset))
				{
					selectorInfo.Result = variablesGroupAsset;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		private static bool EvaluateLocalGroup(ISelectorInfo selectorInfo, IVariableGroup variablleGroup)
		{
			if (variablleGroup == null)
			{
				return false;
			}
			IVariable variable;
			if (variablleGroup != null && variablleGroup.TryGetValue(selectorInfo.SelectorText, out variable))
			{
				FormatCache formatCache = selectorInfo.FormatDetails.FormatCache;
				if (formatCache != null)
				{
					IVariableValueChanged variableValueChanged = variable as IVariableValueChanged;
					if (variableValueChanged != null && !formatCache.VariableTriggers.Contains(variableValueChanged))
					{
						formatCache.VariableTriggers.Add(variableValueChanged);
					}
				}
				selectorInfo.Result = variable.GetSourceValue(selectorInfo);
				return true;
			}
			return false;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000BB10 File Offset: 0x00009D10
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000BB14 File Offset: 0x00009D14
		public void OnAfterDeserialize()
		{
			if (this.m_GroupLookup == null)
			{
				this.m_GroupLookup = new Dictionary<string, PersistentVariablesSource.NameValuePair>();
			}
			this.m_GroupLookup.Clear();
			foreach (PersistentVariablesSource.NameValuePair nameValuePair in this.m_Groups)
			{
				if (!string.IsNullOrEmpty(nameValuePair.name))
				{
					this.m_GroupLookup[nameValuePair.name] = nameValuePair;
				}
			}
		}

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private List<PersistentVariablesSource.NameValuePair> m_Groups = new List<PersistentVariablesSource.NameValuePair>();

		// Token: 0x040000C3 RID: 195
		private Dictionary<string, PersistentVariablesSource.NameValuePair> m_GroupLookup = new Dictionary<string, PersistentVariablesSource.NameValuePair>();

		// Token: 0x040000C4 RID: 196
		internal static int s_IsUpdating;

		// Token: 0x02000135 RID: 309
		[Serializable]
		private class NameValuePair
		{
			// Token: 0x040002BD RID: 701
			public string name;

			// Token: 0x040002BE RID: 702
			[SerializeReference]
			public VariablesGroupAsset group;
		}

		// Token: 0x02000136 RID: 310
		public struct ScopedUpdate : IDisposable
		{
			// Token: 0x06000792 RID: 1938 RVA: 0x00018C1F File Offset: 0x00016E1F
			public void Dispose()
			{
				PersistentVariablesSource.EndUpdating();
			}
		}
	}
}
