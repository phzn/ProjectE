using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000DA RID: 218
	[Metadata]
	[Serializable]
	public class Comment : IMetadata
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00014D21 File Offset: 0x00012F21
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00014D29 File Offset: 0x00012F29
		public string CommentText
		{
			get
			{
				return this.m_CommentText;
			}
			set
			{
				this.m_CommentText = value;
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00014D32 File Offset: 0x00012F32
		public override string ToString()
		{
			return this.CommentText;
		}

		// Token: 0x04000210 RID: 528
		[SerializeField]
		[TextArea(1, 2147483647)]
		private string m_CommentText = "Comment Text";
	}
}
