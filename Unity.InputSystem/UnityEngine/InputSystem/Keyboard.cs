using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200003D RID: 61
	[InputControlLayout(stateType = typeof(KeyboardState), isGenericTypeOfDevice = true)]
	public class Keyboard : InputDevice, ITextInputReceiver, IEventPreProcessor
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060004F0 RID: 1264 RVA: 0x00014C56 File Offset: 0x00012E56
		// (remove) Token: 0x060004F1 RID: 1265 RVA: 0x00014C81 File Offset: 0x00012E81
		public event Action<char> onTextInput
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.m_TextInputListeners.Contains(value))
				{
					this.m_TextInputListeners.Append(value);
				}
			}
			remove
			{
				this.m_TextInputListeners.Remove(value);
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060004F2 RID: 1266 RVA: 0x00014C8F File Offset: 0x00012E8F
		// (remove) Token: 0x060004F3 RID: 1267 RVA: 0x00014CBA File Offset: 0x00012EBA
		public event Action<IMECompositionString> onIMECompositionChange
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.m_ImeCompositionListeners.Contains(value))
				{
					this.m_ImeCompositionListeners.Append(value);
				}
			}
			remove
			{
				this.m_ImeCompositionListeners.Remove(value);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00014CC8 File Offset: 0x00012EC8
		public void SetIMEEnabled(bool enabled)
		{
			EnableIMECompositionCommand enableIMECompositionCommand = EnableIMECompositionCommand.Create(enabled);
			base.ExecuteCommand<EnableIMECompositionCommand>(ref enableIMECompositionCommand);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00014CE8 File Offset: 0x00012EE8
		public void SetIMECursorPosition(Vector2 position)
		{
			SetIMECursorPositionCommand setIMECursorPositionCommand = SetIMECursorPositionCommand.Create(position);
			base.ExecuteCommand<SetIMECursorPositionCommand>(ref setIMECursorPositionCommand);
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00014D05 File Offset: 0x00012F05
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00014D13 File Offset: 0x00012F13
		public string keyboardLayout
		{
			get
			{
				base.RefreshConfigurationIfNeeded();
				return this.m_KeyboardLayoutName;
			}
			protected set
			{
				this.m_KeyboardLayoutName = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00014D1C File Offset: 0x00012F1C
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00014D24 File Offset: 0x00012F24
		public AnyKeyControl anyKey { get; protected set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00014D2D File Offset: 0x00012F2D
		public KeyControl spaceKey
		{
			get
			{
				return this[Key.Space];
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00014D36 File Offset: 0x00012F36
		public KeyControl enterKey
		{
			get
			{
				return this[Key.Enter];
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00014D3F File Offset: 0x00012F3F
		public KeyControl tabKey
		{
			get
			{
				return this[Key.Tab];
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00014D48 File Offset: 0x00012F48
		public KeyControl backquoteKey
		{
			get
			{
				return this[Key.Backquote];
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00014D51 File Offset: 0x00012F51
		public KeyControl quoteKey
		{
			get
			{
				return this[Key.Quote];
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00014D5A File Offset: 0x00012F5A
		public KeyControl semicolonKey
		{
			get
			{
				return this[Key.Semicolon];
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00014D63 File Offset: 0x00012F63
		public KeyControl commaKey
		{
			get
			{
				return this[Key.Comma];
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00014D6C File Offset: 0x00012F6C
		public KeyControl periodKey
		{
			get
			{
				return this[Key.Period];
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00014D75 File Offset: 0x00012F75
		public KeyControl slashKey
		{
			get
			{
				return this[Key.Slash];
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00014D7F File Offset: 0x00012F7F
		public KeyControl backslashKey
		{
			get
			{
				return this[Key.Backslash];
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00014D89 File Offset: 0x00012F89
		public KeyControl leftBracketKey
		{
			get
			{
				return this[Key.LeftBracket];
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00014D93 File Offset: 0x00012F93
		public KeyControl rightBracketKey
		{
			get
			{
				return this[Key.RightBracket];
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00014D9D File Offset: 0x00012F9D
		public KeyControl minusKey
		{
			get
			{
				return this[Key.Minus];
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00014DA7 File Offset: 0x00012FA7
		public KeyControl equalsKey
		{
			get
			{
				return this[Key.Equals];
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00014DB1 File Offset: 0x00012FB1
		public KeyControl aKey
		{
			get
			{
				return this[Key.A];
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00014DBB File Offset: 0x00012FBB
		public KeyControl bKey
		{
			get
			{
				return this[Key.B];
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00014DC5 File Offset: 0x00012FC5
		public KeyControl cKey
		{
			get
			{
				return this[Key.C];
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00014DCF File Offset: 0x00012FCF
		public KeyControl dKey
		{
			get
			{
				return this[Key.D];
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00014DD9 File Offset: 0x00012FD9
		public KeyControl eKey
		{
			get
			{
				return this[Key.E];
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00014DE3 File Offset: 0x00012FE3
		public KeyControl fKey
		{
			get
			{
				return this[Key.F];
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00014DED File Offset: 0x00012FED
		public KeyControl gKey
		{
			get
			{
				return this[Key.G];
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00014DF7 File Offset: 0x00012FF7
		public KeyControl hKey
		{
			get
			{
				return this[Key.H];
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00014E01 File Offset: 0x00013001
		public KeyControl iKey
		{
			get
			{
				return this[Key.I];
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00014E0B File Offset: 0x0001300B
		public KeyControl jKey
		{
			get
			{
				return this[Key.J];
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00014E15 File Offset: 0x00013015
		public KeyControl kKey
		{
			get
			{
				return this[Key.K];
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00014E1F File Offset: 0x0001301F
		public KeyControl lKey
		{
			get
			{
				return this[Key.L];
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00014E29 File Offset: 0x00013029
		public KeyControl mKey
		{
			get
			{
				return this[Key.M];
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00014E33 File Offset: 0x00013033
		public KeyControl nKey
		{
			get
			{
				return this[Key.N];
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00014E3D File Offset: 0x0001303D
		public KeyControl oKey
		{
			get
			{
				return this[Key.O];
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00014E47 File Offset: 0x00013047
		public KeyControl pKey
		{
			get
			{
				return this[Key.P];
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00014E51 File Offset: 0x00013051
		public KeyControl qKey
		{
			get
			{
				return this[Key.Q];
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00014E5B File Offset: 0x0001305B
		public KeyControl rKey
		{
			get
			{
				return this[Key.R];
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00014E65 File Offset: 0x00013065
		public KeyControl sKey
		{
			get
			{
				return this[Key.S];
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00014E6F File Offset: 0x0001306F
		public KeyControl tKey
		{
			get
			{
				return this[Key.T];
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00014E79 File Offset: 0x00013079
		public KeyControl uKey
		{
			get
			{
				return this[Key.U];
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00014E83 File Offset: 0x00013083
		public KeyControl vKey
		{
			get
			{
				return this[Key.V];
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00014E8D File Offset: 0x0001308D
		public KeyControl wKey
		{
			get
			{
				return this[Key.W];
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00014E97 File Offset: 0x00013097
		public KeyControl xKey
		{
			get
			{
				return this[Key.X];
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00014EA1 File Offset: 0x000130A1
		public KeyControl yKey
		{
			get
			{
				return this[Key.Y];
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00014EAB File Offset: 0x000130AB
		public KeyControl zKey
		{
			get
			{
				return this[Key.Z];
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00014EB5 File Offset: 0x000130B5
		public KeyControl digit1Key
		{
			get
			{
				return this[Key.Digit1];
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00014EBF File Offset: 0x000130BF
		public KeyControl digit2Key
		{
			get
			{
				return this[Key.Digit2];
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00014EC9 File Offset: 0x000130C9
		public KeyControl digit3Key
		{
			get
			{
				return this[Key.Digit3];
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00014ED3 File Offset: 0x000130D3
		public KeyControl digit4Key
		{
			get
			{
				return this[Key.Digit4];
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00014EDD File Offset: 0x000130DD
		public KeyControl digit5Key
		{
			get
			{
				return this[Key.Digit5];
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00014EE7 File Offset: 0x000130E7
		public KeyControl digit6Key
		{
			get
			{
				return this[Key.Digit6];
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00014EF1 File Offset: 0x000130F1
		public KeyControl digit7Key
		{
			get
			{
				return this[Key.Digit7];
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00014EFB File Offset: 0x000130FB
		public KeyControl digit8Key
		{
			get
			{
				return this[Key.Digit8];
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00014F05 File Offset: 0x00013105
		public KeyControl digit9Key
		{
			get
			{
				return this[Key.Digit9];
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00014F0F File Offset: 0x0001310F
		public KeyControl digit0Key
		{
			get
			{
				return this[Key.Digit0];
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00014F19 File Offset: 0x00013119
		public KeyControl leftShiftKey
		{
			get
			{
				return this[Key.LeftShift];
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00014F23 File Offset: 0x00013123
		public KeyControl rightShiftKey
		{
			get
			{
				return this[Key.RightShift];
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00014F2D File Offset: 0x0001312D
		public KeyControl leftAltKey
		{
			get
			{
				return this[Key.LeftAlt];
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00014F37 File Offset: 0x00013137
		public KeyControl rightAltKey
		{
			get
			{
				return this[Key.RightAlt];
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00014F41 File Offset: 0x00013141
		public KeyControl leftCtrlKey
		{
			get
			{
				return this[Key.LeftCtrl];
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00014F4B File Offset: 0x0001314B
		public KeyControl rightCtrlKey
		{
			get
			{
				return this[Key.RightCtrl];
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00014F55 File Offset: 0x00013155
		public KeyControl leftMetaKey
		{
			get
			{
				return this[Key.LeftMeta];
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00014F5F File Offset: 0x0001315F
		public KeyControl rightMetaKey
		{
			get
			{
				return this[Key.RightMeta];
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00014F69 File Offset: 0x00013169
		public KeyControl leftWindowsKey
		{
			get
			{
				return this[Key.LeftMeta];
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00014F73 File Offset: 0x00013173
		public KeyControl rightWindowsKey
		{
			get
			{
				return this[Key.RightMeta];
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00014F7D File Offset: 0x0001317D
		public KeyControl leftAppleKey
		{
			get
			{
				return this[Key.LeftMeta];
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00014F87 File Offset: 0x00013187
		public KeyControl rightAppleKey
		{
			get
			{
				return this[Key.RightMeta];
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00014F91 File Offset: 0x00013191
		public KeyControl leftCommandKey
		{
			get
			{
				return this[Key.LeftMeta];
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00014F9B File Offset: 0x0001319B
		public KeyControl rightCommandKey
		{
			get
			{
				return this[Key.RightMeta];
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00014FA5 File Offset: 0x000131A5
		public KeyControl contextMenuKey
		{
			get
			{
				return this[Key.ContextMenu];
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00014FAF File Offset: 0x000131AF
		public KeyControl escapeKey
		{
			get
			{
				return this[Key.Escape];
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00014FB9 File Offset: 0x000131B9
		public KeyControl leftArrowKey
		{
			get
			{
				return this[Key.LeftArrow];
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00014FC3 File Offset: 0x000131C3
		public KeyControl rightArrowKey
		{
			get
			{
				return this[Key.RightArrow];
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00014FCD File Offset: 0x000131CD
		public KeyControl upArrowKey
		{
			get
			{
				return this[Key.UpArrow];
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00014FD7 File Offset: 0x000131D7
		public KeyControl downArrowKey
		{
			get
			{
				return this[Key.DownArrow];
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00014FE1 File Offset: 0x000131E1
		public KeyControl backspaceKey
		{
			get
			{
				return this[Key.Backspace];
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00014FEB File Offset: 0x000131EB
		public KeyControl pageDownKey
		{
			get
			{
				return this[Key.PageDown];
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00014FF5 File Offset: 0x000131F5
		public KeyControl pageUpKey
		{
			get
			{
				return this[Key.PageUp];
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00014FFF File Offset: 0x000131FF
		public KeyControl homeKey
		{
			get
			{
				return this[Key.Home];
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00015009 File Offset: 0x00013209
		public KeyControl endKey
		{
			get
			{
				return this[Key.End];
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00015013 File Offset: 0x00013213
		public KeyControl insertKey
		{
			get
			{
				return this[Key.Insert];
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001501D File Offset: 0x0001321D
		public KeyControl deleteKey
		{
			get
			{
				return this[Key.Delete];
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00015027 File Offset: 0x00013227
		public KeyControl capsLockKey
		{
			get
			{
				return this[Key.CapsLock];
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00015031 File Offset: 0x00013231
		public KeyControl scrollLockKey
		{
			get
			{
				return this[Key.ScrollLock];
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0001503B File Offset: 0x0001323B
		public KeyControl numLockKey
		{
			get
			{
				return this[Key.NumLock];
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00015045 File Offset: 0x00013245
		public KeyControl printScreenKey
		{
			get
			{
				return this[Key.PrintScreen];
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0001504F File Offset: 0x0001324F
		public KeyControl pauseKey
		{
			get
			{
				return this[Key.Pause];
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00015059 File Offset: 0x00013259
		public KeyControl numpadEnterKey
		{
			get
			{
				return this[Key.NumpadEnter];
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00015063 File Offset: 0x00013263
		public KeyControl numpadDivideKey
		{
			get
			{
				return this[Key.NumpadDivide];
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001506D File Offset: 0x0001326D
		public KeyControl numpadMultiplyKey
		{
			get
			{
				return this[Key.NumpadMultiply];
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00015077 File Offset: 0x00013277
		public KeyControl numpadMinusKey
		{
			get
			{
				return this[Key.NumpadMinus];
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00015081 File Offset: 0x00013281
		public KeyControl numpadPlusKey
		{
			get
			{
				return this[Key.NumpadPlus];
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001508B File Offset: 0x0001328B
		public KeyControl numpadPeriodKey
		{
			get
			{
				return this[Key.NumpadPeriod];
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00015095 File Offset: 0x00013295
		public KeyControl numpadEqualsKey
		{
			get
			{
				return this[Key.NumpadEquals];
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001509F File Offset: 0x0001329F
		public KeyControl numpad0Key
		{
			get
			{
				return this[Key.Numpad0];
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000150A9 File Offset: 0x000132A9
		public KeyControl numpad1Key
		{
			get
			{
				return this[Key.Numpad1];
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x000150B3 File Offset: 0x000132B3
		public KeyControl numpad2Key
		{
			get
			{
				return this[Key.Numpad2];
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000150BD File Offset: 0x000132BD
		public KeyControl numpad3Key
		{
			get
			{
				return this[Key.Numpad3];
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000150C7 File Offset: 0x000132C7
		public KeyControl numpad4Key
		{
			get
			{
				return this[Key.Numpad4];
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x000150D1 File Offset: 0x000132D1
		public KeyControl numpad5Key
		{
			get
			{
				return this[Key.Numpad5];
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x000150DB File Offset: 0x000132DB
		public KeyControl numpad6Key
		{
			get
			{
				return this[Key.Numpad6];
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000150E5 File Offset: 0x000132E5
		public KeyControl numpad7Key
		{
			get
			{
				return this[Key.Numpad7];
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x000150EF File Offset: 0x000132EF
		public KeyControl numpad8Key
		{
			get
			{
				return this[Key.Numpad8];
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000150F9 File Offset: 0x000132F9
		public KeyControl numpad9Key
		{
			get
			{
				return this[Key.Numpad9];
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00015103 File Offset: 0x00013303
		public KeyControl f1Key
		{
			get
			{
				return this[Key.F1];
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001510D File Offset: 0x0001330D
		public KeyControl f2Key
		{
			get
			{
				return this[Key.F2];
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00015117 File Offset: 0x00013317
		public KeyControl f3Key
		{
			get
			{
				return this[Key.F3];
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00015121 File Offset: 0x00013321
		public KeyControl f4Key
		{
			get
			{
				return this[Key.F4];
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001512B File Offset: 0x0001332B
		public KeyControl f5Key
		{
			get
			{
				return this[Key.F5];
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00015135 File Offset: 0x00013335
		public KeyControl f6Key
		{
			get
			{
				return this[Key.F6];
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001513F File Offset: 0x0001333F
		public KeyControl f7Key
		{
			get
			{
				return this[Key.F7];
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00015149 File Offset: 0x00013349
		public KeyControl f8Key
		{
			get
			{
				return this[Key.F8];
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00015153 File Offset: 0x00013353
		public KeyControl f9Key
		{
			get
			{
				return this[Key.F9];
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001515D File Offset: 0x0001335D
		public KeyControl f10Key
		{
			get
			{
				return this[Key.F10];
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00015167 File Offset: 0x00013367
		public KeyControl f11Key
		{
			get
			{
				return this[Key.F11];
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00015171 File Offset: 0x00013371
		public KeyControl f12Key
		{
			get
			{
				return this[Key.F12];
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001517B File Offset: 0x0001337B
		public KeyControl oem1Key
		{
			get
			{
				return this[Key.OEM1];
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00015185 File Offset: 0x00013385
		public KeyControl oem2Key
		{
			get
			{
				return this[Key.OEM2];
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001518F File Offset: 0x0001338F
		public KeyControl oem3Key
		{
			get
			{
				return this[Key.OEM3];
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00015199 File Offset: 0x00013399
		public KeyControl oem4Key
		{
			get
			{
				return this[Key.OEM4];
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x000151A3 File Offset: 0x000133A3
		public KeyControl oem5Key
		{
			get
			{
				return this[Key.OEM5];
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x000151AD File Offset: 0x000133AD
		public KeyControl f13Key
		{
			get
			{
				return this[Key.F13];
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x000151B7 File Offset: 0x000133B7
		public KeyControl f14Key
		{
			get
			{
				return this[Key.F14];
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x000151C1 File Offset: 0x000133C1
		public KeyControl f15Key
		{
			get
			{
				return this[Key.F15];
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x000151CB File Offset: 0x000133CB
		public KeyControl f16Key
		{
			get
			{
				return this[Key.F16];
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x000151D5 File Offset: 0x000133D5
		public KeyControl f17Key
		{
			get
			{
				return this[Key.F17];
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x000151DF File Offset: 0x000133DF
		public KeyControl f18Key
		{
			get
			{
				return this[Key.F18];
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x000151E9 File Offset: 0x000133E9
		public KeyControl f19Key
		{
			get
			{
				return this[Key.F19];
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x000151F3 File Offset: 0x000133F3
		public KeyControl f20Key
		{
			get
			{
				return this[Key.F20];
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x000151FD File Offset: 0x000133FD
		public KeyControl f21Key
		{
			get
			{
				return this[Key.F21];
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00015207 File Offset: 0x00013407
		public KeyControl f22Key
		{
			get
			{
				return this[Key.F22];
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00015211 File Offset: 0x00013411
		public KeyControl f23Key
		{
			get
			{
				return this[Key.F23];
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001521B File Offset: 0x0001341B
		public KeyControl f24Key
		{
			get
			{
				return this[Key.F24];
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00015225 File Offset: 0x00013425
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0001522D File Offset: 0x0001342D
		public ButtonControl shiftKey { get; protected set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00015236 File Offset: 0x00013436
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0001523E File Offset: 0x0001343E
		public ButtonControl ctrlKey { get; protected set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00015247 File Offset: 0x00013447
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0001524F File Offset: 0x0001344F
		public ButtonControl altKey { get; protected set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00015258 File Offset: 0x00013458
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00015260 File Offset: 0x00013460
		public ButtonControl imeSelected { get; protected set; }

		// Token: 0x170001DD RID: 477
		public KeyControl this[Key key]
		{
			get
			{
				int num = key - Key.Space;
				if (num < 0 || num >= this.m_Keys.Length)
				{
					throw new ArgumentOutOfRangeException("key");
				}
				return this.m_Keys[num];
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001529F File Offset: 0x0001349F
		public ReadOnlyArray<KeyControl> allKeys
		{
			get
			{
				return new ReadOnlyArray<KeyControl>(this.m_Keys);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x000152AC File Offset: 0x000134AC
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x000152B3 File Offset: 0x000134B3
		public static Keyboard current { get; private set; }

		// Token: 0x06000586 RID: 1414 RVA: 0x000152BB File Offset: 0x000134BB
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			Keyboard.current = this;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000152C9 File Offset: 0x000134C9
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (Keyboard.current == this)
			{
				Keyboard.current = null;
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000152E0 File Offset: 0x000134E0
		protected override void FinishSetup()
		{
			string[] array = new string[]
			{
				"space", "enter", "tab", "backquote", "quote", "semicolon", "comma", "period", "slash", "backslash",
				"leftbracket", "rightbracket", "minus", "equals", "a", "b", "c", "d", "e", "f",
				"g", "h", "i", "j", "k", "l", "m", "n", "o", "p",
				"q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
				"1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
				"leftshift", "rightshift", "leftalt", "rightalt", "leftctrl", "rightctrl", "leftmeta", "rightmeta", "contextmenu", "escape",
				"leftarrow", "rightarrow", "uparrow", "downarrow", "backspace", "pagedown", "pageup", "home", "end", "insert",
				"delete", "capslock", "numlock", "printscreen", "scrolllock", "pause", "numpadenter", "numpaddivide", "numpadmultiply", "numpadplus",
				"numpadminus", "numpadperiod", "numpadequals", "numpad0", "numpad1", "numpad2", "numpad3", "numpad4", "numpad5", "numpad6",
				"numpad7", "numpad8", "numpad9", "f1", "f2", "f3", "f4", "f5", "f6", "f7",
				"f8", "f9", "f10", "f11", "f12", "oem1", "oem2", "oem3", "oem4", "oem5",
				null, "f13", "f14", "f15", "f16", "f17", "f18", "f19", "f20", "f21",
				"f22", "f23", "f24"
			};
			this.m_Keys = new KeyControl[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					this.m_Keys[i] = base.GetChildControl<KeyControl>(array[i]);
					this.m_Keys[i].keyCode = i + Key.Space;
				}
			}
			this.anyKey = base.GetChildControl<AnyKeyControl>("anyKey");
			this.shiftKey = base.GetChildControl<ButtonControl>("shift");
			this.ctrlKey = base.GetChildControl<ButtonControl>("ctrl");
			this.altKey = base.GetChildControl<ButtonControl>("alt");
			this.imeSelected = base.GetChildControl<ButtonControl>("IMESelected");
			base.FinishSetup();
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x000157D8 File Offset: 0x000139D8
		protected override void RefreshConfiguration()
		{
			this.keyboardLayout = null;
			QueryKeyboardLayoutCommand queryKeyboardLayoutCommand = QueryKeyboardLayoutCommand.Create();
			if (base.ExecuteCommand<QueryKeyboardLayoutCommand>(ref queryKeyboardLayoutCommand) >= 0L)
			{
				this.keyboardLayout = queryKeyboardLayoutCommand.ReadLayoutName();
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001580C File Offset: 0x00013A0C
		public void OnTextInput(char character)
		{
			for (int i = 0; i < this.m_TextInputListeners.length; i++)
			{
				this.m_TextInputListeners[i](character);
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00015844 File Offset: 0x00013A44
		public KeyControl FindKeyOnCurrentKeyboardLayout(string displayName)
		{
			ReadOnlyArray<KeyControl> allKeys = this.allKeys;
			for (int i = 0; i < allKeys.Count; i++)
			{
				if (string.Equals(allKeys[i].displayName, displayName, StringComparison.CurrentCultureIgnoreCase))
				{
					return allKeys[i];
				}
			}
			return null;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001588C File Offset: 0x00013A8C
		public void OnIMECompositionChanged(IMECompositionString compositionString)
		{
			if (this.m_ImeCompositionListeners.length > 0)
			{
				for (int i = 0; i < this.m_ImeCompositionListeners.length; i++)
				{
					this.m_ImeCompositionListeners[i](compositionString);
				}
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000158D0 File Offset: 0x00013AD0
		unsafe bool IEventPreProcessor.PreProcessEvent(InputEventPtr currentEventPtr)
		{
			if (currentEventPtr.type == 1398030676)
			{
				StateEvent* ptr = StateEvent.FromUnchecked(currentEventPtr);
				if (ptr->stateFormat == KeyboardState.Format)
				{
					KeyboardState* ptr2 = (KeyboardState*)(&ptr->stateData.FixedElementField);
					if (ptr2->Get(Key.IMESelected))
					{
						ptr2->Set(Key.IMESelected, false);
						ptr2->Set((Key)127, true);
					}
				}
			}
			return true;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00015938 File Offset: 0x00013B38
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00015940 File Offset: 0x00013B40
		protected KeyControl[] keys
		{
			get
			{
				return this.m_Keys;
			}
			set
			{
				this.m_Keys = value;
			}
		}

		// Token: 0x04000212 RID: 530
		public const int KeyCount = 110;

		// Token: 0x04000213 RID: 531
		internal const int ExtendedKeyCount = 123;

		// Token: 0x0400021A RID: 538
		private InlinedArray<Action<char>> m_TextInputListeners;

		// Token: 0x0400021B RID: 539
		private string m_KeyboardLayoutName;

		// Token: 0x0400021C RID: 540
		private KeyControl[] m_Keys;

		// Token: 0x0400021D RID: 541
		private InlinedArray<Action<IMECompositionString>> m_ImeCompositionListeners;
	}
}
