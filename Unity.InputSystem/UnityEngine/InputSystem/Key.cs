using System;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200003B RID: 59
	public enum Key
	{
		// Token: 0x0400018D RID: 397
		None,
		// Token: 0x0400018E RID: 398
		Space,
		// Token: 0x0400018F RID: 399
		Enter,
		// Token: 0x04000190 RID: 400
		Tab,
		// Token: 0x04000191 RID: 401
		Backquote,
		// Token: 0x04000192 RID: 402
		Quote,
		// Token: 0x04000193 RID: 403
		Semicolon,
		// Token: 0x04000194 RID: 404
		Comma,
		// Token: 0x04000195 RID: 405
		Period,
		// Token: 0x04000196 RID: 406
		Slash,
		// Token: 0x04000197 RID: 407
		Backslash,
		// Token: 0x04000198 RID: 408
		LeftBracket,
		// Token: 0x04000199 RID: 409
		RightBracket,
		// Token: 0x0400019A RID: 410
		Minus,
		// Token: 0x0400019B RID: 411
		Equals,
		// Token: 0x0400019C RID: 412
		A,
		// Token: 0x0400019D RID: 413
		B,
		// Token: 0x0400019E RID: 414
		C,
		// Token: 0x0400019F RID: 415
		D,
		// Token: 0x040001A0 RID: 416
		E,
		// Token: 0x040001A1 RID: 417
		F,
		// Token: 0x040001A2 RID: 418
		G,
		// Token: 0x040001A3 RID: 419
		H,
		// Token: 0x040001A4 RID: 420
		I,
		// Token: 0x040001A5 RID: 421
		J,
		// Token: 0x040001A6 RID: 422
		K,
		// Token: 0x040001A7 RID: 423
		L,
		// Token: 0x040001A8 RID: 424
		M,
		// Token: 0x040001A9 RID: 425
		N,
		// Token: 0x040001AA RID: 426
		O,
		// Token: 0x040001AB RID: 427
		P,
		// Token: 0x040001AC RID: 428
		Q,
		// Token: 0x040001AD RID: 429
		R,
		// Token: 0x040001AE RID: 430
		S,
		// Token: 0x040001AF RID: 431
		T,
		// Token: 0x040001B0 RID: 432
		U,
		// Token: 0x040001B1 RID: 433
		V,
		// Token: 0x040001B2 RID: 434
		W,
		// Token: 0x040001B3 RID: 435
		X,
		// Token: 0x040001B4 RID: 436
		Y,
		// Token: 0x040001B5 RID: 437
		Z,
		// Token: 0x040001B6 RID: 438
		Digit1,
		// Token: 0x040001B7 RID: 439
		Digit2,
		// Token: 0x040001B8 RID: 440
		Digit3,
		// Token: 0x040001B9 RID: 441
		Digit4,
		// Token: 0x040001BA RID: 442
		Digit5,
		// Token: 0x040001BB RID: 443
		Digit6,
		// Token: 0x040001BC RID: 444
		Digit7,
		// Token: 0x040001BD RID: 445
		Digit8,
		// Token: 0x040001BE RID: 446
		Digit9,
		// Token: 0x040001BF RID: 447
		Digit0,
		// Token: 0x040001C0 RID: 448
		LeftShift,
		// Token: 0x040001C1 RID: 449
		RightShift,
		// Token: 0x040001C2 RID: 450
		LeftAlt,
		// Token: 0x040001C3 RID: 451
		RightAlt,
		// Token: 0x040001C4 RID: 452
		AltGr = 54,
		// Token: 0x040001C5 RID: 453
		LeftCtrl,
		// Token: 0x040001C6 RID: 454
		RightCtrl,
		// Token: 0x040001C7 RID: 455
		LeftMeta,
		// Token: 0x040001C8 RID: 456
		RightMeta,
		// Token: 0x040001C9 RID: 457
		LeftWindows = 57,
		// Token: 0x040001CA RID: 458
		RightWindows,
		// Token: 0x040001CB RID: 459
		LeftApple = 57,
		// Token: 0x040001CC RID: 460
		RightApple,
		// Token: 0x040001CD RID: 461
		LeftCommand = 57,
		// Token: 0x040001CE RID: 462
		RightCommand,
		// Token: 0x040001CF RID: 463
		ContextMenu,
		// Token: 0x040001D0 RID: 464
		Escape,
		// Token: 0x040001D1 RID: 465
		LeftArrow,
		// Token: 0x040001D2 RID: 466
		RightArrow,
		// Token: 0x040001D3 RID: 467
		UpArrow,
		// Token: 0x040001D4 RID: 468
		DownArrow,
		// Token: 0x040001D5 RID: 469
		Backspace,
		// Token: 0x040001D6 RID: 470
		PageDown,
		// Token: 0x040001D7 RID: 471
		PageUp,
		// Token: 0x040001D8 RID: 472
		Home,
		// Token: 0x040001D9 RID: 473
		End,
		// Token: 0x040001DA RID: 474
		Insert,
		// Token: 0x040001DB RID: 475
		Delete,
		// Token: 0x040001DC RID: 476
		CapsLock,
		// Token: 0x040001DD RID: 477
		NumLock,
		// Token: 0x040001DE RID: 478
		PrintScreen,
		// Token: 0x040001DF RID: 479
		ScrollLock,
		// Token: 0x040001E0 RID: 480
		Pause,
		// Token: 0x040001E1 RID: 481
		NumpadEnter,
		// Token: 0x040001E2 RID: 482
		NumpadDivide,
		// Token: 0x040001E3 RID: 483
		NumpadMultiply,
		// Token: 0x040001E4 RID: 484
		NumpadPlus,
		// Token: 0x040001E5 RID: 485
		NumpadMinus,
		// Token: 0x040001E6 RID: 486
		NumpadPeriod,
		// Token: 0x040001E7 RID: 487
		NumpadEquals,
		// Token: 0x040001E8 RID: 488
		Numpad0,
		// Token: 0x040001E9 RID: 489
		Numpad1,
		// Token: 0x040001EA RID: 490
		Numpad2,
		// Token: 0x040001EB RID: 491
		Numpad3,
		// Token: 0x040001EC RID: 492
		Numpad4,
		// Token: 0x040001ED RID: 493
		Numpad5,
		// Token: 0x040001EE RID: 494
		Numpad6,
		// Token: 0x040001EF RID: 495
		Numpad7,
		// Token: 0x040001F0 RID: 496
		Numpad8,
		// Token: 0x040001F1 RID: 497
		Numpad9,
		// Token: 0x040001F2 RID: 498
		F1,
		// Token: 0x040001F3 RID: 499
		F2,
		// Token: 0x040001F4 RID: 500
		F3,
		// Token: 0x040001F5 RID: 501
		F4,
		// Token: 0x040001F6 RID: 502
		F5,
		// Token: 0x040001F7 RID: 503
		F6,
		// Token: 0x040001F8 RID: 504
		F7,
		// Token: 0x040001F9 RID: 505
		F8,
		// Token: 0x040001FA RID: 506
		F9,
		// Token: 0x040001FB RID: 507
		F10,
		// Token: 0x040001FC RID: 508
		F11,
		// Token: 0x040001FD RID: 509
		F12,
		// Token: 0x040001FE RID: 510
		OEM1,
		// Token: 0x040001FF RID: 511
		OEM2,
		// Token: 0x04000200 RID: 512
		OEM3,
		// Token: 0x04000201 RID: 513
		OEM4,
		// Token: 0x04000202 RID: 514
		OEM5,
		// Token: 0x04000203 RID: 515
		[Obsolete("IMESelected is a dummy key and will be removed in the future. Please use ButtonControl imeSelected")]
		IMESelected,
		// Token: 0x04000204 RID: 516
		F13,
		// Token: 0x04000205 RID: 517
		F14,
		// Token: 0x04000206 RID: 518
		F15,
		// Token: 0x04000207 RID: 519
		F16,
		// Token: 0x04000208 RID: 520
		F17,
		// Token: 0x04000209 RID: 521
		F18,
		// Token: 0x0400020A RID: 522
		F19,
		// Token: 0x0400020B RID: 523
		F20,
		// Token: 0x0400020C RID: 524
		F21,
		// Token: 0x0400020D RID: 525
		F22,
		// Token: 0x0400020E RID: 526
		F23,
		// Token: 0x0400020F RID: 527
		F24
	}
}
