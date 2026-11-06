using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Text;

namespace System.Runtime.Serialization
{
	/// <summary>Keeps track of objects as they are deserialized.</summary>
	// Token: 0x02000661 RID: 1633
	[ComVisible(true)]
	public class ObjectManager
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.ObjectManager" /> class.</summary>
		/// <param name="selector">The surrogate selector to use. The <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> determines the correct surrogate to use when deserializing objects of a given type. At deserialization time, the surrogate selector creates a new instance of the object from the information transmitted on the stream.</param>
		/// <param name="context">The streaming context. The <see cref="T:System.Runtime.Serialization.StreamingContext" /> is not used by <see langword="ObjectManager" />, but is passed as a parameter to any objects implementing <see cref="T:System.Runtime.Serialization.ISerializable" /> or having a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. These objects can take specific actions depending on the source of the information to deserialize.</param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003CEB RID: 15595 RVA: 0x000D2D4E File Offset: 0x000D0F4E
		[SecuritySafeCritical]
		public ObjectManager(ISurrogateSelector selector, StreamingContext context)
			: this(selector, context, true, false)
		{
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x000D2D5A File Offset: 0x000D0F5A
		[SecurityCritical]
		internal ObjectManager(ISurrogateSelector selector, StreamingContext context, bool checkSecurity, bool isCrossAppDomain)
		{
			this.m_objects = new ObjectHolder[16];
			this.m_selector = selector;
			this.m_context = context;
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x000D2D7F File Offset: 0x000D0F7F
		[SecurityCritical]
		private bool CanCallGetType(object obj)
		{
			return !RemotingServices.IsTransparentProxy(obj);
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003CEF RID: 15599 RVA: 0x000D2D95 File Offset: 0x000D0F95
		// (set) Token: 0x06003CEE RID: 15598 RVA: 0x000D2D8C File Offset: 0x000D0F8C
		internal object TopObject
		{
			get
			{
				return this.m_topObject;
			}
			set
			{
				this.m_topObject = value;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003CF0 RID: 15600 RVA: 0x000D2D9D File Offset: 0x000D0F9D
		internal ObjectHolderList SpecialFixupObjects
		{
			get
			{
				if (this.m_specialFixupObjects == null)
				{
					this.m_specialFixupObjects = new ObjectHolderList();
				}
				return this.m_specialFixupObjects;
			}
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x000D2DB8 File Offset: 0x000D0FB8
		internal ObjectHolder FindObjectHolder(long objectID)
		{
			int num = (int)(objectID & 4095L);
			if (num >= this.m_objects.Length)
			{
				return null;
			}
			ObjectHolder objectHolder;
			for (objectHolder = this.m_objects[num]; objectHolder != null; objectHolder = objectHolder.m_next)
			{
				if (objectHolder.m_id == objectID)
				{
					return objectHolder;
				}
			}
			return objectHolder;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x000D2E00 File Offset: 0x000D1000
		internal ObjectHolder FindOrCreateObjectHolder(long objectID)
		{
			ObjectHolder objectHolder = this.FindObjectHolder(objectID);
			if (objectHolder == null)
			{
				objectHolder = new ObjectHolder(objectID);
				this.AddObjectHolder(objectHolder);
			}
			return objectHolder;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x000D2E28 File Offset: 0x000D1028
		private void AddObjectHolder(ObjectHolder holder)
		{
			if (holder.m_id >= (long)this.m_objects.Length && this.m_objects.Length != 4096)
			{
				int num = 4096;
				if (holder.m_id < 2048L)
				{
					num = this.m_objects.Length * 2;
					while ((long)num <= holder.m_id && num < 4096)
					{
						num *= 2;
					}
					if (num > 4096)
					{
						num = 4096;
					}
				}
				ObjectHolder[] array = new ObjectHolder[num];
				Array.Copy(this.m_objects, array, this.m_objects.Length);
				this.m_objects = array;
			}
			int num2 = (int)(holder.m_id & 4095L);
			ObjectHolder objectHolder = this.m_objects[num2];
			holder.m_next = objectHolder;
			this.m_objects[num2] = holder;
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x000D2EE4 File Offset: 0x000D10E4
		private bool GetCompletionInfo(FixupHolder fixup, out ObjectHolder holder, out object member, bool bThrowIfMissing)
		{
			member = fixup.m_fixupInfo;
			holder = this.FindObjectHolder(fixup.m_id);
			if (!holder.CompletelyFixed && holder.ObjectValue != null && holder.ObjectValue is ValueType)
			{
				this.SpecialFixupObjects.Add(holder);
				return false;
			}
			if (holder != null && !holder.CanObjectValueChange && holder.ObjectValue != null)
			{
				return true;
			}
			if (!bThrowIfMissing)
			{
				return false;
			}
			if (holder == null)
			{
				throw new SerializationException(Environment.GetResourceString("A fixup is registered to the object with ID {0}, but the object does not appear in the graph.", new object[] { fixup.m_id }));
			}
			if (holder.IsIncompleteObjectReference)
			{
				throw new SerializationException(Environment.GetResourceString("The object with ID {0} implements the IObjectReference interface for which all dependencies cannot be resolved. The likely cause is two instances of IObjectReference that have a mutual dependency on each other.", new object[] { fixup.m_id }));
			}
			throw new SerializationException(Environment.GetResourceString("The object with ID {0} was referenced in a fixup but does not exist.", new object[] { fixup.m_id }));
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x000D2FCC File Offset: 0x000D11CC
		[SecurityCritical]
		private void FixupSpecialObject(ObjectHolder holder)
		{
			ISurrogateSelector surrogateSelector = null;
			if (holder.HasSurrogate)
			{
				ISerializationSurrogate surrogate = holder.Surrogate;
				object obj = surrogate.SetObjectData(holder.ObjectValue, holder.SerializationInfo, this.m_context, surrogateSelector);
				if (obj != null)
				{
					if (!holder.CanSurrogatedObjectValueChange && obj != holder.ObjectValue)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("{0}.SetObjectData returns a value that is neither null nor equal to the first parameter. Such Surrogates cannot be part of cyclical reference."), surrogate.GetType().FullName));
					}
					holder.SetObjectValue(obj, this);
				}
				holder.m_surrogate = null;
				holder.SetFlags();
			}
			else
			{
				this.CompleteISerializableObject(holder.ObjectValue, holder.SerializationInfo, this.m_context);
			}
			holder.SerializationInfo = null;
			holder.RequiresSerInfoFixup = false;
			if (holder.RequiresValueTypeFixup && holder.ValueTypeFixupPerformed)
			{
				this.DoValueTypeFixup(null, holder, holder.ObjectValue);
			}
			this.DoNewlyRegisteredObjectFixups(holder);
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x000D30A0 File Offset: 0x000D12A0
		[SecurityCritical]
		private bool ResolveObjectReference(ObjectHolder holder)
		{
			int num = 0;
			try
			{
				object objectValue;
				for (;;)
				{
					objectValue = holder.ObjectValue;
					holder.SetObjectValue(((IObjectReference)holder.ObjectValue).GetRealObject(this.m_context), this);
					if (holder.ObjectValue == null)
					{
						break;
					}
					if (num++ == 100)
					{
						goto Block_3;
					}
					if (!(holder.ObjectValue is IObjectReference) || objectValue == holder.ObjectValue)
					{
						goto IL_0069;
					}
				}
				holder.SetObjectValue(objectValue, this);
				return false;
				Block_3:
				throw new SerializationException(Environment.GetResourceString("The implementation of the IObjectReference interface returns too many nested references to other objects that implement IObjectReference."));
				IL_0069:;
			}
			catch (NullReferenceException)
			{
				return false;
			}
			holder.IsIncompleteObjectReference = false;
			this.DoNewlyRegisteredObjectFixups(holder);
			return true;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x000D3140 File Offset: 0x000D1340
		[SecurityCritical]
		private bool DoValueTypeFixup(FieldInfo memberToFix, ObjectHolder holder, object value)
		{
			FieldInfo[] array = new FieldInfo[4];
			int num = 0;
			int[] array2 = null;
			object obj = holder.ObjectValue;
			while (holder.RequiresValueTypeFixup)
			{
				if (num + 1 >= array.Length)
				{
					FieldInfo[] array3 = new FieldInfo[array.Length * 2];
					Array.Copy(array, array3, array.Length);
					array = array3;
				}
				ValueTypeFixupInfo valueFixup = holder.ValueFixup;
				obj = holder.ObjectValue;
				if (valueFixup.ParentField != null)
				{
					FieldInfo parentField = valueFixup.ParentField;
					ObjectHolder objectHolder = this.FindObjectHolder(valueFixup.ContainerID);
					if (objectHolder.ObjectValue == null)
					{
						break;
					}
					if (Nullable.GetUnderlyingType(parentField.FieldType) != null)
					{
						array[num] = parentField.FieldType.GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
						num++;
					}
					array[num] = parentField;
					holder = objectHolder;
					num++;
				}
				else
				{
					holder = this.FindObjectHolder(valueFixup.ContainerID);
					array2 = valueFixup.ParentIndex;
					if (holder.ObjectValue == null)
					{
						break;
					}
					break;
				}
			}
			if (!(holder.ObjectValue is Array) && holder.ObjectValue != null)
			{
				obj = holder.ObjectValue;
			}
			if (num != 0)
			{
				FieldInfo[] array4 = new FieldInfo[num];
				for (int i = 0; i < num; i++)
				{
					FieldInfo fieldInfo = array[num - 1 - i];
					SerializationFieldInfo serializationFieldInfo = fieldInfo as SerializationFieldInfo;
					array4[i] = ((serializationFieldInfo == null) ? fieldInfo : serializationFieldInfo.FieldInfo);
				}
				TypedReference typedReference = TypedReference.MakeTypedReference(obj, array4);
				if (memberToFix != null)
				{
					((RuntimeFieldInfo)memberToFix).SetValueDirect(typedReference, value);
				}
				else
				{
					TypedReference.SetTypedReference(typedReference, value);
				}
			}
			else if (memberToFix != null)
			{
				FormatterServices.SerializationSetValue(memberToFix, obj, value);
			}
			if (array2 != null && holder.ObjectValue != null)
			{
				((Array)holder.ObjectValue).SetValue(obj, array2);
			}
			return true;
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x000D32F8 File Offset: 0x000D14F8
		[Conditional("SER_LOGGING")]
		private void DumpValueTypeFixup(object obj, FieldInfo[] intermediateFields, FieldInfo memberToFix, object value)
		{
			StringBuilder stringBuilder = new StringBuilder("  " + ((obj != null) ? obj.ToString() : null));
			if (intermediateFields != null)
			{
				for (int i = 0; i < intermediateFields.Length; i++)
				{
					stringBuilder.Append("." + intermediateFields[i].Name);
				}
			}
			stringBuilder.Append("." + memberToFix.Name + "=" + ((value != null) ? value.ToString() : null));
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x000D3378 File Offset: 0x000D1578
		[SecurityCritical]
		internal void CompleteObject(ObjectHolder holder, bool bObjectFullyComplete)
		{
			FixupHolderList missingElements = holder.m_missingElements;
			object obj = null;
			ObjectHolder objectHolder = null;
			int num = 0;
			if (holder.ObjectValue == null)
			{
				throw new SerializationException(Environment.GetResourceString("The object with ID {0} was referenced in a fixup but has not been registered.", new object[] { holder.m_id }));
			}
			if (missingElements == null)
			{
				return;
			}
			if (holder.HasSurrogate || holder.HasISerializable)
			{
				SerializationInfo serInfo = holder.m_serInfo;
				if (serInfo == null)
				{
					throw new SerializationException(Environment.GetResourceString("A fixup on an object implementing ISerializable or having a surrogate was discovered for an object which does not have a SerializationInfo available."));
				}
				if (missingElements != null)
				{
					for (int i = 0; i < missingElements.m_count; i++)
					{
						if (missingElements.m_values[i] != null && this.GetCompletionInfo(missingElements.m_values[i], out objectHolder, out obj, bObjectFullyComplete))
						{
							object objectValue = objectHolder.ObjectValue;
							if (this.CanCallGetType(objectValue))
							{
								serInfo.UpdateValue((string)obj, objectValue, objectValue.GetType());
							}
							else
							{
								serInfo.UpdateValue((string)obj, objectValue, typeof(MarshalByRefObject));
							}
							num++;
							missingElements.m_values[i] = null;
							if (!bObjectFullyComplete)
							{
								holder.DecrementFixupsRemaining(this);
								objectHolder.RemoveDependency(holder.m_id);
							}
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < missingElements.m_count; j++)
				{
					FixupHolder fixupHolder = missingElements.m_values[j];
					if (fixupHolder != null && this.GetCompletionInfo(fixupHolder, out objectHolder, out obj, bObjectFullyComplete))
					{
						if (objectHolder.TypeLoadExceptionReachable)
						{
							holder.TypeLoadException = objectHolder.TypeLoadException;
							if (holder.Reachable)
							{
								throw new SerializationException(Environment.GetResourceString("Unable to load type {0} required for deserialization.", new object[] { holder.TypeLoadException.TypeName }));
							}
						}
						if (holder.Reachable)
						{
							objectHolder.Reachable = true;
						}
						int fixupType = fixupHolder.m_fixupType;
						if (fixupType != 1)
						{
							if (fixupType != 2)
							{
								throw new SerializationException(Environment.GetResourceString("Cannot perform fixup."));
							}
							MemberInfo memberInfo = (MemberInfo)obj;
							if (memberInfo.MemberType != MemberTypes.Field)
							{
								throw new SerializationException(Environment.GetResourceString("Cannot perform fixup."));
							}
							if (holder.RequiresValueTypeFixup && holder.ValueTypeFixupPerformed)
							{
								if (!this.DoValueTypeFixup((FieldInfo)memberInfo, holder, objectHolder.ObjectValue))
								{
									throw new SerializationException(Environment.GetResourceString("Fixing up a partially available ValueType chain is not implemented."));
								}
							}
							else
							{
								FormatterServices.SerializationSetValue(memberInfo, holder.ObjectValue, objectHolder.ObjectValue);
							}
							if (objectHolder.RequiresValueTypeFixup)
							{
								objectHolder.ValueTypeFixupPerformed = true;
							}
						}
						else
						{
							if (holder.RequiresValueTypeFixup)
							{
								throw new SerializationException(Environment.GetResourceString("ValueType fixup on Arrays is not implemented."));
							}
							((Array)holder.ObjectValue).SetValue(objectHolder.ObjectValue, (int[])obj);
						}
						num++;
						missingElements.m_values[j] = null;
						if (!bObjectFullyComplete)
						{
							holder.DecrementFixupsRemaining(this);
							objectHolder.RemoveDependency(holder.m_id);
						}
					}
				}
			}
			this.m_fixupCount -= (long)num;
			if (missingElements.m_count == num)
			{
				holder.m_missingElements = null;
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x000D3658 File Offset: 0x000D1858
		[SecurityCritical]
		private void DoNewlyRegisteredObjectFixups(ObjectHolder holder)
		{
			if (holder.CanObjectValueChange)
			{
				return;
			}
			LongList dependentObjects = holder.DependentObjects;
			if (dependentObjects == null)
			{
				return;
			}
			dependentObjects.StartEnumeration();
			while (dependentObjects.MoveNext())
			{
				long num = dependentObjects.Current;
				ObjectHolder objectHolder = this.FindObjectHolder(num);
				objectHolder.DecrementFixupsRemaining(this);
				if (objectHolder.DirectlyDependentObjects == 0)
				{
					if (objectHolder.ObjectValue != null)
					{
						this.CompleteObject(objectHolder, true);
					}
					else
					{
						objectHolder.MarkForCompletionWhenAvailable();
					}
				}
			}
		}

		/// <summary>Returns the object with the specified object ID.</summary>
		/// <param name="objectID">The ID of the requested object.</param>
		/// <returns>The object with the specified object ID if it has been previously stored or <see langword="null" /> if no such object has been registered.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero.</exception>
		// Token: 0x06003CFC RID: 15612 RVA: 0x000D36C0 File Offset: 0x000D18C0
		public virtual object GetObject(long objectID)
		{
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", Environment.GetResourceString("objectID cannot be less than or equal to zero."));
			}
			ObjectHolder objectHolder = this.FindObjectHolder(objectID);
			if (objectHolder == null || objectHolder.CanObjectValueChange)
			{
				return null;
			}
			return objectHolder.ObjectValue;
		}

		/// <summary>Registers an object as it is deserialized, associating it with <paramref name="objectID" />.</summary>
		/// <param name="obj">The object to register.</param>
		/// <param name="objectID">The ID of the object to register.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />.</exception>
		// Token: 0x06003CFD RID: 15613 RVA: 0x000D3702 File Offset: 0x000D1902
		[SecurityCritical]
		public virtual void RegisterObject(object obj, long objectID)
		{
			this.RegisterObject(obj, objectID, null, 0L, null);
		}

		/// <summary>Registers an object as it is deserialized, associating it with <paramref name="objectID" />, and recording the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used with it.</summary>
		/// <param name="obj">The object to register.</param>
		/// <param name="objectID">The ID of the object to register.</param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used if <paramref name="obj" /> implements <see cref="T:System.Runtime.Serialization.ISerializable" /> or has a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. <paramref name="info" /> will be completed with any required fixup information and then passed to the required object when that object is completed.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />.</exception>
		// Token: 0x06003CFE RID: 15614 RVA: 0x000D3710 File Offset: 0x000D1910
		[SecurityCritical]
		public void RegisterObject(object obj, long objectID, SerializationInfo info)
		{
			this.RegisterObject(obj, objectID, info, 0L, null);
		}

		/// <summary>Registers a member of an object as it is deserialized, associating it with <paramref name="objectID" />, and recording the <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="obj">The object to register.</param>
		/// <param name="objectID">The ID of the object to register.</param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used if <paramref name="obj" /> implements <see cref="T:System.Runtime.Serialization.ISerializable" /> or has a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. <paramref name="info" /> will be completed with any required fixup information and then passed to the required object when that object is completed.</param>
		/// <param name="idOfContainingObj">The ID of the object that contains <paramref name="obj" />. This parameter is required only if <paramref name="obj" /> is a value type.</param>
		/// <param name="member">The field in the containing object where <paramref name="obj" /> exists. This parameter has meaning only if <paramref name="obj" /> is a value type.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />, or <paramref name="member" /> is not a <see cref="T:System.Reflection.FieldInfo" /> and <paramref name="member" /> is not <see langword="null" />.</exception>
		// Token: 0x06003CFF RID: 15615 RVA: 0x000D371E File Offset: 0x000D191E
		[SecurityCritical]
		public void RegisterObject(object obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member)
		{
			this.RegisterObject(obj, objectID, info, idOfContainingObj, member, null);
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x000D3730 File Offset: 0x000D1930
		internal void RegisterString(string obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member)
		{
			ObjectHolder objectHolder = new ObjectHolder(obj, objectID, info, null, idOfContainingObj, (FieldInfo)member, null);
			this.AddObjectHolder(objectHolder);
		}

		/// <summary>Registers a member of an array contained in an object while it is deserialized, associating it with <paramref name="objectID" />, and recording the <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="obj">The object to register.</param>
		/// <param name="objectID">The ID of the object to register.</param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used if <paramref name="obj" /> implements <see cref="T:System.Runtime.Serialization.ISerializable" /> or has a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. <paramref name="info" /> will be completed with any required fixup information and then passed to the required object when that object is completed.</param>
		/// <param name="idOfContainingObj">The ID of the object that contains <paramref name="obj" />. This parameter is required only if <paramref name="obj" /> is a value type.</param>
		/// <param name="member">The field in the containing object where <paramref name="obj" /> exists. This parameter has meaning only if <paramref name="obj" /> is a value type.</param>
		/// <param name="arrayIndex">If <paramref name="obj" /> is a <see cref="T:System.ValueType" /> and a member of an array, <paramref name="arrayIndex" /> contains the index within that array where <paramref name="obj" /> exists. <paramref name="arrayIndex" /> is ignored if <paramref name="obj" /> is not both a <see cref="T:System.ValueType" /> and a member of an array.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />, or <paramref name="member" /> is not a <see cref="T:System.Reflection.FieldInfo" /> and <paramref name="member" /> isn't <see langword="null" />.</exception>
		// Token: 0x06003D01 RID: 15617 RVA: 0x000D3758 File Offset: 0x000D1958
		[SecurityCritical]
		public void RegisterObject(object obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member, int[] arrayIndex)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", Environment.GetResourceString("objectID cannot be less than or equal to zero."));
			}
			if (member != null && !(member is RuntimeFieldInfo) && !(member is SerializationFieldInfo))
			{
				throw new SerializationException(Environment.GetResourceString("Only FieldInfo, PropertyInfo, and SerializationMemberInfo are recognized."));
			}
			ISerializationSurrogate serializationSurrogate = null;
			if (this.m_selector != null)
			{
				Type type;
				if (this.CanCallGetType(obj))
				{
					type = obj.GetType();
				}
				else
				{
					type = typeof(MarshalByRefObject);
				}
				ISurrogateSelector surrogateSelector;
				serializationSurrogate = this.m_selector.GetSurrogate(type, this.m_context, out surrogateSelector);
			}
			if (obj is IDeserializationCallback)
			{
				DeserializationEventHandler deserializationEventHandler = new DeserializationEventHandler(((IDeserializationCallback)obj).OnDeserialization);
				this.AddOnDeserialization(deserializationEventHandler);
			}
			if (arrayIndex != null)
			{
				arrayIndex = (int[])arrayIndex.Clone();
			}
			ObjectHolder objectHolder = this.FindObjectHolder(objectID);
			if (objectHolder == null)
			{
				objectHolder = new ObjectHolder(obj, objectID, info, serializationSurrogate, idOfContainingObj, (FieldInfo)member, arrayIndex);
				this.AddObjectHolder(objectHolder);
				if (objectHolder.RequiresDelayedFixup)
				{
					this.SpecialFixupObjects.Add(objectHolder);
				}
				this.AddOnDeserialized(obj);
				return;
			}
			if (objectHolder.ObjectValue != null)
			{
				throw new SerializationException(Environment.GetResourceString("An object cannot be registered twice."));
			}
			objectHolder.UpdateData(obj, info, serializationSurrogate, idOfContainingObj, (FieldInfo)member, arrayIndex, this);
			if (objectHolder.DirectlyDependentObjects > 0)
			{
				this.CompleteObject(objectHolder, false);
			}
			if (objectHolder.RequiresDelayedFixup)
			{
				this.SpecialFixupObjects.Add(objectHolder);
			}
			if (objectHolder.CompletelyFixed)
			{
				this.DoNewlyRegisteredObjectFixups(objectHolder);
				objectHolder.DependentObjects = null;
			}
			if (objectHolder.TotalDependentObjects > 0)
			{
				this.AddOnDeserialized(obj);
				return;
			}
			this.RaiseOnDeserializedEvent(obj);
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x000D38F0 File Offset: 0x000D1AF0
		[SecurityCritical]
		internal void CompleteISerializableObject(object obj, SerializationInfo info, StreamingContext context)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!(obj is ISerializable))
			{
				throw new ArgumentException(Environment.GetResourceString("The given object does not implement the ISerializable interface."));
			}
			RuntimeConstructorInfo runtimeConstructorInfo = null;
			RuntimeType runtimeType = (RuntimeType)obj.GetType();
			try
			{
				runtimeConstructorInfo = ObjectManager.GetConstructor(runtimeType);
			}
			catch (Exception ex)
			{
				throw new SerializationException(Environment.GetResourceString("The constructor to deserialize an object of type '{0}' was not found.", new object[] { runtimeType }), ex);
			}
			runtimeConstructorInfo.SerializationInvoke(obj, info, context);
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x000D3970 File Offset: 0x000D1B70
		internal static RuntimeConstructorInfo GetConstructor(RuntimeType t)
		{
			RuntimeConstructorInfo serializationCtor = t.GetSerializationCtor();
			if (serializationCtor == null)
			{
				throw new SerializationException(Environment.GetResourceString("The constructor to deserialize an object of type '{0}' was not found.", new object[] { t.FullName }));
			}
			return serializationCtor;
		}

		/// <summary>Performs all the recorded fixups.</summary>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A fixup was not successfully completed.</exception>
		// Token: 0x06003D04 RID: 15620 RVA: 0x000D39B0 File Offset: 0x000D1BB0
		[SecuritySafeCritical]
		public virtual void DoFixups()
		{
			int num = -1;
			while (num != 0)
			{
				num = 0;
				ObjectHolderListEnumerator fixupEnumerator = this.SpecialFixupObjects.GetFixupEnumerator();
				while (fixupEnumerator.MoveNext())
				{
					ObjectHolder objectHolder = fixupEnumerator.Current;
					if (objectHolder.ObjectValue == null)
					{
						throw new SerializationException(Environment.GetResourceString("The object with ID {0} was referenced in a fixup but does not exist.", new object[] { objectHolder.m_id }));
					}
					if (objectHolder.TotalDependentObjects == 0)
					{
						if (objectHolder.RequiresSerInfoFixup)
						{
							this.FixupSpecialObject(objectHolder);
							num++;
						}
						else if (!objectHolder.IsIncompleteObjectReference)
						{
							this.CompleteObject(objectHolder, true);
						}
						if (objectHolder.IsIncompleteObjectReference && this.ResolveObjectReference(objectHolder))
						{
							num++;
						}
					}
				}
			}
			if (this.m_fixupCount != 0L)
			{
				for (int i = 0; i < this.m_objects.Length; i++)
				{
					for (ObjectHolder objectHolder = this.m_objects[i]; objectHolder != null; objectHolder = objectHolder.m_next)
					{
						if (objectHolder.TotalDependentObjects > 0)
						{
							this.CompleteObject(objectHolder, true);
						}
					}
					if (this.m_fixupCount == 0L)
					{
						return;
					}
				}
				throw new SerializationException(Environment.GetResourceString("The ObjectManager found an invalid number of fixups. This usually indicates a problem in the Formatter."));
			}
			if (this.TopObject is TypeLoadExceptionHolder)
			{
				throw new SerializationException(Environment.GetResourceString("Unable to load type {0} required for deserialization.", new object[] { ((TypeLoadExceptionHolder)this.TopObject).TypeName }));
			}
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x000D3AE8 File Offset: 0x000D1CE8
		private void RegisterFixup(FixupHolder fixup, long objectToBeFixed, long objectRequired)
		{
			ObjectHolder objectHolder = this.FindOrCreateObjectHolder(objectToBeFixed);
			if (objectHolder.RequiresSerInfoFixup && fixup.m_fixupType == 2)
			{
				throw new SerializationException(Environment.GetResourceString("A member fixup was registered for an object which implements ISerializable or has a surrogate. In this situation, a delayed fixup must be used."));
			}
			objectHolder.AddFixup(fixup, this);
			this.FindOrCreateObjectHolder(objectRequired).AddDependency(objectToBeFixed);
			this.m_fixupCount += 1L;
		}

		/// <summary>Records a fixup for a member of an object, to be executed later.</summary>
		/// <param name="objectToBeFixed">The ID of the object that needs the reference to the <paramref name="objectRequired" /> object.</param>
		/// <param name="member">The member of <paramref name="objectToBeFixed" /> where the fixup will be performed.</param>
		/// <param name="objectRequired">The ID of the object required by <paramref name="objectToBeFixed" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="member" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003D06 RID: 15622 RVA: 0x000D3B40 File Offset: 0x000D1D40
		public virtual void RecordFixup(long objectToBeFixed, MemberInfo member, long objectRequired)
		{
			if (objectToBeFixed <= 0L || objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException((objectToBeFixed <= 0L) ? "objectToBeFixed" : "objectRequired", Environment.GetResourceString("Object IDs must be greater than zero."));
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (!(member is RuntimeFieldInfo) && !(member is SerializationFieldInfo))
			{
				throw new SerializationException(Environment.GetResourceString("Only system-provided types can be passed to the GetUninitializedObject method. '{0}' is not a valid instance of a type.", new object[] { member.GetType().ToString() }));
			}
			FixupHolder fixupHolder = new FixupHolder(objectRequired, member, 2);
			this.RegisterFixup(fixupHolder, objectToBeFixed, objectRequired);
		}

		/// <summary>Records a fixup for an object member, to be executed later.</summary>
		/// <param name="objectToBeFixed">The ID of the object that needs the reference to <paramref name="objectRequired" />.</param>
		/// <param name="memberName">The member name of <paramref name="objectToBeFixed" /> where the fixup will be performed.</param>
		/// <param name="objectRequired">The ID of the object required by <paramref name="objectToBeFixed" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="objectToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="memberName" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003D07 RID: 15623 RVA: 0x000D3BD4 File Offset: 0x000D1DD4
		public virtual void RecordDelayedFixup(long objectToBeFixed, string memberName, long objectRequired)
		{
			if (objectToBeFixed <= 0L || objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException((objectToBeFixed <= 0L) ? "objectToBeFixed" : "objectRequired", Environment.GetResourceString("Object IDs must be greater than zero."));
			}
			if (memberName == null)
			{
				throw new ArgumentNullException("memberName");
			}
			FixupHolder fixupHolder = new FixupHolder(objectRequired, memberName, 4);
			this.RegisterFixup(fixupHolder, objectToBeFixed, objectRequired);
		}

		/// <summary>Records a fixup for one element in an array.</summary>
		/// <param name="arrayToBeFixed">The ID of the array used to record a fixup.</param>
		/// <param name="index">The index within arrayFixup that a fixup is requested for.</param>
		/// <param name="objectRequired">The ID of the object that the current array element will point to after fixup is completed.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="arrayToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="index" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003D08 RID: 15624 RVA: 0x000D3C2C File Offset: 0x000D1E2C
		public virtual void RecordArrayElementFixup(long arrayToBeFixed, int index, long objectRequired)
		{
			this.RecordArrayElementFixup(arrayToBeFixed, new int[] { index }, objectRequired);
		}

		/// <summary>Records fixups for the specified elements in an array, to be executed later.</summary>
		/// <param name="arrayToBeFixed">The ID of the array used to record a fixup.</param>
		/// <param name="indices">The indexes within the multidimensional array that a fixup is requested for.</param>
		/// <param name="objectRequired">The ID of the object the array elements will point to after fixup is completed.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="arrayToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="indices" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003D09 RID: 15625 RVA: 0x000D3C50 File Offset: 0x000D1E50
		public virtual void RecordArrayElementFixup(long arrayToBeFixed, int[] indices, long objectRequired)
		{
			if (arrayToBeFixed <= 0L || objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException((arrayToBeFixed <= 0L) ? "objectToBeFixed" : "objectRequired", Environment.GetResourceString("Object IDs must be greater than zero."));
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			FixupHolder fixupHolder = new FixupHolder(objectRequired, indices, 1);
			this.RegisterFixup(fixupHolder, arrayToBeFixed, objectRequired);
		}

		/// <summary>Raises the deserialization event to any registered object that implements <see cref="T:System.Runtime.Serialization.IDeserializationCallback" />.</summary>
		// Token: 0x06003D0A RID: 15626 RVA: 0x000D3CA8 File Offset: 0x000D1EA8
		public virtual void RaiseDeserializationEvent()
		{
			if (this.m_onDeserializedHandler != null)
			{
				this.m_onDeserializedHandler(this.m_context);
			}
			if (this.m_onDeserializationHandler != null)
			{
				this.m_onDeserializationHandler(null);
			}
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x000D3CD7 File Offset: 0x000D1ED7
		internal virtual void AddOnDeserialization(DeserializationEventHandler handler)
		{
			this.m_onDeserializationHandler = (DeserializationEventHandler)Delegate.Combine(this.m_onDeserializationHandler, handler);
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x000D3CF0 File Offset: 0x000D1EF0
		internal virtual void RemoveOnDeserialization(DeserializationEventHandler handler)
		{
			this.m_onDeserializationHandler = (DeserializationEventHandler)Delegate.Remove(this.m_onDeserializationHandler, handler);
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x000D3D0C File Offset: 0x000D1F0C
		[SecuritySafeCritical]
		internal virtual void AddOnDeserialized(object obj)
		{
			SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
			this.m_onDeserializedHandler = serializationEventsForType.AddOnDeserialized(obj, this.m_onDeserializedHandler);
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x000D3D38 File Offset: 0x000D1F38
		internal virtual void RaiseOnDeserializedEvent(object obj)
		{
			SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).InvokeOnDeserialized(obj, this.m_context);
		}

		/// <summary>Invokes the method marked with the <see cref="T:System.Runtime.Serialization.OnDeserializingAttribute" />.</summary>
		/// <param name="obj">The instance of the type that contains the method to be invoked.</param>
		// Token: 0x06003D0F RID: 15631 RVA: 0x000D3D51 File Offset: 0x000D1F51
		public void RaiseOnDeserializingEvent(object obj)
		{
			SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).InvokeOnDeserializing(obj, this.m_context);
		}

		// Token: 0x04002742 RID: 10050
		private const int DefaultInitialSize = 16;

		// Token: 0x04002743 RID: 10051
		private const int MaxArraySize = 4096;

		// Token: 0x04002744 RID: 10052
		private const int ArrayMask = 4095;

		// Token: 0x04002745 RID: 10053
		private const int MaxReferenceDepth = 100;

		// Token: 0x04002746 RID: 10054
		private DeserializationEventHandler m_onDeserializationHandler;

		// Token: 0x04002747 RID: 10055
		private SerializationEventHandler m_onDeserializedHandler;

		// Token: 0x04002748 RID: 10056
		internal ObjectHolder[] m_objects;

		// Token: 0x04002749 RID: 10057
		internal object m_topObject;

		// Token: 0x0400274A RID: 10058
		internal ObjectHolderList m_specialFixupObjects;

		// Token: 0x0400274B RID: 10059
		internal long m_fixupCount;

		// Token: 0x0400274C RID: 10060
		internal ISurrogateSelector m_selector;

		// Token: 0x0400274D RID: 10061
		internal StreamingContext m_context;
	}
}
