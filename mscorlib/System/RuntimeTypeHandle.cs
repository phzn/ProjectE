using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System
{
	/// <summary>Represents a type using an internal metadata token.</summary>
	// Token: 0x0200024C RID: 588
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeTypeHandle : ISerializable
	{
		// Token: 0x06001B07 RID: 6919 RVA: 0x000648EF File Offset: 0x00062AEF
		internal RuntimeTypeHandle(IntPtr val)
		{
			this.value = val;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000648F8 File Offset: 0x00062AF8
		internal RuntimeTypeHandle(RuntimeType type)
		{
			this = new RuntimeTypeHandle(type._impl.value);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0006490C File Offset: 0x00062B0C
		private RuntimeTypeHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			RuntimeType runtimeType = (RuntimeType)info.GetValue("TypeObj", typeof(RuntimeType));
			this.value = runtimeType.TypeHandle.Value;
			if (this.value == IntPtr.Zero)
			{
				throw new SerializationException("Insufficient state.");
			}
		}

		/// <summary>Gets a handle to the type represented by this instance.</summary>
		/// <returns>A handle to the type represented by this instance.</returns>
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x00064973 File Offset: 0x00062B73
		public IntPtr Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data necessary to deserialize the type represented by the current instance.</summary>
		/// <param name="info">The object to be populated with serialization information.</param>
		/// <param name="context">(Reserved) The location where serialized data will be stored and retrieved.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">
		///   <see cref="P:System.RuntimeTypeHandle.Value" /> is invalid.</exception>
		// Token: 0x06001B0B RID: 6923 RVA: 0x0006497C File Offset: 0x00062B7C
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.value == IntPtr.Zero)
			{
				throw new SerializationException("Object fields may not be properly initialized");
			}
			info.AddValue("TypeObj", Type.GetTypeHandle(this), typeof(RuntimeType));
		}

		/// <summary>Indicates whether the specified object is equal to the current <see cref="T:System.RuntimeTypeHandle" /> structure.</summary>
		/// <param name="obj">An object to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.RuntimeTypeHandle" /> structure and is equal to the value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B0C RID: 6924 RVA: 0x000649E0 File Offset: 0x00062BE0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && this.value == ((RuntimeTypeHandle)obj).Value;
		}

		/// <summary>Indicates whether the specified <see cref="T:System.RuntimeTypeHandle" /> structure is equal to the current <see cref="T:System.RuntimeTypeHandle" /> structure.</summary>
		/// <param name="handle">The <see cref="T:System.RuntimeTypeHandle" /> structure to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="handle" /> is equal to the value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B0D RID: 6925 RVA: 0x00064A28 File Offset: 0x00062C28
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeTypeHandle handle)
		{
			return this.value == handle.Value;
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06001B0E RID: 6926 RVA: 0x00064A3C File Offset: 0x00062C3C
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		/// <summary>Indicates whether a <see cref="T:System.RuntimeTypeHandle" /> structure is equal to an object.</summary>
		/// <param name="left">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="right" />.</param>
		/// <param name="right">An object to compare to <paramref name="left" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="right" /> is a <see cref="T:System.RuntimeTypeHandle" /> and is equal to <paramref name="left" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B0F RID: 6927 RVA: 0x00064A49 File Offset: 0x00062C49
		public static bool operator ==(RuntimeTypeHandle left, object right)
		{
			return right != null && right is RuntimeTypeHandle && left.Equals((RuntimeTypeHandle)right);
		}

		/// <summary>Indicates whether a <see cref="T:System.RuntimeTypeHandle" /> structure is not equal to an object.</summary>
		/// <param name="left">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="right" />.</param>
		/// <param name="right">An object to compare to <paramref name="left" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="right" /> is a <see cref="T:System.RuntimeTypeHandle" /> structure and is not equal to <paramref name="left" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B10 RID: 6928 RVA: 0x00064A65 File Offset: 0x00062C65
		public static bool operator !=(RuntimeTypeHandle left, object right)
		{
			return right == null || !(right is RuntimeTypeHandle) || !left.Equals((RuntimeTypeHandle)right);
		}

		/// <summary>Indicates whether an object and a <see cref="T:System.RuntimeTypeHandle" /> structure are equal.</summary>
		/// <param name="left">An object to compare to <paramref name="right" />.</param>
		/// <param name="right">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="left" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is a <see cref="T:System.RuntimeTypeHandle" /> structure and is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B11 RID: 6929 RVA: 0x00064A84 File Offset: 0x00062C84
		public static bool operator ==(object left, RuntimeTypeHandle right)
		{
			return left != null && left is RuntimeTypeHandle && ((RuntimeTypeHandle)left).Equals(right);
		}

		/// <summary>Indicates whether an object and a <see cref="T:System.RuntimeTypeHandle" /> structure are not equal.</summary>
		/// <param name="left">An object to compare to <paramref name="right" />.</param>
		/// <param name="right">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="left" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is a <see cref="T:System.RuntimeTypeHandle" /> and is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B12 RID: 6930 RVA: 0x00064AB0 File Offset: 0x00062CB0
		public static bool operator !=(object left, RuntimeTypeHandle right)
		{
			return left == null || !(left is RuntimeTypeHandle) || !((RuntimeTypeHandle)left).Equals(right);
		}

		// Token: 0x06001B13 RID: 6931
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern TypeAttributes GetAttributes(RuntimeType type);

		/// <summary>Gets a handle to the module that contains the type represented by the current instance.</summary>
		/// <returns>A <see cref="T:System.ModuleHandle" /> structure representing a handle to the module that contains the type represented by the current instance.</returns>
		// Token: 0x06001B14 RID: 6932 RVA: 0x00064ADC File Offset: 0x00062CDC
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public ModuleHandle GetModuleHandle()
		{
			if (this.value == IntPtr.Zero)
			{
				throw new InvalidOperationException("Object fields may not be properly initialized");
			}
			return Type.GetTypeFromHandle(this).Module.ModuleHandle;
		}

		// Token: 0x06001B15 RID: 6933
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMetadataToken(RuntimeType type);

		// Token: 0x06001B16 RID: 6934 RVA: 0x00064B10 File Offset: 0x00062D10
		internal static int GetToken(RuntimeType type)
		{
			return RuntimeTypeHandle.GetMetadataToken(type);
		}

		// Token: 0x06001B17 RID: 6935
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetGenericTypeDefinition_impl(RuntimeType type);

		// Token: 0x06001B18 RID: 6936 RVA: 0x00064B18 File Offset: 0x00062D18
		internal static Type GetGenericTypeDefinition(RuntimeType type)
		{
			return RuntimeTypeHandle.GetGenericTypeDefinition_impl(type);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00064B20 File Offset: 0x00062D20
		internal static bool HasProxyAttribute(RuntimeType type)
		{
			throw new NotImplementedException("HasProxyAttribute");
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00064B2C File Offset: 0x00062D2C
		internal static bool IsPrimitive(RuntimeType type)
		{
			CorElementType corElementType = RuntimeTypeHandle.GetCorElementType(type);
			return (corElementType >= CorElementType.Boolean && corElementType <= CorElementType.R8) || corElementType == CorElementType.I || corElementType == CorElementType.U;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00064B55 File Offset: 0x00062D55
		internal static bool IsByRef(RuntimeType type)
		{
			return RuntimeTypeHandle.GetCorElementType(type) == CorElementType.ByRef;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00064B61 File Offset: 0x00062D61
		internal static bool IsPointer(RuntimeType type)
		{
			return RuntimeTypeHandle.GetCorElementType(type) == CorElementType.Ptr;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00064B70 File Offset: 0x00062D70
		internal static bool IsArray(RuntimeType type)
		{
			CorElementType corElementType = RuntimeTypeHandle.GetCorElementType(type);
			return corElementType == CorElementType.Array || corElementType == CorElementType.SzArray;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00064B90 File Offset: 0x00062D90
		internal static bool IsSzArray(RuntimeType type)
		{
			return RuntimeTypeHandle.GetCorElementType(type) == CorElementType.SzArray;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x00064B9C File Offset: 0x00062D9C
		internal static bool HasElementType(RuntimeType type)
		{
			CorElementType corElementType = RuntimeTypeHandle.GetCorElementType(type);
			return corElementType == CorElementType.Array || corElementType == CorElementType.SzArray || corElementType == CorElementType.Ptr || corElementType == CorElementType.ByRef;
		}

		// Token: 0x06001B20 RID: 6944
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CorElementType GetCorElementType(RuntimeType type);

		// Token: 0x06001B21 RID: 6945
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasInstantiation(RuntimeType type);

		// Token: 0x06001B22 RID: 6946
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsComObject(RuntimeType type);

		// Token: 0x06001B23 RID: 6947
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsInstanceOfType(RuntimeType type, object o);

		// Token: 0x06001B24 RID: 6948
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasReferences(RuntimeType type);

		// Token: 0x06001B25 RID: 6949 RVA: 0x00064BC6 File Offset: 0x00062DC6
		internal static bool IsComObject(RuntimeType type, bool isGenericCOM)
		{
			return !isGenericCOM && RuntimeTypeHandle.IsComObject(type);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0004742F File Offset: 0x0004562F
		internal static bool IsContextful(RuntimeType type)
		{
			return typeof(ContextBoundObject).IsAssignableFrom(type);
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal static bool IsEquivalentTo(RuntimeType rtType1, RuntimeType rtType2)
		{
			return false;
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x00064BD3 File Offset: 0x00062DD3
		internal static bool IsInterface(RuntimeType type)
		{
			return (type.Attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
		}

		// Token: 0x06001B29 RID: 6953
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetArrayRank(RuntimeType type);

		// Token: 0x06001B2A RID: 6954
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RuntimeAssembly GetAssembly(RuntimeType type);

		// Token: 0x06001B2B RID: 6955
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RuntimeType GetElementType(RuntimeType type);

		// Token: 0x06001B2C RID: 6956
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RuntimeModule GetModule(RuntimeType type);

		// Token: 0x06001B2D RID: 6957
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsGenericVariable(RuntimeType type);

		// Token: 0x06001B2E RID: 6958
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RuntimeType GetBaseType(RuntimeType type);

		// Token: 0x06001B2F RID: 6959 RVA: 0x00064BE2 File Offset: 0x00062DE2
		internal static bool CanCastTo(RuntimeType type, RuntimeType target)
		{
			return RuntimeTypeHandle.type_is_assignable_from(target, type);
		}

		// Token: 0x06001B30 RID: 6960
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool type_is_assignable_from(Type a, Type b);

		// Token: 0x06001B31 RID: 6961
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsGenericTypeDefinition(RuntimeType type);

		// Token: 0x06001B32 RID: 6962
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetGenericParameterInfo(RuntimeType type);

		// Token: 0x06001B33 RID: 6963 RVA: 0x00064BEB File Offset: 0x00062DEB
		internal static bool IsSubclassOf(RuntimeType childType, RuntimeType baseType)
		{
			return RuntimeTypeHandle.is_subclass_of(childType._impl.Value, baseType._impl.Value);
		}

		// Token: 0x06001B34 RID: 6964
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool is_subclass_of(IntPtr childType, IntPtr baseType);

		// Token: 0x06001B35 RID: 6965
		[PreserveDependency(".ctor()", "System.Runtime.CompilerServices.IsByRefLikeAttribute")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsByRefLike(RuntimeType type);

		// Token: 0x06001B36 RID: 6966 RVA: 0x00064C08 File Offset: 0x00062E08
		internal static bool IsTypeDefinition(RuntimeType type)
		{
			return !type.HasElementType && !type.IsConstructedGenericType && !type.IsGenericParameter;
		}

		// Token: 0x06001B37 RID: 6967
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RuntimeType internal_from_name(string name, ref StackCrawlMark stackMark, Assembly callerAssembly, bool throwOnError, bool ignoreCase, bool reflectionOnly);

		// Token: 0x06001B38 RID: 6968 RVA: 0x00064C28 File Offset: 0x00062E28
		internal static RuntimeType GetTypeByName(string typeName, bool throwOnError, bool ignoreCase, bool reflectionOnly, ref StackCrawlMark stackMark, bool loadTypeFromPartialName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (typeName == string.Empty)
			{
				if (throwOnError)
				{
					throw new TypeLoadException("A null or zero length string does not represent a valid Type.");
				}
				return null;
			}
			else if (reflectionOnly)
			{
				int num = typeName.IndexOf(',');
				if (num < 0 || num == 0 || num == typeName.Length - 1)
				{
					throw new ArgumentException("Assembly qualifed type name is required", "typeName");
				}
				string text = typeName.Substring(num + 1);
				Assembly assembly;
				try
				{
					assembly = Assembly.ReflectionOnlyLoad(text);
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}
					return null;
				}
				return (RuntimeType)assembly.GetType(typeName.Substring(0, num), throwOnError, ignoreCase);
			}
			else
			{
				RuntimeType runtimeType = RuntimeTypeHandle.internal_from_name(typeName, ref stackMark, null, throwOnError, ignoreCase, false);
				if (throwOnError && runtimeType == null)
				{
					throw new TypeLoadException("Error loading '" + typeName + "'");
				}
				return runtimeType;
			}
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00064D08 File Offset: 0x00062F08
		internal static IntPtr[] CopyRuntimeTypeHandles(RuntimeTypeHandle[] inHandles, out int length)
		{
			if (inHandles == null || inHandles.Length == 0)
			{
				length = 0;
				return null;
			}
			IntPtr[] array = new IntPtr[inHandles.Length];
			for (int i = 0; i < inHandles.Length; i++)
			{
				array[i] = inHandles[i].Value;
			}
			length = array.Length;
			return array;
		}

		// Token: 0x0400177A RID: 6010
		private IntPtr value;
	}
}
