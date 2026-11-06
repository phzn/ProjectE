using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Discovers the attributes of a method and provides access to method metadata.</summary>
	// Token: 0x020008AF RID: 2223
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodInfo))]
	[Serializable]
	public abstract class MethodInfo : MethodBase, _MethodInfo
	{
		/// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a method.</summary>
		/// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a method.</returns>
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x00047F75 File Offset: 0x00046175
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		/// <summary>Gets a <see cref="T:System.Reflection.ParameterInfo" /> object that contains information about the return type of the method, such as whether the return type has custom modifiers.</summary>
		/// <returns>A <see cref="T:System.Reflection.ParameterInfo" /> object that contains information about the return type.</returns>
		/// <exception cref="T:System.NotImplementedException">This method is not implemented.</exception>
		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600495D RID: 18781 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual ParameterInfo ReturnParameter
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		/// <summary>Gets the return type of this method.</summary>
		/// <returns>The return type of this method.</returns>
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type ReturnType
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		/// <summary>Returns an array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition.</summary>
		/// <returns>An array of <see cref="T:System.Type" /> objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
		// Token: 0x0600495F RID: 18783 RVA: 0x0004728E File Offset: 0x0004548E
		public override Type[] GetGenericArguments()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		/// <summary>Returns a <see cref="T:System.Reflection.MethodInfo" /> object that represents a generic method definition from which the current method can be constructed.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object representing a generic method definition from which the current method can be constructed.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current method is not a generic method. That is, <see cref="P:System.Reflection.MethodBase.IsGenericMethod" /> returns <see langword="false" />.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
		// Token: 0x06004960 RID: 18784 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual MethodInfo GetGenericMethodDefinition()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		/// <summary>Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a <see cref="T:System.Reflection.MethodInfo" /> object representing the resulting constructed method.</summary>
		/// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic method definition.</param>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object that represents the constructed method formed by substituting the elements of <paramref name="typeArguments" /> for the type parameters of the current generic method definition.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Reflection.MethodInfo" /> does not represent a generic method definition. That is, <see cref="P:System.Reflection.MethodBase.IsGenericMethodDefinition" /> returns <see langword="false" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeArguments" /> is <see langword="null" />.  
		/// -or-  
		/// Any element of <paramref name="typeArguments" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in <paramref name="typeArguments" /> is not the same as the number of type parameters of the current generic method definition.  
		///  -or-  
		///  An element of <paramref name="typeArguments" /> does not satisfy the constraints specified for the corresponding type parameter of the current generic method definition.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
		// Token: 0x06004961 RID: 18785 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		/// <summary>When overridden in a derived class, returns the <see cref="T:System.Reflection.MethodInfo" /> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object for the first implementation of this method.</returns>
		// Token: 0x06004962 RID: 18786
		public abstract MethodInfo GetBaseDefinition();

		/// <summary>Gets the custom attributes for the return type.</summary>
		/// <returns>An <see langword="ICustomAttributeProvider" /> object representing the custom attributes for the return type.</returns>
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004963 RID: 18787
		public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		/// <summary>Creates a delegate of the specified type from this method.</summary>
		/// <param name="delegateType">The type of the delegate to create.</param>
		/// <returns>The delegate for this method.</returns>
		// Token: 0x06004964 RID: 18788 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Delegate CreateDelegate(Type delegateType)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		/// <summary>Creates a delegate of the specified type with the specified target from this method.</summary>
		/// <param name="delegateType">The type of the delegate to create.</param>
		/// <param name="target">The object targeted by the delegate.</param>
		/// <returns>The delegate for this method.</returns>
		// Token: 0x06004965 RID: 18789 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Delegate CreateDelegate(Type delegateType, object target)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		/// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
		/// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004966 RID: 18790 RVA: 0x000EE1DE File Offset: 0x000EC3DE
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06004967 RID: 18791 RVA: 0x000EE1E7 File Offset: 0x000EC3E7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.MethodInfo" /> objects are equal.</summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004968 RID: 18792 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(MethodInfo left, MethodInfo right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.MethodInfo" /> objects are not equal.</summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004969 RID: 18793 RVA: 0x000EEE9C File Offset: 0x000ED09C
		public static bool operator !=(MethodInfo left, MethodInfo right)
		{
			return !(left == right);
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array that receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x0600496A RID: 18794 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x0600496B RID: 18795 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x0600496C RID: 18796 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">Identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">Pointer to a structure containing an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">Pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">Pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
		// Token: 0x0600496D RID: 18797 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to the <see cref="M:System.Object.GetType" /> method from COM.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the <see cref="T:System.Reflection.MethodInfo" /> type.</returns>
		// Token: 0x0600496E RID: 18798 RVA: 0x00047214 File Offset: 0x00045414
		Type _MethodInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x000EEEA8 File Offset: 0x000ED0A8
		internal virtual int GenericParameterCount
		{
			get
			{
				return this.GetGenericArguments().Length;
			}
		}
	}
}
