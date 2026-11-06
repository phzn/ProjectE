using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000008 RID: 8
	public class InvalidKeyException : Exception
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002521 File Offset: 0x00000721
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002529 File Offset: 0x00000729
		public object Key { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002532 File Offset: 0x00000732
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000253A File Offset: 0x0000073A
		public Type Type { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002543 File Offset: 0x00000743
		public Addressables.MergeMode? MergeMode { get; }

		// Token: 0x06000025 RID: 37 RVA: 0x0000254B File Offset: 0x0000074B
		public InvalidKeyException(object key)
			: this(key, typeof(object))
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000255E File Offset: 0x0000075E
		public InvalidKeyException(object key, Type type)
		{
			this.Key = key;
			this.Type = type;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002574 File Offset: 0x00000774
		internal InvalidKeyException(object key, Type type, AddressablesImpl addr)
		{
			this.Key = key;
			this.Type = type;
			this.m_Addressables = addr;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002591 File Offset: 0x00000791
		public InvalidKeyException(object key, Type type, Addressables.MergeMode mergeMode)
		{
			this.Key = key;
			this.Type = type;
			this.MergeMode = new Addressables.MergeMode?(mergeMode);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000025B3 File Offset: 0x000007B3
		internal InvalidKeyException(object key, Type type, Addressables.MergeMode mergeMode, AddressablesImpl addr)
		{
			this.Key = key;
			this.Type = type;
			this.MergeMode = new Addressables.MergeMode?(mergeMode);
			this.m_Addressables = addr;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000025DD File Offset: 0x000007DD
		public InvalidKeyException()
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000025E5 File Offset: 0x000007E5
		public InvalidKeyException(string message)
			: base(message)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000025EE File Offset: 0x000007EE
		public InvalidKeyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000025F8 File Offset: 0x000007F8
		protected InvalidKeyException(SerializationInfo message, StreamingContext context)
			: base(message, context)
		{
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002604 File Offset: 0x00000804
		public override string Message
		{
			get
			{
				string text = this.Key as string;
				if (!string.IsNullOrEmpty(text))
				{
					if (this.m_Addressables == null)
					{
						return string.Format("{0}, Key={1}, Type={2}", base.Message, text, this.Type);
					}
					return this.GetMessageForSingleKey(text);
				}
				else
				{
					IEnumerable enumerable = this.Key as IEnumerable;
					if (enumerable == null)
					{
						return string.Format("{0}, Key={1}, Type={2}", base.Message, this.Key, this.Type);
					}
					int num = 0;
					List<string> list = new List<string>();
					HashSet<string> hashSet = new HashSet<string>();
					foreach (object obj in enumerable)
					{
						num++;
						hashSet.Add(obj.GetType().ToString());
						if (obj is string)
						{
							list.Add(obj as string);
						}
					}
					if (this.MergeMode == null)
					{
						string csvstring = this.GetCSVString(list, "Key=", "Keys=");
						return string.Format("{0} No MergeMode is set to merge the multiple keys requested. {1}, Type={2}", base.Message, csvstring, this.Type);
					}
					if (num != list.Count)
					{
						string csvstring2 = this.GetCSVString(hashSet, "Type=", "Types=");
						return base.Message + " Enumerable key contains multiple Types. " + csvstring2 + ", all Keys are expected to be strings";
					}
					if (num == 1)
					{
						return this.GetMessageForSingleKey(list[0]);
					}
					return this.GetMessageforMergeKeys(list);
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002788 File Offset: 0x00000988
		private string GetMessageForSingleKey(string keyString)
		{
			HashSet<Type> typesForKey = this.GetTypesForKey(keyString);
			if (typesForKey.Count == 0)
			{
				return this.GetNotFoundMessage(keyString);
			}
			if (typesForKey.Count == 1)
			{
				return this.GetTypeNotAssignableMessage(keyString, typesForKey);
			}
			return this.GetMultipleAssignableTypesMessage(keyString, typesForKey);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000027C7 File Offset: 0x000009C7
		private string GetNotFoundMessage(string keyString)
		{
			return base.Message + " No Location found for Key=" + keyString;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000027DC File Offset: 0x000009DC
		private string GetTypeNotAssignableMessage(string keyString, HashSet<Type> typesAvailableForKey)
		{
			Type type = null;
			using (HashSet<Type>.Enumerator enumerator = typesAvailableForKey.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					type = enumerator.Current;
				}
			}
			if (type == null)
			{
				return string.Format("{0}, Key={1}, Type={2}", base.Message, keyString, this.Type);
			}
			return string.Format("{0} No Asset found with for Key={1}. Key exists as Type={2}, which is not assignable from the requested Type={3}", new object[] { base.Message, keyString, type, this.Type });
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002870 File Offset: 0x00000A70
		private string GetMultipleAssignableTypesMessage(string keyString, HashSet<Type> typesAvailableForKey)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = 0;
			foreach (Type type in typesAvailableForKey)
			{
				num++;
				stringBuilder.Append((num > 1) ? string.Format(", {0}", type) : type.ToString());
			}
			return string.Format("{0} No Asset found with for Key={1}. Key exists as multiple Types={2}, which is not assignable from the requested Type={3}", new object[] { base.Message, keyString, stringBuilder, this.Type });
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002910 File Offset: 0x00000B10
		private string GetMessageforMergeKeys(List<string> keys)
		{
			string csvstring = this.GetCSVString(keys, "Key=", "Keys=");
			string text = "\nNo Location found for Key={0}";
			StringBuilder stringBuilder = null;
			Addressables.MergeMode? mergeMode = this.MergeMode;
			if (mergeMode != null)
			{
				switch (mergeMode.GetValueOrDefault())
				{
				case Addressables.MergeMode.None:
					goto IL_02C9;
				case Addressables.MergeMode.Union:
				{
					stringBuilder = new StringBuilder(string.Format("{0} No {1} of Assets between {2} with Type={3}", new object[]
					{
						base.Message,
						this.MergeMode.Value,
						csvstring,
						this.Type
					}));
					Dictionary<Type, List<string>> dictionary = new Dictionary<Type, List<string>>();
					foreach (string text2 in keys)
					{
						if (!this.GetTypeToKeys(text2, dictionary))
						{
							stringBuilder.Append(string.Format(text, text2));
						}
					}
					using (Dictionary<Type, List<string>>.Enumerator enumerator2 = dictionary.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<Type, List<string>> keyValuePair = enumerator2.Current;
							string csvstring2 = this.GetCSVString(keyValuePair.Value, "Key=", "Keys=");
							List<string> list = new List<string>();
							foreach (string text3 in keys)
							{
								if (!keyValuePair.Value.Contains(text3))
								{
									list.Add(text3);
								}
							}
							if (list.Count == 0)
							{
								stringBuilder.Append(string.Format("\nUnion of Type={0} found with {1}", keyValuePair.Key, csvstring2));
							}
							else
							{
								string csvstring3 = this.GetCSVString(list, "Key=", "Keys=");
								stringBuilder.Append(string.Format("\nUnion of Type={0} found with {1}. Without {2}", keyValuePair.Key, csvstring2, csvstring3));
							}
						}
						goto IL_03BA;
					}
					break;
				}
				case Addressables.MergeMode.Intersection:
					break;
				default:
					goto IL_03BA;
				}
				stringBuilder = new StringBuilder(string.Format("{0} No {1} of Assets between {2} with Type={3}", new object[]
				{
					base.Message,
					this.MergeMode.Value,
					csvstring,
					this.Type
				}));
				bool flag = false;
				Dictionary<Type, List<string>> dictionary2 = new Dictionary<Type, List<string>>();
				foreach (string text4 in keys)
				{
					if (!this.GetTypeToKeys(text4, dictionary2))
					{
						flag = true;
						stringBuilder.Append(string.Format(text, text4));
					}
				}
				if (flag)
				{
					goto IL_03BA;
				}
				using (Dictionary<Type, List<string>>.Enumerator enumerator2 = dictionary2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<Type, List<string>> keyValuePair2 = enumerator2.Current;
						if (keyValuePair2.Value.Count == keys.Count)
						{
							stringBuilder.Append(string.Format("\nAn Intersection exists for Type={0}", keyValuePair2.Key));
						}
					}
					goto IL_03BA;
				}
				IL_02C9:
				stringBuilder = new StringBuilder(string.Format("{0} No {1} Asset within {2} with Type={3}", new object[]
				{
					base.Message,
					this.MergeMode.Value,
					csvstring,
					this.Type
				}));
				Dictionary<Type, List<string>> dictionary3 = new Dictionary<Type, List<string>>();
				foreach (string text5 in keys)
				{
					if (!this.GetTypeToKeys(text5, dictionary3))
					{
						stringBuilder.Append(string.Format(text, text5));
					}
				}
				foreach (KeyValuePair<Type, List<string>> keyValuePair3 in dictionary3)
				{
					string csvstring4 = this.GetCSVString(keyValuePair3.Value, "Key=", "Keys=");
					stringBuilder.Append(string.Format("\nType={0} exists for {1}", keyValuePair3.Key, csvstring4));
				}
			}
			IL_03BA:
			return stringBuilder.ToString();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002D38 File Offset: 0x00000F38
		private HashSet<Type> GetTypesForKey(string keyString)
		{
			HashSet<Type> hashSet = new HashSet<Type>();
			using (IEnumerator<IResourceLocator> enumerator = this.m_Addressables.ResourceLocators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IList<IResourceLocation> list;
					if (enumerator.Current.Locate(keyString, null, out list))
					{
						foreach (IResourceLocation resourceLocation in list)
						{
							hashSet.Add(resourceLocation.ResourceType);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002DD4 File Offset: 0x00000FD4
		private bool GetTypeToKeys(string key, Dictionary<Type, List<string>> typeToKeys)
		{
			HashSet<Type> typesForKey = this.GetTypesForKey(key);
			if (typesForKey.Count == 0)
			{
				return false;
			}
			foreach (Type type in typesForKey)
			{
				List<string> list;
				if (!typeToKeys.TryGetValue(type, out list))
				{
					typeToKeys.Add(type, new List<string> { key });
				}
				else
				{
					list.Add(key);
				}
			}
			return true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E58 File Offset: 0x00001058
		private string GetCSVString(IEnumerable<string> enumerator, string prefixSingle, string prefixPlural)
		{
			StringBuilder stringBuilder = new StringBuilder(prefixPlural);
			int num = 0;
			foreach (string text in enumerator)
			{
				num++;
				stringBuilder.Append((num > 1) ? (", " + text) : text);
			}
			if (num == 1 && !string.IsNullOrEmpty(prefixPlural) && !string.IsNullOrEmpty(prefixSingle))
			{
				stringBuilder.Replace(prefixPlural, prefixSingle);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400000E RID: 14
		private AddressablesImpl m_Addressables;

		// Token: 0x0400000F RID: 15
		private const string BaseInvalidKeyMessageFormat = "{0}, Key={1}, Type={2}";
	}
}
