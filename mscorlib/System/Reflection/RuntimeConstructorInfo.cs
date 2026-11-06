using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020008F9 RID: 2297
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class RuntimeConstructorInfo : ConstructorInfo, ISerializable
	{
		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004D8E RID: 19854 RVA: 0x000F48E8 File Offset: 0x000F2AE8
		public override Module Module
		{
			get
			{
				return this.GetRuntimeModule();
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x000F48F0 File Offset: 0x000F2AF0
		internal RuntimeModule GetRuntimeModule()
		{
			return RuntimeTypeHandle.GetModule((RuntimeType)this.DeclaringType);
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004D90 RID: 19856 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal BindingFlags BindingFlags
		{
			get
			{
				return BindingFlags.Default;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004D91 RID: 19857 RVA: 0x000F39EF File Offset: 0x000F1BEF
		private RuntimeType ReflectedTypeInternal
		{
			get
			{
				return (RuntimeType)this.ReflectedType;
			}
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x000F4902 File Offset: 0x000F2B02
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedTypeInternal, this.ToString(), this.SerializationToString(), MemberTypes.Constructor, null);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x000F4932 File Offset: 0x000F2B32
		internal string SerializationToString()
		{
			return this.FormatNameAndSig(true);
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x000F493B File Offset: 0x000F2B3B
		internal void SerializationInvoke(object target, SerializationInfo info, StreamingContext context)
		{
			base.Invoke(target, new object[] { info, context });
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x000F4958 File Offset: 0x000F2B58
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return MonoMethodInfo.GetMethodImplementationFlags(this.mhandle);
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x000F4965 File Offset: 0x000F2B65
		public override ParameterInfo[] GetParameters()
		{
			return MonoMethodInfo.GetParametersInfo(this.mhandle, this);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x000F4965 File Offset: 0x000F2B65
		internal override ParameterInfo[] GetParametersInternal()
		{
			return MonoMethodInfo.GetParametersInfo(this.mhandle, this);
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x000F4974 File Offset: 0x000F2B74
		internal override int GetParametersCount()
		{
			ParameterInfo[] parametersInfo = MonoMethodInfo.GetParametersInfo(this.mhandle, this);
			if (parametersInfo != null)
			{
				return parametersInfo.Length;
			}
			return 0;
		}

		// Token: 0x06004D99 RID: 19865
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object InternalInvoke(object obj, object[] parameters, out Exception exc);

		// Token: 0x06004D9A RID: 19866 RVA: 0x000F4996 File Offset: 0x000F2B96
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (obj == null)
			{
				if (!base.IsStatic)
				{
					throw new TargetException("Instance constructor requires a target");
				}
			}
			else if (!this.DeclaringType.IsInstanceOfType(obj))
			{
				throw new TargetException("Constructor does not match target type");
			}
			return this.DoInvoke(obj, invokeAttr, binder, parameters, culture);
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x000F49D4 File Offset: 0x000F2BD4
		private object DoInvoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			ParameterInfo[] parametersInfo = MonoMethodInfo.GetParametersInfo(this.mhandle, this);
			RuntimeMethodInfo.ConvertValues(binder, parameters, parametersInfo, culture, invokeAttr);
			if (obj == null && this.DeclaringType.ContainsGenericParameters)
			{
				string text = "Cannot create an instance of ";
				Type declaringType = this.DeclaringType;
				throw new MemberAccessException(text + ((declaringType != null) ? declaringType.ToString() : null) + " because Type.ContainsGenericParameters is true.");
			}
			if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default && this.DeclaringType.IsAbstract)
			{
				throw new MemberAccessException(string.Format("Cannot create an instance of {0} because it is an abstract class", this.DeclaringType));
			}
			return this.InternalInvoke(obj, parameters, (invokeAttr & BindingFlags.DoNotWrapExceptions) == BindingFlags.Default);
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x000F4A7C File Offset: 0x000F2C7C
		public object InternalInvoke(object obj, object[] parameters, bool wrapExceptions)
		{
			object obj2 = null;
			Exception ex;
			if (wrapExceptions)
			{
				try
				{
					obj2 = this.InternalInvoke(obj, parameters, out ex);
					goto IL_0026;
				}
				catch (OverflowException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					throw new TargetInvocationException(ex2);
				}
			}
			obj2 = this.InternalInvoke(obj, parameters, out ex);
			IL_0026:
			if (ex != null)
			{
				throw ex;
			}
			if (obj != null)
			{
				return null;
			}
			return obj2;
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x000F4AD8 File Offset: 0x000F2CD8
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.DoInvoke(null, invokeAttr, binder, parameters, culture);
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x000F4AE6 File Offset: 0x000F2CE6
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return new RuntimeMethodHandle(this.mhandle);
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x000F4AF3 File Offset: 0x000F2CF3
		public override MethodAttributes Attributes
		{
			get
			{
				return MonoMethodInfo.GetAttributes(this.mhandle);
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x000F4B00 File Offset: 0x000F2D00
		public override CallingConventions CallingConvention
		{
			get
			{
				return MonoMethodInfo.GetCallingConvention(this.mhandle);
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x000F4B0D File Offset: 0x000F2D0D
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.DeclaringType.ContainsGenericParameters;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x000F4B1A File Offset: 0x000F2D1A
		public override Type ReflectedType
		{
			get
			{
				return this.reftype;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x000F4B22 File Offset: 0x000F2D22
		public override Type DeclaringType
		{
			get
			{
				return MonoMethodInfo.GetDeclaringType(this.mhandle);
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004DA4 RID: 19876 RVA: 0x000F4B2F File Offset: 0x000F2D2F
		public override string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return RuntimeMethodInfo.get_name(this);
			}
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x00052A6A File Offset: 0x00050C6A
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x000F1915 File Offset: 0x000EFB15
		public override object[] GetCustomAttributes(bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, inherit);
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x000F191E File Offset: 0x000EFB1E
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x000F4B46 File Offset: 0x000F2D46
		public override MethodBody GetMethodBody()
		{
			return RuntimeMethodInfo.GetMethodBody(this.mhandle);
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x000F4B53 File Offset: 0x000F2D53
		public override string ToString()
		{
			return "Void " + this.FormatNameAndSig(false);
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x000F3C48 File Offset: 0x000F1E48
		public override IList<CustomAttributeData> GetCustomAttributesData()
		{
			return CustomAttributeData.GetCustomAttributes(this);
		}

		// Token: 0x06004DAB RID: 19883
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int get_core_clr_security_level();

		// Token: 0x06004DAC RID: 19884 RVA: 0x000F4B66 File Offset: 0x000F2D66
		public sealed override bool HasSameMetadataDefinitionAs(MemberInfo other)
		{
			return base.HasSameMetadataDefinitionAsCore<RuntimeConstructorInfo>(other);
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x000F4B6F File Offset: 0x000F2D6F
		public override bool IsSecurityTransparent
		{
			get
			{
				return this.get_core_clr_security_level() == 0;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06004DAE RID: 19886 RVA: 0x000F4B7A File Offset: 0x000F2D7A
		public override bool IsSecurityCritical
		{
			get
			{
				return this.get_core_clr_security_level() > 0;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x000F4B85 File Offset: 0x000F2D85
		public override bool IsSecuritySafeCritical
		{
			get
			{
				return this.get_core_clr_security_level() == 1;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x000F4B90 File Offset: 0x000F2D90
		public override int MetadataToken
		{
			get
			{
				return RuntimeConstructorInfo.get_metadata_token(this);
			}
		}

		// Token: 0x06004DB1 RID: 19889
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int get_metadata_token(RuntimeConstructorInfo method);

		// Token: 0x0400305D RID: 12381
		internal IntPtr mhandle;

		// Token: 0x0400305E RID: 12382
		private string name;

		// Token: 0x0400305F RID: 12383
		private Type reftype;
	}
}
