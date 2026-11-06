using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000EA5 RID: 3749
	public class OxGramLifeSimulator : MonoBehaviour
	{
		// Token: 0x0600709C RID: 28828 RVA: 0x002DB794 File Offset: 0x002D9994
		public void Init(OxGramChatView view, OxGramTheme theme, OxGramReactionPool reactionPool = null)
		{
			this._view = view;
			this._theme = theme;
			this._reactionPool = reactionPool;
		}

		// Token: 0x0600709D RID: 28829 RVA: 0x002DB7AC File Offset: 0x002D99AC
		public void ReactToPhoto(OxGramMessage photoMsg)
		{
			if (this._view == null || this._chat == null || photoMsg == null)
			{
				return;
			}
			if (!OxGramPhotoTag.IsKnown(OxGramPhotoTag.Of(photoMsg)))
			{
				return;
			}
			if (this._chat.members == null || this._chat.members.Count == 0)
			{
				return;
			}
			this.StopConversation();
			this._reactionRoutine = base.StartCoroutine(this.ReactRoutine(photoMsg));
		}

		// Token: 0x0600709E RID: 28830 RVA: 0x002DB81A File Offset: 0x002D9A1A
		private IEnumerator ReactRoutine(OxGramMessage photoMsg)
		{
			OxGramChat chat = this._chat;
			string alias = OxGramUser.DisplayName;
			string text = OxGramPhotoTag.Of(photoMsg);
			if (!OxGramPhotoTag.IsKnown(text))
			{
				yield break;
			}
			List<string> list = ((this._reactionPool != null) ? this._reactionPool.LinesFor(text) : OxGramReactionPool.DefaultFor(text));
			if (list == null || list.Count == 0)
			{
				yield break;
			}
			int count = ((!this._chat.isGroup) ? 1 : ((this._reactionPool != null) ? this._reactionPool.RollCount() : Random.Range(2, 4)));
			count = Mathf.Min(count, this._chat.members.Count);
			List<string> authors = OxGramLifeSimulator.PickDistinct<string>(this._chat.members, count);
			List<string> picks = OxGramLifeSimulator.PickDistinct<string>(list, count);
			yield return new WaitForSecondsRealtime(Random.Range(this.reactionFirstDelay.x, this.reactionFirstDelay.y));
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				if (this._chat != chat)
				{
					yield break;
				}
				string author = authors[i % authors.Count];
				this._view.SetTyping(author);
				yield return new WaitForSecondsRealtime(Random.Range(this.typingTime.x, this.typingTime.y));
				if (this._chat != chat)
				{
					yield break;
				}
				this._view.SetTyping(null);
				string text2 = OxGramReactionPool.FormatReply(picks[i % picks.Count], alias);
				OxGramMessage oxGramMessage = OxGramMessage.Text(OxGramSender.Other, author, text2, DateTime.Now.ToString("HH:mm"));
				oxGramMessage.reactionTemplate = picks[i % picks.Count];
				oxGramMessage.reactionAlias = alias;
				this._view.AppendMessage(oxGramMessage);
				if (i < count - 1)
				{
					yield return new WaitForSecondsRealtime(Random.Range(this.reactionGap.x, this.reactionGap.y));
				}
				author = null;
				num = i;
			}
			this._reactionRoutine = null;
			this.StartAmbientLoop();
			yield break;
		}

		// Token: 0x0600709F RID: 28831 RVA: 0x002DB830 File Offset: 0x002D9A30
		public void ReactToExposure(OxGramMessage photoMsg, string playerName)
		{
			if (this._view == null || this._chat == null || photoMsg == null)
			{
				return;
			}
			if (this._chat.members == null || this._chat.members.Count == 0)
			{
				return;
			}
			this.StopConversation();
			this._reactionRoutine = base.StartCoroutine(this.ExposureRoutine(playerName));
		}

		// Token: 0x060070A0 RID: 28832 RVA: 0x002DB890 File Offset: 0x002D9A90
		private IEnumerator ExposureRoutine(string playerName)
		{
			OxGramChat chat = this._chat;
			List<string> list = ((this._reactionPool != null) ? this._reactionPool.ExposedLines() : OxGramReactionPool.DefaultExposed());
			if (list == null || list.Count == 0)
			{
				yield break;
			}
			int count = ((!this._chat.isGroup) ? 1 : ((this._reactionPool != null) ? this._reactionPool.RollExposedCount() : Random.Range(4, 7)));
			count = Mathf.Min(count, this._chat.members.Count);
			if (count <= 0)
			{
				yield break;
			}
			List<string> authors = OxGramLifeSimulator.PickDistinct<string>(this._chat.members, count);
			List<string> picks = OxGramLifeSimulator.PickDistinct<string>(list, count);
			yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.8f));
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				if (this._chat != chat)
				{
					yield break;
				}
				string author = authors[i % authors.Count];
				this._view.SetTyping(author);
				yield return new WaitForSecondsRealtime(Random.Range(0.4f, 1.1f));
				if (this._chat != chat)
				{
					yield break;
				}
				this._view.SetTyping(null);
				string text = OxGramReactionPool.FormatReply(picks[i % picks.Count], playerName);
				OxGramMessage oxGramMessage = OxGramMessage.Text(OxGramSender.Other, author, text, DateTime.Now.ToString("HH:mm"));
				oxGramMessage.reactionTemplate = picks[i % picks.Count];
				oxGramMessage.reactionAlias = playerName;
				this._view.AppendMessage(oxGramMessage);
				if (i < count - 1)
				{
					yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.9f));
				}
				author = null;
				num = i;
			}
			this._reactionRoutine = null;
			this.StartAmbientLoop();
			yield break;
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x002DB8A8 File Offset: 0x002D9AA8
		private static List<T> PickDistinct<T>(List<T> source, int n)
		{
			List<T> list = new List<T>(source);
			List<T> list2 = new List<T>();
			n = Mathf.Min(n, list.Count);
			for (int i = 0; i < n; i++)
			{
				int num = Random.Range(0, list.Count);
				list2.Add(list[num]);
				list.RemoveAt(num);
			}
			return list2;
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x002DB8FE File Offset: 0x002D9AFE
		public void OnChatOpened(OxGramChat chat)
		{
			this.StopConversation();
			this._chat = chat;
			this.StartAmbientLoop();
		}

		// Token: 0x060070A3 RID: 28835 RVA: 0x002DB914 File Offset: 0x002D9B14
		private void StartAmbientLoop()
		{
			if (base.isActiveAndEnabled && this._chat != null && this._chat.members != null && this._chat.members.Count > 0 && this._chat.ambientLines != null && this._chat.ambientLines.Count > 0)
			{
				this._loop = base.StartCoroutine(this.AmbientLoop());
			}
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x002DB983 File Offset: 0x002D9B83
		private void OnDisable()
		{
			this.StopConversation();
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x002DB98C File Offset: 0x002D9B8C
		private void StopConversation()
		{
			if (this._loop != null)
			{
				base.StopCoroutine(this._loop);
				this._loop = null;
			}
			if (this._reactionRoutine != null)
			{
				base.StopCoroutine(this._reactionRoutine);
				this._reactionRoutine = null;
			}
			if (this._view != null)
			{
				this._view.SetTyping(null);
			}
		}

		// Token: 0x060070A6 RID: 28838 RVA: 0x002DB9E9 File Offset: 0x002D9BE9
		private IEnumerator AmbientLoop()
		{
			for (;;)
			{
				yield return new WaitForSecondsRealtime(Random.Range(this.interval.x, this.interval.y));
				if (this._chat == null)
				{
					break;
				}
				OxGramChat chat = this._chat;
				string author = this._chat.members[Random.Range(0, this._chat.members.Count)];
				this._view.SetTyping(author);
				yield return new WaitForSecondsRealtime(Random.Range(this.typingTime.x, this.typingTime.y));
				if (this._chat != chat)
				{
					goto Block_1;
				}
				this._view.SetTyping(null);
				string text = this._chat.ambientLines[Random.Range(0, this._chat.ambientLines.Count)];
				OxGramMessage oxGramMessage = OxGramMessage.Text(OxGramSender.Other, author, text, DateTime.Now.ToString("HH:mm"));
				this._view.AppendMessage(oxGramMessage);
				chat = null;
				author = null;
			}
			yield break;
			Block_1:
			yield break;
			yield break;
		}

		// Token: 0x0400734C RID: 29516
		[Tooltip("Min/max seconds between ambient NPC messages.")]
		[SerializeField]
		private Vector2 interval = new Vector2(8f, 20f);

		// Token: 0x0400734D RID: 29517
		[Tooltip("How long the 'typing…' indicator shows before the message lands.")]
		[SerializeField]
		private Vector2 typingTime = new Vector2(1.5f, 3.5f);

		// Token: 0x0400734E RID: 29518
		[Header("Reazioni alle foto")]
		[Tooltip("Pausa prima che parta la prima reazione a una foto (s).")]
		[SerializeField]
		private Vector2 reactionFirstDelay = new Vector2(1.2f, 2.5f);

		// Token: 0x0400734F RID: 29519
		[Tooltip("Pausa tra una reazione e la successiva (s).")]
		[SerializeField]
		private Vector2 reactionGap = new Vector2(1f, 2.8f);

		// Token: 0x04007350 RID: 29520
		private OxGramChatView _view;

		// Token: 0x04007351 RID: 29521
		private OxGramTheme _theme;

		// Token: 0x04007352 RID: 29522
		private OxGramReactionPool _reactionPool;

		// Token: 0x04007353 RID: 29523
		private OxGramChat _chat;

		// Token: 0x04007354 RID: 29524
		private Coroutine _loop;

		// Token: 0x04007355 RID: 29525
		private Coroutine _reactionRoutine;
	}
}
