using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x020008FA RID: 2298
	[ComDefaultInterface(typeof(_Module))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class RuntimeModule : Module
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x000F4BA0 File Offset: 0x000F2DA0
		public override Assembly Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x000F4BA8 File Offset: 0x000F2DA8
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x000F4BB0 File Offset: 0x000F2DB0
		public override string ScopeName
		{
			get
			{
				return this.scopename;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x000F4BB8 File Offset: 0x000F2DB8
		public override int MDStreamVersion
		{
			get
			{
				if (this._impl == IntPtr.Zero)
				{
					throw new NotSupportedException();
				}
				return RuntimeModule.GetMDStreamVersion(this._impl);
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x000EF150 File Offset: 0x000ED350
		public override Guid ModuleVersionId
		{
			get
			{
				return this.GetModuleVersionId();
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x000F4BDD File Offset: 0x000F2DDD
		public override string FullyQualifiedName
		{
			get
			{
				if (SecurityManager.SecurityEnabled)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.fqname).Demand();
				}
				return this.fqname;
			}
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x000F4BFD File Offset: 0x000F2DFD
		public override bool IsResource()
		{
			return this.is_resource;
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x000F4C08 File Offset: 0x000F2E08
		public override Type[] FindTypes(TypeFilter filter, object filterCriteria)
		{
			List<Type> list = new List<Type>();
			foreach (Type type in this.GetTypes())
			{
				if (filter(type, filterCriteria))
				{
					list.Add(type);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x000F1915 File Offset: 0x000EFB15
		public override object[] GetCustomAttributes(bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, inherit);
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x000F191E File Offset: 0x000EFB1E
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x000F4C4C File Offset: 0x000F2E4C
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.IsResource())
			{
				return null;
			}
			Type globalType = RuntimeModule.GetGlobalType(this._impl);
			if (!(globalType != null))
			{
				return null;
			}
			return globalType.GetField(name, bindingAttr);
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x000F4C90 File Offset: 0x000F2E90
		public override FieldInfo[] GetFields(BindingFlags bindingFlags)
		{
			if (this.IsResource())
			{
				return new FieldInfo[0];
			}
			Type globalType = RuntimeModule.GetGlobalType(this._impl);
			if (!(globalType != null))
			{
				return new FieldInfo[0];
			}
			return globalType.GetFields(bindingFlags);
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x000F4CCF File Offset: 0x000F2ECF
		public override int MetadataToken
		{
			get
			{
				return RuntimeModule.get_MetadataToken(this);
			}
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x000F4CD8 File Offset: 0x000F2ED8
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.IsResource())
			{
				return null;
			}
			Type globalType = RuntimeModule.GetGlobalType(this._impl);
			if (globalType == null)
			{
				return null;
			}
			if (types == null)
			{
				return globalType.GetMethod(name);
			}
			return globalType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x000F4D24 File Offset: 0x000F2F24
		public override MethodInfo[] GetMethods(BindingFlags bindingFlags)
		{
			if (this.IsResource())
			{
				return new MethodInfo[0];
			}
			Type globalType = RuntimeModule.GetGlobalType(this._impl);
			if (!(globalType != null))
			{
				return new MethodInfo[0];
			}
			return globalType.GetMethods(bindingFlags);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x000F4D63 File Offset: 0x000F2F63
		internal override ModuleHandle GetModuleHandleImpl()
		{
			return new ModuleHandle(this._impl);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x000F4D70 File Offset: 0x000F2F70
		public override void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			RuntimeModule.GetPEKind(this._impl, out peKind, out machine);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x000F4D7F File Offset: 0x000F2F7F
		public override Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			if (className == null)
			{
				throw new ArgumentNullException("className");
			}
			if (className == string.Empty)
			{
				throw new ArgumentException("Type name can't be empty");
			}
			return this.assembly.InternalGetType(this, className, throwOnError, ignoreCase);
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x00052A6A File Offset: 0x00050C6A
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x000F4DB6 File Offset: 0x000F2FB6
		public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveField(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x000F4DC8 File Offset: 0x000F2FC8
		internal static FieldInfo ResolveField(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			ResolveTokenError resolveTokenError;
			IntPtr intPtr = RuntimeModule.ResolveFieldToken(monoModule, metadataToken, RuntimeModule.ptrs_from_types(genericTypeArguments), RuntimeModule.ptrs_from_types(genericMethodArguments), out resolveTokenError);
			if (intPtr == IntPtr.Zero)
			{
				throw RuntimeModule.resolve_token_exception(module.Name, metadataToken, resolveTokenError, "Field");
			}
			return FieldInfo.GetFieldFromHandle(new RuntimeFieldHandle(intPtr));
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x000F4E17 File Offset: 0x000F3017
		public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveMember(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x000F4E28 File Offset: 0x000F3028
		internal static MemberInfo ResolveMember(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			ResolveTokenError resolveTokenError;
			MemberInfo memberInfo = RuntimeModule.ResolveMemberToken(monoModule, metadataToken, RuntimeModule.ptrs_from_types(genericTypeArguments), RuntimeModule.ptrs_from_types(genericMethodArguments), out resolveTokenError);
			if (memberInfo == null)
			{
				throw RuntimeModule.resolve_token_exception(module.Name, metadataToken, resolveTokenError, "MemberInfo");
			}
			return memberInfo;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x000F4E69 File Offset: 0x000F3069
		public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveMethod(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x000F4E7C File Offset: 0x000F307C
		internal static MethodBase ResolveMethod(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			ResolveTokenError resolveTokenError;
			IntPtr intPtr = RuntimeModule.ResolveMethodToken(monoModule, metadataToken, RuntimeModule.ptrs_from_types(genericTypeArguments), RuntimeModule.ptrs_from_types(genericMethodArguments), out resolveTokenError);
			if (intPtr == IntPtr.Zero)
			{
				throw RuntimeModule.resolve_token_exception(module.Name, metadataToken, resolveTokenError, "MethodBase");
			}
			return RuntimeMethodInfo.GetMethodFromHandleNoGenericCheck(new RuntimeMethodHandle(intPtr));
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x000F4ECB File Offset: 0x000F30CB
		public override string ResolveString(int metadataToken)
		{
			return RuntimeModule.ResolveString(this, this._impl, metadataToken);
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x000F4EDC File Offset: 0x000F30DC
		internal static string ResolveString(Module module, IntPtr monoModule, int metadataToken)
		{
			ResolveTokenError resolveTokenError;
			string text = RuntimeModule.ResolveStringToken(monoModule, metadataToken, out resolveTokenError);
			if (text == null)
			{
				throw RuntimeModule.resolve_token_exception(module.Name, metadataToken, resolveTokenError, "string");
			}
			return text;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x000F4F0A File Offset: 0x000F310A
		public override Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveType(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x000F4F1C File Offset: 0x000F311C
		internal static Type ResolveType(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			ResolveTokenError resolveTokenError;
			IntPtr intPtr = RuntimeModule.ResolveTypeToken(monoModule, metadataToken, RuntimeModule.ptrs_from_types(genericTypeArguments), RuntimeModule.ptrs_from_types(genericMethodArguments), out resolveTokenError);
			if (intPtr == IntPtr.Zero)
			{
				throw RuntimeModule.resolve_token_exception(module.Name, metadataToken, resolveTokenError, "Type");
			}
			return Type.GetTypeFromHandle(new RuntimeTypeHandle(intPtr));
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x000F4F6B File Offset: 0x000F316B
		public override byte[] ResolveSignature(int metadataToken)
		{
			return RuntimeModule.ResolveSignature(this, this._impl, metadataToken);
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x000F4F7C File Offset: 0x000F317C
		internal static byte[] ResolveSignature(Module module, IntPtr monoModule, int metadataToken)
		{
			ResolveTokenError resolveTokenError;
			byte[] array = RuntimeModule.ResolveSignature(monoModule, metadataToken, out resolveTokenError);
			if (array == null)
			{
				throw RuntimeModule.resolve_token_exception(module.Name, metadataToken, resolveTokenError, "signature");
			}
			return array;
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x000F4FAA File Offset: 0x000F31AA
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 5, this.ScopeName, this.GetRuntimeAssembly());
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x000F4FD0 File Offset: 0x000F31D0
		public override X509Certificate GetSignerCertificate()
		{
			X509Certificate x509Certificate;
			try
			{
				x509Certificate = X509Certificate.CreateFromSignedFile(this.assembly.Location);
			}
			catch
			{
				x509Certificate = null;
			}
			return x509Certificate;
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x000F5008 File Offset: 0x000F3208
		public override Type[] GetTypes()
		{
			return RuntimeModule.InternalGetTypes(this._impl);
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x000F5015 File Offset: 0x000F3215
		public override IList<CustomAttributeData> GetCustomAttributesData()
		{
			return CustomAttributeData.GetCustomAttributes(this);
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x000F501D File Offset: 0x000F321D
		internal RuntimeAssembly GetRuntimeAssembly()
		{
			return (RuntimeAssembly)this.assembly;
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x000F502A File Offset: 0x000F322A
		internal IntPtr MonoModule
		{
			get
			{
				return this._impl;
			}
		}

		// Token: 0x06004DD8 RID: 19928 RVA: 0x000F5034 File Offset: 0x000F3234
		internal override Guid GetModuleVersionId()
		{
			byte[] array = new byte[16];
			RuntimeModule.GetGuidInternal(this._impl, array);
			return new Guid(array);
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x000F505B File Offset: 0x000F325B
		internal static Exception resolve_token_exception(string name, int metadataToken, ResolveTokenError error, string tokenType)
		{
			if (error == ResolveTokenError.OutOfRange)
			{
				return new ArgumentOutOfRangeException("metadataToken", string.Format("Token 0x{0:x} is not valid in the scope of module {1}", metadataToken, name));
			}
			return new ArgumentException(string.Format("Token 0x{0:x} is not a valid {1} token in the scope of module {2}", metadataToken, tokenType, name), "metadataToken");
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x000F5098 File Offset: 0x000F3298
		internal static IntPtr[] ptrs_from_types(Type[] types)
		{
			if (types == null)
			{
				return null;
			}
			IntPtr[] array = new IntPtr[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentException();
				}
				array[i] = types[i].TypeHandle.Value;
			}
			return array;
		}

		// Token: 0x06004DDB RID: 19931
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int get_MetadataToken(Module module);

		// Token: 0x06004DDC RID: 19932
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetMDStreamVersion(IntPtr module);

		// Token: 0x06004DDD RID: 19933
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type[] InternalGetTypes(IntPtr module);

		// Token: 0x06004DDE RID: 19934
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetHINSTANCE(IntPtr module);

		// Token: 0x06004DDF RID: 19935
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGuidInternal(IntPtr module, byte[] guid);

		// Token: 0x06004DE0 RID: 19936
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type GetGlobalType(IntPtr module);

		// Token: 0x06004DE1 RID: 19937
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr ResolveTypeToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

		// Token: 0x06004DE2 RID: 19938
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr ResolveMethodToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

		// Token: 0x06004DE3 RID: 19939
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr ResolveFieldToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

		// Token: 0x06004DE4 RID: 19940
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string ResolveStringToken(IntPtr module, int token, out ResolveTokenError error);

		// Token: 0x06004DE5 RID: 19941
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MemberInfo ResolveMemberToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

		// Token: 0x06004DE6 RID: 19942
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] ResolveSignature(IntPtr module, int metadataToken, out ResolveTokenError error);

		// Token: 0x06004DE7 RID: 19943
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetPEKind(IntPtr module, out PortableExecutableKinds peKind, out ImageFileMachine machine);

		// Token: 0x04003060 RID: 12384
		internal IntPtr _impl;

		// Token: 0x04003061 RID: 12385
		internal Assembly assembly;

		// Token: 0x04003062 RID: 12386
		internal string fqname;

		// Token: 0x04003063 RID: 12387
		internal string name;

		// Token: 0x04003064 RID: 12388
		internal string scopename;

		// Token: 0x04003065 RID: 12389
		internal bool is_resource;

		// Token: 0x04003066 RID: 12390
		internal int token;
	}
}
