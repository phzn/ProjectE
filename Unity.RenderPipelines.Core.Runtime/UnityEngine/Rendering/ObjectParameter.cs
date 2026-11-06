using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Rendering
{
	// Token: 0x02000114 RID: 276
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class ObjectParameter<T> : VolumeParameter<T>
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x000273CA File Offset: 0x000255CA
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x000273D2 File Offset: 0x000255D2
		internal ReadOnlyCollection<VolumeParameter> parameters { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x000273DB File Offset: 0x000255DB
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x000273DE File Offset: 0x000255DE
		public sealed override bool overrideState
		{
			get
			{
				return true;
			}
			set
			{
				this.m_OverrideState = true;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x000273E7 File Offset: 0x000255E7
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x000273F0 File Offset: 0x000255F0
		public sealed override T value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
				if (this.m_Value == null)
				{
					this.parameters = null;
					return;
				}
				this.parameters = (from t in this.m_Value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
					where t.FieldType.IsSubclassOf(typeof(VolumeParameter))
					orderby t.MetadataToken
					select (VolumeParameter)t.GetValue(this.m_Value)).ToList<VolumeParameter>().AsReadOnly();
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002749A File Offset: 0x0002569A
		public ObjectParameter(T value)
		{
			this.m_OverrideState = true;
			this.value = value;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000274B0 File Offset: 0x000256B0
		internal override void Interp(VolumeParameter from, VolumeParameter to, float t)
		{
			if (this.m_Value == null)
			{
				return;
			}
			ReadOnlyCollection<VolumeParameter> parameters = this.parameters;
			ReadOnlyCollection<VolumeParameter> parameters2 = ((ObjectParameter<T>)from).parameters;
			ReadOnlyCollection<VolumeParameter> parameters3 = ((ObjectParameter<T>)to).parameters;
			for (int i = 0; i < parameters2.Count; i++)
			{
				parameters[i].overrideState = parameters3[i].overrideState;
				if (parameters3[i].overrideState)
				{
					parameters[i].Interp(parameters2[i], parameters3[i], t);
				}
			}
		}
	}
}
