using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200003E RID: 62
	internal class TextInfo
	{
		// Token: 0x060001DC RID: 476 RVA: 0x00021A9C File Offset: 0x0001FC9C
		public TextInfo()
		{
			this.textElementInfo = new TextElementInfo[4];
			this.wordInfo = new WordInfo[1];
			this.lineInfo = new LineInfo[1];
			this.pageInfo = new PageInfo[1];
			this.linkInfo = new LinkInfo[0];
			this.meshInfo = new MeshInfo[1];
			this.materialCount = 0;
			this.isDirty = true;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00021B10 File Offset: 0x0001FD10
		internal void Clear()
		{
			this.characterCount = 0;
			this.spaceCount = 0;
			this.wordCount = 0;
			this.linkCount = 0;
			this.lineCount = 0;
			this.pageCount = 0;
			this.spriteCount = 0;
			this.hasMultipleColors = false;
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].vertexCount = 0;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00021B84 File Offset: 0x0001FD84
		internal void ClearMeshInfo(bool updateMesh)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(updateMesh);
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00021BBC File Offset: 0x0001FDBC
		internal void ClearLineInfo()
		{
			bool flag = this.lineInfo == null;
			if (flag)
			{
				this.lineInfo = new LineInfo[1];
			}
			for (int i = 0; i < this.lineInfo.Length; i++)
			{
				this.lineInfo[i].characterCount = 0;
				this.lineInfo[i].spaceCount = 0;
				this.lineInfo[i].wordCount = 0;
				this.lineInfo[i].controlCharacterCount = 0;
				this.lineInfo[i].ascender = TextInfo.s_InfinityVectorNegative.x;
				this.lineInfo[i].baseline = 0f;
				this.lineInfo[i].descender = TextInfo.s_InfinityVectorPositive.x;
				this.lineInfo[i].maxAdvance = 0f;
				this.lineInfo[i].marginLeft = 0f;
				this.lineInfo[i].marginRight = 0f;
				this.lineInfo[i].lineExtents.min = TextInfo.s_InfinityVectorPositive;
				this.lineInfo[i].lineExtents.max = TextInfo.s_InfinityVectorNegative;
				this.lineInfo[i].width = 0f;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00021D24 File Offset: 0x0001FF24
		internal void ClearPageInfo()
		{
			bool flag = this.pageInfo == null;
			if (flag)
			{
				this.pageInfo = new PageInfo[2];
			}
			int num = this.pageInfo.Length;
			for (int i = 0; i < num; i++)
			{
				this.pageInfo[i].firstCharacterIndex = 0;
				this.pageInfo[i].lastCharacterIndex = 0;
				this.pageInfo[i].ascender = -32767f;
				this.pageInfo[i].baseLine = 0f;
				this.pageInfo[i].descender = 32767f;
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00021DCC File Offset: 0x0001FFCC
		internal static void Resize<T>(ref T[] array, int size)
		{
			int num = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size));
			Array.Resize<T>(ref array, num);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00021DFC File Offset: 0x0001FFFC
		internal static void Resize<T>(ref T[] array, int size, bool isBlockAllocated)
		{
			if (isBlockAllocated)
			{
				size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size));
			}
			bool flag = size == array.Length;
			if (!flag)
			{
				Array.Resize<T>(ref array, size);
			}
		}

		// Token: 0x040002C1 RID: 705
		private static Vector2 s_InfinityVectorPositive = new Vector2(32767f, 32767f);

		// Token: 0x040002C2 RID: 706
		private static Vector2 s_InfinityVectorNegative = new Vector2(-32767f, -32767f);

		// Token: 0x040002C3 RID: 707
		public int characterCount;

		// Token: 0x040002C4 RID: 708
		public int spriteCount;

		// Token: 0x040002C5 RID: 709
		public int spaceCount;

		// Token: 0x040002C6 RID: 710
		public int wordCount;

		// Token: 0x040002C7 RID: 711
		public int linkCount;

		// Token: 0x040002C8 RID: 712
		public int lineCount;

		// Token: 0x040002C9 RID: 713
		public int pageCount;

		// Token: 0x040002CA RID: 714
		public int materialCount;

		// Token: 0x040002CB RID: 715
		public TextElementInfo[] textElementInfo;

		// Token: 0x040002CC RID: 716
		public WordInfo[] wordInfo;

		// Token: 0x040002CD RID: 717
		public LinkInfo[] linkInfo;

		// Token: 0x040002CE RID: 718
		public LineInfo[] lineInfo;

		// Token: 0x040002CF RID: 719
		public PageInfo[] pageInfo;

		// Token: 0x040002D0 RID: 720
		public MeshInfo[] meshInfo;

		// Token: 0x040002D1 RID: 721
		public bool isDirty;

		// Token: 0x040002D2 RID: 722
		public bool hasMultipleColors = false;
	}
}
