using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000101 RID: 257
	[Serializable]
	public abstract class JsonSerializerTrackedObject : TrackedObject
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x000160AE File Offset: 0x000142AE
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x000160B6 File Offset: 0x000142B6
		public JsonSerializerTrackedObject.ApplyChangesMethod UpdateType
		{
			get
			{
				return this.m_UpdateType;
			}
			set
			{
				this.m_UpdateType = value;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000160BF File Offset: 0x000142BF
		public override void AddTrackedProperty(ITrackedProperty trackedProperty)
		{
			base.AddTrackedProperty(trackedProperty);
			if (trackedProperty.PropertyPath.Contains(".Array.data[") || trackedProperty.PropertyPath.EndsWith(".Array.size"))
			{
				this.UpdateType = JsonSerializerTrackedObject.ApplyChangesMethod.Full;
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000160F4 File Offset: 0x000142F4
		public override AsyncOperationHandle ApplyLocale(Locale variantLocale, Locale defaultLocale)
		{
			if (base.Target == null)
			{
				return default(AsyncOperationHandle);
			}
			JObject jsonObject;
			if (this.UpdateType == JsonSerializerTrackedObject.ApplyChangesMethod.Full)
			{
				string text = JsonUtility.ToJson(base.Target);
				jsonObject = JObject.Parse(text);
			}
			else
			{
				jsonObject = new JObject();
			}
			List<AsyncOperationHandle> asyncOperations = CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Get();
			List<ArraySizeTrackedProperty> arraySizes = CollectionPool<List<ArraySizeTrackedProperty>, ArraySizeTrackedProperty>.Get();
			bool flag = false;
			LocaleIdentifier defaultLocaleIdentifier = ((defaultLocale != null) ? defaultLocale.Identifier : default(LocaleIdentifier));
			foreach (ITrackedProperty trackedProperty in base.TrackedProperties)
			{
				if (trackedProperty != null)
				{
					ArraySizeTrackedProperty arraySizeTrackedProperty = trackedProperty as ArraySizeTrackedProperty;
					if (arraySizeTrackedProperty == null)
					{
						IStringProperty stringProperty = trackedProperty as IStringProperty;
						if (stringProperty == null)
						{
							ITrackedPropertyValue<Object> trackedPropertyValue = trackedProperty as ITrackedPropertyValue<Object>;
							if (trackedPropertyValue == null)
							{
								LocalizedStringProperty localizedStringProperty = trackedProperty as LocalizedStringProperty;
								if (localizedStringProperty == null)
								{
									LocalizedAssetProperty localizedAssetProperty = trackedProperty as LocalizedAssetProperty;
									if (localizedAssetProperty != null)
									{
										if (!localizedAssetProperty.LocalizedObject.IsEmpty)
										{
											localizedAssetProperty.LocalizedObject.LocaleOverride = variantLocale;
											AsyncOperationHandle<Object> asyncOperationHandle = localizedAssetProperty.LocalizedObject.LoadAssetAsObjectAsync();
											JValue jvalue = (JValue)JsonSerializerTrackedObject.GetPropertyFromPath(trackedProperty.PropertyPath + ".instanceID", jsonObject);
											if (asyncOperationHandle.IsDone)
											{
												Object result = asyncOperationHandle.Result;
												jvalue.Value = InstanceIdHelper.GetInstanceIdString(result);
												AddressablesInterface.Release(asyncOperationHandle);
											}
											else if (localizedAssetProperty.LocalizedObject.ForceSynchronous)
											{
												Object @object = AsyncOperationUtility.SynchronousLoad<Object>(asyncOperationHandle);
												jvalue.Value = InstanceIdHelper.GetInstanceIdString(@object);
												AddressablesInterface.Release(asyncOperationHandle);
											}
											else
											{
												JsonSerializerTrackedObject.DeferredJsonObjectOperation deferredJsonObjectOperation = JsonSerializerTrackedObject.DeferredJsonObjectOperation.Pool.Get();
												deferredJsonObjectOperation.jsonValue = jvalue;
												asyncOperationHandle.Completed += deferredJsonObjectOperation.callback;
												asyncOperations.Add(asyncOperationHandle);
											}
											flag = true;
										}
									}
								}
								else if (!localizedStringProperty.LocalizedString.IsEmpty)
								{
									localizedStringProperty.LocalizedString.LocaleOverride = variantLocale;
									AsyncOperationHandle<string> localizedStringAsync = localizedStringProperty.LocalizedString.GetLocalizedStringAsync();
									JValue jvalue2 = (JValue)JsonSerializerTrackedObject.GetPropertyFromPath(trackedProperty.PropertyPath, jsonObject);
									if (localizedStringAsync.IsDone)
									{
										jvalue2.Value = localizedStringAsync.Result;
										AddressablesInterface.Release(localizedStringAsync);
									}
									else if (localizedStringProperty.LocalizedString.ForceSynchronous)
									{
										jvalue2.Value = AsyncOperationUtility.SynchronousLoad<string>(localizedStringAsync);
										AddressablesInterface.Release(localizedStringAsync);
									}
									else
									{
										JsonSerializerTrackedObject.DeferredJsonStringOperation deferredJsonStringOperation = JsonSerializerTrackedObject.DeferredJsonStringOperation.Pool.Get();
										deferredJsonStringOperation.jsonValue = jvalue2;
										localizedStringAsync.Completed += deferredJsonStringOperation.callback;
										asyncOperations.Add(localizedStringAsync);
									}
									flag = true;
								}
							}
							else
							{
								Object object2;
								trackedPropertyValue.GetValue(variantLocale.Identifier, defaultLocaleIdentifier, out object2);
								((JValue)JsonSerializerTrackedObject.GetPropertyFromPath(trackedProperty.PropertyPath + ".instanceID", jsonObject)).Value = InstanceIdHelper.GetInstanceIdString(object2);
								flag = true;
							}
						}
						else
						{
							string valueAsString = stringProperty.GetValueAsString(variantLocale.Identifier, defaultLocaleIdentifier);
							if (valueAsString != null)
							{
								JValue jvalue3 = (JValue)JsonSerializerTrackedObject.GetPropertyFromPath(trackedProperty.PropertyPath, jsonObject);
								PseudoLocale pseudoLocale = variantLocale as PseudoLocale;
								jvalue3.Value = ((pseudoLocale != null) ? pseudoLocale.GetPseudoString(valueAsString) : valueAsString);
								flag = true;
							}
						}
					}
					else
					{
						arraySizes.Add(arraySizeTrackedProperty);
						flag = true;
					}
				}
			}
			if (asyncOperations.Count > 0)
			{
				AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle2 = AddressablesInterface.CreateGroupOperation(asyncOperations);
				asyncOperationHandle2.Completed += delegate(AsyncOperationHandle<IList<AsyncOperationHandle>> res)
				{
					this.ApplyArraySizes(arraySizes, jsonObject, variantLocale.Identifier, defaultLocaleIdentifier);
					this.ApplyJson(jsonObject);
					foreach (AsyncOperationHandle asyncOperationHandle3 in res.Result)
					{
						AddressablesInterface.Release(asyncOperationHandle3);
					}
					CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Release(asyncOperations);
					CollectionPool<List<ArraySizeTrackedProperty>, ArraySizeTrackedProperty>.Release(arraySizes);
					AddressablesInterface.Release(res);
				};
				return asyncOperationHandle2;
			}
			if (flag)
			{
				this.ApplyArraySizes(arraySizes, jsonObject, variantLocale.Identifier, defaultLocaleIdentifier);
				this.ApplyJson(jsonObject);
			}
			CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Release(asyncOperations);
			CollectionPool<List<ArraySizeTrackedProperty>, ArraySizeTrackedProperty>.Release(arraySizes);
			return default(AsyncOperationHandle);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00016548 File Offset: 0x00014748
		private void ApplyArraySizes(IEnumerable<ArraySizeTrackedProperty> arraySizes, JObject jsonObject, LocaleIdentifier variantLocale, LocaleIdentifier defaultLocale)
		{
			foreach (ArraySizeTrackedProperty arraySizeTrackedProperty in arraySizes)
			{
				JArray jarray = (JArray)JsonSerializerTrackedObject.GetPropertyFromPath(arraySizeTrackedProperty.PropertyPath, jsonObject);
				uint num;
				if (arraySizeTrackedProperty.GetValue(variantLocale, defaultLocale, out num))
				{
					if ((long)jarray.Count > (long)((ulong)num))
					{
						while ((long)jarray.Count > (long)((ulong)num))
						{
							jarray.RemoveAt(jarray.Count - 1);
						}
					}
					else if ((long)jarray.Count < (long)((ulong)num))
					{
						while ((long)jarray.Count < (long)((ulong)num))
						{
							jarray.Add(new JObject());
						}
					}
				}
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000165F4 File Offset: 0x000147F4
		private void ApplyJson(JObject jsonObject)
		{
			JsonUtility.FromJsonOverwrite(jsonObject.ToString(), base.Target);
			this.PostApplyTrackedProperties();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00016610 File Offset: 0x00014810
		internal static JsonSerializerTrackedObject.ArrayResult GetNextArrayItem(string path, int startIndex)
		{
			if (path.Length < startIndex + ".Array.".Length)
			{
				return new JsonSerializerTrackedObject.ArrayResult(null, -1, -1, -1);
			}
			int num = path.IndexOf(".Array.", startIndex, StringComparison.Ordinal);
			if (num != -1)
			{
				if (path.Length > num + ".Array.".Length + "data[".Length)
				{
					int num2 = path.IndexOf("data[", num + ".Array.".Length, StringComparison.Ordinal);
					if (num2 != -1)
					{
						num2 += "data[".Length;
						int num3 = path.IndexOf(']', num2);
						if (num3 != -1)
						{
							return new JsonSerializerTrackedObject.ArrayResult(path, num + 1, num2, num3);
						}
					}
				}
				if (path.Length == num + "size".Length + ".Array.".Length && path.EndsWith("size"))
				{
					return new JsonSerializerTrackedObject.ArrayResult(path, num + 1, -1, -1);
				}
			}
			return new JsonSerializerTrackedObject.ArrayResult(null, -1, -1, -1);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000166F4 File Offset: 0x000148F4
		internal static JToken GetPropertyFromPath(string path, JContainer obj)
		{
			int num = 0;
			JsonSerializerTrackedObject.ArrayResult arrayResult = JsonSerializerTrackedObject.GetNextArrayItem(path, 0);
			JContainer jcontainer = obj;
			while (num != -1 && num < path.Length)
			{
				if (num == arrayResult.arrayStartIndex)
				{
					JArray jarray = jcontainer as JArray;
					if (jarray == null)
					{
						jarray = new JArray();
						jcontainer.Add(jarray);
					}
					if (arrayResult.IsArraySize)
					{
						return jarray;
					}
					int dataIndex = arrayResult.GetDataIndex();
					if (dataIndex == -1)
					{
						return null;
					}
					while (jarray.Count <= dataIndex)
					{
						jarray.Add(new JObject());
					}
					if (arrayResult.IsArrayElement)
					{
						JValue jvalue = jarray[dataIndex] as JValue;
						if (jvalue == null)
						{
							jvalue = new JValue(string.Empty);
							jarray[dataIndex] = jvalue;
						}
						return jvalue;
					}
					jcontainer = jarray[dataIndex] as JObject;
					if (jcontainer == null)
					{
						jcontainer = new JObject();
						jarray[dataIndex] = jcontainer;
					}
					num = arrayResult.arrayDataIndexEnd + 2;
					arrayResult = JsonSerializerTrackedObject.GetNextArrayItem(path, num);
				}
				else
				{
					int num2 = path.IndexOf('.', num);
					string text = ((num2 == -1) ? path.Substring(num) : path.Substring(num, num2 - num));
					if (num2 == -1)
					{
						JToken jtoken = jcontainer[text];
						JProperty jproperty = (JProperty)((jtoken != null) ? jtoken.Parent : null);
						JValue jvalue2;
						if (jproperty == null)
						{
							jvalue2 = new JValue(string.Empty);
							jproperty = new JProperty(text, jvalue2);
							jcontainer.Add(jproperty);
						}
						else
						{
							jvalue2 = jproperty.Value as JValue;
							if (jvalue2 == null)
							{
								jvalue2 = new JValue(string.Empty);
								jproperty.Value = jvalue2;
							}
						}
						return jvalue2;
					}
					JContainer jcontainer2 = (JContainer)jcontainer[text];
					if (jcontainer2 == null)
					{
						jcontainer2 = new JObject();
						jcontainer[text] = jcontainer2;
					}
					jcontainer = jcontainer2;
					num = num2 + 1;
				}
			}
			return null;
		}

		// Token: 0x04000233 RID: 563
		[Tooltip("Determines the type of property update that will be performed.- Full update reads the entire object into JSON, patches the properties, then reapplies the new JSON.\n- Partial update generates a partial patch and applies the changes for the tracked properties only.\nPartial update provides better performance however is not supported when modifying collections or properties that contain a serialized version such as Rect.\nThis value is automatically set based on the properties tracked.")]
		[SerializeField]
		private JsonSerializerTrackedObject.ApplyChangesMethod m_UpdateType;

		// Token: 0x02000165 RID: 357
		public enum ApplyChangesMethod
		{
			// Token: 0x04000325 RID: 805
			Partial,
			// Token: 0x04000326 RID: 806
			Full
		}

		// Token: 0x02000166 RID: 358
		private class DeferredJsonStringOperation
		{
			// Token: 0x06000821 RID: 2081 RVA: 0x00019878 File Offset: 0x00017A78
			public DeferredJsonStringOperation()
			{
				this.callback = new Action<AsyncOperationHandle<string>>(this.OnStringLoaded);
			}

			// Token: 0x06000822 RID: 2082 RVA: 0x00019892 File Offset: 0x00017A92
			private void OnStringLoaded(AsyncOperationHandle<string> asyncOperationHandle)
			{
				this.jsonValue.Value = asyncOperationHandle.Result;
				this.jsonValue = null;
				JsonSerializerTrackedObject.DeferredJsonStringOperation.Pool.Release(this);
			}

			// Token: 0x04000327 RID: 807
			public JValue jsonValue;

			// Token: 0x04000328 RID: 808
			public readonly Action<AsyncOperationHandle<string>> callback;

			// Token: 0x04000329 RID: 809
			public static readonly ObjectPool<JsonSerializerTrackedObject.DeferredJsonStringOperation> Pool = new ObjectPool<JsonSerializerTrackedObject.DeferredJsonStringOperation>(() => new JsonSerializerTrackedObject.DeferredJsonStringOperation(), null, null, null, false, 10, 10000);
		}

		// Token: 0x02000167 RID: 359
		private class DeferredJsonObjectOperation
		{
			// Token: 0x06000824 RID: 2084 RVA: 0x000198DF File Offset: 0x00017ADF
			public DeferredJsonObjectOperation()
			{
				this.callback = new Action<AsyncOperationHandle<Object>>(this.OnAssetLoaded);
			}

			// Token: 0x06000825 RID: 2085 RVA: 0x000198F9 File Offset: 0x00017AF9
			private void OnAssetLoaded(AsyncOperationHandle<Object> asyncOperationHandle)
			{
				this.jsonValue.Value = InstanceIdHelper.GetInstanceIdString(asyncOperationHandle.Result);
				this.jsonValue = null;
				JsonSerializerTrackedObject.DeferredJsonObjectOperation.Pool.Release(this);
			}

			// Token: 0x0400032A RID: 810
			public JValue jsonValue;

			// Token: 0x0400032B RID: 811
			public readonly Action<AsyncOperationHandle<Object>> callback;

			// Token: 0x0400032C RID: 812
			public static readonly ObjectPool<JsonSerializerTrackedObject.DeferredJsonObjectOperation> Pool = new ObjectPool<JsonSerializerTrackedObject.DeferredJsonObjectOperation>(() => new JsonSerializerTrackedObject.DeferredJsonObjectOperation(), null, null, null, false, 10, 10000);
		}

		// Token: 0x02000168 RID: 360
		internal struct ArrayResult
		{
			// Token: 0x17000198 RID: 408
			// (get) Token: 0x06000827 RID: 2087 RVA: 0x0001994B File Offset: 0x00017B4B
			public bool IsArraySize
			{
				get
				{
					return this.arrayStartIndex != -1 && this.arrayDataIndexStart == -1;
				}
			}

			// Token: 0x17000199 RID: 409
			// (get) Token: 0x06000828 RID: 2088 RVA: 0x00019964 File Offset: 0x00017B64
			public bool IsArrayElement
			{
				get
				{
					string text = this.path;
					int? num = ((text != null) ? new int?(text.Length) : null);
					int num2 = this.arrayDataIndexEnd + 1;
					return (num.GetValueOrDefault() == num2) & (num != null);
				}
			}

			// Token: 0x06000829 RID: 2089 RVA: 0x000199AC File Offset: 0x00017BAC
			public int GetDataIndex()
			{
				if (this.arrayDataIndexStart == -1)
				{
					return -1;
				}
				string text = this.path.Substring(this.arrayDataIndexStart, this.arrayDataIndexEnd - this.arrayDataIndexStart);
				uint num;
				if (uint.TryParse(text, out num))
				{
					return (int)num;
				}
				Debug.LogError(string.Concat(new string[] { "Failed to parse Array index `", text, "` from property path `", this.path, "`" }));
				return -1;
			}

			// Token: 0x0600082A RID: 2090 RVA: 0x00019A25 File Offset: 0x00017C25
			public ArrayResult(string p, int start, int bracketStart, int bracketEnd)
			{
				this.path = p;
				this.arrayStartIndex = start;
				this.arrayDataIndexStart = bracketStart;
				this.arrayDataIndexEnd = bracketEnd;
			}

			// Token: 0x0400032D RID: 813
			public string path;

			// Token: 0x0400032E RID: 814
			public int arrayStartIndex;

			// Token: 0x0400032F RID: 815
			public int arrayDataIndexStart;

			// Token: 0x04000330 RID: 816
			public int arrayDataIndexEnd;
		}
	}
}
