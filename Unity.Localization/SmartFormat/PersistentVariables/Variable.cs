using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public class Variable<T> : IVariableValueChanged, IVariable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000294 RID: 660 RVA: 0x0000A13C File Offset: 0x0000833C
		// (remove) Token: 0x06000295 RID: 661 RVA: 0x0000A174 File Offset: 0x00008374
		public event Action<IVariable> ValueChanged;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000A1A9 File Offset: 0x000083A9
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000A1B1 File Offset: 0x000083B1
		public T Value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				if (EqualityComparer<T>.Default.Equals(this.m_Value, value))
				{
					return;
				}
				this.m_Value = value;
				this.SendValueChangedEvent();
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A1D4 File Offset: 0x000083D4
		public object GetSourceValue(ISelectorInfo _)
		{
			return this.Value;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A1E1 File Offset: 0x000083E1
		private void SendValueChangedEvent()
		{
			Action<IVariable> valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A1F4 File Offset: 0x000083F4
		public override string ToString()
		{
			T value = this.Value;
			return value.ToString();
		}

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private T m_Value;
	}
}
