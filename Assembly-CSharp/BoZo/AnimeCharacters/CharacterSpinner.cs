using System;
using System.Collections;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200084B RID: 2123
	public class CharacterSpinner : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x06003833 RID: 14387 RVA: 0x001487F5 File Offset: 0x001469F5
		public void SetCharacter(Transform character)
		{
			this.character = character;
			this.anim = ((character != null) ? character.GetComponentInChildren<Animator>() : null);
			if (character != null)
			{
				this.frontRotation = character.localRotation;
			}
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x0014882B File Offset: 0x00146A2B
		public void TweenToContextYaw(float yawDegrees, float duration = 0.35f)
		{
			if (this.character == null)
			{
				return;
			}
			if (this.contextRotationRoutine != null)
			{
				base.StopCoroutine(this.contextRotationRoutine);
			}
			this.contextRotationRoutine = base.StartCoroutine(this.RotateToContext(yawDegrees, duration));
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x00148864 File Offset: 0x00146A64
		private IEnumerator RotateToContext(float yawDegrees, float duration)
		{
			this.spinning = false;
			this.isLeftDragActive = false;
			this.spinDir = 0f;
			Quaternion start = this.character.localRotation;
			Quaternion target = this.frontRotation * Quaternion.Euler(0f, yawDegrees, 0f);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = ((duration > 0f) ? Mathf.Clamp01(elapsed / duration) : 1f);
				this.character.localRotation = Quaternion.Slerp(start, target, Mathf.SmoothStep(0f, 1f, num));
				yield return null;
			}
			this.character.localRotation = target;
			this.contextRotationRoutine = null;
			yield break;
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x00148881 File Offset: 0x00146A81
		public void OnDrag(PointerEventData eventData)
		{
			if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.isLeftDragActive = true;
			this.spinning = true;
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00148881 File Offset: 0x00146A81
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.isLeftDragActive = true;
			this.spinning = true;
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x0014889D File Offset: 0x00146A9D
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.isLeftDragActive = false;
			this.spinning = false;
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x001488B9 File Offset: 0x00146AB9
		private void Start()
		{
			if (this.character != null)
			{
				this.SetCharacter(this.character);
			}
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x001488D8 File Offset: 0x00146AD8
		private void Update()
		{
			if (this.character != null && ControllerInput.IsControllerActive && ControllerInput.Modifier && ControllerUINavigation.CanControlPreview(this))
			{
				if (this.contextRotationRoutine != null)
				{
					base.StopCoroutine(this.contextRotationRoutine);
					this.contextRotationRoutine = null;
				}
				this.character.Rotate(0f, -ControllerInput.Look.x * 120f * Time.unscaledDeltaTime, 0f);
				this.spinDir = 0f;
				return;
			}
			if (this.spinning)
			{
				this.spinDir = -Input.GetAxis("Mouse X") * 5f;
				this.dizzyTimer = 0.5f;
			}
			if (this.isLeftDragActive && !Input.GetMouseButton(0))
			{
				this.isLeftDragActive = false;
				this.spinning = false;
			}
			if (this.character == null)
			{
				return;
			}
			this.character.Rotate(0f, this.spinDir, 0f);
			this.spinDir = Mathf.Lerp(this.spinDir, 0f, Time.deltaTime);
			this.dizzyTimer -= Time.deltaTime;
			if (this.dizzyTimer <= 0f)
			{
				if (this.anim == null)
				{
					return;
				}
				if (this.spinDir >= 5f || this.spinDir <= -5f)
				{
					this.anim.SetBool("Dizzy", true);
					return;
				}
				this.anim.SetBool("Dizzy", false);
			}
		}

		// Token: 0x04003917 RID: 14615
		public float spinDir;

		// Token: 0x04003918 RID: 14616
		public Transform character;

		// Token: 0x04003919 RID: 14617
		private Animator anim;

		// Token: 0x0400391A RID: 14618
		private float dizzyTimer = 1f;

		// Token: 0x0400391B RID: 14619
		private bool spinning;

		// Token: 0x0400391C RID: 14620
		private bool isLeftDragActive;

		// Token: 0x0400391D RID: 14621
		private Quaternion frontRotation;

		// Token: 0x0400391E RID: 14622
		private Coroutine contextRotationRoutine;
	}
}
