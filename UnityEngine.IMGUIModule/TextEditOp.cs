using System;

namespace UnityEngine
{
	// Token: 0x0200003C RID: 60
	internal enum TextEditOp
	{
		// Token: 0x04000141 RID: 321
		MoveLeft,
		// Token: 0x04000142 RID: 322
		MoveRight,
		// Token: 0x04000143 RID: 323
		MoveUp,
		// Token: 0x04000144 RID: 324
		MoveDown,
		// Token: 0x04000145 RID: 325
		MoveLineStart,
		// Token: 0x04000146 RID: 326
		MoveLineEnd,
		// Token: 0x04000147 RID: 327
		MoveTextStart,
		// Token: 0x04000148 RID: 328
		MoveTextEnd,
		// Token: 0x04000149 RID: 329
		MovePageUp,
		// Token: 0x0400014A RID: 330
		MovePageDown,
		// Token: 0x0400014B RID: 331
		MoveGraphicalLineStart,
		// Token: 0x0400014C RID: 332
		MoveGraphicalLineEnd,
		// Token: 0x0400014D RID: 333
		MoveWordLeft,
		// Token: 0x0400014E RID: 334
		MoveWordRight,
		// Token: 0x0400014F RID: 335
		MoveParagraphForward,
		// Token: 0x04000150 RID: 336
		MoveParagraphBackward,
		// Token: 0x04000151 RID: 337
		MoveToStartOfNextWord,
		// Token: 0x04000152 RID: 338
		MoveToEndOfPreviousWord,
		// Token: 0x04000153 RID: 339
		Delete,
		// Token: 0x04000154 RID: 340
		Backspace,
		// Token: 0x04000155 RID: 341
		DeleteWordBack,
		// Token: 0x04000156 RID: 342
		DeleteWordForward,
		// Token: 0x04000157 RID: 343
		DeleteLineBack,
		// Token: 0x04000158 RID: 344
		Cut,
		// Token: 0x04000159 RID: 345
		Paste,
		// Token: 0x0400015A RID: 346
		ScrollStart,
		// Token: 0x0400015B RID: 347
		ScrollEnd,
		// Token: 0x0400015C RID: 348
		ScrollPageUp,
		// Token: 0x0400015D RID: 349
		ScrollPageDown
	}
}
