using System;
using DA_Assets.FCU.Model;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000031 RID: 49
	public class DiffInfo : IHaveId
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000037EA File Offset: 0x000019EA
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000037F2 File Offset: 0x000019F2
		public string Id { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000037FB File Offset: 0x000019FB
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00003803 File Offset: 0x00001A03
		public string Name { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000380C File Offset: 0x00001A0C
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003814 File Offset: 0x00001A14
		public bool IsFrame { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000381D File Offset: 0x00001A1D
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00003825 File Offset: 0x00001A25
		public SyncData RootFrame { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000382E File Offset: 0x00001A2E
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003836 File Offset: 0x00001A36
		public FObject NewData { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000383F File Offset: 0x00001A3F
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003847 File Offset: 0x00001A47
		public SyncData OldData { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003850 File Offset: 0x00001A50
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00003858 File Offset: 0x00001A58
		public bool IsNew { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003861 File Offset: 0x00001A61
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00003869 File Offset: 0x00001A69
		public bool HasFigmaDiff { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003872 File Offset: 0x00001A72
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000387A File Offset: 0x00001A7A
		public TProp<Color, Color> Color { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003883 File Offset: 0x00001A83
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x0000388B File Offset: 0x00001A8B
		public TProp<Vector2, Vector2> Size { get; set; }
	}
}
