using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x0200005F RID: 95
	[CreateAssetMenu(menuName = "Localization/Variables Group")]
	public class VariablesGroupAsset : ScriptableObject, IVariableGroup, IVariable, IDictionary<string, IVariable>, ICollection<KeyValuePair<string, IVariable>>, IEnumerable<KeyValuePair<string, IVariable>>, IEnumerable, ISerializationCallbackReceiver
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000A2E0 File Offset: 0x000084E0
		public int Count
		{
			get
			{
				return this.m_VariableLookup.Count;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000A2ED File Offset: 0x000084ED
		public ICollection<string> Keys
		{
			get
			{
				return this.m_VariableLookup.Keys;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000A2FA File Offset: 0x000084FA
		public ICollection<IVariable> Values
		{
			get
			{
				return this.m_VariableLookup.Values.Select<VariableNameValuePair, IVariable>((VariableNameValuePair s) => s.variable).ToList<IVariable>();
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000A330 File Offset: 0x00008530
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008B RID: 139
		public IVariable this[string name]
		{
			get
			{
				return this.m_VariableLookup[name].variable;
			}
			set
			{
				this.Add(name, value);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A350 File Offset: 0x00008550
		public object GetSourceValue(ISelectorInfo _)
		{
			return this;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000A354 File Offset: 0x00008554
		public bool TryGetValue(string name, out IVariable value)
		{
			VariableNameValuePair variableNameValuePair;
			if (this.m_VariableLookup.TryGetValue(name, out variableNameValuePair))
			{
				value = variableNameValuePair.variable;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000A380 File Offset: 0x00008580
		public void Add(string name, IVariable variable)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name", "Name must not be null or empty.");
			}
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			name = name.ReplaceWhiteSpaces("-");
			VariableNameValuePair variableNameValuePair = new VariableNameValuePair
			{
				name = name,
				variable = variable
			};
			this.m_VariableLookup.Add(name, variableNameValuePair);
			this.m_Variables.Add(variableNameValuePair);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A3ED File Offset: 0x000085ED
		public void Add(KeyValuePair<string, IVariable> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A404 File Offset: 0x00008604
		public bool Remove(string name)
		{
			VariableNameValuePair variableNameValuePair;
			if (this.m_VariableLookup.TryGetValue(name, out variableNameValuePair))
			{
				this.m_Variables.Remove(variableNameValuePair);
				this.m_VariableLookup.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000A43E File Offset: 0x0000863E
		public bool Remove(KeyValuePair<string, IVariable> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000A44D File Offset: 0x0000864D
		public bool ContainsKey(string name)
		{
			return this.m_VariableLookup.ContainsKey(name);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A45C File Offset: 0x0000865C
		public bool Contains(KeyValuePair<string, IVariable> item)
		{
			IVariable variable;
			return this.TryGetValue(item.Key, out variable) && variable == item.Value;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A488 File Offset: 0x00008688
		public void CopyTo(KeyValuePair<string, IVariable>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			foreach (KeyValuePair<string, VariableNameValuePair> keyValuePair in this.m_VariableLookup)
			{
				array[arrayIndex++] = new KeyValuePair<string, IVariable>(keyValuePair.Key, keyValuePair.Value.variable);
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A508 File Offset: 0x00008708
		IEnumerator<KeyValuePair<string, IVariable>> IEnumerable<KeyValuePair<string, IVariable>>.GetEnumerator()
		{
			foreach (KeyValuePair<string, VariableNameValuePair> keyValuePair in this.m_VariableLookup)
			{
				yield return new KeyValuePair<string, IVariable>(keyValuePair.Key, keyValuePair.Value.variable);
			}
			Dictionary<string, VariableNameValuePair>.Enumerator enumerator = default(Dictionary<string, VariableNameValuePair>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A517 File Offset: 0x00008717
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, VariableNameValuePair> keyValuePair in this.m_VariableLookup)
			{
				yield return new KeyValuePair<string, IVariable>(keyValuePair.Key, keyValuePair.Value.variable);
			}
			Dictionary<string, VariableNameValuePair>.Enumerator enumerator = default(Dictionary<string, VariableNameValuePair>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000A526 File Offset: 0x00008726
		[Obsolete("Please use ContainsKey instead.", false)]
		public bool ContainsName(string name)
		{
			return this.ContainsKey(name);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A52F File Offset: 0x0000872F
		public void Clear()
		{
			this.m_VariableLookup.Clear();
			this.m_Variables.Clear();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A547 File Offset: 0x00008747
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000A54C File Offset: 0x0000874C
		public void OnAfterDeserialize()
		{
			if (this.m_VariableLookup == null)
			{
				this.m_VariableLookup = new Dictionary<string, VariableNameValuePair>();
			}
			this.m_VariableLookup.Clear();
			foreach (VariableNameValuePair variableNameValuePair in this.m_Variables)
			{
				if (!string.IsNullOrEmpty(variableNameValuePair.name))
				{
					this.m_VariableLookup[variableNameValuePair.name] = variableNameValuePair;
				}
			}
		}

		// Token: 0x040000BB RID: 187
		[SerializeField]
		internal List<VariableNameValuePair> m_Variables = new List<VariableNameValuePair>();

		// Token: 0x040000BC RID: 188
		private Dictionary<string, VariableNameValuePair> m_VariableLookup = new Dictionary<string, VariableNameValuePair>();
	}
}
