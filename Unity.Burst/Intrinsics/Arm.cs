using System;
using System.Diagnostics;

namespace Unity.Burst.Intrinsics
{
	// Token: 0x02000032 RID: 50
	public static class Arm
	{
		// Token: 0x02000033 RID: 51
		public class Neon
		{
			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060000F7 RID: 247 RVA: 0x00002556 File Offset: 0x00000756
			public static bool IsNeonSupported
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060000F8 RID: 248 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vadd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000F9 RID: 249 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000FA RID: 250 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vadd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000FB RID: 251 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000FC RID: 252 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vadd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000FD RID: 253 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000FE RID: 254 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vadd_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060000FF RID: 255 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000100 RID: 256 RVA: 0x00005BEF File Offset: 0x00003DEF
			[DebuggerStepThrough]
			public static v64 vadd_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vadd_s8(a0, a1);
			}

			// Token: 0x06000101 RID: 257 RVA: 0x00005BF8 File Offset: 0x00003DF8
			[DebuggerStepThrough]
			public static v128 vaddq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddq_s8(a0, a1);
			}

			// Token: 0x06000102 RID: 258 RVA: 0x00005C01 File Offset: 0x00003E01
			[DebuggerStepThrough]
			public static v64 vadd_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vadd_s16(a0, a1);
			}

			// Token: 0x06000103 RID: 259 RVA: 0x00005C0A File Offset: 0x00003E0A
			[DebuggerStepThrough]
			public static v128 vaddq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddq_s16(a0, a1);
			}

			// Token: 0x06000104 RID: 260 RVA: 0x00005C13 File Offset: 0x00003E13
			[DebuggerStepThrough]
			public static v64 vadd_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vadd_s32(a0, a1);
			}

			// Token: 0x06000105 RID: 261 RVA: 0x00005C1C File Offset: 0x00003E1C
			[DebuggerStepThrough]
			public static v128 vaddq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddq_s32(a0, a1);
			}

			// Token: 0x06000106 RID: 262 RVA: 0x00005C25 File Offset: 0x00003E25
			[DebuggerStepThrough]
			public static v64 vadd_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vadd_s64(a0, a1);
			}

			// Token: 0x06000107 RID: 263 RVA: 0x00005C2E File Offset: 0x00003E2E
			[DebuggerStepThrough]
			public static v128 vaddq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddq_s64(a0, a1);
			}

			// Token: 0x06000108 RID: 264 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vadd_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000109 RID: 265 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600010A RID: 266 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600010B RID: 267 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600010C RID: 268 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600010D RID: 269 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600010E RID: 270 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600010F RID: 271 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000110 RID: 272 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_s8(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000111 RID: 273 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_s16(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000112 RID: 274 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_s32(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000113 RID: 275 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_u8(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000114 RID: 276 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_u16(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000115 RID: 277 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_u32(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000116 RID: 278 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhadd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000117 RID: 279 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhaddq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000118 RID: 280 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhadd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000119 RID: 281 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhaddq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600011A RID: 282 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhadd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600011B RID: 283 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhaddq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600011C RID: 284 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhadd_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600011D RID: 285 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhaddq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600011E RID: 286 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhadd_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600011F RID: 287 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhaddq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000120 RID: 288 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhadd_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000121 RID: 289 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhaddq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000122 RID: 290 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrhadd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000123 RID: 291 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrhaddq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000124 RID: 292 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrhadd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000125 RID: 293 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrhaddq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000126 RID: 294 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrhadd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000127 RID: 295 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrhaddq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000128 RID: 296 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrhadd_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000129 RID: 297 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrhaddq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600012A RID: 298 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrhadd_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600012B RID: 299 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrhaddq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600012C RID: 300 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrhadd_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600012D RID: 301 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrhaddq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600012E RID: 302 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600012F RID: 303 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000130 RID: 304 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000131 RID: 305 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000132 RID: 306 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000133 RID: 307 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000134 RID: 308 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000135 RID: 309 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000136 RID: 310 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000137 RID: 311 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000138 RID: 312 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000139 RID: 313 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600013A RID: 314 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600013B RID: 315 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600013C RID: 316 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqadd_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600013D RID: 317 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqaddq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600013E RID: 318 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaddhn_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600013F RID: 319 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaddhn_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000140 RID: 320 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaddhn_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000141 RID: 321 RVA: 0x00005C37 File Offset: 0x00003E37
			[DebuggerStepThrough]
			public static v64 vaddhn_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddhn_s16(a0, a1);
			}

			// Token: 0x06000142 RID: 322 RVA: 0x00005C40 File Offset: 0x00003E40
			[DebuggerStepThrough]
			public static v64 vaddhn_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddhn_s32(a0, a1);
			}

			// Token: 0x06000143 RID: 323 RVA: 0x00005C49 File Offset: 0x00003E49
			[DebuggerStepThrough]
			public static v64 vaddhn_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vaddhn_s64(a0, a1);
			}

			// Token: 0x06000144 RID: 324 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vraddhn_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000145 RID: 325 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vraddhn_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000146 RID: 326 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vraddhn_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000147 RID: 327 RVA: 0x00005C52 File Offset: 0x00003E52
			[DebuggerStepThrough]
			public static v64 vraddhn_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vraddhn_s16(a0, a1);
			}

			// Token: 0x06000148 RID: 328 RVA: 0x00005C5B File Offset: 0x00003E5B
			[DebuggerStepThrough]
			public static v64 vraddhn_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vraddhn_s32(a0, a1);
			}

			// Token: 0x06000149 RID: 329 RVA: 0x00005C64 File Offset: 0x00003E64
			[DebuggerStepThrough]
			public static v64 vraddhn_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vraddhn_s64(a0, a1);
			}

			// Token: 0x0600014A RID: 330 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600014B RID: 331 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600014C RID: 332 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600014D RID: 333 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600014E RID: 334 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600014F RID: 335 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000150 RID: 336 RVA: 0x00005C6D File Offset: 0x00003E6D
			[DebuggerStepThrough]
			public static v64 vmul_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vmul_s8(a0, a1);
			}

			// Token: 0x06000151 RID: 337 RVA: 0x00005C76 File Offset: 0x00003E76
			[DebuggerStepThrough]
			public static v128 vmulq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vmulq_s8(a0, a1);
			}

			// Token: 0x06000152 RID: 338 RVA: 0x00005C7F File Offset: 0x00003E7F
			[DebuggerStepThrough]
			public static v64 vmul_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vmul_s16(a0, a1);
			}

			// Token: 0x06000153 RID: 339 RVA: 0x00005C88 File Offset: 0x00003E88
			[DebuggerStepThrough]
			public static v128 vmulq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vmulq_s16(a0, a1);
			}

			// Token: 0x06000154 RID: 340 RVA: 0x00005C91 File Offset: 0x00003E91
			[DebuggerStepThrough]
			public static v64 vmul_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vmul_s32(a0, a1);
			}

			// Token: 0x06000155 RID: 341 RVA: 0x00005C9A File Offset: 0x00003E9A
			[DebuggerStepThrough]
			public static v128 vmulq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vmulq_s32(a0, a1);
			}

			// Token: 0x06000156 RID: 342 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000157 RID: 343 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000158 RID: 344 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_s8(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000159 RID: 345 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600015A RID: 346 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_s16(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600015B RID: 347 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600015C RID: 348 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_s32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600015D RID: 349 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600015E RID: 350 RVA: 0x00005CA3 File Offset: 0x00003EA3
			[DebuggerStepThrough]
			public static v64 vmla_u8(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vmla_s8(a0, a1, a2);
			}

			// Token: 0x0600015F RID: 351 RVA: 0x00005CAD File Offset: 0x00003EAD
			[DebuggerStepThrough]
			public static v128 vmlaq_u8(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vmlaq_s8(a0, a1, a2);
			}

			// Token: 0x06000160 RID: 352 RVA: 0x00005CB7 File Offset: 0x00003EB7
			[DebuggerStepThrough]
			public static v64 vmla_u16(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vmla_s16(a0, a1, a2);
			}

			// Token: 0x06000161 RID: 353 RVA: 0x00005CC1 File Offset: 0x00003EC1
			[DebuggerStepThrough]
			public static v128 vmlaq_u16(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vmlaq_s16(a0, a1, a2);
			}

			// Token: 0x06000162 RID: 354 RVA: 0x00005CCB File Offset: 0x00003ECB
			[DebuggerStepThrough]
			public static v64 vmla_u32(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vmla_s32(a0, a1, a2);
			}

			// Token: 0x06000163 RID: 355 RVA: 0x00005CD5 File Offset: 0x00003ED5
			[DebuggerStepThrough]
			public static v128 vmlaq_u32(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vmlaq_s32(a0, a1, a2);
			}

			// Token: 0x06000164 RID: 356 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_f32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000165 RID: 357 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_f32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000166 RID: 358 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_s8(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000167 RID: 359 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_s16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000168 RID: 360 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_s32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000169 RID: 361 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_u8(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600016A RID: 362 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_u16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600016B RID: 363 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_u32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600016C RID: 364 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_s8(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600016D RID: 365 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600016E RID: 366 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_s16(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600016F RID: 367 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000170 RID: 368 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_s32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000171 RID: 369 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00005CDF File Offset: 0x00003EDF
			[DebuggerStepThrough]
			public static v64 vmls_u8(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vmls_s8(a0, a1, a2);
			}

			// Token: 0x06000173 RID: 371 RVA: 0x00005CE9 File Offset: 0x00003EE9
			[DebuggerStepThrough]
			public static v128 vmlsq_u8(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vmlsq_s8(a0, a1, a2);
			}

			// Token: 0x06000174 RID: 372 RVA: 0x00005CF3 File Offset: 0x00003EF3
			[DebuggerStepThrough]
			public static v64 vmls_u16(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vmls_s16(a0, a1, a2);
			}

			// Token: 0x06000175 RID: 373 RVA: 0x00005CFD File Offset: 0x00003EFD
			[DebuggerStepThrough]
			public static v128 vmlsq_u16(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vmlsq_s16(a0, a1, a2);
			}

			// Token: 0x06000176 RID: 374 RVA: 0x00005D07 File Offset: 0x00003F07
			[DebuggerStepThrough]
			public static v64 vmls_u32(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vmls_s32(a0, a1, a2);
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00005D11 File Offset: 0x00003F11
			[DebuggerStepThrough]
			public static v128 vmlsq_u32(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vmlsq_s32(a0, a1, a2);
			}

			// Token: 0x06000178 RID: 376 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_f32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000179 RID: 377 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_f32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600017A RID: 378 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_s8(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600017B RID: 379 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_s16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600017C RID: 380 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_s32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600017D RID: 381 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_u8(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600017E RID: 382 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_u16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600017F RID: 383 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_u32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000180 RID: 384 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_f32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000181 RID: 385 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_f32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000182 RID: 386 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_f32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000183 RID: 387 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_f32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000184 RID: 388 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000185 RID: 389 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000186 RID: 390 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000187 RID: 391 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000188 RID: 392 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000189 RID: 393 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600018A RID: 394 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600018B RID: 395 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600018C RID: 396 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_s16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600018D RID: 397 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_s32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600018E RID: 398 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_s16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600018F RID: 399 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_s32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000190 RID: 400 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000191 RID: 401 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000192 RID: 402 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000193 RID: 403 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000194 RID: 404 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000195 RID: 405 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000196 RID: 406 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000197 RID: 407 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000198 RID: 408 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsub_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000199 RID: 409 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600019A RID: 410 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsub_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600019B RID: 411 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600019C RID: 412 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsub_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600019D RID: 413 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600019E RID: 414 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsub_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600019F RID: 415 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001A0 RID: 416 RVA: 0x00005D1B File Offset: 0x00003F1B
			[DebuggerStepThrough]
			public static v64 vsub_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vsub_s8(a0, a1);
			}

			// Token: 0x060001A1 RID: 417 RVA: 0x00005D24 File Offset: 0x00003F24
			[DebuggerStepThrough]
			public static v128 vsubq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubq_s8(a0, a1);
			}

			// Token: 0x060001A2 RID: 418 RVA: 0x00005D2D File Offset: 0x00003F2D
			[DebuggerStepThrough]
			public static v64 vsub_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vsub_s16(a0, a1);
			}

			// Token: 0x060001A3 RID: 419 RVA: 0x00005D36 File Offset: 0x00003F36
			[DebuggerStepThrough]
			public static v128 vsubq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubq_s16(a0, a1);
			}

			// Token: 0x060001A4 RID: 420 RVA: 0x00005D3F File Offset: 0x00003F3F
			[DebuggerStepThrough]
			public static v64 vsub_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vsub_s32(a0, a1);
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x00005D48 File Offset: 0x00003F48
			[DebuggerStepThrough]
			public static v128 vsubq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubq_s32(a0, a1);
			}

			// Token: 0x060001A6 RID: 422 RVA: 0x00005D51 File Offset: 0x00003F51
			[DebuggerStepThrough]
			public static v64 vsub_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vsub_s64(a0, a1);
			}

			// Token: 0x060001A7 RID: 423 RVA: 0x00005D5A File Offset: 0x00003F5A
			[DebuggerStepThrough]
			public static v128 vsubq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubq_s64(a0, a1);
			}

			// Token: 0x060001A8 RID: 424 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsub_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001AA RID: 426 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001AB RID: 427 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001AC RID: 428 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001AD RID: 429 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001AE RID: 430 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001AF RID: 431 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B0 RID: 432 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_s8(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B1 RID: 433 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_s16(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B2 RID: 434 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_s32(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B3 RID: 435 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_u8(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_u16(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_u32(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhsub_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhsubq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B8 RID: 440 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhsub_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhsubq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001BA RID: 442 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhsub_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001BB RID: 443 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhsubq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001BC RID: 444 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhsub_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001BD RID: 445 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhsubq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001BE RID: 446 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhsub_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001BF RID: 447 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhsubq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C0 RID: 448 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vhsub_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vhsubq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C2 RID: 450 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C4 RID: 452 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C5 RID: 453 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C6 RID: 454 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C7 RID: 455 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C8 RID: 456 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001C9 RID: 457 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001CA RID: 458 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001CB RID: 459 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001CC RID: 460 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001CD RID: 461 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001CE RID: 462 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001CF RID: 463 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D0 RID: 464 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqsub_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D1 RID: 465 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqsubq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsubhn_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsubhn_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D4 RID: 468 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsubhn_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D5 RID: 469 RVA: 0x00005D63 File Offset: 0x00003F63
			[DebuggerStepThrough]
			public static v64 vsubhn_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubhn_s16(a0, a1);
			}

			// Token: 0x060001D6 RID: 470 RVA: 0x00005D6C File Offset: 0x00003F6C
			[DebuggerStepThrough]
			public static v64 vsubhn_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubhn_s32(a0, a1);
			}

			// Token: 0x060001D7 RID: 471 RVA: 0x00005D75 File Offset: 0x00003F75
			[DebuggerStepThrough]
			public static v64 vsubhn_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vsubhn_s64(a0, a1);
			}

			// Token: 0x060001D8 RID: 472 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsubhn_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001D9 RID: 473 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsubhn_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001DA RID: 474 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsubhn_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001DB RID: 475 RVA: 0x00005D7E File Offset: 0x00003F7E
			[DebuggerStepThrough]
			public static v64 vrsubhn_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vrsubhn_s16(a0, a1);
			}

			// Token: 0x060001DC RID: 476 RVA: 0x00005D87 File Offset: 0x00003F87
			[DebuggerStepThrough]
			public static v64 vrsubhn_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vrsubhn_s32(a0, a1);
			}

			// Token: 0x060001DD RID: 477 RVA: 0x00005D90 File Offset: 0x00003F90
			[DebuggerStepThrough]
			public static v64 vrsubhn_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vrsubhn_s64(a0, a1);
			}

			// Token: 0x060001DE RID: 478 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceq_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001DF RID: 479 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceq_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001E2 RID: 482 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceq_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001E3 RID: 483 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001E4 RID: 484 RVA: 0x00005D99 File Offset: 0x00003F99
			[DebuggerStepThrough]
			public static v64 vceq_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vceq_s8(a0, a1);
			}

			// Token: 0x060001E5 RID: 485 RVA: 0x00005DA2 File Offset: 0x00003FA2
			[DebuggerStepThrough]
			public static v128 vceqq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vceqq_s8(a0, a1);
			}

			// Token: 0x060001E6 RID: 486 RVA: 0x00005DAB File Offset: 0x00003FAB
			[DebuggerStepThrough]
			public static v64 vceq_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vceq_s16(a0, a1);
			}

			// Token: 0x060001E7 RID: 487 RVA: 0x00005DB4 File Offset: 0x00003FB4
			[DebuggerStepThrough]
			public static v128 vceqq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vceqq_s16(a0, a1);
			}

			// Token: 0x060001E8 RID: 488 RVA: 0x00005DBD File Offset: 0x00003FBD
			[DebuggerStepThrough]
			public static v64 vceq_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vceq_s32(a0, a1);
			}

			// Token: 0x060001E9 RID: 489 RVA: 0x00005DC6 File Offset: 0x00003FC6
			[DebuggerStepThrough]
			public static v128 vceqq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vceqq_s32(a0, a1);
			}

			// Token: 0x060001EA RID: 490 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceq_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001EB RID: 491 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001EC RID: 492 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001ED RID: 493 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001EE RID: 494 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001EF RID: 495 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F0 RID: 496 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F1 RID: 497 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F4 RID: 500 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F5 RID: 501 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F6 RID: 502 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F7 RID: 503 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F8 RID: 504 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001F9 RID: 505 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001FA RID: 506 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001FB RID: 507 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001FC RID: 508 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001FD RID: 509 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001FE RID: 510 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060001FF RID: 511 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000200 RID: 512 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000201 RID: 513 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000202 RID: 514 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000203 RID: 515 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000204 RID: 516 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000205 RID: 517 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000206 RID: 518 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000207 RID: 519 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000208 RID: 520 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000209 RID: 521 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600020A RID: 522 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600020B RID: 523 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600020C RID: 524 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600020D RID: 525 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600020E RID: 526 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600020F RID: 527 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000210 RID: 528 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000211 RID: 529 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000212 RID: 530 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000213 RID: 531 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000214 RID: 532 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000215 RID: 533 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000216 RID: 534 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000217 RID: 535 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000218 RID: 536 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000219 RID: 537 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600021A RID: 538 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600021B RID: 539 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600021C RID: 540 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600021D RID: 541 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600021E RID: 542 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600021F RID: 543 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000220 RID: 544 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000221 RID: 545 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000222 RID: 546 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000223 RID: 547 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000224 RID: 548 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcage_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000225 RID: 549 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcageq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000226 RID: 550 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcale_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000227 RID: 551 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcaleq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000228 RID: 552 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcagt_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000229 RID: 553 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcagtq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600022A RID: 554 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcalt_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600022B RID: 555 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcaltq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600022C RID: 556 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtst_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600022D RID: 557 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtstq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600022E RID: 558 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtst_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600022F RID: 559 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtstq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000230 RID: 560 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtst_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000231 RID: 561 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtstq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000232 RID: 562 RVA: 0x00005DCF File Offset: 0x00003FCF
			[DebuggerStepThrough]
			public static v64 vtst_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vtst_s8(a0, a1);
			}

			// Token: 0x06000233 RID: 563 RVA: 0x00005DD8 File Offset: 0x00003FD8
			[DebuggerStepThrough]
			public static v128 vtstq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vtstq_s8(a0, a1);
			}

			// Token: 0x06000234 RID: 564 RVA: 0x00005DE1 File Offset: 0x00003FE1
			[DebuggerStepThrough]
			public static v64 vtst_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vtst_s16(a0, a1);
			}

			// Token: 0x06000235 RID: 565 RVA: 0x00005DEA File Offset: 0x00003FEA
			[DebuggerStepThrough]
			public static v128 vtstq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vtstq_s16(a0, a1);
			}

			// Token: 0x06000236 RID: 566 RVA: 0x00005DF3 File Offset: 0x00003FF3
			[DebuggerStepThrough]
			public static v64 vtst_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vtst_s32(a0, a1);
			}

			// Token: 0x06000237 RID: 567 RVA: 0x00005DFC File Offset: 0x00003FFC
			[DebuggerStepThrough]
			public static v128 vtstq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vtstq_s32(a0, a1);
			}

			// Token: 0x06000238 RID: 568 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000239 RID: 569 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023A RID: 570 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023B RID: 571 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023C RID: 572 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023D RID: 573 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023E RID: 574 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600023F RID: 575 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000240 RID: 576 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000241 RID: 577 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000242 RID: 578 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000243 RID: 579 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000244 RID: 580 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000245 RID: 581 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000246 RID: 582 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000247 RID: 583 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000248 RID: 584 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000249 RID: 585 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600024A RID: 586 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600024B RID: 587 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600024C RID: 588 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaba_s8(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600024D RID: 589 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabaq_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600024E RID: 590 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaba_s16(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600024F RID: 591 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabaq_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000250 RID: 592 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaba_s32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000251 RID: 593 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabaq_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000252 RID: 594 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaba_u8(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000253 RID: 595 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabaq_u8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000254 RID: 596 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaba_u16(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000255 RID: 597 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabaq_u16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000256 RID: 598 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vaba_u32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000257 RID: 599 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabaq_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000258 RID: 600 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_s8(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000259 RID: 601 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_s16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600025A RID: 602 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_s32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600025B RID: 603 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_u8(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600025C RID: 604 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_u16(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600025D RID: 605 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_u32(v128 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600025E RID: 606 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600025F RID: 607 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000260 RID: 608 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000261 RID: 609 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000262 RID: 610 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000263 RID: 611 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000264 RID: 612 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000265 RID: 613 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000266 RID: 614 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000267 RID: 615 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000268 RID: 616 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000269 RID: 617 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600026A RID: 618 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600026B RID: 619 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600026C RID: 620 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600026D RID: 621 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600026E RID: 622 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600026F RID: 623 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000270 RID: 624 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000271 RID: 625 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000272 RID: 626 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000273 RID: 627 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000274 RID: 628 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000275 RID: 629 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000276 RID: 630 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000277 RID: 631 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000278 RID: 632 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000279 RID: 633 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600027A RID: 634 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600027B RID: 635 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600027C RID: 636 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600027D RID: 637 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600027E RID: 638 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600027F RID: 639 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000280 RID: 640 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000281 RID: 641 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000282 RID: 642 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000283 RID: 643 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000284 RID: 644 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000285 RID: 645 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000286 RID: 646 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000287 RID: 647 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000288 RID: 648 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000289 RID: 649 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600028A RID: 650 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600028B RID: 651 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600028C RID: 652 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600028D RID: 653 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600028E RID: 654 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600028F RID: 655 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000290 RID: 656 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000291 RID: 657 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000292 RID: 658 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000293 RID: 659 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000294 RID: 660 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000295 RID: 661 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000296 RID: 662 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000297 RID: 663 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000298 RID: 664 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshl_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000299 RID: 665 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshlq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600029A RID: 666 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600029B RID: 667 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600029C RID: 668 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600029D RID: 669 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600029E RID: 670 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600029F RID: 671 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A0 RID: 672 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A1 RID: 673 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A2 RID: 674 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A3 RID: 675 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A4 RID: 676 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A5 RID: 677 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A6 RID: 678 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A7 RID: 679 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A8 RID: 680 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshl_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002A9 RID: 681 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshlq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002AA RID: 682 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002AB RID: 683 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002AC RID: 684 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002AD RID: 685 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002AE RID: 686 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002AF RID: 687 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B0 RID: 688 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B1 RID: 689 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B2 RID: 690 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B3 RID: 691 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B4 RID: 692 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B5 RID: 693 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B6 RID: 694 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B7 RID: 695 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B8 RID: 696 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshl_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002B9 RID: 697 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshlq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002BA RID: 698 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002BB RID: 699 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002BC RID: 700 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002BD RID: 701 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002BE RID: 702 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002BF RID: 703 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C0 RID: 704 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_s64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C1 RID: 705 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C2 RID: 706 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C3 RID: 707 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C4 RID: 708 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C5 RID: 709 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C6 RID: 710 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C7 RID: 711 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C8 RID: 712 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshr_n_u64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002C9 RID: 713 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrq_n_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002CA RID: 714 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002CB RID: 715 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002CC RID: 716 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002CD RID: 717 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002CE RID: 718 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002CF RID: 719 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D0 RID: 720 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_s64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D1 RID: 721 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D2 RID: 722 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D3 RID: 723 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D4 RID: 724 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D5 RID: 725 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D6 RID: 726 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D7 RID: 727 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D8 RID: 728 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshl_n_u64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002D9 RID: 729 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshlq_n_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002DA RID: 730 RVA: 0x00005E05 File Offset: 0x00004005
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_s8(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_s8(a0, new v64((sbyte)(-(sbyte)a1)));
			}

			// Token: 0x060002DB RID: 731 RVA: 0x00005E15 File Offset: 0x00004015
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_s8(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_s8(a0, new v128((sbyte)(-(sbyte)a1)));
			}

			// Token: 0x060002DC RID: 732 RVA: 0x00005E25 File Offset: 0x00004025
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_s16(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_s16(a0, new v64((short)(-(short)a1)));
			}

			// Token: 0x060002DD RID: 733 RVA: 0x00005E35 File Offset: 0x00004035
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_s16(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_s16(a0, new v128((short)(-(short)a1)));
			}

			// Token: 0x060002DE RID: 734 RVA: 0x00005E45 File Offset: 0x00004045
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_s32(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_s32(a0, new v64(-a1));
			}

			// Token: 0x060002DF RID: 735 RVA: 0x00005E54 File Offset: 0x00004054
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_s32(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_s32(a0, new v128(-a1));
			}

			// Token: 0x060002E0 RID: 736 RVA: 0x00005E63 File Offset: 0x00004063
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_s64(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_s64(a0, new v64((long)(-(long)a1)));
			}

			// Token: 0x060002E1 RID: 737 RVA: 0x00005E73 File Offset: 0x00004073
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_s64(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_s64(a0, new v128((long)(-(long)a1)));
			}

			// Token: 0x060002E2 RID: 738 RVA: 0x00005E83 File Offset: 0x00004083
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_u8(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_u8(a0, new v64((byte)(-(byte)a1)));
			}

			// Token: 0x060002E3 RID: 739 RVA: 0x00005E93 File Offset: 0x00004093
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_u8(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_u8(a0, new v128((byte)(-(byte)a1)));
			}

			// Token: 0x060002E4 RID: 740 RVA: 0x00005EA3 File Offset: 0x000040A3
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_u16(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_u16(a0, new v64((ushort)(-(ushort)a1)));
			}

			// Token: 0x060002E5 RID: 741 RVA: 0x00005EB3 File Offset: 0x000040B3
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_u16(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_u16(a0, new v128((ushort)(-(ushort)a1)));
			}

			// Token: 0x060002E6 RID: 742 RVA: 0x00005EC3 File Offset: 0x000040C3
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_u32(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_u32(a0, new v64(-a1));
			}

			// Token: 0x060002E7 RID: 743 RVA: 0x00005ED2 File Offset: 0x000040D2
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_u32(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_u32(a0, new v128(-a1));
			}

			// Token: 0x060002E8 RID: 744 RVA: 0x00005EE1 File Offset: 0x000040E1
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrshr_n_u64(v64 a0, int a1)
			{
				return Arm.Neon.vrshl_u64(a0, new v64((ulong)((long)(-(long)a1))));
			}

			// Token: 0x060002E9 RID: 745 RVA: 0x00005EF1 File Offset: 0x000040F1
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrshrq_n_u64(v128 a0, int a1)
			{
				return Arm.Neon.vrshlq_u64(a0, new v128((ulong)((long)(-(long)a1))));
			}

			// Token: 0x060002EA RID: 746 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_s8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002EB RID: 747 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_s8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002EC RID: 748 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002ED RID: 749 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002EE RID: 750 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002EF RID: 751 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F0 RID: 752 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_s64(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F1 RID: 753 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_s64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F2 RID: 754 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_u8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F3 RID: 755 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_u8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F4 RID: 756 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_u16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F5 RID: 757 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_u16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F6 RID: 758 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_u32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F7 RID: 759 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_u32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F8 RID: 760 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsra_n_u64(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002F9 RID: 761 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsraq_n_u64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060002FA RID: 762 RVA: 0x00005F01 File Offset: 0x00004101
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_s8(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_s8(a0, Arm.Neon.vrshr_n_s8(a1, a2));
			}

			// Token: 0x060002FB RID: 763 RVA: 0x00005F10 File Offset: 0x00004110
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_s8(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_s8(a0, Arm.Neon.vrshrq_n_s8(a1, a2));
			}

			// Token: 0x060002FC RID: 764 RVA: 0x00005F1F File Offset: 0x0000411F
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_s16(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_s16(a0, Arm.Neon.vrshr_n_s16(a1, a2));
			}

			// Token: 0x060002FD RID: 765 RVA: 0x00005F2E File Offset: 0x0000412E
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_s16(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_s16(a0, Arm.Neon.vrshrq_n_s16(a1, a2));
			}

			// Token: 0x060002FE RID: 766 RVA: 0x00005F3D File Offset: 0x0000413D
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_s32(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_s32(a0, Arm.Neon.vrshr_n_s32(a1, a2));
			}

			// Token: 0x060002FF RID: 767 RVA: 0x00005F4C File Offset: 0x0000414C
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_s32(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_s32(a0, Arm.Neon.vrshrq_n_s32(a1, a2));
			}

			// Token: 0x06000300 RID: 768 RVA: 0x00005F5B File Offset: 0x0000415B
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_s64(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_s64(a0, Arm.Neon.vrshr_n_s64(a1, a2));
			}

			// Token: 0x06000301 RID: 769 RVA: 0x00005F6A File Offset: 0x0000416A
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_s64(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_s64(a0, Arm.Neon.vrshrq_n_s64(a1, a2));
			}

			// Token: 0x06000302 RID: 770 RVA: 0x00005F79 File Offset: 0x00004179
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_u8(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_u8(a0, Arm.Neon.vrshr_n_u8(a1, a2));
			}

			// Token: 0x06000303 RID: 771 RVA: 0x00005F88 File Offset: 0x00004188
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_u8(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_u8(a0, Arm.Neon.vrshrq_n_u8(a1, a2));
			}

			// Token: 0x06000304 RID: 772 RVA: 0x00005F97 File Offset: 0x00004197
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_u16(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_u16(a0, Arm.Neon.vrshr_n_u16(a1, a2));
			}

			// Token: 0x06000305 RID: 773 RVA: 0x00005FA6 File Offset: 0x000041A6
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_u16(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_u16(a0, Arm.Neon.vrshrq_n_u16(a1, a2));
			}

			// Token: 0x06000306 RID: 774 RVA: 0x00005FB5 File Offset: 0x000041B5
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_u32(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_u32(a0, Arm.Neon.vrshr_n_u32(a1, a2));
			}

			// Token: 0x06000307 RID: 775 RVA: 0x00005FC4 File Offset: 0x000041C4
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_u32(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_u32(a0, Arm.Neon.vrshrq_n_u32(a1, a2));
			}

			// Token: 0x06000308 RID: 776 RVA: 0x00005FD3 File Offset: 0x000041D3
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vrsra_n_u64(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vadd_u64(a0, Arm.Neon.vrshr_n_u64(a1, a2));
			}

			// Token: 0x06000309 RID: 777 RVA: 0x00005FE2 File Offset: 0x000041E2
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vrsraq_n_u64(v128 a0, v128 a1, int a2)
			{
				return Arm.Neon.vaddq_u64(a0, Arm.Neon.vrshrq_n_u64(a1, a2));
			}

			// Token: 0x0600030A RID: 778 RVA: 0x00005FF1 File Offset: 0x000041F1
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_s8(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_s8(a0, new v64((sbyte)a1));
			}

			// Token: 0x0600030B RID: 779 RVA: 0x00006000 File Offset: 0x00004200
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_s8(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_s8(a0, new v128((sbyte)a1));
			}

			// Token: 0x0600030C RID: 780 RVA: 0x0000600F File Offset: 0x0000420F
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_s16(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_s16(a0, new v64((short)a1));
			}

			// Token: 0x0600030D RID: 781 RVA: 0x0000601E File Offset: 0x0000421E
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_s16(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_s16(a0, new v128((short)a1));
			}

			// Token: 0x0600030E RID: 782 RVA: 0x0000602D File Offset: 0x0000422D
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_s32(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_s32(a0, new v64(a1));
			}

			// Token: 0x0600030F RID: 783 RVA: 0x0000603B File Offset: 0x0000423B
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_s32(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_s32(a0, new v128(a1));
			}

			// Token: 0x06000310 RID: 784 RVA: 0x00006049 File Offset: 0x00004249
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_s64(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_s64(a0, new v64((long)a1));
			}

			// Token: 0x06000311 RID: 785 RVA: 0x00006058 File Offset: 0x00004258
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_s64(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_s64(a0, new v128((long)a1));
			}

			// Token: 0x06000312 RID: 786 RVA: 0x00006067 File Offset: 0x00004267
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_u8(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_u8(a0, new v64((byte)a1));
			}

			// Token: 0x06000313 RID: 787 RVA: 0x00006076 File Offset: 0x00004276
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_u8(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_u8(a0, new v128((byte)a1));
			}

			// Token: 0x06000314 RID: 788 RVA: 0x00006085 File Offset: 0x00004285
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_u16(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_u16(a0, new v64((ushort)a1));
			}

			// Token: 0x06000315 RID: 789 RVA: 0x00006094 File Offset: 0x00004294
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_u16(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_u16(a0, new v128((ushort)a1));
			}

			// Token: 0x06000316 RID: 790 RVA: 0x000060A3 File Offset: 0x000042A3
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_u32(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_u32(a0, new v64((uint)a1));
			}

			// Token: 0x06000317 RID: 791 RVA: 0x000060B1 File Offset: 0x000042B1
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_u32(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_u32(a0, new v128((uint)a1));
			}

			// Token: 0x06000318 RID: 792 RVA: 0x000060BF File Offset: 0x000042BF
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v64 vqshl_n_u64(v64 a0, int a1)
			{
				return Arm.Neon.vqshl_u64(a0, new v64((ulong)((long)a1)));
			}

			// Token: 0x06000319 RID: 793 RVA: 0x000060CE File Offset: 0x000042CE
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV7A_NEON32)]
			public static v128 vqshlq_n_u64(v128 a0, int a1)
			{
				return Arm.Neon.vqshlq_u64(a0, new v128((ulong)((long)a1)));
			}

			// Token: 0x0600031A RID: 794 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshlu_n_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600031B RID: 795 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshluq_n_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600031C RID: 796 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshlu_n_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600031D RID: 797 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshluq_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600031E RID: 798 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshlu_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600031F RID: 799 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshluq_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000320 RID: 800 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshlu_n_s64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000321 RID: 801 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshluq_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000322 RID: 802 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshrn_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000323 RID: 803 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshrn_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000324 RID: 804 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshrn_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000325 RID: 805 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshrn_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000326 RID: 806 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshrn_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000327 RID: 807 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vshrn_n_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000328 RID: 808 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrun_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000329 RID: 809 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrun_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600032A RID: 810 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrun_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600032B RID: 811 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrun_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600032C RID: 812 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrun_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600032D RID: 813 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrun_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600032E RID: 814 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrn_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600032F RID: 815 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrn_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000330 RID: 816 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrn_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000331 RID: 817 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrn_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000332 RID: 818 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrn_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000333 RID: 819 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqshrn_n_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000334 RID: 820 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshrn_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000335 RID: 821 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshrn_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000336 RID: 822 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshrn_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000337 RID: 823 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshrn_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000338 RID: 824 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshrn_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000339 RID: 825 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrshrn_n_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600033A RID: 826 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrn_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600033B RID: 827 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrn_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600033C RID: 828 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrn_n_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600033D RID: 829 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrn_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600033E RID: 830 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrn_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600033F RID: 831 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrshrn_n_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000340 RID: 832 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_n_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000341 RID: 833 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_n_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000342 RID: 834 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000343 RID: 835 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_n_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000344 RID: 836 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_n_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000345 RID: 837 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_n_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000346 RID: 838 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_s8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000347 RID: 839 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_s8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000348 RID: 840 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000349 RID: 841 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600034A RID: 842 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600034B RID: 843 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600034C RID: 844 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_s64(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600034D RID: 845 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_s64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600034E RID: 846 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_u8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600034F RID: 847 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_u8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000350 RID: 848 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_u16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000351 RID: 849 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_u16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000352 RID: 850 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_u32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000353 RID: 851 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_u32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000354 RID: 852 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsri_n_u64(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000355 RID: 853 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsriq_n_u64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000356 RID: 854 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_s8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000357 RID: 855 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_s8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000358 RID: 856 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000359 RID: 857 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600035A RID: 858 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600035B RID: 859 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600035C RID: 860 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_s64(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600035D RID: 861 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_s64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600035E RID: 862 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_u8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600035F RID: 863 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_u8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000360 RID: 864 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_u16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000361 RID: 865 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_u16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000362 RID: 866 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_u32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000363 RID: 867 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_u32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000364 RID: 868 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsli_n_u64(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000365 RID: 869 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsliq_n_u64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000366 RID: 870 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_s32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000367 RID: 871 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_s32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000368 RID: 872 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_u32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000369 RID: 873 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_u32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600036A RID: 874 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_s32_f32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600036B RID: 875 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_s32_f32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600036C RID: 876 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_u32_f32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600036D RID: 877 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_u32_f32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600036E RID: 878 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_f32_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600036F RID: 879 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_f32_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000370 RID: 880 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_f32_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000371 RID: 881 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_f32_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000372 RID: 882 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_f32_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000373 RID: 883 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_f32_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000374 RID: 884 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_f32_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000375 RID: 885 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_f32_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000376 RID: 886 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmovn_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000377 RID: 887 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmovn_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000378 RID: 888 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmovn_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000379 RID: 889 RVA: 0x000060DD File Offset: 0x000042DD
			[DebuggerStepThrough]
			public static v64 vmovn_u16(v128 a0)
			{
				return Arm.Neon.vmovn_s16(a0);
			}

			// Token: 0x0600037A RID: 890 RVA: 0x000060E5 File Offset: 0x000042E5
			[DebuggerStepThrough]
			public static v64 vmovn_u32(v128 a0)
			{
				return Arm.Neon.vmovn_s32(a0);
			}

			// Token: 0x0600037B RID: 891 RVA: 0x000060ED File Offset: 0x000042ED
			[DebuggerStepThrough]
			public static v64 vmovn_u64(v128 a0)
			{
				return Arm.Neon.vmovn_s64(a0);
			}

			// Token: 0x0600037C RID: 892 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovn_high_s16(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600037D RID: 893 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovn_high_s32(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600037E RID: 894 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovn_high_s64(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600037F RID: 895 RVA: 0x000060F5 File Offset: 0x000042F5
			[DebuggerStepThrough]
			public static v128 vmovn_high_u16(v64 a0, v128 a1)
			{
				return Arm.Neon.vmovn_high_s16(a0, a1);
			}

			// Token: 0x06000380 RID: 896 RVA: 0x000060FE File Offset: 0x000042FE
			[DebuggerStepThrough]
			public static v128 vmovn_high_u32(v64 a0, v128 a1)
			{
				return Arm.Neon.vmovn_high_s32(a0, a1);
			}

			// Token: 0x06000381 RID: 897 RVA: 0x00006107 File Offset: 0x00004307
			[DebuggerStepThrough]
			public static v128 vmovn_high_u64(v64 a0, v128 a1)
			{
				return Arm.Neon.vmovn_high_s64(a0, a1);
			}

			// Token: 0x06000382 RID: 898 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000383 RID: 899 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000384 RID: 900 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000385 RID: 901 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_u8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000386 RID: 902 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_u16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000387 RID: 903 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000388 RID: 904 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovn_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000389 RID: 905 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovn_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600038A RID: 906 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovn_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600038B RID: 907 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovn_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600038C RID: 908 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovn_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600038D RID: 909 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovn_u64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600038E RID: 910 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovun_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600038F RID: 911 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovun_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000390 RID: 912 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqmovun_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000391 RID: 913 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_lane_s16(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000392 RID: 914 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000393 RID: 915 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_lane_s32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000394 RID: 916 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000395 RID: 917 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_lane_u16(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000396 RID: 918 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_lane_u16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000397 RID: 919 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_lane_u32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000398 RID: 920 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_lane_u32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000399 RID: 921 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_lane_f32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600039A RID: 922 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_lane_f32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600039B RID: 923 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_lane_s16(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600039C RID: 924 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_lane_s32(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600039D RID: 925 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_lane_u16(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600039E RID: 926 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_lane_u32(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600039F RID: 927 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_lane_s16(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A0 RID: 928 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_lane_s32(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A1 RID: 929 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_lane_s16(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A2 RID: 930 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A3 RID: 931 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_lane_s32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A5 RID: 933 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_lane_u16(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A6 RID: 934 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_lane_u16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A7 RID: 935 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_lane_u32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A8 RID: 936 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_lane_u32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003A9 RID: 937 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_lane_f32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003AA RID: 938 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_lane_f32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003AB RID: 939 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_lane_s16(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003AC RID: 940 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_lane_s32(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003AD RID: 941 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_lane_u16(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003AE RID: 942 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_lane_u32(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003AF RID: 943 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_lane_s16(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B0 RID: 944 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_lane_s32(v128 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B1 RID: 945 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_n_s16(v64 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B2 RID: 946 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_n_s16(v128 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B3 RID: 947 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B4 RID: 948 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B5 RID: 949 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_n_u16(v64 a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B6 RID: 950 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_n_u16(v128 a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B7 RID: 951 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_n_u32(v64 a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B8 RID: 952 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_n_u32(v128 a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003B9 RID: 953 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_n_f32(v64 a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003BA RID: 954 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_n_f32(v128 a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003BB RID: 955 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_lane_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003BC RID: 956 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_lane_s16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003BD RID: 957 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_lane_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003BE RID: 958 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_lane_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003BF RID: 959 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_lane_u16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C0 RID: 960 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_lane_u16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C1 RID: 961 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_lane_u32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C2 RID: 962 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_lane_u32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C3 RID: 963 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_lane_f32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C4 RID: 964 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_lane_f32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C5 RID: 965 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_n_s16(v64 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C6 RID: 966 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C7 RID: 967 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_n_u16(v64 a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C8 RID: 968 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_n_u32(v64 a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003C9 RID: 969 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_lane_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003CA RID: 970 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_lane_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003CB RID: 971 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_lane_u16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003CC RID: 972 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_lane_u32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003CD RID: 973 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_n_s16(v64 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003CE RID: 974 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003CF RID: 975 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_lane_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D0 RID: 976 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_lane_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D1 RID: 977 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_n_s16(v64 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D2 RID: 978 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_n_s16(v128 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D3 RID: 979 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D4 RID: 980 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D5 RID: 981 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_lane_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D6 RID: 982 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_lane_s16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D7 RID: 983 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_lane_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D8 RID: 984 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_lane_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003D9 RID: 985 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_n_s16(v64 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003DA RID: 986 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_n_s16(v128 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003DB RID: 987 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_n_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003DC RID: 988 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003DD RID: 989 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_lane_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003DE RID: 990 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_lane_s16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003DF RID: 991 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_lane_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E0 RID: 992 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_lane_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E1 RID: 993 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_n_s16(v64 a0, v64 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E2 RID: 994 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_n_s16(v128 a0, v128 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E3 RID: 995 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_n_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E4 RID: 996 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E5 RID: 997 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_n_u16(v64 a0, v64 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E6 RID: 998 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_n_u16(v128 a0, v128 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E7 RID: 999 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_n_u32(v64 a0, v64 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E8 RID: 1000 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_n_u32(v128 a0, v128 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003E9 RID: 1001 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_n_f32(v64 a0, v64 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003EA RID: 1002 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_n_f32(v128 a0, v128 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003EB RID: 1003 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_n_s16(v128 a0, v64 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003EC RID: 1004 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_n_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003ED RID: 1005 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_n_u16(v128 a0, v64 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003EE RID: 1006 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_n_u32(v128 a0, v64 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003EF RID: 1007 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_n_s16(v128 a0, v64 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F0 RID: 1008 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_n_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F1 RID: 1009 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_n_s16(v64 a0, v64 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F2 RID: 1010 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_n_s16(v128 a0, v128 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F3 RID: 1011 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_n_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F4 RID: 1012 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F5 RID: 1013 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_n_u16(v64 a0, v64 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F6 RID: 1014 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_n_u16(v128 a0, v128 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_n_u32(v64 a0, v64 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_n_u32(v128 a0, v128 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003F9 RID: 1017 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_n_f32(v64 a0, v64 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003FA RID: 1018 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_n_f32(v128 a0, v128 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003FB RID: 1019 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_n_s16(v128 a0, v64 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003FC RID: 1020 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_n_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003FD RID: 1021 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_n_u16(v128 a0, v64 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003FE RID: 1022 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_n_u32(v128 a0, v64 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060003FF RID: 1023 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_n_s16(v128 a0, v64 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000400 RID: 1024 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_n_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000401 RID: 1025 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabs_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000402 RID: 1026 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabsq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000403 RID: 1027 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabs_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000404 RID: 1028 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabsq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000405 RID: 1029 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabs_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000406 RID: 1030 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabsq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000407 RID: 1031 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabs_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000408 RID: 1032 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabsq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000409 RID: 1033 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqabs_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600040A RID: 1034 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqabsq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600040B RID: 1035 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqabs_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600040C RID: 1036 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqabsq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600040D RID: 1037 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqabs_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600040E RID: 1038 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqabsq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600040F RID: 1039 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vneg_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000410 RID: 1040 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vnegq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000411 RID: 1041 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vneg_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000412 RID: 1042 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vnegq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000413 RID: 1043 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vneg_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000414 RID: 1044 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vnegq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000415 RID: 1045 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vneg_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000416 RID: 1046 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vnegq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000417 RID: 1047 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqneg_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000418 RID: 1048 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqnegq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000419 RID: 1049 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqneg_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600041A RID: 1050 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqnegq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600041B RID: 1051 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqneg_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600041C RID: 1052 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqnegq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600041D RID: 1053 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcls_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600041E RID: 1054 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclsq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600041F RID: 1055 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcls_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000420 RID: 1056 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclsq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000421 RID: 1057 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcls_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000422 RID: 1058 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclsq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000423 RID: 1059 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclz_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000424 RID: 1060 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclzq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000425 RID: 1061 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclz_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000426 RID: 1062 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclzq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000427 RID: 1063 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclz_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000428 RID: 1064 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclzq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000429 RID: 1065 RVA: 0x00006110 File Offset: 0x00004310
			[DebuggerStepThrough]
			public static v64 vclz_u8(v64 a0)
			{
				return Arm.Neon.vclz_s8(a0);
			}

			// Token: 0x0600042A RID: 1066 RVA: 0x00006118 File Offset: 0x00004318
			[DebuggerStepThrough]
			public static v128 vclzq_u8(v128 a0)
			{
				return Arm.Neon.vclzq_s8(a0);
			}

			// Token: 0x0600042B RID: 1067 RVA: 0x00006120 File Offset: 0x00004320
			[DebuggerStepThrough]
			public static v64 vclz_u16(v64 a0)
			{
				return Arm.Neon.vclz_s16(a0);
			}

			// Token: 0x0600042C RID: 1068 RVA: 0x00006128 File Offset: 0x00004328
			[DebuggerStepThrough]
			public static v128 vclzq_u16(v128 a0)
			{
				return Arm.Neon.vclzq_s16(a0);
			}

			// Token: 0x0600042D RID: 1069 RVA: 0x00006130 File Offset: 0x00004330
			[DebuggerStepThrough]
			public static v64 vclz_u32(v64 a0)
			{
				return Arm.Neon.vclz_s32(a0);
			}

			// Token: 0x0600042E RID: 1070 RVA: 0x00006138 File Offset: 0x00004338
			[DebuggerStepThrough]
			public static v128 vclzq_u32(v128 a0)
			{
				return Arm.Neon.vclzq_s32(a0);
			}

			// Token: 0x0600042F RID: 1071 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcnt_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000430 RID: 1072 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcntq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000431 RID: 1073 RVA: 0x00006140 File Offset: 0x00004340
			[DebuggerStepThrough]
			public static v64 vcnt_u8(v64 a0)
			{
				return Arm.Neon.vcnt_s8(a0);
			}

			// Token: 0x06000432 RID: 1074 RVA: 0x00006148 File Offset: 0x00004348
			[DebuggerStepThrough]
			public static v128 vcntq_u8(v128 a0)
			{
				return Arm.Neon.vcntq_s8(a0);
			}

			// Token: 0x06000433 RID: 1075 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrecpe_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000434 RID: 1076 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrecpeq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000435 RID: 1077 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrecpe_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000436 RID: 1078 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrecpeq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000437 RID: 1079 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrecps_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000438 RID: 1080 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrecpsq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000439 RID: 1081 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsqrte_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600043A RID: 1082 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsqrteq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600043B RID: 1083 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsqrte_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600043C RID: 1084 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsqrteq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600043D RID: 1085 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsqrts_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600043E RID: 1086 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsqrtsq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600043F RID: 1087 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmvn_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000440 RID: 1088 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmvnq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x00006150 File Offset: 0x00004350
			[DebuggerStepThrough]
			public static v64 vmvn_s16(v64 a0)
			{
				return Arm.Neon.vmvn_s8(a0);
			}

			// Token: 0x06000442 RID: 1090 RVA: 0x00006158 File Offset: 0x00004358
			[DebuggerStepThrough]
			public static v128 vmvnq_s16(v128 a0)
			{
				return Arm.Neon.vmvnq_s8(a0);
			}

			// Token: 0x06000443 RID: 1091 RVA: 0x00006150 File Offset: 0x00004350
			[DebuggerStepThrough]
			public static v64 vmvn_s32(v64 a0)
			{
				return Arm.Neon.vmvn_s8(a0);
			}

			// Token: 0x06000444 RID: 1092 RVA: 0x00006158 File Offset: 0x00004358
			[DebuggerStepThrough]
			public static v128 vmvnq_s32(v128 a0)
			{
				return Arm.Neon.vmvnq_s8(a0);
			}

			// Token: 0x06000445 RID: 1093 RVA: 0x00006150 File Offset: 0x00004350
			[DebuggerStepThrough]
			public static v64 vmvn_u8(v64 a0)
			{
				return Arm.Neon.vmvn_s8(a0);
			}

			// Token: 0x06000446 RID: 1094 RVA: 0x00006158 File Offset: 0x00004358
			[DebuggerStepThrough]
			public static v128 vmvnq_u8(v128 a0)
			{
				return Arm.Neon.vmvnq_s8(a0);
			}

			// Token: 0x06000447 RID: 1095 RVA: 0x00006150 File Offset: 0x00004350
			[DebuggerStepThrough]
			public static v64 vmvn_u16(v64 a0)
			{
				return Arm.Neon.vmvn_s8(a0);
			}

			// Token: 0x06000448 RID: 1096 RVA: 0x00006158 File Offset: 0x00004358
			[DebuggerStepThrough]
			public static v128 vmvnq_u16(v128 a0)
			{
				return Arm.Neon.vmvnq_s8(a0);
			}

			// Token: 0x06000449 RID: 1097 RVA: 0x00006150 File Offset: 0x00004350
			[DebuggerStepThrough]
			public static v64 vmvn_u32(v64 a0)
			{
				return Arm.Neon.vmvn_s8(a0);
			}

			// Token: 0x0600044A RID: 1098 RVA: 0x00006158 File Offset: 0x00004358
			[DebuggerStepThrough]
			public static v128 vmvnq_u32(v128 a0)
			{
				return Arm.Neon.vmvnq_s8(a0);
			}

			// Token: 0x0600044B RID: 1099 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vand_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600044C RID: 1100 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vandq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600044D RID: 1101 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_s16(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x0600044E RID: 1102 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_s16(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x0600044F RID: 1103 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_s32(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x06000450 RID: 1104 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_s32(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x06000451 RID: 1105 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_s64(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x06000452 RID: 1106 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_s64(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x06000453 RID: 1107 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x06000454 RID: 1108 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x06000455 RID: 1109 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x06000456 RID: 1110 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x06000457 RID: 1111 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x06000458 RID: 1112 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x06000459 RID: 1113 RVA: 0x00006160 File Offset: 0x00004360
			[DebuggerStepThrough]
			public static v64 vand_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vand_s8(a0, a1);
			}

			// Token: 0x0600045A RID: 1114 RVA: 0x00006169 File Offset: 0x00004369
			[DebuggerStepThrough]
			public static v128 vandq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vandq_s8(a0, a1);
			}

			// Token: 0x0600045B RID: 1115 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vorr_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600045C RID: 1116 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vorrq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600045D RID: 1117 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_s16(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x0600045E RID: 1118 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_s16(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x0600045F RID: 1119 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_s32(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x06000460 RID: 1120 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_s32(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x06000461 RID: 1121 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_s64(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x06000462 RID: 1122 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_s64(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x06000463 RID: 1123 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x06000464 RID: 1124 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x06000465 RID: 1125 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x06000466 RID: 1126 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x06000467 RID: 1127 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x06000468 RID: 1128 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x06000469 RID: 1129 RVA: 0x00006172 File Offset: 0x00004372
			[DebuggerStepThrough]
			public static v64 vorr_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vorr_s8(a0, a1);
			}

			// Token: 0x0600046A RID: 1130 RVA: 0x0000617B File Offset: 0x0000437B
			[DebuggerStepThrough]
			public static v128 vorrq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vorrq_s8(a0, a1);
			}

			// Token: 0x0600046B RID: 1131 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 veor_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 veorq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600046D RID: 1133 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_s16(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x0600046E RID: 1134 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_s16(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x0600046F RID: 1135 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_s32(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x06000470 RID: 1136 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_s32(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x06000471 RID: 1137 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_s64(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x06000472 RID: 1138 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_s64(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x06000473 RID: 1139 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x06000474 RID: 1140 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x06000477 RID: 1143 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x06000479 RID: 1145 RVA: 0x00006184 File Offset: 0x00004384
			[DebuggerStepThrough]
			public static v64 veor_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.veor_s8(a0, a1);
			}

			// Token: 0x0600047A RID: 1146 RVA: 0x0000618D File Offset: 0x0000438D
			[DebuggerStepThrough]
			public static v128 veorq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.veorq_s8(a0, a1);
			}

			// Token: 0x0600047B RID: 1147 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vbic_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600047C RID: 1148 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vbicq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600047D RID: 1149 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_s16(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x0600047E RID: 1150 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_s16(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x0600047F RID: 1151 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_s32(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x06000480 RID: 1152 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_s32(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x06000481 RID: 1153 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_s64(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x06000482 RID: 1154 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_s64(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x06000483 RID: 1155 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x06000484 RID: 1156 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x06000485 RID: 1157 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x06000487 RID: 1159 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x06000489 RID: 1161 RVA: 0x00006196 File Offset: 0x00004396
			[DebuggerStepThrough]
			public static v64 vbic_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vbic_s8(a0, a1);
			}

			// Token: 0x0600048A RID: 1162 RVA: 0x0000619F File Offset: 0x0000439F
			[DebuggerStepThrough]
			public static v128 vbicq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vbicq_s8(a0, a1);
			}

			// Token: 0x0600048B RID: 1163 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vorn_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600048C RID: 1164 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vornq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600048D RID: 1165 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_s16(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x0600048E RID: 1166 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_s16(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x0600048F RID: 1167 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_s32(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x06000490 RID: 1168 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_s32(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_s64(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_s64(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x06000493 RID: 1171 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x06000494 RID: 1172 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x06000495 RID: 1173 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x06000497 RID: 1175 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x06000499 RID: 1177 RVA: 0x000061A8 File Offset: 0x000043A8
			[DebuggerStepThrough]
			public static v64 vorn_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vorn_s8(a0, a1);
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x000061B1 File Offset: 0x000043B1
			[DebuggerStepThrough]
			public static v128 vornq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vornq_s8(a0, a1);
			}

			// Token: 0x0600049B RID: 1179 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vbsl_s8(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vbslq_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600049D RID: 1181 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_s16(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x0600049E RID: 1182 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_s16(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x0600049F RID: 1183 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_s32(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_s32(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004A1 RID: 1185 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_s64(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_s64(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004A3 RID: 1187 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_u8(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004A4 RID: 1188 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_u8(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_u16(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004A6 RID: 1190 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_u16(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004A7 RID: 1191 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_u32(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_u32(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_u64(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004AA RID: 1194 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_u64(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004AB RID: 1195 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_f32(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x060004AC RID: 1196 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_f32(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x060004AD RID: 1197 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004AE RID: 1198 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004AF RID: 1199 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B0 RID: 1200 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B1 RID: 1201 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B2 RID: 1202 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B3 RID: 1203 RVA: 0x000061CE File Offset: 0x000043CE
			[DebuggerStepThrough]
			public static v64 vdup_lane_s64(v64 a0, int a1)
			{
				return a0;
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x000061D1 File Offset: 0x000043D1
			[DebuggerStepThrough]
			public static v128 vdupq_lane_s64(v64 a0, int a1)
			{
				return new v128(a0, a0);
			}

			// Token: 0x060004B5 RID: 1205 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B7 RID: 1207 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B8 RID: 1208 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004B9 RID: 1209 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004BA RID: 1210 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004BB RID: 1211 RVA: 0x000061CE File Offset: 0x000043CE
			[DebuggerStepThrough]
			public static v64 vdup_lane_u64(v64 a0, int a1)
			{
				return a0;
			}

			// Token: 0x060004BC RID: 1212 RVA: 0x000061D1 File Offset: 0x000043D1
			[DebuggerStepThrough]
			public static v128 vdupq_lane_u64(v64 a0, int a1)
			{
				return new v128(a0, a0);
			}

			// Token: 0x060004BD RID: 1213 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_lane_f32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_lane_f32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004BF RID: 1215 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C0 RID: 1216 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C1 RID: 1217 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x000061DA File Offset: 0x000043DA
			[DebuggerStepThrough]
			public static v64 vpadd_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vpadd_s8(a0, a1);
			}

			// Token: 0x060004C3 RID: 1219 RVA: 0x000061E3 File Offset: 0x000043E3
			[DebuggerStepThrough]
			public static v64 vpadd_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vpadd_s16(a0, a1);
			}

			// Token: 0x060004C4 RID: 1220 RVA: 0x000061EC File Offset: 0x000043EC
			[DebuggerStepThrough]
			public static v64 vpadd_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vpadd_s32(a0, a1);
			}

			// Token: 0x060004C5 RID: 1221 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadd_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C6 RID: 1222 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpaddl_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C7 RID: 1223 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddlq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpaddl_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004C9 RID: 1225 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddlq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpaddl_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004CB RID: 1227 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddlq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004CC RID: 1228 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpaddl_u8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004CD RID: 1229 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddlq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004CE RID: 1230 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpaddl_u16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004CF RID: 1231 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddlq_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D0 RID: 1232 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpaddl_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D1 RID: 1233 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddlq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D2 RID: 1234 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadal_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D3 RID: 1235 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpadalq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D4 RID: 1236 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadal_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D5 RID: 1237 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpadalq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadal_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D7 RID: 1239 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpadalq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D8 RID: 1240 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadal_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004D9 RID: 1241 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpadalq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadal_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004DB RID: 1243 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpadalq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpadal_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004DD RID: 1245 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpadalq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E1 RID: 1249 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E2 RID: 1250 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E3 RID: 1251 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmax_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E5 RID: 1253 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E6 RID: 1254 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E7 RID: 1255 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E8 RID: 1256 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004E9 RID: 1257 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004EA RID: 1258 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004EB RID: 1259 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmin_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004EC RID: 1260 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_s8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004ED RID: 1261 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_s8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_s16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004EF RID: 1263 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F0 RID: 1264 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_s32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F1 RID: 1265 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F2 RID: 1266 RVA: 0x000061CE File Offset: 0x000043CE
			[DebuggerStepThrough]
			public static v64 vext_s64(v64 a0, v64 a1, int a2)
			{
				return a0;
			}

			// Token: 0x060004F3 RID: 1267 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_s64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F4 RID: 1268 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_u8(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F5 RID: 1269 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_u8(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F6 RID: 1270 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_u16(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F7 RID: 1271 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_u16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F8 RID: 1272 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_u32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004F9 RID: 1273 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_u32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004FA RID: 1274 RVA: 0x000061CE File Offset: 0x000043CE
			[DebuggerStepThrough]
			public static v64 vext_u64(v64 a0, v64 a1, int a2)
			{
				return a0;
			}

			// Token: 0x060004FB RID: 1275 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_u64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vext_f32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004FD RID: 1277 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_f32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004FE RID: 1278 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrev64_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060004FF RID: 1279 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrev64q_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000500 RID: 1280 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrev64_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000501 RID: 1281 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrev64q_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000502 RID: 1282 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrev64_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000503 RID: 1283 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrev64q_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x000061F5 File Offset: 0x000043F5
			[DebuggerStepThrough]
			public static v64 vrev64_u8(v64 a0)
			{
				return Arm.Neon.vrev64_s8(a0);
			}

			// Token: 0x06000505 RID: 1285 RVA: 0x000061FD File Offset: 0x000043FD
			[DebuggerStepThrough]
			public static v128 vrev64q_u8(v128 a0)
			{
				return Arm.Neon.vrev64q_s8(a0);
			}

			// Token: 0x06000506 RID: 1286 RVA: 0x00006205 File Offset: 0x00004405
			[DebuggerStepThrough]
			public static v64 vrev64_u16(v64 a0)
			{
				return Arm.Neon.vrev64_s16(a0);
			}

			// Token: 0x06000507 RID: 1287 RVA: 0x0000620D File Offset: 0x0000440D
			[DebuggerStepThrough]
			public static v128 vrev64q_u16(v128 a0)
			{
				return Arm.Neon.vrev64q_s16(a0);
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x00006215 File Offset: 0x00004415
			[DebuggerStepThrough]
			public static v64 vrev64_u32(v64 a0)
			{
				return Arm.Neon.vrev64_s32(a0);
			}

			// Token: 0x06000509 RID: 1289 RVA: 0x0000621D File Offset: 0x0000441D
			[DebuggerStepThrough]
			public static v128 vrev64q_u32(v128 a0)
			{
				return Arm.Neon.vrev64q_s32(a0);
			}

			// Token: 0x0600050A RID: 1290 RVA: 0x00006215 File Offset: 0x00004415
			[DebuggerStepThrough]
			public static v64 vrev64_f32(v64 a0)
			{
				return Arm.Neon.vrev64_s32(a0);
			}

			// Token: 0x0600050B RID: 1291 RVA: 0x0000621D File Offset: 0x0000441D
			[DebuggerStepThrough]
			public static v128 vrev64q_f32(v128 a0)
			{
				return Arm.Neon.vrev64q_s32(a0);
			}

			// Token: 0x0600050C RID: 1292 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrev32_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600050D RID: 1293 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrev32q_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600050E RID: 1294 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrev32_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600050F RID: 1295 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrev32q_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000510 RID: 1296 RVA: 0x00006225 File Offset: 0x00004425
			[DebuggerStepThrough]
			public static v64 vrev32_u8(v64 a0)
			{
				return Arm.Neon.vrev32_s8(a0);
			}

			// Token: 0x06000511 RID: 1297 RVA: 0x0000622D File Offset: 0x0000442D
			[DebuggerStepThrough]
			public static v128 vrev32q_u8(v128 a0)
			{
				return Arm.Neon.vrev32q_s8(a0);
			}

			// Token: 0x06000512 RID: 1298 RVA: 0x00006235 File Offset: 0x00004435
			[DebuggerStepThrough]
			public static v64 vrev32_u16(v64 a0)
			{
				return Arm.Neon.vrev32_s16(a0);
			}

			// Token: 0x06000513 RID: 1299 RVA: 0x0000623D File Offset: 0x0000443D
			[DebuggerStepThrough]
			public static v128 vrev32q_u16(v128 a0)
			{
				return Arm.Neon.vrev32q_s16(a0);
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrev16_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000515 RID: 1301 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrev16q_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x00006245 File Offset: 0x00004445
			[DebuggerStepThrough]
			public static v64 vrev16_u8(v64 a0)
			{
				return Arm.Neon.vrev16_s8(a0);
			}

			// Token: 0x06000517 RID: 1303 RVA: 0x0000624D File Offset: 0x0000444D
			[DebuggerStepThrough]
			public static v128 vrev16q_u8(v128 a0)
			{
				return Arm.Neon.vrev16q_s8(a0);
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtbl1_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000519 RID: 1305 RVA: 0x00006255 File Offset: 0x00004455
			[DebuggerStepThrough]
			public static v64 vtbl1_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vtbl1_s8(a0, a1);
			}

			// Token: 0x0600051A RID: 1306 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtbx1_s8(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600051B RID: 1307 RVA: 0x0000625E File Offset: 0x0000445E
			[DebuggerStepThrough]
			public static v64 vtbx1_u8(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vtbx1_s8(a0, a1, a2);
			}

			// Token: 0x0600051C RID: 1308 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vget_lane_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600051D RID: 1309 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vget_lane_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600051E RID: 1310 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vget_lane_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600051F RID: 1311 RVA: 0x00006268 File Offset: 0x00004468
			[DebuggerStepThrough]
			public static ulong vget_lane_u64(v64 a0, int a1)
			{
				return a0.ULong0;
			}

			// Token: 0x06000520 RID: 1312 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vget_lane_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000521 RID: 1313 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vget_lane_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000522 RID: 1314 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vget_lane_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000523 RID: 1315 RVA: 0x00006270 File Offset: 0x00004470
			[DebuggerStepThrough]
			public static long vget_lane_s64(v64 a0, int a1)
			{
				return a0.SLong0;
			}

			// Token: 0x06000524 RID: 1316 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vget_lane_f32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000525 RID: 1317 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vgetq_lane_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000526 RID: 1318 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vgetq_lane_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000527 RID: 1319 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vgetq_lane_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000528 RID: 1320 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vgetq_lane_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000529 RID: 1321 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vgetq_lane_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600052A RID: 1322 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vgetq_lane_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600052B RID: 1323 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vgetq_lane_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600052C RID: 1324 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vgetq_lane_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600052D RID: 1325 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vgetq_lane_f32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_u8(byte a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600052F RID: 1327 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_u16(ushort a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_u32(uint a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vset_lane_u64(ulong a0, v64 a1, int a2)
			{
				return new v64(a0);
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_s8(sbyte a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000533 RID: 1331 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_s16(short a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000534 RID: 1332 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_s32(int a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000535 RID: 1333 RVA: 0x00006280 File Offset: 0x00004480
			[DebuggerStepThrough]
			public static v64 vset_lane_s64(long a0, v64 a1, int a2)
			{
				return new v64(a0);
			}

			// Token: 0x06000536 RID: 1334 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vset_lane_f32(float a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_u8(byte a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_u16(ushort a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000539 RID: 1337 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_u32(uint a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_u64(ulong a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600053B RID: 1339 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_s8(sbyte a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_s16(short a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600053D RID: 1341 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_s32(int a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_s64(long a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600053F RID: 1343 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_f32(float a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000540 RID: 1344 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_n_f32(v64 a0, v64 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000541 RID: 1345 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_n_f32(v128 a0, v128 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000542 RID: 1346 RVA: 0x00002556 File Offset: 0x00000756
			public static bool IsNeonArmv82FeaturesSupported
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vadd_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000544 RID: 1348 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000545 RID: 1349 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vaddd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vaddd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000547 RID: 1351 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_high_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000548 RID: 1352 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_high_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600054B RID: 1355 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_high_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600054C RID: 1356 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddl_high_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600054D RID: 1357 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_high_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600054F RID: 1359 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_high_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_high_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddw_high_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000553 RID: 1363 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqaddb_s8(sbyte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000554 RID: 1364 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqaddh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000555 RID: 1365 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqadds_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqaddd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqaddb_u8(byte a0, byte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqaddh_u16(ushort a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqadds_u32(uint a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vqaddd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600055B RID: 1371 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuqadd_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600055C RID: 1372 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuqaddq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600055D RID: 1373 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuqadd_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuqaddq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuqadd_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuqaddq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuqadd_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuqaddq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vuqaddb_s8(sbyte a0, byte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vuqaddh_s16(short a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vuqadds_s32(int a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vuqaddd_s64(long a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsqadd_u8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsqaddq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsqadd_u16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsqaddq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsqadd_u32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsqaddq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600056D RID: 1389 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsqadd_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsqaddq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vsqaddb_u8(byte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vsqaddh_u16(ushort a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000571 RID: 1393 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vsqadds_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vsqaddd_u64(ulong a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000573 RID: 1395 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddhn_high_s16(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000574 RID: 1396 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddhn_high_s32(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaddhn_high_s64(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x00006288 File Offset: 0x00004488
			[DebuggerStepThrough]
			public static v128 vaddhn_high_u16(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vaddhn_high_s16(a0, a1, a2);
			}

			// Token: 0x06000577 RID: 1399 RVA: 0x00006292 File Offset: 0x00004492
			[DebuggerStepThrough]
			public static v128 vaddhn_high_u32(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vaddhn_high_s32(a0, a1, a2);
			}

			// Token: 0x06000578 RID: 1400 RVA: 0x0000629C File Offset: 0x0000449C
			[DebuggerStepThrough]
			public static v128 vaddhn_high_u64(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vaddhn_high_s64(a0, a1, a2);
			}

			// Token: 0x06000579 RID: 1401 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vraddhn_high_s16(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vraddhn_high_s32(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600057B RID: 1403 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vraddhn_high_s64(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x000062A6 File Offset: 0x000044A6
			[DebuggerStepThrough]
			public static v128 vraddhn_high_u16(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vraddhn_high_s16(a0, a1, a2);
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x000062B0 File Offset: 0x000044B0
			[DebuggerStepThrough]
			public static v128 vraddhn_high_u32(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vraddhn_high_s32(a0, a1, a2);
			}

			// Token: 0x0600057E RID: 1406 RVA: 0x000062BA File Offset: 0x000044BA
			[DebuggerStepThrough]
			public static v128 vraddhn_high_u64(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vraddhn_high_s64(a0, a1, a2);
			}

			// Token: 0x0600057F RID: 1407 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000580 RID: 1408 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000581 RID: 1409 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmulx_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulxq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000583 RID: 1411 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmulx_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000584 RID: 1412 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulxq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000585 RID: 1413 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmulxs_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000586 RID: 1414 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vmulxd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000587 RID: 1415 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmulx_lane_f32(v64 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulxq_lane_f32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000589 RID: 1417 RVA: 0x000062C4 File Offset: 0x000044C4
			[DebuggerStepThrough]
			public static v64 vmulx_lane_f64(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vmulx_f64(a0, a1);
			}

			// Token: 0x0600058A RID: 1418 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulxq_lane_f64(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600058B RID: 1419 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmulxs_lane_f32(float a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600058C RID: 1420 RVA: 0x000062CD File Offset: 0x000044CD
			[DebuggerStepThrough]
			public static double vmulxd_lane_f64(double a0, v64 a1, int a2)
			{
				return Arm.Neon.vmulxd_f64(a0, a1.Double0);
			}

			// Token: 0x0600058D RID: 1421 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmulx_laneq_f32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600058E RID: 1422 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulxq_laneq_f32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600058F RID: 1423 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmulx_laneq_f64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000590 RID: 1424 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulxq_laneq_f64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000591 RID: 1425 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmulxs_laneq_f32(float a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000592 RID: 1426 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vmulxd_laneq_f64(double a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000593 RID: 1427 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdiv_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000594 RID: 1428 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdivq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000595 RID: 1429 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdiv_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000596 RID: 1430 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdivq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000597 RID: 1431 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_f64(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000598 RID: 1432 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_f64(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000599 RID: 1433 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600059A RID: 1434 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600059B RID: 1435 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600059C RID: 1436 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_u8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600059D RID: 1437 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_u16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_f64(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A0 RID: 1440 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_f64(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A1 RID: 1441 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A2 RID: 1442 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A3 RID: 1443 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A4 RID: 1444 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_u8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A5 RID: 1445 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_u16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A6 RID: 1446 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A7 RID: 1447 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_f64(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A8 RID: 1448 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_f64(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005A9 RID: 1449 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_lane_f32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005AA RID: 1450 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_lane_f32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005AB RID: 1451 RVA: 0x000062DB File Offset: 0x000044DB
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV8A_AARCH64)]
			public static v64 vfma_lane_f64(v64 a0, v64 a1, v64 a2, int a3)
			{
				return Arm.Neon.vfma_f64(a0, a1, a2);
			}

			// Token: 0x060005AC RID: 1452 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_lane_f64(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005AD RID: 1453 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vfmas_lane_f32(float a0, float a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x000062E5 File Offset: 0x000044E5
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV8A_AARCH64)]
			public static double vfmad_lane_f64(double a0, double a1, v64 a2, int a3)
			{
				return Arm.Neon.vfma_f64(new v64(a0), new v64(a1), a2).Double0;
			}

			// Token: 0x060005AF RID: 1455 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_laneq_f32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_laneq_f32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B1 RID: 1457 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_laneq_f64(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_laneq_f64(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B3 RID: 1459 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vfmas_laneq_f32(float a0, float a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vfmad_laneq_f64(double a0, double a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B5 RID: 1461 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_f64(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B6 RID: 1462 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_f64(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B7 RID: 1463 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_lane_f32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B8 RID: 1464 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_lane_f32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005B9 RID: 1465 RVA: 0x000062FE File Offset: 0x000044FE
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV8A_AARCH64)]
			public static v64 vfms_lane_f64(v64 a0, v64 a1, v64 a2, int a3)
			{
				return Arm.Neon.vfms_f64(a0, a1, a2);
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_lane_f64(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005BB RID: 1467 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vfmss_lane_f32(float a0, float a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x00006308 File Offset: 0x00004508
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV8A_AARCH64)]
			public static double vfmsd_lane_f64(double a0, double a1, v64 a2, int a3)
			{
				return Arm.Neon.vfms_f64(new v64(a0), new v64(a1), a2).Double0;
			}

			// Token: 0x060005BD RID: 1469 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_laneq_f32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_laneq_f32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005BF RID: 1471 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_laneq_f64(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_laneq_f64(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C1 RID: 1473 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vfmss_laneq_f32(float a0, float a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vfmsd_laneq_f64(double a0, double a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqdmulhh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmulhs_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C5 RID: 1477 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmulhh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmulhs_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmlalh_s16(int a0, short a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmlals_s32(long a0, int a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmlslh_s16(int a0, short a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmlsls_s32(long a0, int a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D0 RID: 1488 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D1 RID: 1489 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D2 RID: 1490 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D3 RID: 1491 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D4 RID: 1492 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D5 RID: 1493 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmullh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D6 RID: 1494 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmulls_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D7 RID: 1495 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsub_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005DB RID: 1499 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vsubd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vsubd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005DD RID: 1501 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_high_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_high_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_high_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubl_high_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_high_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_high_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_high_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E8 RID: 1512 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubw_high_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005E9 RID: 1513 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqsubb_s8(sbyte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005EA RID: 1514 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqsubh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqsubs_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005EC RID: 1516 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqsubd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqsubb_u8(byte a0, byte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqsubh_u16(ushort a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqsubs_u32(uint a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vqsubd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubhn_high_s16(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubhn_high_s32(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsubhn_high_s64(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x00006321 File Offset: 0x00004521
			[DebuggerStepThrough]
			public static v128 vsubhn_high_u16(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vsubhn_high_s16(a0, a1, a2);
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x0000632B File Offset: 0x0000452B
			[DebuggerStepThrough]
			public static v128 vsubhn_high_u32(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vsubhn_high_s32(a0, a1, a2);
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x00006335 File Offset: 0x00004535
			[DebuggerStepThrough]
			public static v128 vsubhn_high_u64(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vsubhn_high_s64(a0, a1, a2);
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsubhn_high_s16(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsubhn_high_s32(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsubhn_high_s64(v64 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005FA RID: 1530 RVA: 0x0000633F File Offset: 0x0000453F
			[DebuggerStepThrough]
			public static v128 vrsubhn_high_u16(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vrsubhn_high_s16(a0, a1, a2);
			}

			// Token: 0x060005FB RID: 1531 RVA: 0x00006349 File Offset: 0x00004549
			[DebuggerStepThrough]
			public static v128 vrsubhn_high_u32(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vrsubhn_high_s32(a0, a1, a2);
			}

			// Token: 0x060005FC RID: 1532 RVA: 0x00006353 File Offset: 0x00004553
			[DebuggerStepThrough]
			public static v128 vrsubhn_high_u64(v64 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vrsubhn_high_s64(a0, a1, a2);
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceq_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x0000635D File Offset: 0x0000455D
			[DebuggerStepThrough]
			public static v64 vceq_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vceq_s64(a0, a1);
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x00006366 File Offset: 0x00004566
			[DebuggerStepThrough]
			public static v128 vceqq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vceqq_s64(a0, a1);
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceq_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000602 RID: 1538 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vceqd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vceqd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vceqs_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vceqd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceqz_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqzq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceqz_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqzq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600060B RID: 1547 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceqz_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqzq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x0000636F File Offset: 0x0000456F
			[DebuggerStepThrough]
			public static v64 vceqz_u8(v64 a0)
			{
				return Arm.Neon.vceqz_s8(a0);
			}

			// Token: 0x0600060E RID: 1550 RVA: 0x00006377 File Offset: 0x00004577
			[DebuggerStepThrough]
			public static v128 vceqzq_u8(v128 a0)
			{
				return Arm.Neon.vceqzq_s8(a0);
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x0000637F File Offset: 0x0000457F
			[DebuggerStepThrough]
			public static v64 vceqz_u16(v64 a0)
			{
				return Arm.Neon.vceqz_s16(a0);
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x00006387 File Offset: 0x00004587
			[DebuggerStepThrough]
			public static v128 vceqzq_u16(v128 a0)
			{
				return Arm.Neon.vceqzq_s16(a0);
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x0000638F File Offset: 0x0000458F
			[DebuggerStepThrough]
			public static v64 vceqz_u32(v64 a0)
			{
				return Arm.Neon.vceqz_s32(a0);
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x00006397 File Offset: 0x00004597
			[DebuggerStepThrough]
			public static v128 vceqzq_u32(v128 a0)
			{
				return Arm.Neon.vceqzq_s32(a0);
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceqz_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000614 RID: 1556 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqzq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000615 RID: 1557 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceqz_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000616 RID: 1558 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqzq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000617 RID: 1559 RVA: 0x0000639F File Offset: 0x0000459F
			[DebuggerStepThrough]
			public static v64 vceqz_u64(v64 a0)
			{
				return Arm.Neon.vceqz_s64(a0);
			}

			// Token: 0x06000618 RID: 1560 RVA: 0x000063A7 File Offset: 0x000045A7
			[DebuggerStepThrough]
			public static v128 vceqzq_u64(v128 a0)
			{
				return Arm.Neon.vceqzq_s64(a0);
			}

			// Token: 0x06000619 RID: 1561 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vceqz_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vceqzq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600061B RID: 1563 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vceqzd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600061C RID: 1564 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vceqzd_u64(ulong a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vceqzs_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vceqzd_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600061F RID: 1567 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000620 RID: 1568 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000621 RID: 1569 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000622 RID: 1570 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000623 RID: 1571 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcge_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000624 RID: 1572 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgeq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000625 RID: 1573 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcged_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000626 RID: 1574 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcged_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000627 RID: 1575 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcges_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000628 RID: 1576 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcged_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000629 RID: 1577 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgez_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600062A RID: 1578 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgezq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600062B RID: 1579 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgez_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600062C RID: 1580 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgezq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgez_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600062E RID: 1582 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgezq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgez_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000630 RID: 1584 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgezq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000631 RID: 1585 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgez_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000632 RID: 1586 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgezq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000633 RID: 1587 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgez_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000634 RID: 1588 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgezq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000635 RID: 1589 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgezd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000636 RID: 1590 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcgezs_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000637 RID: 1591 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgezd_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000638 RID: 1592 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000639 RID: 1593 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600063A RID: 1594 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcle_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcleq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcled_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcled_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcles_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcled_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclez_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclezq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000644 RID: 1604 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclez_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000645 RID: 1605 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclezq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000646 RID: 1606 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclez_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000647 RID: 1607 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclezq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000648 RID: 1608 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclez_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000649 RID: 1609 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclezq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600064A RID: 1610 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclez_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600064B RID: 1611 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclezq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600064C RID: 1612 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclez_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600064D RID: 1613 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vclezq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600064E RID: 1614 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vclezd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600064F RID: 1615 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vclezs_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000650 RID: 1616 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vclezd_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000651 RID: 1617 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000652 RID: 1618 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000653 RID: 1619 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000654 RID: 1620 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000655 RID: 1621 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgt_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000656 RID: 1622 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000657 RID: 1623 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgtd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000658 RID: 1624 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgtd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000659 RID: 1625 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcgts_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600065A RID: 1626 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgtd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600065B RID: 1627 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgtz_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600065C RID: 1628 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtzq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600065D RID: 1629 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgtz_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600065E RID: 1630 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtzq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600065F RID: 1631 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgtz_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000660 RID: 1632 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtzq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000661 RID: 1633 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgtz_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000662 RID: 1634 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtzq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000663 RID: 1635 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgtz_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000664 RID: 1636 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtzq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000665 RID: 1637 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcgtz_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000666 RID: 1638 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcgtzq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000667 RID: 1639 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgtzd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000668 RID: 1640 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcgtzs_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000669 RID: 1641 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcgtzd_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600066A RID: 1642 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600066B RID: 1643 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600066C RID: 1644 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_u64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600066D RID: 1645 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_u64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600066E RID: 1646 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vclt_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600066F RID: 1647 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000670 RID: 1648 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcltd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000671 RID: 1649 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcltd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000672 RID: 1650 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vclts_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000673 RID: 1651 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcltd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000674 RID: 1652 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcltz_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000675 RID: 1653 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltzq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000676 RID: 1654 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcltz_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000677 RID: 1655 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltzq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000678 RID: 1656 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcltz_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltzq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600067A RID: 1658 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcltz_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600067B RID: 1659 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltzq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600067C RID: 1660 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcltz_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600067D RID: 1661 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltzq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600067E RID: 1662 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcltz_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600067F RID: 1663 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcltzq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000680 RID: 1664 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcltzd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000681 RID: 1665 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcltzs_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000682 RID: 1666 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcltzd_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000683 RID: 1667 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcage_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000684 RID: 1668 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcageq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000685 RID: 1669 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcages_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000686 RID: 1670 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcaged_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000687 RID: 1671 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcale_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000688 RID: 1672 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcaleq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000689 RID: 1673 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcales_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600068A RID: 1674 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcaled_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600068B RID: 1675 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcagt_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600068C RID: 1676 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcagtq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600068D RID: 1677 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcagts_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600068E RID: 1678 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcagtd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600068F RID: 1679 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcalt_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000690 RID: 1680 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcaltq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000691 RID: 1681 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcalts_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000692 RID: 1682 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcaltd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000693 RID: 1683 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtst_s64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000694 RID: 1684 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtstq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000695 RID: 1685 RVA: 0x000063AF File Offset: 0x000045AF
			[DebuggerStepThrough]
			public static v64 vtst_u64(v64 a0, v64 a1)
			{
				return Arm.Neon.vtst_s64(a0, a1);
			}

			// Token: 0x06000696 RID: 1686 RVA: 0x000063B8 File Offset: 0x000045B8
			[DebuggerStepThrough]
			public static v128 vtstq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vtstq_s64(a0, a1);
			}

			// Token: 0x06000697 RID: 1687 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vtstd_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000698 RID: 1688 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vtstd_u64(ulong a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000699 RID: 1689 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabd_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600069A RID: 1690 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600069B RID: 1691 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vabds_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600069C RID: 1692 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vabdd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600069D RID: 1693 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_high_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600069E RID: 1694 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_high_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600069F RID: 1695 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_high_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A0 RID: 1696 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_high_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A1 RID: 1697 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_high_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A2 RID: 1698 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabdl_high_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A3 RID: 1699 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_high_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A4 RID: 1700 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_high_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_high_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_high_u8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_high_u16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A8 RID: 1704 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabal_high_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006A9 RID: 1705 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmax_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006AA RID: 1706 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006AB RID: 1707 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmin_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006AC RID: 1708 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006AD RID: 1709 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmaxnm_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006AE RID: 1710 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxnmq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006AF RID: 1711 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmaxnm_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B0 RID: 1712 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmaxnmq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B1 RID: 1713 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vminnm_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B2 RID: 1714 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminnmq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B3 RID: 1715 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vminnm_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B4 RID: 1716 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vminnmq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B5 RID: 1717 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vshld_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B6 RID: 1718 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vshld_u64(ulong a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B7 RID: 1719 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqshlb_s8(sbyte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B8 RID: 1720 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqshlh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006B9 RID: 1721 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqshls_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006BA RID: 1722 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqshld_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006BB RID: 1723 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqshlb_u8(byte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006BC RID: 1724 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqshlh_u16(ushort a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006BD RID: 1725 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqshls_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006BE RID: 1726 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vqshld_u64(ulong a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006BF RID: 1727 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vrshld_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C0 RID: 1728 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vrshld_u64(ulong a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C1 RID: 1729 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqrshlb_s8(sbyte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C2 RID: 1730 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrshlh_s16(short a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C3 RID: 1731 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrshls_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C4 RID: 1732 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqrshld_s64(long a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C5 RID: 1733 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqrshlb_u8(byte a0, sbyte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C6 RID: 1734 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqrshlh_u16(ushort a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C7 RID: 1735 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqrshls_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C8 RID: 1736 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vqrshld_u64(ulong a0, long a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006C9 RID: 1737 RVA: 0x000063C1 File Offset: 0x000045C1
			[DebuggerStepThrough]
			public static long vshrd_n_s64(long a0, int a1)
			{
				return a0 >> a1;
			}

			// Token: 0x060006CA RID: 1738 RVA: 0x000063C9 File Offset: 0x000045C9
			[DebuggerStepThrough]
			public static ulong vshrd_n_u64(ulong a0, int a1)
			{
				return a0 >> a1;
			}

			// Token: 0x060006CB RID: 1739 RVA: 0x000063D1 File Offset: 0x000045D1
			[DebuggerStepThrough]
			public static long vshld_n_s64(long a0, int a1)
			{
				return a0 << a1;
			}

			// Token: 0x060006CC RID: 1740 RVA: 0x000063D1 File Offset: 0x000045D1
			[DebuggerStepThrough]
			public static ulong vshld_n_u64(ulong a0, int a1)
			{
				return a0 << a1;
			}

			// Token: 0x060006CD RID: 1741 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vrshrd_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006CE RID: 1742 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vrshrd_n_u64(ulong a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006CF RID: 1743 RVA: 0x000063D9 File Offset: 0x000045D9
			[DebuggerStepThrough]
			public static long vsrad_n_s64(long a0, long a1, int a2)
			{
				return a0 + (a1 >> a2);
			}

			// Token: 0x060006D0 RID: 1744 RVA: 0x000063E3 File Offset: 0x000045E3
			[DebuggerStepThrough]
			public static ulong vsrad_n_u64(ulong a0, ulong a1, int a2)
			{
				return a0 + (a1 >> a2);
			}

			// Token: 0x060006D1 RID: 1745 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vrsrad_n_s64(long a0, long a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D2 RID: 1746 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vrsrad_n_u64(ulong a0, ulong a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D3 RID: 1747 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqshlb_n_s8(sbyte a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D4 RID: 1748 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqshlh_n_s16(short a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D5 RID: 1749 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqshls_n_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D6 RID: 1750 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqshld_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D7 RID: 1751 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqshlb_n_u8(byte a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D8 RID: 1752 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqshlh_n_u16(ushort a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006D9 RID: 1753 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqshls_n_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006DA RID: 1754 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vqshld_n_u64(ulong a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006DB RID: 1755 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqshlub_n_s8(sbyte a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006DC RID: 1756 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqshluh_n_s16(short a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006DD RID: 1757 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqshlus_n_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006DE RID: 1758 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vqshlud_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006DF RID: 1759 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrn_high_n_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E0 RID: 1760 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrn_high_n_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E1 RID: 1761 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrn_high_n_s64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrn_high_n_u16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrn_high_n_u32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E4 RID: 1764 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshrn_high_n_u64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqshrunh_n_s16(short a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E6 RID: 1766 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqshruns_n_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E7 RID: 1767 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqshrund_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E8 RID: 1768 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrun_high_n_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006E9 RID: 1769 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrun_high_n_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006EA RID: 1770 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrun_high_n_s64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqrshrunh_n_s16(short a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006EC RID: 1772 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqrshruns_n_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006ED RID: 1773 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqrshrund_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006EE RID: 1774 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrun_high_n_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006EF RID: 1775 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrun_high_n_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F0 RID: 1776 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrun_high_n_s64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F1 RID: 1777 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqshrnh_n_s16(short a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F2 RID: 1778 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqshrns_n_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F3 RID: 1779 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqshrnd_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F4 RID: 1780 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqshrnh_n_u16(ushort a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F5 RID: 1781 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqshrns_n_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F6 RID: 1782 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqshrnd_n_u64(ulong a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F7 RID: 1783 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrn_high_n_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F8 RID: 1784 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrn_high_n_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006F9 RID: 1785 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrn_high_n_s64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006FA RID: 1786 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrn_high_n_u16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006FB RID: 1787 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrn_high_n_u32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006FC RID: 1788 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqshrn_high_n_u64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006FD RID: 1789 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshrn_high_n_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006FE RID: 1790 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshrn_high_n_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060006FF RID: 1791 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshrn_high_n_s64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000700 RID: 1792 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshrn_high_n_u16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000701 RID: 1793 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshrn_high_n_u32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000702 RID: 1794 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrshrn_high_n_u64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000703 RID: 1795 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqrshrnh_n_s16(short a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000704 RID: 1796 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrshrns_n_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000705 RID: 1797 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrshrnd_n_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000706 RID: 1798 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqrshrnh_n_u16(ushort a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000707 RID: 1799 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqrshrns_n_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000708 RID: 1800 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqrshrnd_n_u64(ulong a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000709 RID: 1801 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrn_high_n_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600070A RID: 1802 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrn_high_n_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600070B RID: 1803 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrn_high_n_s64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600070C RID: 1804 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrn_high_n_u16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600070D RID: 1805 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrn_high_n_u32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600070E RID: 1806 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrshrn_high_n_u64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600070F RID: 1807 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_high_n_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000710 RID: 1808 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_high_n_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000711 RID: 1809 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_high_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000712 RID: 1810 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_high_n_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000713 RID: 1811 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_high_n_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000714 RID: 1812 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vshll_high_n_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000715 RID: 1813 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vsrid_n_s64(long a0, long a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000716 RID: 1814 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vsrid_n_u64(ulong a0, ulong a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000717 RID: 1815 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vslid_n_s64(long a0, long a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000718 RID: 1816 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vslid_n_u64(ulong a0, ulong a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000719 RID: 1817 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtn_s32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600071A RID: 1818 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtnq_s32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600071B RID: 1819 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtn_u32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtnq_u32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600071D RID: 1821 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtm_s32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtmq_s32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtm_u32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000720 RID: 1824 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtmq_u32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtp_s32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000722 RID: 1826 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtpq_s32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000723 RID: 1827 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtp_u32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtpq_u32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000725 RID: 1829 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvta_s32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000726 RID: 1830 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtaq_s32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000727 RID: 1831 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvta_u32_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000728 RID: 1832 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtaq_u32_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000729 RID: 1833 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vcvts_s32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600072A RID: 1834 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcvts_u32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600072B RID: 1835 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vcvtns_s32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600072C RID: 1836 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcvtns_u32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600072D RID: 1837 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vcvtms_s32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600072E RID: 1838 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcvtms_u32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600072F RID: 1839 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vcvtps_s32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000730 RID: 1840 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcvtps_u32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000731 RID: 1841 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vcvtas_s32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000732 RID: 1842 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcvtas_u32_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000733 RID: 1843 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_s64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000734 RID: 1844 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_s64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000735 RID: 1845 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_u64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000736 RID: 1846 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_u64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000737 RID: 1847 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtn_s64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000738 RID: 1848 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtnq_s64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000739 RID: 1849 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtn_u64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600073A RID: 1850 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtnq_u64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600073B RID: 1851 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtm_s64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600073C RID: 1852 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtmq_s64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600073D RID: 1853 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtm_u64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600073E RID: 1854 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtmq_u64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600073F RID: 1855 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtp_s64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000740 RID: 1856 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtpq_s64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000741 RID: 1857 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtp_u64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000742 RID: 1858 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtpq_u64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000743 RID: 1859 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvta_s64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000744 RID: 1860 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtaq_s64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000745 RID: 1861 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvta_u64_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000746 RID: 1862 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtaq_u64_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000747 RID: 1863 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vcvtd_s64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000748 RID: 1864 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcvtd_u64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000749 RID: 1865 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vcvtnd_s64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600074A RID: 1866 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcvtnd_u64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600074B RID: 1867 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vcvtmd_s64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600074C RID: 1868 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcvtmd_u64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600074D RID: 1869 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vcvtpd_s64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600074E RID: 1870 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcvtpd_u64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600074F RID: 1871 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vcvtad_s64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000750 RID: 1872 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcvtad_u64_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000751 RID: 1873 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vcvts_n_s32_f32(float a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000752 RID: 1874 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vcvts_n_u32_f32(float a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000753 RID: 1875 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_s64_f64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000754 RID: 1876 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_s64_f64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000755 RID: 1877 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_u64_f64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000756 RID: 1878 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_u64_f64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000757 RID: 1879 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vcvtd_n_s64_f64(double a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000758 RID: 1880 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vcvtd_n_u64_f64(double a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000759 RID: 1881 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vcvts_f32_s32(int a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600075A RID: 1882 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vcvts_f32_u32(uint a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600075B RID: 1883 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_f64_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600075C RID: 1884 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_f64_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600075D RID: 1885 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_f64_u64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600075E RID: 1886 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_f64_u64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600075F RID: 1887 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vcvtd_f64_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000760 RID: 1888 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vcvtd_f64_u64(ulong a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000761 RID: 1889 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vcvts_n_f32_s32(int a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000762 RID: 1890 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vcvts_n_f32_u32(uint a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000763 RID: 1891 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_f64_s64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000764 RID: 1892 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_f64_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_n_f64_u64(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000766 RID: 1894 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtq_n_f64_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000767 RID: 1895 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vcvtd_n_f64_s64(long a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000768 RID: 1896 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vcvtd_n_f64_u64(ulong a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000769 RID: 1897 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvt_f32_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600076A RID: 1898 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvt_high_f32_f64(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600076B RID: 1899 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvt_f64_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600076C RID: 1900 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvt_high_f64_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600076D RID: 1901 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcvtx_f32_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600076E RID: 1902 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vcvtxd_f32_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600076F RID: 1903 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcvtx_high_f32_f64(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000770 RID: 1904 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrnd_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000771 RID: 1905 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000772 RID: 1906 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrnd_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000773 RID: 1907 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000774 RID: 1908 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndn_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000775 RID: 1909 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndnq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000776 RID: 1910 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndn_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000777 RID: 1911 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndnq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000778 RID: 1912 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vrndns_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000779 RID: 1913 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndm_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600077A RID: 1914 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndmq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600077B RID: 1915 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndm_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600077C RID: 1916 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndmq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600077D RID: 1917 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndp_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600077E RID: 1918 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndpq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600077F RID: 1919 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndp_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000780 RID: 1920 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndpq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000781 RID: 1921 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrnda_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndaq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrnda_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000784 RID: 1924 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndaq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000785 RID: 1925 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndi_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000786 RID: 1926 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndiq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000787 RID: 1927 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndi_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000788 RID: 1928 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndiq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndx_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600078A RID: 1930 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndxq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600078B RID: 1931 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrndx_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600078C RID: 1932 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrndxq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600078D RID: 1933 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_high_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600078E RID: 1934 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_high_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600078F RID: 1935 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_high_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000790 RID: 1936 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_high_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000791 RID: 1937 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_high_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000792 RID: 1938 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmovl_high_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000793 RID: 1939 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqmovnh_s16(short a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000794 RID: 1940 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqmovns_s32(int a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000795 RID: 1941 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqmovnd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000796 RID: 1942 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqmovnh_u16(ushort a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000797 RID: 1943 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqmovns_u32(uint a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000798 RID: 1944 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqmovnd_u64(ulong a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000799 RID: 1945 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovn_high_s16(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600079A RID: 1946 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovn_high_s32(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600079B RID: 1947 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovn_high_s64(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600079C RID: 1948 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovn_high_u16(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600079D RID: 1949 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovn_high_u32(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600079E RID: 1950 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovn_high_u64(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600079F RID: 1951 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vqmovunh_s16(short a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vqmovuns_s32(int a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A1 RID: 1953 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vqmovund_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A2 RID: 1954 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovun_high_s16(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A3 RID: 1955 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovun_high_s32(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A4 RID: 1956 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqmovun_high_s64(v64 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A5 RID: 1957 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_laneq_s16(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A6 RID: 1958 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A7 RID: 1959 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_laneq_s32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A8 RID: 1960 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007A9 RID: 1961 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_laneq_u16(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007AA RID: 1962 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_laneq_u16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007AB RID: 1963 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_laneq_u32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007AC RID: 1964 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_laneq_u32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007AD RID: 1965 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmla_laneq_f32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007AE RID: 1966 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlaq_laneq_f32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007AF RID: 1967 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B0 RID: 1968 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B1 RID: 1969 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_lane_u16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B2 RID: 1970 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_lane_u32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B3 RID: 1971 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_laneq_s16(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B4 RID: 1972 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_laneq_s32(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B5 RID: 1973 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_laneq_u16(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B6 RID: 1974 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_laneq_u32(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B7 RID: 1975 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B8 RID: 1976 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007B9 RID: 1977 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_laneq_u16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007BA RID: 1978 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_laneq_u32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007BB RID: 1979 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmlalh_lane_s16(int a0, short a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007BC RID: 1980 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmlals_lane_s32(long a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007BD RID: 1981 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007BE RID: 1982 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007BF RID: 1983 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_laneq_s16(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C0 RID: 1984 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_laneq_s32(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C1 RID: 1985 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmlalh_laneq_s16(int a0, short a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C2 RID: 1986 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmlals_laneq_s32(long a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C3 RID: 1987 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C4 RID: 1988 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C5 RID: 1989 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_laneq_s16(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C6 RID: 1990 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C7 RID: 1991 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_laneq_s32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C8 RID: 1992 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007C9 RID: 1993 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_laneq_u16(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007CA RID: 1994 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_laneq_u16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007CB RID: 1995 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_laneq_u32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007CC RID: 1996 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_laneq_u32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007CD RID: 1997 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmls_laneq_f32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007CE RID: 1998 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsq_laneq_f32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007CF RID: 1999 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D0 RID: 2000 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D1 RID: 2001 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_lane_u16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D2 RID: 2002 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_lane_u32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D3 RID: 2003 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_laneq_s16(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D4 RID: 2004 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_laneq_s32(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D5 RID: 2005 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_laneq_u16(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D6 RID: 2006 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_laneq_u32(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D7 RID: 2007 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D8 RID: 2008 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007D9 RID: 2009 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_laneq_u16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007DA RID: 2010 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_laneq_u32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007DB RID: 2011 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmlslh_lane_s16(int a0, short a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmlsls_lane_s32(long a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007DD RID: 2013 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007DE RID: 2014 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_laneq_s16(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E0 RID: 2016 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_laneq_s32(v128 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E1 RID: 2017 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmlslh_laneq_s16(int a0, short a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E2 RID: 2018 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmlsls_laneq_s32(long a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E3 RID: 2019 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E4 RID: 2020 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E5 RID: 2021 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_n_f64(v64 a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E6 RID: 2022 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_n_f64(v128 a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x000063ED File Offset: 0x000045ED
			[DebuggerStepThrough]
			[BurstTargetCpu(BurstTargetCpu.ARMV8A_AARCH64)]
			public static v64 vmul_lane_f64(v64 a0, v64 a1, int a2)
			{
				return Arm.Neon.vmul_f64(a0, a1);
			}

			// Token: 0x060007E8 RID: 2024 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_lane_f64(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007E9 RID: 2025 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmuls_lane_f32(float a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007EA RID: 2026 RVA: 0x000063F6 File Offset: 0x000045F6
			[DebuggerStepThrough]
			public static double vmuld_lane_f64(double a0, v64 a1, int a2)
			{
				return a0 * a1.Double0;
			}

			// Token: 0x060007EB RID: 2027 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_laneq_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007EC RID: 2028 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_laneq_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007ED RID: 2029 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_laneq_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007EE RID: 2030 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_laneq_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007EF RID: 2031 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_laneq_u16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F0 RID: 2032 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_laneq_u16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F1 RID: 2033 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_laneq_u32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F2 RID: 2034 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_laneq_u32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_laneq_f32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F4 RID: 2036 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_laneq_f32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F5 RID: 2037 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vmul_laneq_f64(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmulq_laneq_f64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F7 RID: 2039 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmuls_laneq_f32(float a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F8 RID: 2040 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vmuld_laneq_f64(double a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_n_s16(v128 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007FA RID: 2042 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007FB RID: 2043 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_n_u16(v128 a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007FC RID: 2044 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_n_u32(v128 a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007FD RID: 2045 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_lane_s16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007FE RID: 2046 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_lane_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060007FF RID: 2047 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_lane_u16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000800 RID: 2048 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_lane_u32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000801 RID: 2049 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_laneq_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000802 RID: 2050 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_laneq_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000803 RID: 2051 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_laneq_u16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000804 RID: 2052 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_laneq_u32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000805 RID: 2053 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_laneq_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000806 RID: 2054 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_laneq_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000807 RID: 2055 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_laneq_u16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000808 RID: 2056 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmull_high_laneq_u32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000809 RID: 2057 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_n_s16(v128 a0, short a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600080A RID: 2058 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_n_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600080B RID: 2059 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmullh_lane_s16(short a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600080C RID: 2060 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmulls_lane_s32(int a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600080D RID: 2061 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_lane_s16(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600080E RID: 2062 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_lane_s32(v128 a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600080F RID: 2063 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_laneq_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000810 RID: 2064 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_laneq_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000811 RID: 2065 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmullh_laneq_s16(short a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000812 RID: 2066 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqdmulls_laneq_s32(int a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000813 RID: 2067 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_laneq_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000814 RID: 2068 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmull_high_laneq_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000815 RID: 2069 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqdmulhh_lane_s16(short a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000816 RID: 2070 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmulhs_lane_s32(int a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000817 RID: 2071 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_laneq_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000818 RID: 2072 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_laneq_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000819 RID: 2073 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqdmulh_laneq_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600081A RID: 2074 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmulhq_laneq_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600081B RID: 2075 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqdmulhh_laneq_s16(short a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600081C RID: 2076 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqdmulhs_laneq_s32(int a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600081D RID: 2077 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmulhh_lane_s16(short a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600081E RID: 2078 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmulhs_lane_s32(int a0, v64 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600081F RID: 2079 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_laneq_s16(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000820 RID: 2080 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_laneq_s16(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000821 RID: 2081 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmulh_laneq_s32(v64 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000822 RID: 2082 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmulhq_laneq_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000823 RID: 2083 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmulhh_laneq_s16(short a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000824 RID: 2084 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmulhs_laneq_s32(int a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000825 RID: 2085 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_n_s16(v128 a0, v128 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000826 RID: 2086 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000827 RID: 2087 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_n_u16(v128 a0, v128 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000828 RID: 2088 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlal_high_n_u32(v128 a0, v128 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000829 RID: 2089 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_n_s16(v128 a0, v128 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600082A RID: 2090 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlal_high_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600082B RID: 2091 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_n_s16(v128 a0, v128 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600082C RID: 2092 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600082D RID: 2093 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_n_u16(v128 a0, v128 a1, ushort a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600082E RID: 2094 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vmlsl_high_n_u32(v128 a0, v128 a1, uint a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600082F RID: 2095 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_n_s16(v128 a0, v128 a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000830 RID: 2096 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqdmlsl_high_n_s32(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000831 RID: 2097 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabs_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000832 RID: 2098 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vabsd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000833 RID: 2099 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabsq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000834 RID: 2100 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vabs_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000835 RID: 2101 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vabsq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000836 RID: 2102 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqabs_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000837 RID: 2103 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqabsq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000838 RID: 2104 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqabsb_s8(sbyte a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000839 RID: 2105 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqabsh_s16(short a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083A RID: 2106 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqabss_s32(int a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083B RID: 2107 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqabsd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083C RID: 2108 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vneg_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083D RID: 2109 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vnegd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083E RID: 2110 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vnegq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600083F RID: 2111 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vneg_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000840 RID: 2112 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vnegq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000841 RID: 2113 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqneg_s64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000842 RID: 2114 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqnegq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000843 RID: 2115 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vqnegb_s8(sbyte a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000844 RID: 2116 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqnegh_s16(short a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000845 RID: 2117 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqnegs_s32(int a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000846 RID: 2118 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vqnegd_s64(long a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000847 RID: 2119 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrecpe_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000848 RID: 2120 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrecpeq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000849 RID: 2121 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vrecpes_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600084A RID: 2122 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vrecped_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600084B RID: 2123 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrecps_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600084C RID: 2124 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrecpsq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600084D RID: 2125 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vrecpss_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600084E RID: 2126 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vrecpsd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600084F RID: 2127 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsqrt_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000850 RID: 2128 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsqrtq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000851 RID: 2129 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vsqrt_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000852 RID: 2130 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsqrtq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000853 RID: 2131 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsqrte_f64(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000854 RID: 2132 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsqrteq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000855 RID: 2133 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vrsqrtes_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vrsqrted_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrsqrts_f64(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000858 RID: 2136 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrsqrtsq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000859 RID: 2137 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vrsqrtss_f32(float a0, float a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600085A RID: 2138 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vrsqrtsd_f64(double a0, double a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600085B RID: 2139 RVA: 0x000061BA File Offset: 0x000043BA
			[DebuggerStepThrough]
			public static v64 vbsl_f64(v64 a0, v64 a1, v64 a2)
			{
				return Arm.Neon.vbsl_s8(a0, a1, a2);
			}

			// Token: 0x0600085C RID: 2140 RVA: 0x000061C4 File Offset: 0x000043C4
			[DebuggerStepThrough]
			public static v128 vbslq_f64(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vbslq_s8(a0, a1, a2);
			}

			// Token: 0x0600085D RID: 2141 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_s8(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600085E RID: 2142 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_s8(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600085F RID: 2143 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_s16(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000860 RID: 2144 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_s16(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000861 RID: 2145 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_s32(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000862 RID: 2146 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_s32(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000863 RID: 2147 RVA: 0x00006400 File Offset: 0x00004600
			[DebuggerStepThrough]
			public static v64 vcopy_lane_s64(v64 a0, int a1, v64 a2, int a3)
			{
				return a2;
			}

			// Token: 0x06000864 RID: 2148 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_s64(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000865 RID: 2149 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_u8(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000866 RID: 2150 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_u8(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000867 RID: 2151 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_u16(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000868 RID: 2152 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_u16(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000869 RID: 2153 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_u32(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600086A RID: 2154 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_u32(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600086B RID: 2155 RVA: 0x00006400 File Offset: 0x00004600
			[DebuggerStepThrough]
			public static v64 vcopy_lane_u64(v64 a0, int a1, v64 a2, int a3)
			{
				return a2;
			}

			// Token: 0x0600086C RID: 2156 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_u64(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600086D RID: 2157 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_lane_f32(v64 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600086E RID: 2158 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_f32(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600086F RID: 2159 RVA: 0x00006400 File Offset: 0x00004600
			[DebuggerStepThrough]
			public static v64 vcopy_lane_f64(v64 a0, int a1, v64 a2, int a3)
			{
				return a2;
			}

			// Token: 0x06000870 RID: 2160 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_lane_f64(v128 a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000871 RID: 2161 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_s8(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000872 RID: 2162 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_s8(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000873 RID: 2163 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_s16(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000874 RID: 2164 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_s16(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000875 RID: 2165 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_s32(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000876 RID: 2166 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_s32(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000877 RID: 2167 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_s64(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000878 RID: 2168 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_s64(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000879 RID: 2169 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_u8(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600087A RID: 2170 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_u8(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600087B RID: 2171 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_u16(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600087C RID: 2172 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_u16(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600087D RID: 2173 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_u32(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600087E RID: 2174 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_u32(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600087F RID: 2175 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_u64(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000880 RID: 2176 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_u64(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000881 RID: 2177 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_f32(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000882 RID: 2178 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_f32(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000883 RID: 2179 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vcopy_laneq_f64(v64 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000884 RID: 2180 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vcopyq_laneq_f64(v128 a0, int a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000885 RID: 2181 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vrbit_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000886 RID: 2182 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vrbitq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000887 RID: 2183 RVA: 0x00006403 File Offset: 0x00004603
			[DebuggerStepThrough]
			public static v64 vrbit_u8(v64 a0)
			{
				return Arm.Neon.vrbit_s8(a0);
			}

			// Token: 0x06000888 RID: 2184 RVA: 0x0000640B File Offset: 0x0000460B
			[DebuggerStepThrough]
			public static v128 vrbitq_u8(v128 a0)
			{
				return Arm.Neon.vrbitq_s8(a0);
			}

			// Token: 0x06000889 RID: 2185 RVA: 0x000061CE File Offset: 0x000043CE
			[DebuggerStepThrough]
			public static v64 vdup_lane_f64(v64 a0, int a1)
			{
				return a0;
			}

			// Token: 0x0600088A RID: 2186 RVA: 0x000061D1 File Offset: 0x000043D1
			[DebuggerStepThrough]
			public static v128 vdupq_lane_f64(v64 a0, int a1)
			{
				return new v128(a0, a0);
			}

			// Token: 0x0600088B RID: 2187 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600088C RID: 2188 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600088D RID: 2189 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600088E RID: 2190 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600088F RID: 2191 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000890 RID: 2192 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000891 RID: 2193 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000892 RID: 2194 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000893 RID: 2195 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000894 RID: 2196 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000895 RID: 2197 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000896 RID: 2198 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000897 RID: 2199 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000898 RID: 2200 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000899 RID: 2201 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600089A RID: 2202 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600089B RID: 2203 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_f32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600089C RID: 2204 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_f32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600089D RID: 2205 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdup_laneq_f64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600089E RID: 2206 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdupq_laneq_f64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600089F RID: 2207 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vdupb_lane_s8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A0 RID: 2208 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vduph_lane_s16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A1 RID: 2209 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vdups_lane_s32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A2 RID: 2210 RVA: 0x00006270 File Offset: 0x00004470
			[DebuggerStepThrough]
			public static long vdupd_lane_s64(v64 a0, int a1)
			{
				return a0.SLong0;
			}

			// Token: 0x060008A3 RID: 2211 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vdupb_lane_u8(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A4 RID: 2212 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vduph_lane_u16(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A5 RID: 2213 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vdups_lane_u32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A6 RID: 2214 RVA: 0x00006268 File Offset: 0x00004468
			[DebuggerStepThrough]
			public static ulong vdupd_lane_u64(v64 a0, int a1)
			{
				return a0.ULong0;
			}

			// Token: 0x060008A7 RID: 2215 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vdups_lane_f32(v64 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008A8 RID: 2216 RVA: 0x00006413 File Offset: 0x00004613
			[DebuggerStepThrough]
			public static double vdupd_lane_f64(v64 a0, int a1)
			{
				return a0.Double0;
			}

			// Token: 0x060008A9 RID: 2217 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vdupb_laneq_s8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008AA RID: 2218 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vduph_laneq_s16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008AB RID: 2219 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vdups_laneq_s32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008AC RID: 2220 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vdupd_laneq_s64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008AD RID: 2221 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vdupb_laneq_u8(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008AE RID: 2222 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vduph_laneq_u16(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008AF RID: 2223 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vdups_laneq_u32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B0 RID: 2224 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vdupd_laneq_u64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B1 RID: 2225 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vdups_laneq_f32(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B2 RID: 2226 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vdupd_laneq_f64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B3 RID: 2227 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B5 RID: 2229 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B6 RID: 2230 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddq_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008B7 RID: 2231 RVA: 0x0000641B File Offset: 0x0000461B
			[DebuggerStepThrough]
			public static v128 vpaddq_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vpaddq_s8(a0, a1);
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x00006424 File Offset: 0x00004624
			[DebuggerStepThrough]
			public static v128 vpaddq_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vpaddq_s16(a0, a1);
			}

			// Token: 0x060008B9 RID: 2233 RVA: 0x0000642D File Offset: 0x0000462D
			[DebuggerStepThrough]
			public static v128 vpaddq_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vpaddq_s32(a0, a1);
			}

			// Token: 0x060008BA RID: 2234 RVA: 0x00006436 File Offset: 0x00004636
			[DebuggerStepThrough]
			public static v128 vpaddq_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vpaddq_s64(a0, a1);
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpaddq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008BD RID: 2237 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008BE RID: 2238 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008BF RID: 2239 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C1 RID: 2241 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C2 RID: 2242 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C3 RID: 2243 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C4 RID: 2244 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C6 RID: 2246 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C7 RID: 2247 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C8 RID: 2248 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008C9 RID: 2249 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_u16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008CA RID: 2250 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008CB RID: 2251 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008CC RID: 2252 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008CD RID: 2253 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpmaxnm_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008CE RID: 2254 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxnmq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008CF RID: 2255 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpmaxnmq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D0 RID: 2256 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vpminnm_f32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D1 RID: 2257 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminnmq_f32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D2 RID: 2258 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vpminnmq_f64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D3 RID: 2259 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vpaddd_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D4 RID: 2260 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vpaddd_u64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D5 RID: 2261 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vpadds_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D6 RID: 2262 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vpaddd_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D7 RID: 2263 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vpmaxs_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D8 RID: 2264 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vpmaxqd_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008D9 RID: 2265 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vpmins_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008DA RID: 2266 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vpminqd_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008DB RID: 2267 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vpmaxnms_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008DC RID: 2268 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vpmaxnmqd_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008DD RID: 2269 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vpminnms_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008DE RID: 2270 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vpminnmqd_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008DF RID: 2271 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vaddv_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E0 RID: 2272 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vaddvq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E1 RID: 2273 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vaddv_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E2 RID: 2274 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vaddvq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E3 RID: 2275 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vaddv_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E4 RID: 2276 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vaddvq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E5 RID: 2277 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vaddvq_s64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E6 RID: 2278 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vaddv_u8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E7 RID: 2279 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vaddvq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E8 RID: 2280 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vaddv_u16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008E9 RID: 2281 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vaddvq_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008EA RID: 2282 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vaddv_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008EB RID: 2283 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vaddvq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008EC RID: 2284 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vaddvq_u64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008ED RID: 2285 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vaddv_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008EE RID: 2286 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vaddvq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008EF RID: 2287 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vaddvq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F0 RID: 2288 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vaddlv_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F1 RID: 2289 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vaddlvq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F2 RID: 2290 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vaddlv_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F3 RID: 2291 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vaddlvq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F4 RID: 2292 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vaddlv_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F5 RID: 2293 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static long vaddlvq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F6 RID: 2294 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vaddlv_u8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F7 RID: 2295 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vaddlvq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F8 RID: 2296 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vaddlv_u16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008F9 RID: 2297 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vaddlvq_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008FA RID: 2298 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vaddlv_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008FB RID: 2299 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ulong vaddlvq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008FC RID: 2300 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vmaxv_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008FD RID: 2301 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vmaxvq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008FE RID: 2302 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vmaxv_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060008FF RID: 2303 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vmaxvq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000900 RID: 2304 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vmaxv_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000901 RID: 2305 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vmaxvq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000902 RID: 2306 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vmaxv_u8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000903 RID: 2307 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vmaxvq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000904 RID: 2308 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vmaxv_u16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000905 RID: 2309 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vmaxvq_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000906 RID: 2310 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vmaxv_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000907 RID: 2311 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vmaxvq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000908 RID: 2312 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmaxv_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000909 RID: 2313 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmaxvq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600090A RID: 2314 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vmaxvq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600090B RID: 2315 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vminv_s8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600090C RID: 2316 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static sbyte vminvq_s8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600090D RID: 2317 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vminv_s16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600090E RID: 2318 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vminvq_s16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600090F RID: 2319 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vminv_s32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000910 RID: 2320 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vminvq_s32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000911 RID: 2321 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vminv_u8(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000912 RID: 2322 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static byte vminvq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000913 RID: 2323 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vminv_u16(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000914 RID: 2324 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static ushort vminvq_u16(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000915 RID: 2325 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vminv_u32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000916 RID: 2326 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vminvq_u32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000917 RID: 2327 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vminv_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000918 RID: 2328 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vminvq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000919 RID: 2329 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vminvq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600091A RID: 2330 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmaxnmv_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600091B RID: 2331 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vmaxnmvq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600091C RID: 2332 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vmaxnmvq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600091D RID: 2333 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vminnmv_f32(v64 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600091E RID: 2334 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vminnmvq_f32(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600091F RID: 2335 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vminnmvq_f64(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000920 RID: 2336 RVA: 0x000061CE File Offset: 0x000043CE
			[DebuggerStepThrough]
			public static v64 vext_f64(v64 a0, v64 a1, int a2)
			{
				return a0;
			}

			// Token: 0x06000921 RID: 2337 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vextq_f64(v128 a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000922 RID: 2338 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vzip1_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000923 RID: 2339 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip1q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000924 RID: 2340 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vzip1_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000925 RID: 2341 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip1q_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000926 RID: 2342 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vzip1_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000927 RID: 2343 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip1q_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000928 RID: 2344 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip1q_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000929 RID: 2345 RVA: 0x0000643F File Offset: 0x0000463F
			[DebuggerStepThrough]
			public static v64 vzip1_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip1_s8(a0, a1);
			}

			// Token: 0x0600092A RID: 2346 RVA: 0x00006448 File Offset: 0x00004648
			[DebuggerStepThrough]
			public static v128 vzip1q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip1q_s8(a0, a1);
			}

			// Token: 0x0600092B RID: 2347 RVA: 0x00006451 File Offset: 0x00004651
			[DebuggerStepThrough]
			public static v64 vzip1_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip1_s16(a0, a1);
			}

			// Token: 0x0600092C RID: 2348 RVA: 0x0000645A File Offset: 0x0000465A
			[DebuggerStepThrough]
			public static v128 vzip1q_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip1q_s16(a0, a1);
			}

			// Token: 0x0600092D RID: 2349 RVA: 0x00006463 File Offset: 0x00004663
			[DebuggerStepThrough]
			public static v64 vzip1_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip1_s32(a0, a1);
			}

			// Token: 0x0600092E RID: 2350 RVA: 0x0000646C File Offset: 0x0000466C
			[DebuggerStepThrough]
			public static v128 vzip1q_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip1q_s32(a0, a1);
			}

			// Token: 0x0600092F RID: 2351 RVA: 0x00006475 File Offset: 0x00004675
			[DebuggerStepThrough]
			public static v128 vzip1q_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip1q_s64(a0, a1);
			}

			// Token: 0x06000930 RID: 2352 RVA: 0x00006463 File Offset: 0x00004663
			[DebuggerStepThrough]
			public static v64 vzip1_f32(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip1_s32(a0, a1);
			}

			// Token: 0x06000931 RID: 2353 RVA: 0x0000646C File Offset: 0x0000466C
			[DebuggerStepThrough]
			public static v128 vzip1q_f32(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip1q_s32(a0, a1);
			}

			// Token: 0x06000932 RID: 2354 RVA: 0x00006475 File Offset: 0x00004675
			[DebuggerStepThrough]
			public static v128 vzip1q_f64(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip1q_s64(a0, a1);
			}

			// Token: 0x06000933 RID: 2355 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vzip2_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000934 RID: 2356 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip2q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000935 RID: 2357 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vzip2_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000936 RID: 2358 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip2q_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000937 RID: 2359 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vzip2_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000938 RID: 2360 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip2q_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000939 RID: 2361 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vzip2q_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600093A RID: 2362 RVA: 0x0000647E File Offset: 0x0000467E
			[DebuggerStepThrough]
			public static v64 vzip2_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip2_s8(a0, a1);
			}

			// Token: 0x0600093B RID: 2363 RVA: 0x00006487 File Offset: 0x00004687
			[DebuggerStepThrough]
			public static v128 vzip2q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip2q_s8(a0, a1);
			}

			// Token: 0x0600093C RID: 2364 RVA: 0x00006490 File Offset: 0x00004690
			[DebuggerStepThrough]
			public static v64 vzip2_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip2_s16(a0, a1);
			}

			// Token: 0x0600093D RID: 2365 RVA: 0x00006499 File Offset: 0x00004699
			[DebuggerStepThrough]
			public static v128 vzip2q_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip2q_s16(a0, a1);
			}

			// Token: 0x0600093E RID: 2366 RVA: 0x000064A2 File Offset: 0x000046A2
			[DebuggerStepThrough]
			public static v64 vzip2_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip2_s32(a0, a1);
			}

			// Token: 0x0600093F RID: 2367 RVA: 0x000064AB File Offset: 0x000046AB
			[DebuggerStepThrough]
			public static v128 vzip2q_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip2q_s32(a0, a1);
			}

			// Token: 0x06000940 RID: 2368 RVA: 0x000064B4 File Offset: 0x000046B4
			[DebuggerStepThrough]
			public static v128 vzip2q_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip2q_s64(a0, a1);
			}

			// Token: 0x06000941 RID: 2369 RVA: 0x000064A2 File Offset: 0x000046A2
			[DebuggerStepThrough]
			public static v64 vzip2_f32(v64 a0, v64 a1)
			{
				return Arm.Neon.vzip2_s32(a0, a1);
			}

			// Token: 0x06000942 RID: 2370 RVA: 0x000064AB File Offset: 0x000046AB
			[DebuggerStepThrough]
			public static v128 vzip2q_f32(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip2q_s32(a0, a1);
			}

			// Token: 0x06000943 RID: 2371 RVA: 0x000064B4 File Offset: 0x000046B4
			[DebuggerStepThrough]
			public static v128 vzip2q_f64(v128 a0, v128 a1)
			{
				return Arm.Neon.vzip2q_s64(a0, a1);
			}

			// Token: 0x06000944 RID: 2372 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuzp1_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000945 RID: 2373 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp1q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000946 RID: 2374 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuzp1_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000947 RID: 2375 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp1q_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000948 RID: 2376 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuzp1_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000949 RID: 2377 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp1q_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600094A RID: 2378 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp1q_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600094B RID: 2379 RVA: 0x000064BD File Offset: 0x000046BD
			[DebuggerStepThrough]
			public static v64 vuzp1_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp1_s8(a0, a1);
			}

			// Token: 0x0600094C RID: 2380 RVA: 0x000064C6 File Offset: 0x000046C6
			[DebuggerStepThrough]
			public static v128 vuzp1q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp1q_s8(a0, a1);
			}

			// Token: 0x0600094D RID: 2381 RVA: 0x000064CF File Offset: 0x000046CF
			[DebuggerStepThrough]
			public static v64 vuzp1_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp1_s16(a0, a1);
			}

			// Token: 0x0600094E RID: 2382 RVA: 0x000064D8 File Offset: 0x000046D8
			[DebuggerStepThrough]
			public static v128 vuzp1q_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp1q_s16(a0, a1);
			}

			// Token: 0x0600094F RID: 2383 RVA: 0x000064E1 File Offset: 0x000046E1
			[DebuggerStepThrough]
			public static v64 vuzp1_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp1_s32(a0, a1);
			}

			// Token: 0x06000950 RID: 2384 RVA: 0x000064EA File Offset: 0x000046EA
			[DebuggerStepThrough]
			public static v128 vuzp1q_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp1q_s32(a0, a1);
			}

			// Token: 0x06000951 RID: 2385 RVA: 0x000064F3 File Offset: 0x000046F3
			[DebuggerStepThrough]
			public static v128 vuzp1q_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp1q_s64(a0, a1);
			}

			// Token: 0x06000952 RID: 2386 RVA: 0x000064E1 File Offset: 0x000046E1
			[DebuggerStepThrough]
			public static v64 vuzp1_f32(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp1_s32(a0, a1);
			}

			// Token: 0x06000953 RID: 2387 RVA: 0x000064EA File Offset: 0x000046EA
			[DebuggerStepThrough]
			public static v128 vuzp1q_f32(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp1q_s32(a0, a1);
			}

			// Token: 0x06000954 RID: 2388 RVA: 0x000064F3 File Offset: 0x000046F3
			[DebuggerStepThrough]
			public static v128 vuzp1q_f64(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp1q_s64(a0, a1);
			}

			// Token: 0x06000955 RID: 2389 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuzp2_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000956 RID: 2390 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp2q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000957 RID: 2391 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuzp2_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000958 RID: 2392 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp2q_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000959 RID: 2393 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vuzp2_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600095A RID: 2394 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp2q_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600095B RID: 2395 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vuzp2q_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600095C RID: 2396 RVA: 0x000064FC File Offset: 0x000046FC
			[DebuggerStepThrough]
			public static v64 vuzp2_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp2_s8(a0, a1);
			}

			// Token: 0x0600095D RID: 2397 RVA: 0x00006505 File Offset: 0x00004705
			[DebuggerStepThrough]
			public static v128 vuzp2q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp2q_s8(a0, a1);
			}

			// Token: 0x0600095E RID: 2398 RVA: 0x0000650E File Offset: 0x0000470E
			[DebuggerStepThrough]
			public static v64 vuzp2_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp2_s16(a0, a1);
			}

			// Token: 0x0600095F RID: 2399 RVA: 0x00006517 File Offset: 0x00004717
			[DebuggerStepThrough]
			public static v128 vuzp2q_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp2q_s16(a0, a1);
			}

			// Token: 0x06000960 RID: 2400 RVA: 0x00006520 File Offset: 0x00004720
			[DebuggerStepThrough]
			public static v64 vuzp2_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp2_s32(a0, a1);
			}

			// Token: 0x06000961 RID: 2401 RVA: 0x00006529 File Offset: 0x00004729
			[DebuggerStepThrough]
			public static v128 vuzp2q_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp2q_s32(a0, a1);
			}

			// Token: 0x06000962 RID: 2402 RVA: 0x00006532 File Offset: 0x00004732
			[DebuggerStepThrough]
			public static v128 vuzp2q_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp2q_s64(a0, a1);
			}

			// Token: 0x06000963 RID: 2403 RVA: 0x00006520 File Offset: 0x00004720
			[DebuggerStepThrough]
			public static v64 vuzp2_f32(v64 a0, v64 a1)
			{
				return Arm.Neon.vuzp2_s32(a0, a1);
			}

			// Token: 0x06000964 RID: 2404 RVA: 0x00006529 File Offset: 0x00004729
			[DebuggerStepThrough]
			public static v128 vuzp2q_f32(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp2q_s32(a0, a1);
			}

			// Token: 0x06000965 RID: 2405 RVA: 0x00006532 File Offset: 0x00004732
			[DebuggerStepThrough]
			public static v128 vuzp2q_f64(v128 a0, v128 a1)
			{
				return Arm.Neon.vuzp2q_s64(a0, a1);
			}

			// Token: 0x06000966 RID: 2406 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtrn1_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000967 RID: 2407 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn1q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000968 RID: 2408 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtrn1_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000969 RID: 2409 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn1q_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600096A RID: 2410 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtrn1_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600096B RID: 2411 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn1q_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600096C RID: 2412 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn1q_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600096D RID: 2413 RVA: 0x0000653B File Offset: 0x0000473B
			[DebuggerStepThrough]
			public static v64 vtrn1_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn1_s8(a0, a1);
			}

			// Token: 0x0600096E RID: 2414 RVA: 0x00006544 File Offset: 0x00004744
			[DebuggerStepThrough]
			public static v128 vtrn1q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn1q_s8(a0, a1);
			}

			// Token: 0x0600096F RID: 2415 RVA: 0x0000654D File Offset: 0x0000474D
			[DebuggerStepThrough]
			public static v64 vtrn1_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn1_s16(a0, a1);
			}

			// Token: 0x06000970 RID: 2416 RVA: 0x00006556 File Offset: 0x00004756
			[DebuggerStepThrough]
			public static v128 vtrn1q_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn1q_s16(a0, a1);
			}

			// Token: 0x06000971 RID: 2417 RVA: 0x0000655F File Offset: 0x0000475F
			[DebuggerStepThrough]
			public static v64 vtrn1_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn1_s32(a0, a1);
			}

			// Token: 0x06000972 RID: 2418 RVA: 0x00006568 File Offset: 0x00004768
			[DebuggerStepThrough]
			public static v128 vtrn1q_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn1q_s32(a0, a1);
			}

			// Token: 0x06000973 RID: 2419 RVA: 0x00006571 File Offset: 0x00004771
			[DebuggerStepThrough]
			public static v128 vtrn1q_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn1q_s64(a0, a1);
			}

			// Token: 0x06000974 RID: 2420 RVA: 0x0000655F File Offset: 0x0000475F
			[DebuggerStepThrough]
			public static v64 vtrn1_f32(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn1_s32(a0, a1);
			}

			// Token: 0x06000975 RID: 2421 RVA: 0x00006568 File Offset: 0x00004768
			[DebuggerStepThrough]
			public static v128 vtrn1q_f32(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn1q_s32(a0, a1);
			}

			// Token: 0x06000976 RID: 2422 RVA: 0x00006571 File Offset: 0x00004771
			[DebuggerStepThrough]
			public static v128 vtrn1q_f64(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn1q_s64(a0, a1);
			}

			// Token: 0x06000977 RID: 2423 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtrn2_s8(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000978 RID: 2424 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn2q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000979 RID: 2425 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtrn2_s16(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600097A RID: 2426 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn2q_s16(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600097B RID: 2427 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vtrn2_s32(v64 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600097C RID: 2428 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn2q_s32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600097D RID: 2429 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vtrn2q_s64(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600097E RID: 2430 RVA: 0x0000657A File Offset: 0x0000477A
			[DebuggerStepThrough]
			public static v64 vtrn2_u8(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn2_s8(a0, a1);
			}

			// Token: 0x0600097F RID: 2431 RVA: 0x00006583 File Offset: 0x00004783
			[DebuggerStepThrough]
			public static v128 vtrn2q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn2q_s8(a0, a1);
			}

			// Token: 0x06000980 RID: 2432 RVA: 0x0000658C File Offset: 0x0000478C
			[DebuggerStepThrough]
			public static v64 vtrn2_u16(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn2_s16(a0, a1);
			}

			// Token: 0x06000981 RID: 2433 RVA: 0x00006595 File Offset: 0x00004795
			[DebuggerStepThrough]
			public static v128 vtrn2q_u16(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn2q_s16(a0, a1);
			}

			// Token: 0x06000982 RID: 2434 RVA: 0x0000659E File Offset: 0x0000479E
			[DebuggerStepThrough]
			public static v64 vtrn2_u32(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn2_s32(a0, a1);
			}

			// Token: 0x06000983 RID: 2435 RVA: 0x000065A7 File Offset: 0x000047A7
			[DebuggerStepThrough]
			public static v128 vtrn2q_u32(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn2q_s32(a0, a1);
			}

			// Token: 0x06000984 RID: 2436 RVA: 0x000065B0 File Offset: 0x000047B0
			[DebuggerStepThrough]
			public static v128 vtrn2q_u64(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn2q_s64(a0, a1);
			}

			// Token: 0x06000985 RID: 2437 RVA: 0x0000659E File Offset: 0x0000479E
			[DebuggerStepThrough]
			public static v64 vtrn2_f32(v64 a0, v64 a1)
			{
				return Arm.Neon.vtrn2_s32(a0, a1);
			}

			// Token: 0x06000986 RID: 2438 RVA: 0x000065A7 File Offset: 0x000047A7
			[DebuggerStepThrough]
			public static v128 vtrn2q_f32(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn2q_s32(a0, a1);
			}

			// Token: 0x06000987 RID: 2439 RVA: 0x000065B0 File Offset: 0x000047B0
			[DebuggerStepThrough]
			public static v128 vtrn2q_f64(v128 a0, v128 a1)
			{
				return Arm.Neon.vtrn2q_s64(a0, a1);
			}

			// Token: 0x06000988 RID: 2440 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqtbl1_s8(v128 a0, v64 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000989 RID: 2441 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqtbl1q_s8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600098A RID: 2442 RVA: 0x000065B9 File Offset: 0x000047B9
			[DebuggerStepThrough]
			public static v64 vqtbl1_u8(v128 a0, v64 a1)
			{
				return Arm.Neon.vqtbl1_s8(a0, a1);
			}

			// Token: 0x0600098B RID: 2443 RVA: 0x000065C2 File Offset: 0x000047C2
			[DebuggerStepThrough]
			public static v128 vqtbl1q_u8(v128 a0, v128 a1)
			{
				return Arm.Neon.vqtbl1q_s8(a0, a1);
			}

			// Token: 0x0600098C RID: 2444 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqtbx1_s8(v64 a0, v128 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600098D RID: 2445 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqtbx1q_s8(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600098E RID: 2446 RVA: 0x000065CB File Offset: 0x000047CB
			[DebuggerStepThrough]
			public static v64 vqtbx1_u8(v64 a0, v128 a1, v64 a2)
			{
				return Arm.Neon.vqtbx1_s8(a0, a1, a2);
			}

			// Token: 0x0600098F RID: 2447 RVA: 0x000065D5 File Offset: 0x000047D5
			[DebuggerStepThrough]
			public static v128 vqtbx1q_u8(v128 a0, v128 a1, v128 a2)
			{
				return Arm.Neon.vqtbx1q_s8(a0, a1, a2);
			}

			// Token: 0x06000990 RID: 2448 RVA: 0x00006413 File Offset: 0x00004613
			[DebuggerStepThrough]
			public static double vget_lane_f64(v64 a0, int a1)
			{
				return a0.Double0;
			}

			// Token: 0x06000991 RID: 2449 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vgetq_lane_f64(v128 a0, int a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000992 RID: 2450 RVA: 0x000065DF File Offset: 0x000047DF
			[DebuggerStepThrough]
			public static v64 vset_lane_f64(double a0, v64 a1, int a2)
			{
				return new v64(a0);
			}

			// Token: 0x06000993 RID: 2451 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsetq_lane_f64(double a0, v128 a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000994 RID: 2452 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static float vrecpxs_f32(float a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000995 RID: 2453 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static double vrecpxd_f64(double a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000996 RID: 2454 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_n_f32(v64 a0, v64 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000997 RID: 2455 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_n_f32(v128 a0, v128 a1, float a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000998 RID: 2456 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfma_n_f64(v64 a0, v64 a1, double a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000999 RID: 2457 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmaq_n_f64(v128 a0, v128 a1, double a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vfms_n_f64(v64 a0, v64 a1, double a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600099B RID: 2459 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vfmsq_n_f64(v128 a0, v128 a1, double a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x0600099C RID: 2460 RVA: 0x00002556 File Offset: 0x00000756
			public static bool IsNeonCryptoSupported
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600099D RID: 2461 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha1cq_u32(v128 a0, uint a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600099E RID: 2462 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha1pq_u32(v128 a0, uint a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x0600099F RID: 2463 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha1mq_u32(v128 a0, uint a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A0 RID: 2464 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint vsha1h_u32(uint a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A1 RID: 2465 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha1su0q_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A2 RID: 2466 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha1su1q_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A3 RID: 2467 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha256hq_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A4 RID: 2468 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha256h2q_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A5 RID: 2469 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha256su0q_u32(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A6 RID: 2470 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vsha256su1q_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32b(uint a0, byte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A8 RID: 2472 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32h(uint a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009A9 RID: 2473 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32w(uint a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009AA RID: 2474 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32d(uint a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009AB RID: 2475 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32cb(uint a0, byte a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009AC RID: 2476 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32ch(uint a0, ushort a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009AD RID: 2477 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32cw(uint a0, uint a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009AE RID: 2478 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static uint __crc32cd(uint a0, ulong a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009AF RID: 2479 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaeseq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B0 RID: 2480 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaesdq_u8(v128 a0, v128 a1)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B1 RID: 2481 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaesmcq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B2 RID: 2482 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vaesimcq_u8(v128 a0)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00002556 File Offset: 0x00000756
			public static bool IsNeonDotProdSupported
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060009B4 RID: 2484 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdot_u32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B5 RID: 2485 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdot_s32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B6 RID: 2486 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdotq_u32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B7 RID: 2487 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdotq_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B8 RID: 2488 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdot_lane_u32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009B9 RID: 2489 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdot_lane_s32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009BA RID: 2490 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdotq_laneq_u32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009BB RID: 2491 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdotq_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009BC RID: 2492 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdot_laneq_u32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009BD RID: 2493 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vdot_laneq_s32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009BE RID: 2494 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdotq_lane_u32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009BF RID: 2495 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vdotq_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00002556 File Offset: 0x00000756
			public static bool IsNeonRDMASupported
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060009C1 RID: 2497 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlah_s16(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C2 RID: 2498 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlah_s32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C3 RID: 2499 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlahq_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C4 RID: 2500 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlahq_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C5 RID: 2501 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlsh_s16(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C6 RID: 2502 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlsh_s32(v64 a0, v64 a1, v64 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C7 RID: 2503 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlshq_s16(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C8 RID: 2504 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlshq_s32(v128 a0, v128 a1, v128 a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009C9 RID: 2505 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlah_lane_s16(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009CA RID: 2506 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlahq_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009CB RID: 2507 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlah_laneq_s16(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009CC RID: 2508 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlahq_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009CD RID: 2509 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlah_lane_s32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009CE RID: 2510 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlahq_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009CF RID: 2511 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlah_laneq_s32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D0 RID: 2512 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlahq_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D1 RID: 2513 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlsh_lane_s16(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D2 RID: 2514 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlshq_lane_s16(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D3 RID: 2515 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlsh_laneq_s16(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D4 RID: 2516 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlshq_laneq_s16(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D5 RID: 2517 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlsh_lane_s32(v64 a0, v64 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D6 RID: 2518 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlshq_lane_s32(v128 a0, v128 a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D7 RID: 2519 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v64 vqrdmlsh_laneq_s32(v64 a0, v64 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D8 RID: 2520 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static v128 vqrdmlshq_laneq_s32(v128 a0, v128 a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009D9 RID: 2521 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmlahh_s16(short a0, short a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009DA RID: 2522 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmlahs_s32(int a0, int a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009DB RID: 2523 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmlshh_s16(short a0, short a1, short a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009DC RID: 2524 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmlshs_s32(int a0, int a1, int a2)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009DD RID: 2525 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmlahh_lane_s16(short a0, short a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009DE RID: 2526 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmlahh_laneq_s16(short a0, short a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009DF RID: 2527 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmlahs_lane_s32(int a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009E0 RID: 2528 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmlshh_lane_s16(short a0, short a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009E1 RID: 2529 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static short vqrdmlshh_laneq_s16(short a0, short a1, v128 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x000022C6 File Offset: 0x000004C6
			[DebuggerStepThrough]
			public static int vqrdmlshs_lane_s32(int a0, int a1, v64 a2, int a3)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060009E3 RID: 2531 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_s8(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009E4 RID: 2532 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_s16(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009E5 RID: 2533 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_s32(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_s64(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009E7 RID: 2535 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_u8(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009E8 RID: 2536 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_u16(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009E9 RID: 2537 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_u32(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009EA RID: 2538 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_u64(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009EB RID: 2539 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_f16(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009EC RID: 2540 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_f32(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009ED RID: 2541 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vcreate_f64(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009EE RID: 2542 RVA: 0x000065E7 File Offset: 0x000047E7
			[DebuggerStepThrough]
			public static v64 vdup_n_s8(sbyte a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009EF RID: 2543 RVA: 0x000065EF File Offset: 0x000047EF
			[DebuggerStepThrough]
			public static v128 vdupq_n_s8(sbyte a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009F0 RID: 2544 RVA: 0x000065F7 File Offset: 0x000047F7
			[DebuggerStepThrough]
			public static v64 vdup_n_s16(short a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009F1 RID: 2545 RVA: 0x000065FF File Offset: 0x000047FF
			[DebuggerStepThrough]
			public static v128 vdupq_n_s16(short a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009F2 RID: 2546 RVA: 0x00006607 File Offset: 0x00004807
			[DebuggerStepThrough]
			public static v64 vdup_n_s32(int a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009F3 RID: 2547 RVA: 0x0000660F File Offset: 0x0000480F
			[DebuggerStepThrough]
			public static v128 vdupq_n_s32(int a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009F4 RID: 2548 RVA: 0x00006280 File Offset: 0x00004480
			[DebuggerStepThrough]
			public static v64 vdup_n_s64(long a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009F5 RID: 2549 RVA: 0x00006617 File Offset: 0x00004817
			[DebuggerStepThrough]
			public static v128 vdupq_n_s64(long a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009F6 RID: 2550 RVA: 0x0000661F File Offset: 0x0000481F
			[DebuggerStepThrough]
			public static v64 vdup_n_u8(byte a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009F7 RID: 2551 RVA: 0x00006627 File Offset: 0x00004827
			[DebuggerStepThrough]
			public static v128 vdupq_n_u8(byte a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009F8 RID: 2552 RVA: 0x0000662F File Offset: 0x0000482F
			[DebuggerStepThrough]
			public static v64 vdup_n_u16(ushort a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009F9 RID: 2553 RVA: 0x00006637 File Offset: 0x00004837
			[DebuggerStepThrough]
			public static v128 vdupq_n_u16(ushort a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009FA RID: 2554 RVA: 0x0000663F File Offset: 0x0000483F
			[DebuggerStepThrough]
			public static v64 vdup_n_u32(uint a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009FB RID: 2555 RVA: 0x00006647 File Offset: 0x00004847
			[DebuggerStepThrough]
			public static v128 vdupq_n_u32(uint a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009FC RID: 2556 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vdup_n_u64(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009FD RID: 2557 RVA: 0x0000664F File Offset: 0x0000484F
			[DebuggerStepThrough]
			public static v128 vdupq_n_u64(ulong a0)
			{
				return new v128(a0);
			}

			// Token: 0x060009FE RID: 2558 RVA: 0x00006657 File Offset: 0x00004857
			[DebuggerStepThrough]
			public static v64 vdup_n_f32(float a0)
			{
				return new v64(a0);
			}

			// Token: 0x060009FF RID: 2559 RVA: 0x0000665F File Offset: 0x0000485F
			[DebuggerStepThrough]
			public static v128 vdupq_n_f32(float a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A00 RID: 2560 RVA: 0x000065DF File Offset: 0x000047DF
			[DebuggerStepThrough]
			public static v64 vdup_n_f64(double a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A01 RID: 2561 RVA: 0x00006667 File Offset: 0x00004867
			[DebuggerStepThrough]
			public static v128 vdupq_n_f64(double a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A02 RID: 2562 RVA: 0x000065E7 File Offset: 0x000047E7
			[DebuggerStepThrough]
			public static v64 vmov_n_s8(sbyte a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A03 RID: 2563 RVA: 0x000065EF File Offset: 0x000047EF
			[DebuggerStepThrough]
			public static v128 vmovq_n_s8(sbyte a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A04 RID: 2564 RVA: 0x000065F7 File Offset: 0x000047F7
			[DebuggerStepThrough]
			public static v64 vmov_n_s16(short a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A05 RID: 2565 RVA: 0x000065FF File Offset: 0x000047FF
			[DebuggerStepThrough]
			public static v128 vmovq_n_s16(short a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A06 RID: 2566 RVA: 0x00006607 File Offset: 0x00004807
			[DebuggerStepThrough]
			public static v64 vmov_n_s32(int a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A07 RID: 2567 RVA: 0x0000660F File Offset: 0x0000480F
			[DebuggerStepThrough]
			public static v128 vmovq_n_s32(int a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A08 RID: 2568 RVA: 0x00006280 File Offset: 0x00004480
			[DebuggerStepThrough]
			public static v64 vmov_n_s64(long a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A09 RID: 2569 RVA: 0x00006617 File Offset: 0x00004817
			[DebuggerStepThrough]
			public static v128 vmovq_n_s64(long a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A0A RID: 2570 RVA: 0x0000661F File Offset: 0x0000481F
			[DebuggerStepThrough]
			public static v64 vmov_n_u8(byte a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A0B RID: 2571 RVA: 0x00006627 File Offset: 0x00004827
			[DebuggerStepThrough]
			public static v128 vmovq_n_u8(byte a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A0C RID: 2572 RVA: 0x0000662F File Offset: 0x0000482F
			[DebuggerStepThrough]
			public static v64 vmov_n_u16(ushort a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A0D RID: 2573 RVA: 0x00006637 File Offset: 0x00004837
			[DebuggerStepThrough]
			public static v128 vmovq_n_u16(ushort a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A0E RID: 2574 RVA: 0x0000663F File Offset: 0x0000483F
			[DebuggerStepThrough]
			public static v64 vmov_n_u32(uint a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A0F RID: 2575 RVA: 0x00006647 File Offset: 0x00004847
			[DebuggerStepThrough]
			public static v128 vmovq_n_u32(uint a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A10 RID: 2576 RVA: 0x00006278 File Offset: 0x00004478
			[DebuggerStepThrough]
			public static v64 vmov_n_u64(ulong a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A11 RID: 2577 RVA: 0x0000664F File Offset: 0x0000484F
			[DebuggerStepThrough]
			public static v128 vmovq_n_u64(ulong a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A12 RID: 2578 RVA: 0x00006657 File Offset: 0x00004857
			[DebuggerStepThrough]
			public static v64 vmov_n_f32(float a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A13 RID: 2579 RVA: 0x0000665F File Offset: 0x0000485F
			[DebuggerStepThrough]
			public static v128 vmovq_n_f32(float a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A14 RID: 2580 RVA: 0x000065DF File Offset: 0x000047DF
			[DebuggerStepThrough]
			public static v64 vmov_n_f64(double a0)
			{
				return new v64(a0);
			}

			// Token: 0x06000A15 RID: 2581 RVA: 0x00006667 File Offset: 0x00004867
			[DebuggerStepThrough]
			public static v128 vmovq_n_f64(double a0)
			{
				return new v128(a0);
			}

			// Token: 0x06000A16 RID: 2582 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_s8(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A17 RID: 2583 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_s16(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A18 RID: 2584 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_s32(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A19 RID: 2585 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_s64(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A1A RID: 2586 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_u8(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A1B RID: 2587 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_u16(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A1C RID: 2588 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_u32(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A1D RID: 2589 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_u64(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A1E RID: 2590 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_f16(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A1F RID: 2591 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_f32(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A20 RID: 2592 RVA: 0x0000666F File Offset: 0x0000486F
			[DebuggerStepThrough]
			public static v128 vcombine_f64(v64 a0, v64 a1)
			{
				return new v128(a0, a1);
			}

			// Token: 0x06000A21 RID: 2593 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_s8(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A22 RID: 2594 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_s16(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A23 RID: 2595 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_s32(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A24 RID: 2596 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_s64(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A25 RID: 2597 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_u8(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A26 RID: 2598 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_u16(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A27 RID: 2599 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_u32(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A28 RID: 2600 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_u64(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A29 RID: 2601 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_f32(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A2A RID: 2602 RVA: 0x00006678 File Offset: 0x00004878
			[DebuggerStepThrough]
			public static v64 vget_high_f64(v128 a0)
			{
				return a0.Hi64;
			}

			// Token: 0x06000A2B RID: 2603 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_s8(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A2C RID: 2604 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_s16(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A2D RID: 2605 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_s32(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A2E RID: 2606 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_s64(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A2F RID: 2607 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_u8(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A30 RID: 2608 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_u16(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A31 RID: 2609 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_u32(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A32 RID: 2610 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_u64(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A33 RID: 2611 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_f32(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A34 RID: 2612 RVA: 0x00006680 File Offset: 0x00004880
			[DebuggerStepThrough]
			public static v64 vget_low_f64(v128 a0)
			{
				return a0.Lo64;
			}

			// Token: 0x06000A35 RID: 2613 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_s8(sbyte* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A36 RID: 2614 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_s8(sbyte* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A37 RID: 2615 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_s16(short* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A38 RID: 2616 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_s16(short* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A39 RID: 2617 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_s32(int* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A3A RID: 2618 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_s32(int* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A3B RID: 2619 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_s64(long* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A3C RID: 2620 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_s64(long* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A3D RID: 2621 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_u8(byte* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A3E RID: 2622 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_u8(byte* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A3F RID: 2623 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_u16(ushort* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A40 RID: 2624 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_u16(ushort* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A41 RID: 2625 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_u32(uint* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A42 RID: 2626 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_u32(uint* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A43 RID: 2627 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_u64(ulong* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A44 RID: 2628 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_u64(ulong* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A45 RID: 2629 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_f32(float* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A46 RID: 2630 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_f32(float* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A47 RID: 2631 RVA: 0x00006688 File Offset: 0x00004888
			[DebuggerStepThrough]
			public unsafe static v64 vld1_f64(double* a0)
			{
				return *(v64*)a0;
			}

			// Token: 0x06000A48 RID: 2632 RVA: 0x00006690 File Offset: 0x00004890
			[DebuggerStepThrough]
			public unsafe static v128 vld1q_f64(double* a0)
			{
				return *(v128*)a0;
			}

			// Token: 0x06000A49 RID: 2633 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_s8(sbyte* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A4A RID: 2634 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_s8(sbyte* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A4B RID: 2635 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_s16(short* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A4C RID: 2636 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_s16(short* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A4D RID: 2637 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_s32(int* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A4E RID: 2638 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_s32(int* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A4F RID: 2639 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_s64(long* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A50 RID: 2640 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_s64(long* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A51 RID: 2641 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_u8(byte* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A52 RID: 2642 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_u8(byte* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A53 RID: 2643 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_u16(ushort* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A54 RID: 2644 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_u16(ushort* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A55 RID: 2645 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_u32(uint* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A56 RID: 2646 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_u32(uint* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A57 RID: 2647 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_u64(ulong* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A58 RID: 2648 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_u64(ulong* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A59 RID: 2649 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_f32(float* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A5A RID: 2650 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_f32(float* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}

			// Token: 0x06000A5B RID: 2651 RVA: 0x00006698 File Offset: 0x00004898
			[DebuggerStepThrough]
			public unsafe static void vst1_f64(double* a0, v64 a1)
			{
				*(v64*)a0 = a1;
			}

			// Token: 0x06000A5C RID: 2652 RVA: 0x000066A1 File Offset: 0x000048A1
			[DebuggerStepThrough]
			public unsafe static void vst1q_f64(double* a0, v128 a1)
			{
				*(v128*)a0 = a1;
			}
		}
	}
}
