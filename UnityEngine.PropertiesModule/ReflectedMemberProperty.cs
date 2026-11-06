using System;
using System.Reflection;
using System.Reflection.Emit;
using Unity.Collections;

namespace Unity.Properties
{
	// Token: 0x02000030 RID: 48
	public class ReflectedMemberProperty<TContainer, TValue> : Property<TContainer, TValue>
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004E13 File Offset: 0x00003013
		public override string Name { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004E1B File Offset: 0x0000301B
		public override bool IsReadOnly { get; }

		// Token: 0x060000E6 RID: 230 RVA: 0x00004E23 File Offset: 0x00003023
		public ReflectedMemberProperty(FieldInfo info, string name)
			: this(new FieldMember(info), name)
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004E39 File Offset: 0x00003039
		public ReflectedMemberProperty(PropertyInfo info, string name)
			: this(new PropertyMember(info), name)
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004E50 File Offset: 0x00003050
		internal ReflectedMemberProperty(IMemberInfo info, string name)
		{
			this.Name = name;
			this.m_Info = info;
			this.m_IsStructContainerType = TypeTraits<TContainer>.IsValueType;
			base.AddAttributes(info.GetCustomAttributes());
			bool flag = this.m_Info.IsReadOnly || base.HasAttribute<ReadOnlyAttribute>();
			this.IsReadOnly = flag;
			IMemberInfo memberInfo = this.m_Info;
			FieldMember fieldMember;
			bool flag2;
			if (memberInfo is FieldMember)
			{
				fieldMember = (FieldMember)memberInfo;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3)
			{
				FieldInfo fieldInfo = fieldMember.m_FieldInfo;
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, fieldInfo.FieldType, new Type[] { this.m_IsStructContainerType ? fieldInfo.ReflectedType.MakeByRefType() : fieldInfo.ReflectedType }, true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
				ilgenerator.Emit(OpCodes.Ret);
				bool isStructContainerType = this.m_IsStructContainerType;
				if (isStructContainerType)
				{
					this.m_GetStructValueAction = (ReflectedMemberProperty<TContainer, TValue>.GetStructValueAction)dynamicMethod.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.GetStructValueAction));
				}
				else
				{
					this.m_GetClassValueAction = (ReflectedMemberProperty<TContainer, TValue>.GetClassValueAction)dynamicMethod.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.GetClassValueAction));
				}
				bool flag4 = !flag;
				if (flag4)
				{
					dynamicMethod = new DynamicMethod(string.Empty, typeof(void), new Type[]
					{
						this.m_IsStructContainerType ? fieldInfo.ReflectedType.MakeByRefType() : fieldInfo.ReflectedType,
						fieldInfo.FieldType
					}, true);
					ilgenerator = dynamicMethod.GetILGenerator();
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Ldarg_1);
					ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
					ilgenerator.Emit(OpCodes.Ret);
					bool isStructContainerType2 = this.m_IsStructContainerType;
					if (isStructContainerType2)
					{
						this.m_SetStructValueAction = (ReflectedMemberProperty<TContainer, TValue>.SetStructValueAction)dynamicMethod.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.SetStructValueAction));
					}
					else
					{
						this.m_SetClassValueAction = (ReflectedMemberProperty<TContainer, TValue>.SetClassValueAction)dynamicMethod.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.SetClassValueAction));
					}
				}
			}
			else
			{
				memberInfo = this.m_Info;
				PropertyMember propertyMember;
				bool flag5;
				if (memberInfo is PropertyMember)
				{
					propertyMember = (PropertyMember)memberInfo;
					flag5 = true;
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				if (flag6)
				{
					bool isStructContainerType3 = this.m_IsStructContainerType;
					if (isStructContainerType3)
					{
						MethodInfo getMethod = propertyMember.m_PropertyInfo.GetGetMethod(true);
						this.m_GetStructValueAction = (ReflectedMemberProperty<TContainer, TValue>.GetStructValueAction)Delegate.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.GetStructValueAction), getMethod);
						bool flag7 = !flag;
						if (flag7)
						{
							MethodInfo setMethod = propertyMember.m_PropertyInfo.GetSetMethod(true);
							this.m_SetStructValueAction = (ReflectedMemberProperty<TContainer, TValue>.SetStructValueAction)Delegate.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.SetStructValueAction), setMethod);
						}
					}
					else
					{
						MethodInfo getMethod2 = propertyMember.m_PropertyInfo.GetGetMethod(true);
						this.m_GetClassValueAction = (ReflectedMemberProperty<TContainer, TValue>.GetClassValueAction)Delegate.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.GetClassValueAction), getMethod2);
						bool flag8 = !flag;
						if (flag8)
						{
							MethodInfo setMethod2 = propertyMember.m_PropertyInfo.GetSetMethod(true);
							this.m_SetClassValueAction = (ReflectedMemberProperty<TContainer, TValue>.SetClassValueAction)Delegate.CreateDelegate(typeof(ReflectedMemberProperty<TContainer, TValue>.SetClassValueAction), setMethod2);
						}
					}
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005164 File Offset: 0x00003364
		public override TValue GetValue(ref TContainer container)
		{
			bool isStructContainerType = this.m_IsStructContainerType;
			TValue tvalue;
			if (isStructContainerType)
			{
				tvalue = ((this.m_GetStructValueAction == null) ? ((TValue)((object)this.m_Info.GetValue(container))) : this.m_GetStructValueAction(ref container));
			}
			else
			{
				tvalue = ((this.m_GetClassValueAction == null) ? ((TValue)((object)this.m_Info.GetValue(container))) : this.m_GetClassValueAction(container));
			}
			return tvalue;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000051EC File Offset: 0x000033EC
		public override void SetValue(ref TContainer container, TValue value)
		{
			bool isReadOnly = this.IsReadOnly;
			if (isReadOnly)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}
			bool isStructContainerType = this.m_IsStructContainerType;
			if (isStructContainerType)
			{
				bool flag = this.m_SetStructValueAction == null;
				if (flag)
				{
					object obj = container;
					this.m_Info.SetValue(obj, value);
					container = (TContainer)((object)obj);
				}
				else
				{
					this.m_SetStructValueAction(ref container, value);
				}
			}
			else
			{
				bool flag2 = this.m_SetClassValueAction == null;
				if (flag2)
				{
					this.m_Info.SetValue(container, value);
				}
				else
				{
					this.m_SetClassValueAction(container, value);
				}
			}
		}

		// Token: 0x04000051 RID: 81
		private readonly IMemberInfo m_Info;

		// Token: 0x04000052 RID: 82
		private readonly bool m_IsStructContainerType;

		// Token: 0x04000053 RID: 83
		private ReflectedMemberProperty<TContainer, TValue>.GetStructValueAction m_GetStructValueAction;

		// Token: 0x04000054 RID: 84
		private ReflectedMemberProperty<TContainer, TValue>.SetStructValueAction m_SetStructValueAction;

		// Token: 0x04000055 RID: 85
		private ReflectedMemberProperty<TContainer, TValue>.GetClassValueAction m_GetClassValueAction;

		// Token: 0x04000056 RID: 86
		private ReflectedMemberProperty<TContainer, TValue>.SetClassValueAction m_SetClassValueAction;

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x060000EC RID: 236
		private delegate TValue GetStructValueAction(ref TContainer container);

		// Token: 0x02000032 RID: 50
		// (Invoke) Token: 0x060000F0 RID: 240
		private delegate void SetStructValueAction(ref TContainer container, TValue value);

		// Token: 0x02000033 RID: 51
		// (Invoke) Token: 0x060000F4 RID: 244
		private delegate TValue GetClassValueAction(TContainer container);

		// Token: 0x02000034 RID: 52
		// (Invoke) Token: 0x060000F8 RID: 248
		private delegate void SetClassValueAction(TContainer container, TValue value);
	}
}
