using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class vDebugUtils : MonoBehaviour
{
	// Token: 0x0600099E RID: 2462 RVA: 0x000444D8 File Offset: 0x000426D8
	private void Start()
	{
		this.currentFixedDeltaTime = Time.fixedDeltaTime;
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x000444E8 File Offset: 0x000426E8
	private void Update()
	{
		if (GameplayInput.GetKeyDown(this.timeScaleDown))
		{
			Time.timeScale = Mathf.Clamp(Time.timeScale - this.timeScaleChangeValue, 0f, 1f);
			if (this.affectFixedDeltaTime)
			{
				Time.fixedDeltaTime = Time.timeScale * this.currentFixedDeltaTime;
				return;
			}
		}
		else if (GameplayInput.GetKeyDown(this.timeScaleUp))
		{
			Time.timeScale = Mathf.Clamp(Time.timeScale + this.timeScaleChangeValue, 0f, 1f);
			if (this.affectFixedDeltaTime)
			{
				Time.fixedDeltaTime = Time.timeScale * this.currentFixedDeltaTime;
			}
		}
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00044582 File Offset: 0x00042782
	public void PrintMessage(string message)
	{
		Debug.Log(message);
		this.debugMessages.Add(new vDebugUtils.DebugMessage(message, Time.time, this.displayMessageTime, vDebugUtils.MessageType.Normal));
		this.scrool.y = this.lastRectPosition;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x000445B8 File Offset: 0x000427B8
	public void PrintMessageWarning(string message)
	{
		Debug.LogWarning(message);
		this.debugMessages.Add(new vDebugUtils.DebugMessage(message, Time.time, this.displayMessageTime, vDebugUtils.MessageType.Warning));
		this.scrool.y = this.lastRectPosition;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x000445EE File Offset: 0x000427EE
	public void PrintMessageError(string message)
	{
		Debug.LogError(message);
		this.debugMessages.Add(new vDebugUtils.DebugMessage(message, Time.time, this.displayMessageTime, vDebugUtils.MessageType.Error));
		this.scrool.y = this.lastRectPosition;
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00044624 File Offset: 0x00042824
	private void OnGUI()
	{
		GUILayout.Label("TimeScale:" + Time.timeScale.ToString(), Array.Empty<GUILayoutOption>());
		if (this.debugMessages.Count > 0)
		{
			if (this.messageStyle == null)
			{
				this.messageStyle = new GUIStyle(GUI.skin.box);
				this.messageStyle.wordWrap = true;
				this.messageStyle.alignment = TextAnchor.MiddleLeft;
				this.messageStyle.fontSize = 10;
			}
			this.scrool = GUILayout.BeginScrollView(this.scrool, "box", new GUILayoutOption[]
			{
				GUILayout.MinHeight(100f),
				GUILayout.Width((float)Screen.width * 0.2f),
				GUILayout.MaxHeight((float)Screen.height * 0.8f)
			});
			for (int i = 0; i < this.debugMessages.Count; i++)
			{
				vDebugUtils.DebugMessage debugMessage = this.debugMessages[i];
				if (debugMessage.isAlive)
				{
					vDebugUtils.MessageType messageType = debugMessage.messageType;
					GUI.color = ((messageType == vDebugUtils.MessageType.Warning) ? Color.yellow : ((messageType == vDebugUtils.MessageType.Error) ? Color.red : Color.white));
					GUILayout.Label(string.Format("[{0}] : {1}", debugMessage.timeString, debugMessage.message), this.messageStyle, Array.Empty<GUILayoutOption>());
					this.lastRectPosition = GUILayoutUtility.GetLastRect().y;
				}
				else
				{
					this.debugMessages.RemoveAt(i);
					i--;
				}
			}
			GUILayout.EndScrollView();
		}
	}

	// Token: 0x040008E6 RID: 2278
	public KeyCode timeScaleDown = KeyCode.KeypadMinus;

	// Token: 0x040008E7 RID: 2279
	public KeyCode timeScaleUp = KeyCode.KeypadPlus;

	// Token: 0x040008E8 RID: 2280
	public float timeScaleChangeValue = 0.1f;

	// Token: 0x040008E9 RID: 2281
	public bool affectFixedDeltaTime = true;

	// Token: 0x040008EA RID: 2282
	private float currentFixedDeltaTime;

	// Token: 0x040008EB RID: 2283
	private List<vDebugUtils.DebugMessage> debugMessages = new List<vDebugUtils.DebugMessage>();

	// Token: 0x040008EC RID: 2284
	public float displayMessageTime = 5f;

	// Token: 0x040008ED RID: 2285
	private Vector2 scrool;

	// Token: 0x040008EE RID: 2286
	private GUIStyle messageStyle;

	// Token: 0x040008EF RID: 2287
	private float lastRectPosition;

	// Token: 0x02000110 RID: 272
	private enum MessageType
	{
		// Token: 0x040008F1 RID: 2289
		Normal,
		// Token: 0x040008F2 RID: 2290
		Warning,
		// Token: 0x040008F3 RID: 2291
		Error
	}

	// Token: 0x02000111 RID: 273
	private class DebugMessage
	{
		// Token: 0x060009A5 RID: 2469 RVA: 0x000447F8 File Offset: 0x000429F8
		public DebugMessage(string message, float startTime, float duration, vDebugUtils.MessageType messageType)
		{
			this.message = message;
			this.startTime = startTime;
			this.messageType = messageType;
			this.duration = duration;
			this.timeString = DateTime.Now.ToString("HH\\:mm\\:ss");
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00044840 File Offset: 0x00042A40
		public bool isAlive
		{
			get
			{
				return this.startTime + this.duration > Time.time;
			}
		}

		// Token: 0x040008F4 RID: 2292
		public string message;

		// Token: 0x040008F5 RID: 2293
		public float startTime;

		// Token: 0x040008F6 RID: 2294
		public float duration;

		// Token: 0x040008F7 RID: 2295
		public string timeString;

		// Token: 0x040008F8 RID: 2296
		public vDebugUtils.MessageType messageType;
	}
}
