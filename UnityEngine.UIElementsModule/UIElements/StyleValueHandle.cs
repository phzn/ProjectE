using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200035A RID: 858
	[Serializable]
	internal struct StyleValueHandle
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x0006F9EC File Offset: 0x0006DBEC
		// (set) Token: 0x06001CC1 RID: 7361 RVA: 0x0006FA04 File Offset: 0x0006DC04
		public StyleValueType valueType
		{
			get
			{
				return this.m_ValueType;
			}
			internal set
			{
				this.m_ValueType = value;
			}
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x0006FA0E File Offset: 0x0006DC0E
		internal StyleValueHandle(int valueIndex, StyleValueType valueType)
		{
			this.valueIndex = valueIndex;
			this.m_ValueType = valueType;
		}

		// Token: 0x04000BFC RID: 3068
		[SerializeField]
		private StyleValueType m_ValueType;

		// Token: 0x04000BFD RID: 3069
		[SerializeField]
		internal int valueIndex;
	}
}
