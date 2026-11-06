using System;
using UnityEngine.Serialization;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class AssetLabelReference : IKeyEvaluator
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005A7F File Offset: 0x00003C7F
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00005A87 File Offset: 0x00003C87
		public string labelString
		{
			get
			{
				return this.m_LabelString;
			}
			set
			{
				this.m_LabelString = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005A90 File Offset: 0x00003C90
		public object RuntimeKey
		{
			get
			{
				if (this.labelString == null)
				{
					this.labelString = string.Empty;
				}
				return this.labelString;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005AAB File Offset: 0x00003CAB
		public bool RuntimeKeyIsValid()
		{
			return !string.IsNullOrEmpty(this.RuntimeKey.ToString());
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public override int GetHashCode()
		{
			return this.labelString.GetHashCode();
		}

		// Token: 0x04000027 RID: 39
		[FormerlySerializedAs("m_labelString")]
		[SerializeField]
		private string m_LabelString;
	}
}
