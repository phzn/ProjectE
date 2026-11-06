using System;

namespace UnityEngine.Localization.Pseudo
{
	// Token: 0x020000AF RID: 175
	[Serializable]
	public class Encapsulator : IPseudoLocalizationMethod
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x000116F6 File Offset: 0x0000F8F6
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x000116FE File Offset: 0x0000F8FE
		public string Start
		{
			get
			{
				return this.m_Start;
			}
			set
			{
				this.m_Start = value;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00011707 File Offset: 0x0000F907
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0001170F File Offset: 0x0000F90F
		public string End
		{
			get
			{
				return this.m_End;
			}
			set
			{
				this.m_End = value;
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00011718 File Offset: 0x0000F918
		public void Transform(Message message)
		{
			ReadOnlyMessageFragment readOnlyMessageFragment = message.CreateReadonlyTextFragment(this.Start);
			ReadOnlyMessageFragment readOnlyMessageFragment2 = message.CreateReadonlyTextFragment(this.End);
			message.Fragments.Insert(0, readOnlyMessageFragment);
			message.Fragments.Add(readOnlyMessageFragment2);
		}

		// Token: 0x0400016A RID: 362
		[SerializeField]
		private string m_Start = "[";

		// Token: 0x0400016B RID: 363
		[SerializeField]
		private string m_End = "]";
	}
}
