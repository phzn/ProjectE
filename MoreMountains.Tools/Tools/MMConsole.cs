using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F7 RID: 247
	public class MMConsole : MonoBehaviour
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x0001910C File Offset: 0x0001730C
		protected virtual void OnGUI()
		{
			GUI.skin.GetStyle("label").fontSize = this._fontSize;
			int num = this._numberOfMessages * this._characterHeight;
			int num2 = this._largestMessageLength * this._characterWidth;
			GUI.Box(new Rect((float)this._marginLeft, (float)this._marginTop, (float)(num2 + this._padding * 2), (float)(num + this._padding * 2)), "");
			GUI.Label(new Rect((float)(this._marginLeft + this._padding), (float)(this._marginTop + this._padding), (float)num2, (float)num), this._messageStack);
			this._messageStackHasBeenDisplayed = true;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000191B9 File Offset: 0x000173B9
		public virtual void SetFontSize(int fontSize)
		{
			this._fontSize = fontSize;
			this._characterHeight = (int)Mathf.Round(1.6f * (float)fontSize + 0.49f);
			this._characterWidth = (int)Mathf.Round(0.6f * (float)fontSize + 0.49f);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000191F6 File Offset: 0x000173F6
		public virtual void SetScreenOffset(int top = 10, int left = 10)
		{
			this._marginTop = top;
			this._marginLeft = left;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00019206 File Offset: 0x00017406
		public virtual void SetMessage(string newMessage)
		{
			this._messageStack = newMessage;
			this._numberOfMessages = 1;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00019218 File Offset: 0x00017418
		public virtual void AddMessage(string newMessage)
		{
			if (this._messageStackHasBeenDisplayed)
			{
				this._messageStack = "";
				this._messageStackHasBeenDisplayed = false;
				this._numberOfMessages = 0;
				this._largestMessageLength = 0;
			}
			this._messageStack = this._messageStack + newMessage + "\n";
			if (newMessage.Length > this._largestMessageLength)
			{
				this._largestMessageLength = newMessage.Length;
			}
			this._numberOfMessages++;
		}

		// Token: 0x040006CB RID: 1739
		protected string _messageStack;

		// Token: 0x040006CC RID: 1740
		protected int _numberOfMessages;

		// Token: 0x040006CD RID: 1741
		protected bool _messageStackHasBeenDisplayed;

		// Token: 0x040006CE RID: 1742
		protected int _largestMessageLength;

		// Token: 0x040006CF RID: 1743
		protected int _marginTop = 10;

		// Token: 0x040006D0 RID: 1744
		protected int _marginLeft = 10;

		// Token: 0x040006D1 RID: 1745
		protected int _padding = 10;

		// Token: 0x040006D2 RID: 1746
		protected int _fontSize = 10;

		// Token: 0x040006D3 RID: 1747
		protected int _characterHeight = 16;

		// Token: 0x040006D4 RID: 1748
		protected int _characterWidth = 6;
	}
}
