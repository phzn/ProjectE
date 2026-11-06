using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000027 RID: 39
	public abstract class InputBindingComposite
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002D9 RID: 729
		public abstract Type valueType { get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002DA RID: 730
		public abstract int valueSizeInBytes { get; }

		// Token: 0x060002DB RID: 731
		public unsafe abstract void ReadValue(ref InputBindingCompositeContext context, void* buffer, int bufferSize);

		// Token: 0x060002DC RID: 732
		public abstract object ReadValueAsObject(ref InputBindingCompositeContext context);

		// Token: 0x060002DD RID: 733 RVA: 0x0000CE48 File Offset: 0x0000B048
		public virtual float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			return -1f;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000CE4F File Offset: 0x0000B04F
		protected virtual void FinishSetup(ref InputBindingCompositeContext context)
		{
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000CE51 File Offset: 0x0000B051
		internal void CallFinishSetup(ref InputBindingCompositeContext context)
		{
			this.FinishSetup(ref context);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000CE5C File Offset: 0x0000B05C
		internal static Type GetValueType(string composite)
		{
			if (string.IsNullOrEmpty(composite))
			{
				throw new ArgumentNullException("composite");
			}
			Type type = InputBindingComposite.s_Composites.LookupTypeRegistration(composite);
			if (type == null)
			{
				return null;
			}
			return TypeHelpers.GetGenericTypeArgumentFromHierarchy(type, typeof(InputBindingComposite<>), 0);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
		public static string GetExpectedControlLayoutName(string composite, string part)
		{
			if (string.IsNullOrEmpty(composite))
			{
				throw new ArgumentNullException("composite");
			}
			if (string.IsNullOrEmpty(part))
			{
				throw new ArgumentNullException("part");
			}
			Type type = InputBindingComposite.s_Composites.LookupTypeRegistration(composite);
			if (type == null)
			{
				return null;
			}
			FieldInfo field = type.GetField(part, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (field == null)
			{
				return null;
			}
			InputControlAttribute customAttribute = field.GetCustomAttribute<InputControlAttribute>(false);
			if (customAttribute == null)
			{
				return null;
			}
			return customAttribute.layout;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000CF15 File Offset: 0x0000B115
		internal static IEnumerable<string> GetPartNames(string composite)
		{
			if (string.IsNullOrEmpty(composite))
			{
				throw new ArgumentNullException("composite");
			}
			Type type = InputBindingComposite.s_Composites.LookupTypeRegistration(composite);
			if (type == null)
			{
				yield break;
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				if (fieldInfo.GetCustomAttribute<InputControlAttribute>() != null)
				{
					yield return fieldInfo.Name;
				}
			}
			FieldInfo[] array = null;
			yield break;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000CF28 File Offset: 0x0000B128
		internal static string GetDisplayFormatString(string composite)
		{
			if (string.IsNullOrEmpty(composite))
			{
				throw new ArgumentNullException("composite");
			}
			Type type = InputBindingComposite.s_Composites.LookupTypeRegistration(composite);
			if (type == null)
			{
				return null;
			}
			DisplayStringFormatAttribute customAttribute = type.GetCustomAttribute<DisplayStringFormatAttribute>();
			if (customAttribute == null)
			{
				return null;
			}
			return customAttribute.formatString;
		}

		// Token: 0x040000EF RID: 239
		internal static TypeTable s_Composites;
	}
}
