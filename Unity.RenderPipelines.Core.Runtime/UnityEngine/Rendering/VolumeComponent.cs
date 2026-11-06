using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Rendering
{
	// Token: 0x020000EB RID: 235
	[Serializable]
	public class VolumeComponent : ScriptableObject
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00025AFE File Offset: 0x00023CFE
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x00025B06 File Offset: 0x00023D06
		public string displayName { get; protected set; } = "";

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00025B0F File Offset: 0x00023D0F
		public ReadOnlyCollection<VolumeParameter> parameters
		{
			get
			{
				if (this.m_ParameterReadOnlyCollection == null)
				{
					this.m_ParameterReadOnlyCollection = this.parameterList.AsReadOnly();
				}
				return this.m_ParameterReadOnlyCollection;
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00025B30 File Offset: 0x00023D30
		internal static void FindParameters(object o, List<VolumeParameter> parameters, Func<FieldInfo, bool> filter = null)
		{
			if (o == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in from t in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				orderby t.MetadataToken
				select t)
			{
				if (fieldInfo.FieldType.IsSubclassOf(typeof(VolumeParameter)))
				{
					if (filter == null || filter(fieldInfo))
					{
						VolumeParameter volumeParameter = (VolumeParameter)fieldInfo.GetValue(o);
						parameters.Add(volumeParameter);
					}
				}
				else if (!fieldInfo.FieldType.IsArray && fieldInfo.FieldType.IsClass)
				{
					VolumeComponent.FindParameters(fieldInfo.GetValue(o), parameters, filter);
				}
			}
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00025C0C File Offset: 0x00023E0C
		protected virtual void OnEnable()
		{
			this.parameterList.Clear();
			VolumeComponent.FindParameters(this, this.parameterList, null);
			foreach (VolumeParameter volumeParameter in this.parameterList)
			{
				if (volumeParameter != null)
				{
					volumeParameter.OnEnable();
				}
				else
				{
					Debug.LogWarning("Volume Component " + base.GetType().Name + " contains a null parameter; please make sure all parameters are initialized to a default value. Until this is fixed the null parameters will not be considered by the system.");
				}
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00025C9C File Offset: 0x00023E9C
		protected virtual void OnDisable()
		{
			foreach (VolumeParameter volumeParameter in this.parameterList)
			{
				if (volumeParameter != null)
				{
					volumeParameter.OnDisable();
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00025CF4 File Offset: 0x00023EF4
		public virtual void Override(VolumeComponent state, float interpFactor)
		{
			int count = this.parameterList.Count;
			for (int i = 0; i < count; i++)
			{
				VolumeParameter volumeParameter = state.parameterList[i];
				VolumeParameter volumeParameter2 = this.parameterList[i];
				if (volumeParameter2.overrideState)
				{
					volumeParameter.overrideState = volumeParameter2.overrideState;
					volumeParameter.Interp(volumeParameter, volumeParameter2, interpFactor);
				}
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00025D50 File Offset: 0x00023F50
		public void SetAllOverridesTo(bool state)
		{
			this.SetOverridesTo(this.parameterList, state);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00025D60 File Offset: 0x00023F60
		internal void SetOverridesTo(IEnumerable<VolumeParameter> enumerable, bool state)
		{
			foreach (VolumeParameter volumeParameter in enumerable)
			{
				volumeParameter.overrideState = state;
				Type type = volumeParameter.GetType();
				if (VolumeParameter.IsObjectParameter(type))
				{
					ReadOnlyCollection<VolumeParameter> readOnlyCollection = (ReadOnlyCollection<VolumeParameter>)type.GetProperty("parameters", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(volumeParameter, null);
					if (readOnlyCollection != null)
					{
						this.SetOverridesTo(readOnlyCollection, state);
					}
				}
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00025DDC File Offset: 0x00023FDC
		public override int GetHashCode()
		{
			int num = 17;
			for (int i = 0; i < this.parameterList.Count; i++)
			{
				num = num * 23 + this.parameterList[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00025E1C File Offset: 0x0002401C
		public bool AnyPropertiesIsOverridden()
		{
			for (int i = 0; i < this.parameterList.Count; i++)
			{
				if (this.parameterList[i].overrideState)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00025E55 File Offset: 0x00024055
		protected virtual void OnDestroy()
		{
			this.Release();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00025E60 File Offset: 0x00024060
		public void Release()
		{
			if (this.parameterList == null)
			{
				return;
			}
			for (int i = 0; i < this.parameterList.Count; i++)
			{
				if (this.parameterList[i] != null)
				{
					this.parameterList[i].Release();
				}
			}
		}

		// Token: 0x040004C3 RID: 1219
		public bool active = true;

		// Token: 0x040004C5 RID: 1221
		internal readonly List<VolumeParameter> parameterList = new List<VolumeParameter>();

		// Token: 0x040004C6 RID: 1222
		private ReadOnlyCollection<VolumeParameter> m_ParameterReadOnlyCollection;

		// Token: 0x020001D6 RID: 470
		public sealed class Indent : PropertyAttribute
		{
			// Token: 0x06000B6C RID: 2924 RVA: 0x00030210 File Offset: 0x0002E410
			public Indent(int relativeAmount = 1)
			{
				this.relativeAmount = relativeAmount;
			}

			// Token: 0x040007A4 RID: 1956
			public readonly int relativeAmount;
		}
	}
}
