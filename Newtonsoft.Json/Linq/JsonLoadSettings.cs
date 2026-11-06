using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C0 RID: 192
	public class JsonLoadSettings
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x00029FA0 File Offset: 0x000281A0
		public JsonLoadSettings()
		{
			this._lineInfoHandling = LineInfoHandling.Load;
			this._commentHandling = CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00029FBD File Offset: 0x000281BD
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x00029FC5 File Offset: 0x000281C5
		public CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < CommentHandling.Ignore || value > CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00029FE1 File Offset: 0x000281E1
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x00029FE9 File Offset: 0x000281E9
		public LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0002A005 File Offset: 0x00028205
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x0002A00D File Offset: 0x0002820D
		public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < DuplicatePropertyNameHandling.Replace || value > DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		// Token: 0x04000385 RID: 901
		private CommentHandling _commentHandling;

		// Token: 0x04000386 RID: 902
		private LineInfoHandling _lineInfoHandling;

		// Token: 0x04000387 RID: 903
		private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;
	}
}
