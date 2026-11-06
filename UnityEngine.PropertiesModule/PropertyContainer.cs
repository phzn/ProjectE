using System;
using System.Collections.Generic;
using Unity.Properties.Internal;
using UnityEngine.Pool;

namespace Unity.Properties
{
	// Token: 0x02000004 RID: 4
	public static class PropertyContainer
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002064 File Offset: 0x00000264
		public static void Accept<TContainer>(IPropertyBagVisitor visitor, TContainer container, VisitParameters parameters = default(VisitParameters))
		{
			VisitReturnCode visitReturnCode = VisitReturnCode.Ok;
			try
			{
				bool flag = PropertyContainer.TryAccept<TContainer>(visitor, ref container, out visitReturnCode, parameters);
				if (flag)
				{
					return;
				}
			}
			catch (Exception)
			{
				bool flag2 = (parameters.IgnoreExceptions & VisitExceptionKind.Visitor) == VisitExceptionKind.None;
				if (flag2)
				{
					throw;
				}
			}
			bool flag3 = (parameters.IgnoreExceptions & VisitExceptionKind.Internal) > VisitExceptionKind.None;
			if (!flag3)
			{
				switch (visitReturnCode)
				{
				case VisitReturnCode.Ok:
				case VisitReturnCode.InvalidContainerType:
					break;
				case VisitReturnCode.NullContainer:
					throw new ArgumentException("The given container was null. Visitation only works for valid non-null containers.");
				case VisitReturnCode.MissingPropertyBag:
					throw new MissingPropertyBagException(container.GetType());
				default:
					throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002120 File Offset: 0x00000320
		public static void Accept<TContainer>(IPropertyBagVisitor visitor, ref TContainer container, VisitParameters parameters = default(VisitParameters))
		{
			VisitReturnCode visitReturnCode = VisitReturnCode.Ok;
			try
			{
				bool flag = PropertyContainer.TryAccept<TContainer>(visitor, ref container, out visitReturnCode, parameters);
				if (flag)
				{
					return;
				}
			}
			catch (Exception)
			{
				bool flag2 = (parameters.IgnoreExceptions & VisitExceptionKind.Visitor) == VisitExceptionKind.None;
				if (flag2)
				{
					throw;
				}
			}
			bool flag3 = (parameters.IgnoreExceptions & VisitExceptionKind.Internal) > VisitExceptionKind.None;
			if (!flag3)
			{
				switch (visitReturnCode)
				{
				case VisitReturnCode.Ok:
				case VisitReturnCode.InvalidContainerType:
					break;
				case VisitReturnCode.NullContainer:
					throw new ArgumentException("The given container was null. Visitation only works for valid non-null containers.");
				case VisitReturnCode.MissingPropertyBag:
					throw new MissingPropertyBagException(container.GetType());
				default:
					throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021DC File Offset: 0x000003DC
		public static bool TryAccept<TContainer>(IPropertyBagVisitor visitor, ref TContainer container, VisitParameters parameters = default(VisitParameters))
		{
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TryAccept<TContainer>(visitor, ref container, out visitReturnCode, parameters);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021F8 File Offset: 0x000003F8
		public static bool TryAccept<TContainer>(IPropertyBagVisitor visitor, ref TContainer container, out VisitReturnCode returnCode, VisitParameters parameters = default(VisitParameters))
		{
			bool flag = !TypeTraits<TContainer>.IsContainer;
			bool flag2;
			if (flag)
			{
				returnCode = VisitReturnCode.InvalidContainerType;
				flag2 = false;
			}
			else
			{
				bool canBeNull = TypeTraits<TContainer>.CanBeNull;
				if (canBeNull)
				{
					bool flag3 = EqualityComparer<TContainer>.Default.Equals(container, default(TContainer));
					if (flag3)
					{
						returnCode = VisitReturnCode.NullContainer;
						return false;
					}
				}
				bool flag4 = !TypeTraits<TContainer>.IsValueType && typeof(TContainer) != container.GetType();
				if (flag4)
				{
					bool flag5 = !TypeTraits.IsContainer(container.GetType());
					if (flag5)
					{
						returnCode = VisitReturnCode.InvalidContainerType;
						return false;
					}
					IPropertyBag propertyBag = PropertyBagStore.GetPropertyBag(container.GetType());
					bool flag6 = propertyBag == null;
					if (flag6)
					{
						returnCode = VisitReturnCode.MissingPropertyBag;
						return false;
					}
					object obj = container;
					propertyBag.Accept(visitor, ref obj);
					container = (TContainer)((object)obj);
				}
				else
				{
					IPropertyBag<TContainer> propertyBag2 = PropertyBagStore.GetPropertyBag<TContainer>();
					bool flag7 = propertyBag2 == null;
					if (flag7)
					{
						returnCode = VisitReturnCode.MissingPropertyBag;
						return false;
					}
					PropertyBag.AcceptWithSpecializedVisitor<TContainer>(propertyBag2, visitor, ref container);
				}
				returnCode = VisitReturnCode.Ok;
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000231C File Offset: 0x0000051C
		public static void Accept<TContainer>(IPropertyVisitor visitor, ref TContainer container, in PropertyPath path, VisitParameters parameters = default(VisitParameters))
		{
			PropertyContainer.ValueAtPathVisitor valueAtPathVisitor = PropertyContainer.ValueAtPathVisitor.Pool.Get();
			try
			{
				valueAtPathVisitor.Path = path;
				valueAtPathVisitor.Visitor = visitor;
				PropertyContainer.Accept<TContainer>(valueAtPathVisitor, ref container, parameters);
				bool flag = (parameters.IgnoreExceptions & VisitExceptionKind.Internal) == VisitExceptionKind.None;
				if (flag)
				{
					VisitReturnCode returnCode = valueAtPathVisitor.ReturnCode;
					VisitReturnCode visitReturnCode = returnCode;
					if (visitReturnCode != VisitReturnCode.Ok)
					{
						if (visitReturnCode != VisitReturnCode.InvalidPath)
						{
							throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", valueAtPathVisitor.ReturnCode));
						}
						throw new InvalidPathException(string.Format("Failed to Visit at Path=[{0}]", path));
					}
				}
			}
			finally
			{
				PropertyContainer.ValueAtPathVisitor.Pool.Release(valueAtPathVisitor);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023D8 File Offset: 0x000005D8
		public static bool TryAccept<TContainer>(IPropertyVisitor visitor, ref TContainer container, in PropertyPath path, out VisitReturnCode returnCode, VisitParameters parameters = default(VisitParameters))
		{
			PropertyContainer.ValueAtPathVisitor valueAtPathVisitor = PropertyContainer.ValueAtPathVisitor.Pool.Get();
			bool flag;
			try
			{
				valueAtPathVisitor.Path = path;
				valueAtPathVisitor.Visitor = visitor;
				flag = PropertyContainer.TryAccept<TContainer>(valueAtPathVisitor, ref container, out returnCode, parameters);
			}
			finally
			{
				PropertyContainer.ValueAtPathVisitor.Pool.Release(valueAtPathVisitor);
			}
			return flag;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002434 File Offset: 0x00000634
		public static IProperty GetProperty<TContainer>(TContainer container, in PropertyPath path)
		{
			return PropertyContainer.GetProperty<TContainer>(ref container, in path);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002440 File Offset: 0x00000640
		public static IProperty GetProperty<TContainer>(ref TContainer container, in PropertyPath path)
		{
			IProperty property;
			VisitReturnCode visitReturnCode;
			bool flag = PropertyContainer.TryGetProperty<TContainer>(ref container, in path, out property, out visitReturnCode);
			if (flag)
			{
				return property;
			}
			switch (visitReturnCode)
			{
			case VisitReturnCode.NullContainer:
				throw new ArgumentNullException("container");
			case VisitReturnCode.InvalidContainerType:
				throw new InvalidContainerTypeException(container.GetType());
			case VisitReturnCode.MissingPropertyBag:
				throw new MissingPropertyBagException(container.GetType());
			case VisitReturnCode.InvalidPath:
				throw new ArgumentException(string.Format("Failed to get property for path=[{0}]", path));
			default:
				throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024F0 File Offset: 0x000006F0
		public static bool TryGetProperty<TContainer>(TContainer container, in PropertyPath path, out IProperty property)
		{
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TryGetProperty<TContainer>(ref container, in path, out property, out visitReturnCode);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002508 File Offset: 0x00000708
		public static bool TryGetProperty<TContainer>(ref TContainer container, in PropertyPath path, out IProperty property)
		{
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TryGetProperty<TContainer>(ref container, in path, out property, out visitReturnCode);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002520 File Offset: 0x00000720
		public static bool TryGetProperty<TContainer>(ref TContainer container, in PropertyPath path, out IProperty property, out VisitReturnCode returnCode)
		{
			PropertyContainer.GetPropertyVisitor getPropertyVisitor = PropertyContainer.GetPropertyVisitor.Pool.Get();
			bool flag2;
			try
			{
				getPropertyVisitor.Path = path;
				bool flag = !PropertyContainer.TryAccept<TContainer>(getPropertyVisitor, ref container, out returnCode, default(VisitParameters));
				if (flag)
				{
					property = null;
					flag2 = false;
				}
				else
				{
					returnCode = getPropertyVisitor.ReturnCode;
					property = getPropertyVisitor.Property;
					flag2 = returnCode == VisitReturnCode.Ok;
				}
			}
			finally
			{
				PropertyContainer.GetPropertyVisitor.Pool.Release(getPropertyVisitor);
			}
			return flag2;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000025A0 File Offset: 0x000007A0
		public static TValue GetValue<TContainer, TValue>(TContainer container, string name)
		{
			return PropertyContainer.GetValue<TContainer, TValue>(ref container, name);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025AC File Offset: 0x000007AC
		public static TValue GetValue<TContainer, TValue>(ref TContainer container, string name)
		{
			PropertyPath propertyPath = new PropertyPath(name);
			return PropertyContainer.GetValue<TContainer, TValue>(ref container, in propertyPath);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025CE File Offset: 0x000007CE
		public static TValue GetValue<TContainer, TValue>(TContainer container, in PropertyPath path)
		{
			return PropertyContainer.GetValue<TContainer, TValue>(ref container, in path);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025D8 File Offset: 0x000007D8
		public static TValue GetValue<TContainer, TValue>(ref TContainer container, in PropertyPath path)
		{
			bool isEmpty = path.IsEmpty;
			if (isEmpty)
			{
				throw new InvalidPathException("The specified PropertyPath is empty.");
			}
			TValue tvalue;
			VisitReturnCode visitReturnCode;
			bool flag = PropertyContainer.TryGetValue<TContainer, TValue>(ref container, in path, out tvalue, out visitReturnCode);
			if (flag)
			{
				return tvalue;
			}
			switch (visitReturnCode)
			{
			case VisitReturnCode.NullContainer:
				throw new ArgumentNullException("container");
			case VisitReturnCode.InvalidContainerType:
				throw new InvalidContainerTypeException(container.GetType());
			case VisitReturnCode.MissingPropertyBag:
				throw new MissingPropertyBagException(container.GetType());
			case VisitReturnCode.InvalidPath:
				throw new InvalidPathException(string.Format("Failed to GetValue for property with Path=[{0}]", path));
			case VisitReturnCode.InvalidCast:
				throw new InvalidCastException(string.Format("Failed to GetValue of Type=[{0}] for property with path=[{1}]", typeof(TValue).Name, path));
			default:
				throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000026C9 File Offset: 0x000008C9
		public static bool TryGetValue<TContainer, TValue>(TContainer container, string name, out TValue value)
		{
			return PropertyContainer.TryGetValue<TContainer, TValue>(ref container, name, out value);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026D4 File Offset: 0x000008D4
		public static bool TryGetValue<TContainer, TValue>(ref TContainer container, string name, out TValue value)
		{
			PropertyPath propertyPath = new PropertyPath(name);
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TryGetValue<TContainer, TValue>(ref container, in propertyPath, out value, out visitReturnCode);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000026FC File Offset: 0x000008FC
		public static bool TryGetValue<TContainer, TValue>(TContainer container, in PropertyPath path, out TValue value)
		{
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TryGetValue<TContainer, TValue>(ref container, in path, out value, out visitReturnCode);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002714 File Offset: 0x00000914
		public static bool TryGetValue<TContainer, TValue>(ref TContainer container, in PropertyPath path, out TValue value)
		{
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TryGetValue<TContainer, TValue>(ref container, in path, out value, out visitReturnCode);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000272C File Offset: 0x0000092C
		public static bool TryGetValue<TContainer, TValue>(ref TContainer container, in PropertyPath path, out TValue value, out VisitReturnCode returnCode)
		{
			bool isEmpty = path.IsEmpty;
			bool flag;
			if (isEmpty)
			{
				returnCode = VisitReturnCode.InvalidPath;
				value = default(TValue);
				flag = false;
			}
			else
			{
				PropertyContainer.GetValueVisitor<TValue> getValueVisitor = PropertyContainer.GetValueVisitor<TValue>.Pool.Get();
				getValueVisitor.Path = path;
				getValueVisitor.ReadonlyVisit = true;
				try
				{
					bool flag2 = !PropertyContainer.TryAccept<TContainer>(getValueVisitor, ref container, out returnCode, default(VisitParameters));
					if (flag2)
					{
						value = default(TValue);
						return false;
					}
					value = getValueVisitor.Value;
					returnCode = getValueVisitor.ReturnCode;
				}
				finally
				{
					PropertyContainer.GetValueVisitor<TValue>.Pool.Release(getValueVisitor);
				}
				flag = returnCode == VisitReturnCode.Ok;
			}
			return flag;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000027D8 File Offset: 0x000009D8
		public static bool IsPathValid<TContainer>(TContainer container, string path)
		{
			PropertyPath propertyPath = new PropertyPath(path);
			return PropertyContainer.IsPathValid<TContainer>(ref container, in propertyPath);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000027F5 File Offset: 0x000009F5
		public static bool IsPathValid<TContainer>(TContainer container, in PropertyPath path)
		{
			return PropertyContainer.IsPathValid<TContainer>(ref container, in path);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002800 File Offset: 0x00000A00
		public static bool IsPathValid<TContainer>(ref TContainer container, string path)
		{
			PropertyContainer.ExistsAtPathVisitor existsAtPathVisitor = PropertyContainer.ExistsAtPathVisitor.Pool.Get();
			bool exists;
			try
			{
				existsAtPathVisitor.Path = new PropertyPath(path);
				PropertyContainer.TryAccept<TContainer>(existsAtPathVisitor, ref container, default(VisitParameters));
				exists = existsAtPathVisitor.Exists;
			}
			finally
			{
				PropertyContainer.ExistsAtPathVisitor.Pool.Release(existsAtPathVisitor);
			}
			return exists;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002864 File Offset: 0x00000A64
		public static bool IsPathValid<TContainer>(ref TContainer container, in PropertyPath path)
		{
			PropertyContainer.ExistsAtPathVisitor existsAtPathVisitor = PropertyContainer.ExistsAtPathVisitor.Pool.Get();
			bool exists;
			try
			{
				existsAtPathVisitor.Path = path;
				PropertyContainer.TryAccept<TContainer>(existsAtPathVisitor, ref container, default(VisitParameters));
				exists = existsAtPathVisitor.Exists;
			}
			finally
			{
				PropertyContainer.ExistsAtPathVisitor.Pool.Release(existsAtPathVisitor);
			}
			return exists;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000028C8 File Offset: 0x00000AC8
		public static void SetValue<TContainer, TValue>(TContainer container, string name, TValue value)
		{
			PropertyContainer.SetValue<TContainer, TValue>(ref container, name, value);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028D4 File Offset: 0x00000AD4
		public static void SetValue<TContainer, TValue>(ref TContainer container, string name, TValue value)
		{
			PropertyPath propertyPath = new PropertyPath(name);
			PropertyContainer.SetValue<TContainer, TValue>(ref container, in propertyPath, value);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028F4 File Offset: 0x00000AF4
		public static void SetValue<TContainer, TValue>(TContainer container, in PropertyPath path, TValue value)
		{
			PropertyContainer.SetValue<TContainer, TValue>(ref container, in path, value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002900 File Offset: 0x00000B00
		public static void SetValue<TContainer, TValue>(ref TContainer container, in PropertyPath path, TValue value)
		{
			bool flag = path.Length == 0;
			if (flag)
			{
				throw new ArgumentNullException("path");
			}
			bool flag2 = path.Length <= 0;
			if (flag2)
			{
				throw new InvalidPathException("The specified PropertyPath is empty.");
			}
			VisitReturnCode visitReturnCode;
			bool flag3 = PropertyContainer.TrySetValue<TContainer, TValue>(ref container, in path, value, out visitReturnCode);
			if (flag3)
			{
				return;
			}
			switch (visitReturnCode)
			{
			case VisitReturnCode.NullContainer:
				throw new ArgumentNullException("container");
			case VisitReturnCode.InvalidContainerType:
				throw new InvalidContainerTypeException(container.GetType());
			case VisitReturnCode.MissingPropertyBag:
				throw new MissingPropertyBagException(container.GetType());
			case VisitReturnCode.InvalidPath:
				throw new InvalidPathException(string.Format("Failed to SetValue for property with Path=[{0}]", path));
			case VisitReturnCode.InvalidCast:
				throw new InvalidCastException(string.Format("Failed to SetValue of Type=[{0}] for property with path=[{1}]", typeof(TValue).Name, path));
			case VisitReturnCode.AccessViolation:
				throw new AccessViolationException(string.Format("Failed to SetValue for read-only property with Path=[{0}]", path));
			default:
				throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A2B File Offset: 0x00000C2B
		public static bool TrySetValue<TContainer, TValue>(TContainer container, string name, TValue value)
		{
			return PropertyContainer.TrySetValue<TContainer, TValue>(ref container, name, value);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A38 File Offset: 0x00000C38
		public static bool TrySetValue<TContainer, TValue>(ref TContainer container, string name, TValue value)
		{
			PropertyPath propertyPath = new PropertyPath(name);
			return PropertyContainer.TrySetValue<TContainer, TValue>(ref container, in propertyPath, value);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002A5B File Offset: 0x00000C5B
		public static bool TrySetValue<TContainer, TValue>(TContainer container, in PropertyPath path, TValue value)
		{
			return PropertyContainer.TrySetValue<TContainer, TValue>(ref container, in path, value);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002A68 File Offset: 0x00000C68
		public static bool TrySetValue<TContainer, TValue>(ref TContainer container, in PropertyPath path, TValue value)
		{
			VisitReturnCode visitReturnCode;
			return PropertyContainer.TrySetValue<TContainer, TValue>(ref container, in path, value, out visitReturnCode);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A80 File Offset: 0x00000C80
		public static bool TrySetValue<TContainer, TValue>(ref TContainer container, in PropertyPath path, TValue value, out VisitReturnCode returnCode)
		{
			bool isEmpty = path.IsEmpty;
			bool flag;
			if (isEmpty)
			{
				returnCode = VisitReturnCode.InvalidPath;
				flag = false;
			}
			else
			{
				PropertyContainer.SetValueVisitor<TValue> setValueVisitor = PropertyContainer.SetValueVisitor<TValue>.Pool.Get();
				setValueVisitor.Path = path;
				setValueVisitor.Value = value;
				try
				{
					bool flag2 = !PropertyContainer.TryAccept<TContainer>(setValueVisitor, ref container, out returnCode, default(VisitParameters));
					if (flag2)
					{
						return false;
					}
					returnCode = setValueVisitor.ReturnCode;
				}
				finally
				{
					PropertyContainer.SetValueVisitor<TValue>.Pool.Release(setValueVisitor);
				}
				flag = returnCode == VisitReturnCode.Ok;
			}
			return flag;
		}

		// Token: 0x02000005 RID: 5
		private class GetPropertyVisitor : PathVisitor
		{
			// Token: 0x06000024 RID: 36 RVA: 0x00002B14 File Offset: 0x00000D14
			public override void Reset()
			{
				base.Reset();
				this.Property = null;
				base.ReadonlyVisit = true;
			}

			// Token: 0x06000025 RID: 37 RVA: 0x00002B2D File Offset: 0x00000D2D
			protected override void VisitPath<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				this.Property = property;
			}

			// Token: 0x04000007 RID: 7
			public static readonly ObjectPool<PropertyContainer.GetPropertyVisitor> Pool = new ObjectPool<PropertyContainer.GetPropertyVisitor>(() => new PropertyContainer.GetPropertyVisitor(), null, delegate(PropertyContainer.GetPropertyVisitor v)
			{
				v.Reset();
			}, null, true, 10, 10000);

			// Token: 0x04000008 RID: 8
			public IProperty Property;
		}

		// Token: 0x02000007 RID: 7
		private class GetValueVisitor<TSrcValue> : PathVisitor
		{
			// Token: 0x0600002C RID: 44 RVA: 0x00002B9B File Offset: 0x00000D9B
			public override void Reset()
			{
				base.Reset();
				this.Value = default(TSrcValue);
				base.ReadonlyVisit = true;
			}

			// Token: 0x0600002D RID: 45 RVA: 0x00002BBC File Offset: 0x00000DBC
			protected override void VisitPath<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				bool flag = !TypeConversion.TryConvert<TValue, TSrcValue>(ref value, out this.Value);
				if (flag)
				{
					base.ReturnCode = VisitReturnCode.InvalidCast;
				}
			}

			// Token: 0x0400000A RID: 10
			public static readonly ObjectPool<PropertyContainer.GetValueVisitor<TSrcValue>> Pool = new ObjectPool<PropertyContainer.GetValueVisitor<TSrcValue>>(() => new PropertyContainer.GetValueVisitor<TSrcValue>(), null, delegate(PropertyContainer.GetValueVisitor<TSrcValue> v)
			{
				v.Reset();
			}, null, true, 10, 10000);

			// Token: 0x0400000B RID: 11
			public TSrcValue Value;
		}

		// Token: 0x02000009 RID: 9
		private class ValueAtPathVisitor : PathVisitor
		{
			// Token: 0x06000034 RID: 52 RVA: 0x00002C30 File Offset: 0x00000E30
			public override void Reset()
			{
				base.Reset();
				this.Visitor = null;
				base.ReadonlyVisit = true;
			}

			// Token: 0x06000035 RID: 53 RVA: 0x00002C49 File Offset: 0x00000E49
			protected override void VisitPath<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				((IPropertyAccept<TContainer>)property).Accept(this.Visitor, ref container);
			}

			// Token: 0x0400000D RID: 13
			public static readonly ObjectPool<PropertyContainer.ValueAtPathVisitor> Pool = new ObjectPool<PropertyContainer.ValueAtPathVisitor>(() => new PropertyContainer.ValueAtPathVisitor(), null, delegate(PropertyContainer.ValueAtPathVisitor v)
			{
				v.Reset();
			}, null, true, 10, 10000);

			// Token: 0x0400000E RID: 14
			public IPropertyVisitor Visitor;
		}

		// Token: 0x0200000B RID: 11
		private class ExistsAtPathVisitor : PathVisitor
		{
			// Token: 0x0600003C RID: 60 RVA: 0x00002CA3 File Offset: 0x00000EA3
			public override void Reset()
			{
				base.Reset();
				this.Exists = false;
				base.ReadonlyVisit = true;
			}

			// Token: 0x0600003D RID: 61 RVA: 0x00002CBC File Offset: 0x00000EBC
			protected override void VisitPath<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				this.Exists = true;
			}

			// Token: 0x04000010 RID: 16
			public static readonly ObjectPool<PropertyContainer.ExistsAtPathVisitor> Pool = new ObjectPool<PropertyContainer.ExistsAtPathVisitor>(() => new PropertyContainer.ExistsAtPathVisitor(), null, delegate(PropertyContainer.ExistsAtPathVisitor v)
			{
				v.Reset();
			}, null, true, 10, 10000);

			// Token: 0x04000011 RID: 17
			public bool Exists;
		}

		// Token: 0x0200000D RID: 13
		internal class SetValueVisitor<TSrcValue> : PathVisitor
		{
			// Token: 0x06000044 RID: 68 RVA: 0x00002D0F File Offset: 0x00000F0F
			public override void Reset()
			{
				base.Reset();
				this.Value = default(TSrcValue);
			}

			// Token: 0x06000045 RID: 69 RVA: 0x00002D28 File Offset: 0x00000F28
			protected override void VisitPath<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				bool isReadOnly = property.IsReadOnly;
				if (isReadOnly)
				{
					base.ReturnCode = VisitReturnCode.AccessViolation;
				}
				else
				{
					TValue tvalue;
					bool flag = TypeConversion.TryConvert<TSrcValue, TValue>(ref this.Value, out tvalue);
					if (flag)
					{
						property.SetValue(ref container, tvalue);
					}
					else
					{
						base.ReturnCode = VisitReturnCode.InvalidCast;
					}
				}
			}

			// Token: 0x04000013 RID: 19
			public static readonly ObjectPool<PropertyContainer.SetValueVisitor<TSrcValue>> Pool = new ObjectPool<PropertyContainer.SetValueVisitor<TSrcValue>>(() => new PropertyContainer.SetValueVisitor<TSrcValue>(), null, delegate(PropertyContainer.SetValueVisitor<TSrcValue> v)
			{
				v.Reset();
			}, null, true, 10, 10000);

			// Token: 0x04000014 RID: 20
			public TSrcValue Value;
		}
	}
}
