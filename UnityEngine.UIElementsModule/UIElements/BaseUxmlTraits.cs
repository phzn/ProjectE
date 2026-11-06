using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine.UIElements
{
	// Token: 0x020003C5 RID: 965
	public abstract class BaseUxmlTraits
	{
		// Token: 0x06001FDB RID: 8155 RVA: 0x00079000 File Offset: 0x00077200
		protected BaseUxmlTraits()
		{
			this.canHaveAnyAttribute = true;
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00079012 File Offset: 0x00077212
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x0007901A File Offset: 0x0007721A
		public bool canHaveAnyAttribute { get; protected set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00079024 File Offset: 0x00077224
		public virtual IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
		{
			get
			{
				foreach (UxmlAttributeDescription attributeDescription in this.GetAllAttributeDescriptionForType(base.GetType()))
				{
					yield return attributeDescription;
					attributeDescription = null;
				}
				IEnumerator<UxmlAttributeDescription> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x00079044 File Offset: 0x00077244
		public virtual IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x00079063 File Offset: 0x00077263
		private IEnumerable<UxmlAttributeDescription> GetAllAttributeDescriptionForType(Type t)
		{
			Type baseType = t.BaseType;
			bool flag = baseType != null;
			if (flag)
			{
				foreach (UxmlAttributeDescription ident in this.GetAllAttributeDescriptionForType(baseType))
				{
					yield return ident;
					ident = null;
				}
				IEnumerator<UxmlAttributeDescription> enumerator = null;
			}
			foreach (FieldInfo fieldInfo in from f in t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where typeof(UxmlAttributeDescription).IsAssignableFrom(f.FieldType)
				select f)
			{
				yield return (UxmlAttributeDescription)fieldInfo.GetValue(this);
				fieldInfo = null;
			}
			IEnumerator<FieldInfo> enumerator2 = null;
			yield break;
			yield break;
		}
	}
}
