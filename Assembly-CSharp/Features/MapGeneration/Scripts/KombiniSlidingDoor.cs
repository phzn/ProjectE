using System;
using Features.MainMenu;
using UnityEngine;

namespace Features.MapGeneration.Scripts
{
	// Token: 0x02001001 RID: 4097
	public class KombiniSlidingDoor : MonoBehaviour
	{
		// Token: 0x06007C58 RID: 31832 RVA: 0x0032C86E File Offset: 0x0032AA6E
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.KombiniSlidingDoor_closeClip)
			{
				return this.closeClip;
			}
			if (field != UniversityGameplayField.KombiniSlidingDoor_openClip)
			{
				throw new InvalidOperationException("Unsupported KombiniSlidingDoor POI binding: " + field.ToString());
			}
			return this.openClip;
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x0032C8A8 File Offset: 0x0032AAA8
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.KombiniSlidingDoor_closeClip)
			{
				this.closeClip = (AudioClip)asset;
				return;
			}
			if (field != UniversityGameplayField.KombiniSlidingDoor_openClip)
			{
				throw new InvalidOperationException("Unsupported KombiniSlidingDoor POI binding: " + field.ToString());
			}
			this.openClip = (AudioClip)asset;
		}

		// Token: 0x06007C5A RID: 31834 RVA: 0x0032C8F7 File Offset: 0x0032AAF7
		private void Awake()
		{
			this.EnsureAudioSource();
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x0032C900 File Offset: 0x0032AB00
		private void EnsureAudioSource()
		{
			if (this.audioSource == null && (this.openClip != null || this.closeClip != null))
			{
				this.audioSource = base.GetComponent<AudioSource>();
				if (this.audioSource == null)
				{
					this.audioSource = base.gameObject.AddComponent<AudioSource>();
					this.audioSource.playOnAwake = false;
					this.audioSource.loop = false;
					this.audioSource.spatialBlend = 1f;
					this.audioSource.rolloffMode = AudioRolloffMode.Linear;
					this.audioSource.minDistance = 2f;
					this.audioSource.maxDistance = 18f;
					this.audioSource.dopplerLevel = 0f;
					this.audioSource.volume = 0.65f;
				}
			}
			if (this.audioSource != null)
			{
				GameAudioRouter.Route(this.audioSource, GameAudioRouter.Channel.Sfx);
			}
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x0032C9FC File Offset: 0x0032ABFC
		private void Start()
		{
			if (this.leftDoor)
			{
				this._leftClosedPos = this.leftDoor.localPosition;
			}
			if (this.rightDoor)
			{
				this._rightClosedPos = this.rightDoor.localPosition;
			}
			if (this.leftDoor)
			{
				Vector3 vector = (this.invertLeftDoorDirection ? (-this.slideAxis) : this.slideAxis);
				this._leftOpenPos = this._leftClosedPos + vector * this.openDistance;
			}
			if (this.rightDoor)
			{
				Vector3 vector2 = (this.invertRightDoorDirection ? (-this.slideAxis) : this.slideAxis);
				this._rightOpenPos = this._rightClosedPos + vector2 * this.openDistance;
			}
			if (this.debugMode)
			{
				Debug.Log("[KombiniSlidingDoor] Initialized on " + base.gameObject.name + ".");
				if (this.leftDoor)
				{
					if (this.leftDoor.gameObject.isStatic)
					{
						Debug.LogError("[KombiniSlidingDoor] ERROR: Left Door '" + this.leftDoor.name + "' is marked as STATIC! It cannot move.");
					}
					Debug.Log(string.Format("[KombiniSlidingDoor] Left Closed: {0}, Open: {1}", this._leftClosedPos, this._leftOpenPos));
				}
				if (this.rightDoor)
				{
					if (this.rightDoor.gameObject.isStatic)
					{
						Debug.LogError("[KombiniSlidingDoor] ERROR: Right Door '" + this.rightDoor.name + "' is marked as STATIC! It cannot move.");
					}
					Debug.Log(string.Format("[KombiniSlidingDoor] Right Closed: {0}, Open: {1}", this._rightClosedPos, this._rightOpenPos));
				}
			}
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x0032CBC4 File Offset: 0x0032ADC4
		private void Update()
		{
			if (this._objectsInZone == 0 && !this._isOpen && this.DoorsAtRest())
			{
				return;
			}
			if (this._objectsInZone > 0)
			{
				this._isOpen = true;
			}
			else if (this._isOpen && this.IsFullyOpen())
			{
				this._isOpen = false;
			}
			this.MoveDoors();
			if (this._isOpen != this._wasOpen)
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("[KombiniSlidingDoor] State changed. Open: {0} (Objects: {1})", this._isOpen, this._objectsInZone));
				}
				this.PlaySound(this._isOpen ? this.openClip : this.closeClip);
				this._wasOpen = this._isOpen;
			}
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x0032CC7C File Offset: 0x0032AE7C
		private bool DoorsAtRest()
		{
			return (!(this.leftDoor != null) || (this.leftDoor.localPosition - this._leftClosedPos).sqrMagnitude <= 0.0001f) && (!(this.rightDoor != null) || (this.rightDoor.localPosition - this._rightClosedPos).sqrMagnitude <= 0.0001f);
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x0032CCF4 File Offset: 0x0032AEF4
		private void MoveDoors()
		{
			float num = this.slideSpeed * Time.deltaTime;
			this.slideAxis.Normalize();
			if (this.leftDoor)
			{
				Vector3 vector = (this.invertLeftDoorDirection ? (-this.slideAxis) : this.slideAxis);
				Vector3 vector2 = this._leftClosedPos + vector * this.openDistance;
				Vector3 vector3 = (this._isOpen ? vector2 : this._leftClosedPos);
				Vector3 localPosition = this.leftDoor.localPosition;
				this.leftDoor.localPosition = Vector3.MoveTowards(localPosition, vector3, num);
			}
			if (this.rightDoor)
			{
				Vector3 vector4 = (this.invertRightDoorDirection ? (-this.slideAxis) : this.slideAxis);
				Vector3 vector5 = this._rightClosedPos + vector4 * this.openDistance;
				Vector3 vector6 = (this._isOpen ? vector5 : this._rightClosedPos);
				this.rightDoor.localPosition = Vector3.MoveTowards(this.rightDoor.localPosition, vector6, num);
			}
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x0032CE04 File Offset: 0x0032B004
		private bool IsFullyOpen()
		{
			float num = 0.01f;
			bool flag = true;
			if (this.leftDoor)
			{
				Vector3 vector = (this.invertLeftDoorDirection ? (-this.slideAxis) : this.slideAxis);
				Vector3 vector2 = this._leftClosedPos + vector * this.openDistance;
				flag = Vector3.Distance(this.leftDoor.localPosition, vector2) < num;
			}
			bool flag2 = true;
			if (this.rightDoor)
			{
				Vector3 vector3 = (this.invertRightDoorDirection ? (-this.slideAxis) : this.slideAxis);
				Vector3 vector4 = this._rightClosedPos + vector3 * this.openDistance;
				flag2 = Vector3.Distance(this.rightDoor.localPosition, vector4) < num;
			}
			return flag && flag2;
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x0032CED0 File Offset: 0x0032B0D0
		private void OnTriggerEnter(Collider other)
		{
			if (this.debugMode)
			{
				Debug.Log(string.Concat(new string[] { "[KombiniSlidingDoor] OnTriggerEnter: ", other.name, " (Tag: ", other.tag, ")" }));
			}
			if (this.IsValidObject(other))
			{
				this._objectsInZone++;
				if (this.debugMode)
				{
					Debug.Log(string.Format("[KombiniSlidingDoor] Object Accepted. Count: {0}", this._objectsInZone));
				}
			}
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x0032CF58 File Offset: 0x0032B158
		private void OnTriggerExit(Collider other)
		{
			if (this.debugMode)
			{
				Debug.Log("[KombiniSlidingDoor] OnTriggerExit: " + other.name);
			}
			if (this.IsValidObject(other))
			{
				this._objectsInZone--;
				if (this._objectsInZone < 0)
				{
					this._objectsInZone = 0;
					if (this.debugMode)
					{
						Debug.LogWarning("[KombiniSlidingDoor] Objects count went negative! Resetting to 0.");
					}
				}
				if (this.debugMode)
				{
					Debug.Log(string.Format("[KombiniSlidingDoor] Object Removed. Count: {0}", this._objectsInZone));
				}
			}
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x0032CFE0 File Offset: 0x0032B1E0
		private bool IsValidObject(Collider other)
		{
			if (other.isTrigger)
			{
				return false;
			}
			if (other.GetComponentInParent<global::CharacterController>() != null)
			{
				return true;
			}
			if (other.GetComponentInParent<AStarNPCController>() != null)
			{
				return true;
			}
			if (other.CompareTag("Player"))
			{
				return true;
			}
			if (other.CompareTag("NPC"))
			{
				return true;
			}
			if (other.transform.parent != null)
			{
				if (other.transform.parent.CompareTag("Player"))
				{
					return true;
				}
				if (other.transform.parent.CompareTag("NPC"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x0032D07B File Offset: 0x0032B27B
		private void PlaySound(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}
			this.EnsureAudioSource();
			if (this.audioSource && clip)
			{
				this.audioSource.PlayOneShot(clip);
			}
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x0032D0B0 File Offset: 0x0032B2B0
		private void OnDrawGizmos()
		{
			if (this.debugMode)
			{
				Gizmos.color = (this._isOpen ? Color.green : Color.red);
				BoxCollider component = base.GetComponent<BoxCollider>();
				if (component != null)
				{
					Gizmos.matrix = base.transform.localToWorldMatrix;
					Gizmos.DrawWireCube(component.center, component.size);
				}
			}
		}

		// Token: 0x0400801B RID: 32795
		[Header("Door References")]
		[Tooltip("La porta sinistra (guardando dall'esterno).")]
		public Transform leftDoor;

		// Token: 0x0400801C RID: 32796
		[Tooltip("La porta destra (guardando dall'esterno).")]
		public Transform rightDoor;

		// Token: 0x0400801D RID: 32797
		[Header("Animation Settings")]
		[Tooltip("Distanza di apertura in metri.")]
		public float openDistance = 1.2f;

		// Token: 0x0400801E RID: 32798
		[Tooltip("Velocità di movimento delle porte.")]
		public float slideSpeed = 3f;

		// Token: 0x0400801F RID: 32799
		[Tooltip("Asse locale su cui scorrono le porte. (Solitamente X: 1,0,0)")]
		public Vector3 slideAxis = Vector3.right;

		// Token: 0x04008020 RID: 32800
		[Tooltip("Inverte la direzione di apertura per la porta sinistra? (Se true, Left va in -Axis, Right va in +Axis)")]
		public bool invertLeftDoorDirection = true;

		// Token: 0x04008021 RID: 32801
		[Tooltip("Inverte la direzione di apertura per la porta destra?")]
		public bool invertRightDoorDirection;

		// Token: 0x04008022 RID: 32802
		[Header("Audio (Optional)")]
		public AudioSource audioSource;

		// Token: 0x04008023 RID: 32803
		public AudioClip openClip;

		// Token: 0x04008024 RID: 32804
		public AudioClip closeClip;

		// Token: 0x04008025 RID: 32805
		[Header("Debug")]
		public bool debugMode = true;

		// Token: 0x04008026 RID: 32806
		private Vector3 _leftClosedPos;

		// Token: 0x04008027 RID: 32807
		private Vector3 _rightClosedPos;

		// Token: 0x04008028 RID: 32808
		private Vector3 _leftOpenPos;

		// Token: 0x04008029 RID: 32809
		private Vector3 _rightOpenPos;

		// Token: 0x0400802A RID: 32810
		private int _objectsInZone;

		// Token: 0x0400802B RID: 32811
		private bool _isOpen;

		// Token: 0x0400802C RID: 32812
		private bool _wasOpen;
	}
}
