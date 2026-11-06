using System;
using System.Collections.Generic;
using DA_Assets.Extensions;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.DAI
{
	// Token: 0x0200003C RID: 60
	public abstract class LinkerBase<T3> where T3 : Object
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000061A8 File Offset: 0x000043A8
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000061B0 File Offset: 0x000043B0
		public T3 MonoBeh
		{
			get
			{
				return this.monoBeh;
			}
			set
			{
				this.monoBeh = value;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000061B9 File Offset: 0x000043B9
		public virtual void OnLink()
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000061BB File Offset: 0x000043BB
		public virtual void Reset()
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000061BD File Offset: 0x000043BD
		public void SetDirty()
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000061C0 File Offset: 0x000043C0
		public void SetValue<T>(ref T currentValue, T newValue)
		{
			if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
			{
				if (LinkerBase<T3>.IsTypeNestedFromType(typeof(T3), typeof(ScriptableObject)))
				{
					SceneBackuper.MakeActiveSceneDirty();
				}
				this.monoBeh.SetDirtyExt();
				currentValue = newValue;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006217 File Offset: 0x00004417
		public static bool IsTypeNestedFromType(Type currentType, Type parentType)
		{
			while (currentType != null)
			{
				if (currentType == parentType)
				{
					return true;
				}
				currentType = currentType.BaseType;
			}
			return false;
		}

		// Token: 0x04000071 RID: 113
		[SerializeField]
		protected T3 monoBeh;
	}
}
