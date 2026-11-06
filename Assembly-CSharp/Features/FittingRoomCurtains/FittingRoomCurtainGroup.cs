using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.Detection;
using Features.NPC;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.FittingRoomCurtains
{
	// Token: 0x0200120E RID: 4622
	[DisallowMultipleComponent]
	public sealed class FittingRoomCurtainGroup : MonoBehaviour
	{
		// Token: 0x06008EBC RID: 36540 RVA: 0x003A599A File Offset: 0x003A3B9A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			FittingRoomCurtainGroup.ActiveGroups.Clear();
			FittingRoomCurtainGroup.ActiveNpcs.Clear();
			FittingRoomCurtainGroup._lastGlobalInteractionFrame = -1;
			FittingRoomCurtainGroup._npcSimulationTime = 0f;
			FittingRoomCurtainGroup._nextNpcRescanTime = 0f;
			FittingRoomCurtainGroup._nextNpcPollTime = 0f;
		}

		// Token: 0x06008EBD RID: 36541 RVA: 0x003A59D4 File Offset: 0x003A3BD4
		private void Awake()
		{
			this.BuildCurtainSlots();
			this.CreatePromptConfigs();
			this.CreateAudioSource();
		}

		// Token: 0x06008EBE RID: 36542 RVA: 0x003A59E8 File Offset: 0x003A3BE8
		private void OnEnable()
		{
			bool flag = FittingRoomCurtainGroup.ActiveGroups.Count == 0;
			if (!FittingRoomCurtainGroup.ActiveGroups.Contains(this))
			{
				FittingRoomCurtainGroup.ActiveGroups.Add(this);
			}
			FittingRoomCurtainGroup._lastGlobalInteractionFrame = -1;
			if (flag)
			{
				FittingRoomCurtainGroup.ActiveNpcs.Clear();
				FittingRoomCurtainGroup._npcSimulationTime = 0f;
				FittingRoomCurtainGroup._nextNpcRescanTime = 0f;
				FittingRoomCurtainGroup._nextNpcPollTime = 0f;
			}
			if (this._slots != null)
			{
				for (int i = 0; i < this._slots.Length; i++)
				{
					FittingRoomCurtainGroup.CurtainSlot curtainSlot = this._slots[i];
					if (((curtainSlot != null) ? curtainSlot.curtain : null) != null)
					{
						FittingRoomCurtainGroup.ResetNpcState(curtainSlot);
						this.ApplyStateImmediately(curtainSlot);
					}
				}
			}
		}

		// Token: 0x06008EBF RID: 36543 RVA: 0x003A5A90 File Offset: 0x003A3C90
		private void OnDisable()
		{
			FittingRoomCurtainGroup.ActiveGroups.Remove(this);
			FittingRoomCurtainGroup._lastGlobalInteractionFrame = -1;
			this._activeSlot = null;
			this._promptAllowed = false;
			this.HidePrompt();
			if (this._promptRoutine != null)
			{
				base.StopCoroutine(this._promptRoutine);
				this._promptRoutine = null;
			}
			if (this._slots != null)
			{
				for (int i = 0; i < this._slots.Length; i++)
				{
					FittingRoomCurtainGroup.CurtainSlot curtainSlot = this._slots[i];
					if (curtainSlot != null)
					{
						if (curtainSlot.npcOpened && curtainSlot.isOpen)
						{
							curtainSlot.isOpen = false;
							this.ApplyStateImmediately(curtainSlot);
						}
						if (curtainSlot.animation != null)
						{
							base.StopCoroutine(curtainSlot.animation);
						}
						curtainSlot.animation = null;
						FittingRoomCurtainGroup.ResetNpcState(curtainSlot);
					}
				}
			}
			if (FittingRoomCurtainGroup.ActiveGroups.Count == 0)
			{
				FittingRoomCurtainGroup.ActiveNpcs.Clear();
				FittingRoomCurtainGroup._npcSimulationTime = 0f;
				FittingRoomCurtainGroup._nextNpcRescanTime = 0f;
				FittingRoomCurtainGroup._nextNpcPollTime = 0f;
			}
		}

		// Token: 0x06008EC0 RID: 36544 RVA: 0x003A5B78 File Offset: 0x003A3D78
		private void OnDestroy()
		{
			if (this._openConfig != null)
			{
				Object.Destroy(this._openConfig);
			}
			if (this._closeConfig != null)
			{
				Object.Destroy(this._closeConfig);
			}
		}

		// Token: 0x06008EC1 RID: 36545 RVA: 0x003A5BAC File Offset: 0x003A3DAC
		private void Update()
		{
			if (FittingRoomCurtainGroup._lastGlobalInteractionFrame == Time.frameCount)
			{
				return;
			}
			FittingRoomCurtainGroup._lastGlobalInteractionFrame = Time.frameCount;
			FittingRoomCurtainGroup.UpdateGlobalInteraction();
			FittingRoomCurtainGroup.UpdateGlobalNpcInteraction();
		}

		// Token: 0x06008EC2 RID: 36546 RVA: 0x003A5BD0 File Offset: 0x003A3DD0
		private void BuildCurtainSlots()
		{
			Transform transform = null;
			Transform transform2 = null;
			foreach (Transform transform3 in base.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform3 == base.transform))
				{
					string text = transform3.name.Trim();
					if (text.Equals("curtain", StringComparison.OrdinalIgnoreCase))
					{
						transform = transform3;
					}
					else if (text.Equals("curtain 2", StringComparison.OrdinalIgnoreCase))
					{
						transform2 = transform3;
					}
				}
			}
			if (transform == null || transform2 == null)
			{
				this._slots = Array.Empty<FittingRoomCurtainGroup.CurtainSlot>();
				Debug.LogWarning("[FittingRoom] '" + base.name + "' does not contain both 'curtain' and 'curtain 2'; interaction was not installed.", this);
				return;
			}
			Transform[] componentsInChildren;
			Renderer renderer = this.FindRoomRenderer(componentsInChildren);
			if (renderer == null)
			{
				this._slots = Array.Empty<FittingRoomCurtainGroup.CurtainSlot>();
				Debug.LogWarning("[FittingRoom] '" + base.name + "' does not contain the fitting-room shell renderer.", this);
				return;
			}
			Vector3 vector = transform2.position - transform.position;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = base.transform.right;
			}
			vector.Normalize();
			Bounds bounds = renderer.bounds;
			float num = FittingRoomCurtainGroup.ProjectedExtent(bounds.extents, vector);
			float num2 = num * 0.5f;
			float num3 = Mathf.Max(0.25f, num * 0.9f);
			float num4 = num3 * 0.18f;
			Vector3 vector2 = bounds.center - vector * num2;
			Vector3 vector3 = bounds.center + vector * num2;
			Vector3 vector4 = vector2 - vector * Mathf.Max(0f, num2 - num4 * 0.5f);
			Vector3 vector5 = vector3 + vector * Mathf.Max(0f, num2 - num4 * 0.5f);
			Renderer[] componentsInChildren2 = transform.GetComponentsInChildren<Renderer>(true);
			Renderer[] componentsInChildren3 = transform2.GetComponentsInChildren<Renderer>(true);
			Vector3 vector6 = (FittingRoomCurtainGroup.GetVisualCentre(transform, componentsInChildren2) + FittingRoomCurtainGroup.GetVisualCentre(transform2, componentsInChildren3)) * 0.5f;
			Vector3 vector7 = vector6 - bounds.center;
			vector7 -= vector * Vector3.Dot(vector7, vector);
			vector7.y = 0f;
			if (vector7.sqrMagnitude < 0.0001f)
			{
				vector7 = Vector3.Cross(vector, Vector3.up);
			}
			vector7.Normalize();
			float num5 = Vector3.Dot(vector6 - bounds.center, vector7);
			Vector3 vector8 = vector2 + vector7 * num5;
			Vector3 vector9 = vector3 + vector7 * num5;
			Vector3 vector10 = vector8 + vector7 * this.promptOutsideOffset;
			Vector3 vector11 = vector9 + vector7 * this.promptOutsideOffset;
			this._slots = new FittingRoomCurtainGroup.CurtainSlot[]
			{
				this.CreateCurtainSlot(transform, true, vector, vector4, vector2, vector10, vector8, vector7, num4, num3),
				this.CreateCurtainSlot(transform2, false, vector, vector5, vector3, vector11, vector9, vector7, num4, num3)
			};
		}

		// Token: 0x06008EC3 RID: 36547 RVA: 0x003A5ED4 File Offset: 0x003A40D4
		private Renderer FindRoomRenderer(Transform[] descendants)
		{
			foreach (Transform transform in descendants)
			{
				if (transform.name.Trim().Equals("fitting rooms", StringComparison.OrdinalIgnoreCase))
				{
					Renderer component = transform.GetComponent<Renderer>();
					if (component != null)
					{
						return component;
					}
				}
			}
			return null;
		}

		// Token: 0x06008EC4 RID: 36548 RVA: 0x003A5F20 File Offset: 0x003A4120
		private FittingRoomCurtainGroup.CurtainSlot CreateCurtainSlot(Transform curtain, bool isOpen, Vector3 stallAxis, Vector3 openVisualCentre, Vector3 closedVisualCentre, Vector3 promptWorldPosition, Vector3 entranceWorldPosition, Vector3 outsideWorldDirection, float openWidth, float closedWidth)
		{
			Renderer[] componentsInChildren = curtain.GetComponentsInChildren<Renderer>(true);
			Vector3 localScale = curtain.localScale;
			Vector3 localPosition = curtain.localPosition;
			int num = FittingRoomCurtainGroup.FindDominantLocalAxis(curtain, stallAxis);
			float num2 = Mathf.Max(0.001f, FittingRoomCurtainGroup.ProjectRendererWidth(componentsInChildren, stallAxis));
			Vector3 vector = localScale;
			Vector3 vector2 = localScale;
			FittingRoomCurtainGroup.SetAxis(ref vector, num, FittingRoomCurtainGroup.GetAxis(localScale, num) * openWidth / num2);
			FittingRoomCurtainGroup.SetAxis(ref vector2, num, FittingRoomCurtainGroup.GetAxis(localScale, num) * closedWidth / num2);
			Vector3 vector3 = FittingRoomCurtainGroup.CalculateLocalPositionForVisualCentre(curtain, componentsInChildren, stallAxis, vector, openVisualCentre, localPosition, localScale);
			Vector3 vector4 = FittingRoomCurtainGroup.CalculateLocalPositionForVisualCentre(curtain, componentsInChildren, stallAxis, vector2, closedVisualCentre, localPosition, localScale);
			curtain.localPosition = localPosition;
			curtain.localScale = localScale;
			return new FittingRoomCurtainGroup.CurtainSlot(curtain, componentsInChildren, isOpen, vector3, vector4, vector, vector2, base.transform.InverseTransformPoint(promptWorldPosition), base.transform.InverseTransformPoint(entranceWorldPosition), base.transform.InverseTransformDirection(outsideWorldDirection));
		}

		// Token: 0x06008EC5 RID: 36549 RVA: 0x003A5FF4 File Offset: 0x003A41F4
		private static Vector3 CalculateLocalPositionForVisualCentre(Transform curtain, Renderer[] renderers, Vector3 stallAxis, Vector3 scale, Vector3 desiredCentre, Vector3 originalPosition, Vector3 originalScale)
		{
			curtain.localPosition = originalPosition;
			curtain.localScale = scale;
			Vector3 visualCentre = FittingRoomCurtainGroup.GetVisualCentre(curtain, renderers);
			float num = Vector3.Dot(desiredCentre - visualCentre, stallAxis);
			curtain.position += stallAxis * num;
			Vector3 localPosition = curtain.localPosition;
			curtain.localPosition = originalPosition;
			curtain.localScale = originalScale;
			return localPosition;
		}

		// Token: 0x06008EC6 RID: 36550 RVA: 0x003A6058 File Offset: 0x003A4258
		private static int FindDominantLocalAxis(Transform curtain, Vector3 worldAxis)
		{
			Vector3 vector = curtain.InverseTransformDirection(worldAxis);
			float num = Mathf.Abs(vector.x);
			float num2 = Mathf.Abs(vector.y);
			float num3 = Mathf.Abs(vector.z);
			if (num >= num2 && num >= num3)
			{
				return 0;
			}
			if (num2 < num3)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06008EC7 RID: 36551 RVA: 0x003A60A0 File Offset: 0x003A42A0
		private static float ProjectRendererWidth(Renderer[] renderers, Vector3 axis)
		{
			bool flag = false;
			Bounds bounds = default(Bounds);
			foreach (Renderer renderer in renderers)
			{
				if (!(renderer == null) && renderer.enabled)
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (!flag)
			{
				return 0f;
			}
			return FittingRoomCurtainGroup.ProjectedExtent(bounds.extents, axis) * 2f;
		}

		// Token: 0x06008EC8 RID: 36552 RVA: 0x003A6111 File Offset: 0x003A4311
		private static float ProjectedExtent(Vector3 extents, Vector3 axis)
		{
			return Mathf.Abs(axis.x) * extents.x + Mathf.Abs(axis.y) * extents.y + Mathf.Abs(axis.z) * extents.z;
		}

		// Token: 0x06008EC9 RID: 36553 RVA: 0x003A614B File Offset: 0x003A434B
		private static float GetAxis(Vector3 value, int axis)
		{
			if (axis == 0)
			{
				return value.x;
			}
			if (axis != 1)
			{
				return value.z;
			}
			return value.y;
		}

		// Token: 0x06008ECA RID: 36554 RVA: 0x003A6168 File Offset: 0x003A4368
		private static void SetAxis(ref Vector3 value, int axis, float component)
		{
			if (axis == 0)
			{
				value.x = component;
				return;
			}
			if (axis == 1)
			{
				value.y = component;
				return;
			}
			value.z = component;
		}

		// Token: 0x06008ECB RID: 36555 RVA: 0x003A6188 File Offset: 0x003A4388
		private void CreatePromptConfigs()
		{
			this._openConfig = FittingRoomCurtainGroup.CreateConfig("Fitting Room", "Open Curtain");
			this._closeConfig = FittingRoomCurtainGroup.CreateConfig("Fitting Room", "Close Curtain");
		}

		// Token: 0x06008ECC RID: 36556 RVA: 0x003A61B4 File Offset: 0x003A43B4
		private void CreateAudioSource()
		{
			FittingRoomCurtainGroup.EnsureCurtainSounds();
			GameObject gameObject = new GameObject("FittingRoomCurtainAudio");
			gameObject.transform.SetParent(base.transform, false);
			this._audioSource = gameObject.AddComponent<AudioSource>();
			this._audioSource.playOnAwake = false;
			this._audioSource.loop = false;
			this._audioSource.spatialBlend = 1f;
			this._audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
			this._audioSource.minDistance = 0.8f;
			this._audioSource.maxDistance = this.soundMaxDistance;
			this._audioSource.dopplerLevel = 0f;
		}

		// Token: 0x06008ECD RID: 36557 RVA: 0x003A6254 File Offset: 0x003A4454
		private static void EnsureCurtainSounds()
		{
			if (FittingRoomCurtainGroup._generatedOpenSound == null)
			{
				FittingRoomCurtainGroup._generatedOpenSound = FittingRoomCurtainGroup.CreateCurtainSound("Curtain Fabric Open", true, 38117);
			}
			if (FittingRoomCurtainGroup._generatedCloseSound == null)
			{
				FittingRoomCurtainGroup._generatedCloseSound = FittingRoomCurtainGroup.CreateCurtainSound("Curtain Fabric Close", false, 99371);
			}
		}

		// Token: 0x06008ECE RID: 36558 RVA: 0x003A62A8 File Offset: 0x003A44A8
		private static AudioClip CreateCurtainSound(string clipName, bool opening, int seed)
		{
			float num = (opening ? 0.45f : 0.49f);
			int num2 = Mathf.CeilToInt(44100f * num);
			float[] array = new float[num2];
			Random random = new Random(seed);
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)i / 44100f;
				float num6 = num5 / num;
				float num7 = (float)(random.NextDouble() * 2.0 - 1.0);
				num3 += (num7 - num3) * 0.045f;
				num4 += (num7 - num4) * 0.22f;
				float num8 = num4 - num3;
				float num9 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num6 / 0.09f));
				float num10 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((1f - num6) / 0.18f));
				float num11 = num9 * num10;
				float num12 = 0.78f + 0.22f * Mathf.Sin(num5 * (opening ? 67f : 58f) + Mathf.Sin(num5 * 19f) * 0.7f);
				array[i] = (num3 * 0.72f + num8 * 0.34f) * num11 * num12;
			}
			float[] array2;
			if (!opening)
			{
				RuntimeHelpers.InitializeArray(array2 = new float[5], fieldof(<PrivateImplementationDetails>.EF2C190DB856469AB9DA8A6052962E28396505358B1F7BA6EE56C8F30F3D5E02).FieldHandle);
			}
			else
			{
				RuntimeHelpers.InitializeArray(array2 = new float[5], fieldof(<PrivateImplementationDetails>.E1984590AA2DB0266EF4B1E956BE30AD7A139D145FC06E229C5EF4D2243BC994).FieldHandle);
			}
			float[] array3 = array2;
			for (int j = 0; j < array3.Length; j++)
			{
				float num13 = 1650f + (float)j * 83f + (opening ? 90f : (-45f));
				FittingRoomCurtainGroup.AddRailContact(array, 44100, array3[j], num13, 0.032f - (float)j * 0.0025f, random);
			}
			if (!opening)
			{
				FittingRoomCurtainGroup.AddSoftStop(array, 44100, num - 0.07f, random);
			}
			FittingRoomCurtainGroup.NormalizeSamples(array, 0.72f);
			AudioClip audioClip = AudioClip.Create(clipName, num2, 1, 44100, false);
			audioClip.hideFlags = HideFlags.HideAndDontSave;
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06008ECF RID: 36559 RVA: 0x003A64B4 File Offset: 0x003A46B4
		private static void AddRailContact(float[] samples, int sampleRate, float startTime, float frequency, float amplitude, Random random)
		{
			int num = Mathf.RoundToInt(startTime * (float)sampleRate);
			int num2 = Mathf.RoundToInt(0.027f * (float)sampleRate);
			int num3 = 0;
			while (num3 < num2 && num + num3 < samples.Length)
			{
				float num4 = (float)num3 / (float)sampleRate;
				float num5 = Mathf.Exp(-num4 * 145f);
				float num6 = Mathf.Sin(6.2831855f * frequency * num4) * 0.62f;
				float num7 = (float)(random.NextDouble() * 2.0 - 1.0) * 0.38f;
				samples[num + num3] += (num6 + num7) * num5 * amplitude;
				num3++;
			}
		}

		// Token: 0x06008ED0 RID: 36560 RVA: 0x003A6554 File Offset: 0x003A4754
		private static void AddSoftStop(float[] samples, int sampleRate, float startTime, Random random)
		{
			int num = Mathf.RoundToInt(startTime * (float)sampleRate);
			int num2 = Mathf.RoundToInt(0.075f * (float)sampleRate);
			float num3 = 0f;
			int num4 = 0;
			while (num4 < num2 && num + num4 < samples.Length)
			{
				float num5 = (float)num4 / (float)sampleRate;
				float num6 = (float)(random.NextDouble() * 2.0 - 1.0);
				num3 += (num6 - num3) * 0.055f;
				float num7 = Mathf.Exp(-num5 * 48f);
				float num8 = Mathf.Sin(659.7345f * num5) * 0.055f;
				samples[num + num4] += (num3 * 0.35f + num8) * num7;
				num4++;
			}
		}

		// Token: 0x06008ED1 RID: 36561 RVA: 0x003A6604 File Offset: 0x003A4804
		private static void NormalizeSamples(float[] samples, float targetPeak)
		{
			float num = 0f;
			for (int i = 0; i < samples.Length; i++)
			{
				num = Mathf.Max(num, Mathf.Abs(samples[i]));
			}
			if (num < 0.0001f)
			{
				return;
			}
			float num2 = targetPeak / num;
			for (int j = 0; j < samples.Length; j++)
			{
				samples[j] *= num2;
			}
		}

		// Token: 0x06008ED2 RID: 36562 RVA: 0x003A665C File Offset: 0x003A485C
		private static InteractionConfigSO CreateConfig(string objectName, string action)
		{
			InteractionConfigSO interactionConfigSO = ScriptableObject.CreateInstance<InteractionConfigSO>();
			interactionConfigSO.name = "Runtime Fitting Room " + action;
			interactionConfigSO.hideFlags = HideFlags.HideAndDontSave;
			interactionConfigSO.objectName = objectName;
			interactionConfigSO.objectNameColor = Color.white;
			interactionConfigSO.mainActionName = action;
			interactionConfigSO.mainKeyDisplayLabel = "E";
			interactionConfigSO.secondaryActionName = string.Empty;
			interactionConfigSO.secondaryKeyDisplayLabel = string.Empty;
			return interactionConfigSO;
		}

		// Token: 0x06008ED3 RID: 36563 RVA: 0x003A66C1 File Offset: 0x003A48C1
		private IEnumerator ResolvePromptTemplate()
		{
			while (this._promptUI == null)
			{
				InteractablePromptUI interactablePromptUI = this.FindPromptTemplate();
				if (interactablePromptUI != null)
				{
					Vector3 lossyScale = interactablePromptUI.transform.lossyScale;
					GameObject gameObject = Object.Instantiate<GameObject>(interactablePromptUI.gameObject, base.transform, true);
					gameObject.name = "FittingRoomInteractUI";
					gameObject.SetActive(true);
					this._promptRoot = gameObject.transform;
					this._promptRoot.rotation = Quaternion.identity;
					FittingRoomCurtainGroup.SetWorldScale(this._promptRoot, lossyScale);
					this._promptUI = gameObject.GetComponent<InteractablePromptUI>();
					if (!(this._promptUI == null))
					{
						this._promptUI.Hide();
						this.RefreshPrompt();
						this._promptRoutine = null;
						yield break;
					}
					Object.Destroy(gameObject);
					this._promptRoot = null;
				}
				yield return new WaitForSeconds(0.5f);
			}
			this._promptRoutine = null;
			yield break;
		}

		// Token: 0x06008ED4 RID: 36564 RVA: 0x003A66D0 File Offset: 0x003A48D0
		private InteractablePromptUI FindPromptTemplate()
		{
			InteractablePromptUI interactablePromptUI = null;
			foreach (InteractablePromptUI interactablePromptUI2 in Object.FindObjectsOfType<InteractablePromptUI>(true))
			{
				if (!(interactablePromptUI2 == null) && !interactablePromptUI2.transform.IsChildOf(base.transform) && !interactablePromptUI2.name.StartsWith("FittingRoomInteractUI", StringComparison.Ordinal))
				{
					if (interactablePromptUI2.name == "ShowerInteractUI")
					{
						return interactablePromptUI2;
					}
					if (interactablePromptUI == null)
					{
						interactablePromptUI = interactablePromptUI2;
					}
				}
			}
			return interactablePromptUI;
		}

		// Token: 0x06008ED5 RID: 36565 RVA: 0x003A6748 File Offset: 0x003A4948
		private Transform ResolvePlayer()
		{
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				this._player = player;
				return this._player;
			}
			if (this._player != null)
			{
				return this._player;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.playerTag);
			this._player = ((gameObject != null) ? gameObject.transform : null);
			return this._player;
		}

		// Token: 0x06008ED6 RID: 36566 RVA: 0x003A67B4 File Offset: 0x003A49B4
		private static void UpdateGlobalInteraction()
		{
			for (int i = FittingRoomCurtainGroup.ActiveGroups.Count - 1; i >= 0; i--)
			{
				if (FittingRoomCurtainGroup.ActiveGroups[i] == null)
				{
					FittingRoomCurtainGroup.ActiveGroups.RemoveAt(i);
				}
			}
			Transform transform = null;
			int num = 0;
			while (num < FittingRoomCurtainGroup.ActiveGroups.Count && transform == null)
			{
				transform = FittingRoomCurtainGroup.ActiveGroups[num].ResolvePlayer();
				num++;
			}
			FittingRoomCurtainGroup fittingRoomCurtainGroup = null;
			FittingRoomCurtainGroup.CurtainSlot curtainSlot = null;
			float num2 = float.PositiveInfinity;
			if (transform != null)
			{
				for (int j = 0; j < FittingRoomCurtainGroup.ActiveGroups.Count; j++)
				{
					FittingRoomCurtainGroup fittingRoomCurtainGroup2 = FittingRoomCurtainGroup.ActiveGroups[j];
					float num3;
					FittingRoomCurtainGroup.CurtainSlot curtainSlot2 = fittingRoomCurtainGroup2.FindNearestCurtain(transform.position, out num3);
					if (curtainSlot2 != null && num3 < num2)
					{
						num2 = num3;
						fittingRoomCurtainGroup = fittingRoomCurtainGroup2;
						curtainSlot = curtainSlot2;
					}
				}
			}
			for (int k = 0; k < FittingRoomCurtainGroup.ActiveGroups.Count; k++)
			{
				FittingRoomCurtainGroup fittingRoomCurtainGroup3 = FittingRoomCurtainGroup.ActiveGroups[k];
				bool flag = fittingRoomCurtainGroup3 == fittingRoomCurtainGroup;
				bool flag2 = flag && curtainSlot != null && transform != null && fittingRoomCurtainGroup3.IsPlayerOutside(curtainSlot, transform.position);
				fittingRoomCurtainGroup3.SetActiveSlot(flag ? curtainSlot : null, flag2);
			}
			if (fittingRoomCurtainGroup == null || curtainSlot == null)
			{
				return;
			}
			if (fittingRoomCurtainGroup._promptAllowed)
			{
				fittingRoomCurtainGroup.UpdatePromptPosition(curtainSlot);
			}
			if (fittingRoomCurtainGroup.interactInput != null && fittingRoomCurtainGroup.interactInput.GetButtonDown())
			{
				fittingRoomCurtainGroup.Toggle(curtainSlot);
			}
		}

		// Token: 0x06008ED7 RID: 36567 RVA: 0x003A692C File Offset: 0x003A4B2C
		private static void UpdateGlobalNpcInteraction()
		{
			bool flag = false;
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			for (int i = FittingRoomCurtainGroup.ActiveGroups.Count - 1; i >= 0; i--)
			{
				FittingRoomCurtainGroup fittingRoomCurtainGroup = FittingRoomCurtainGroup.ActiveGroups[i];
				if (fittingRoomCurtainGroup == null)
				{
					FittingRoomCurtainGroup.ActiveGroups.RemoveAt(i);
				}
				else if (fittingRoomCurtainGroup.npcsCanOpenCurtains)
				{
					flag = true;
					num = Mathf.Min(num, Mathf.Max(0.25f, fittingRoomCurtainGroup.npcRescanInterval));
					num2 = Mathf.Min(num2, Mathf.Max(0.05f, fittingRoomCurtainGroup.npcPollInterval));
				}
			}
			if (!flag)
			{
				return;
			}
			float simulationDeltaTime = NPCSimulationPause.SimulationDeltaTime;
			if (NPCSimulationPause.IsPaused || simulationDeltaTime <= 0f)
			{
				return;
			}
			FittingRoomCurtainGroup._npcSimulationTime += simulationDeltaTime;
			if (FittingRoomCurtainGroup._npcSimulationTime >= FittingRoomCurtainGroup._nextNpcRescanTime)
			{
				FittingRoomCurtainGroup._nextNpcRescanTime = FittingRoomCurtainGroup._npcSimulationTime + num;
				FittingRoomCurtainGroup.ActiveNpcs.Clear();
				FittingRoomCurtainGroup.ActiveNpcs.AddRange(Object.FindObjectsByType<NPCSenses>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			}
			if (FittingRoomCurtainGroup._npcSimulationTime < FittingRoomCurtainGroup._nextNpcPollTime)
			{
				return;
			}
			FittingRoomCurtainGroup._nextNpcPollTime = FittingRoomCurtainGroup._npcSimulationTime + num2;
			FittingRoomCurtainGroup.AssignGlobalApproachingNpcs();
			for (int j = 0; j < FittingRoomCurtainGroup.ActiveGroups.Count; j++)
			{
				FittingRoomCurtainGroup fittingRoomCurtainGroup2 = FittingRoomCurtainGroup.ActiveGroups[j];
				if (fittingRoomCurtainGroup2 != null && fittingRoomCurtainGroup2.npcsCanOpenCurtains)
				{
					fittingRoomCurtainGroup2.PollCurtainsForNpcs();
				}
			}
		}

		// Token: 0x06008ED8 RID: 36568 RVA: 0x003A6A7C File Offset: 0x003A4C7C
		private void PollCurtainsForNpcs()
		{
			if (this._slots == null)
			{
				return;
			}
			for (int i = 0; i < this._slots.Length; i++)
			{
				FittingRoomCurtainGroup.CurtainSlot curtainSlot = this._slots[i];
				if (((curtainSlot != null) ? curtainSlot.curtain : null) == null || !curtainSlot.curtain.gameObject.activeInHierarchy)
				{
					if (curtainSlot != null)
					{
						FittingRoomCurtainGroup.ResetNpcState(curtainSlot);
					}
				}
				else
				{
					bool flag = this.UpdateNearbyNpcs(curtainSlot);
					bool npcWasNearby = curtainSlot.npcWasNearby;
					if (curtainSlot.npcBlockedUntilClear)
					{
						curtainSlot.npcOpened = false;
						bool flag2 = FittingRoomCurtainGroup._npcSimulationTime - curtainSlot.lastPlayerInteractTime >= this.npcPlayerYieldSeconds;
						if (flag || !flag2)
						{
							curtainSlot.npcWasNearby = flag;
							goto IL_02C7;
						}
						curtainSlot.npcBlockedUntilClear = false;
						curtainSlot.npcRolledIds.Clear();
					}
					if (!curtainSlot.isOpen)
					{
						if (curtainSlot.npcOpened && flag)
						{
							this.SetCurtainOpen(curtainSlot, true, false);
							if (this.verboseNpcLogs)
							{
								Debug.Log(string.Concat(new string[]
								{
									"[FittingRoom] NPC reopened '",
									curtainSlot.curtain.name,
									"' on '",
									base.name,
									"'."
								}), this);
							}
						}
						else if (curtainSlot.approachingNpcId != 0 && !curtainSlot.npcOpened)
						{
							curtainSlot.npcRolledIds.Add(curtainSlot.approachingNpcId);
							if (Random.value < this.npcOpenChance)
							{
								this.SetCurtainOpen(curtainSlot, true, false);
								curtainSlot.npcOpened = true;
								if (this.verboseNpcLogs)
								{
									Debug.Log(string.Concat(new string[]
									{
										"[FittingRoom] NPC opened '",
										curtainSlot.curtain.name,
										"' on '",
										base.name,
										"'."
									}), this);
								}
							}
							else if (this.verboseNpcLogs)
							{
								Debug.Log(string.Concat(new string[]
								{
									"[FittingRoom] NPC ignored '",
									curtainSlot.curtain.name,
									"' on '",
									base.name,
									"' (open-chance roll failed)."
								}), this);
							}
						}
					}
					if (!flag && npcWasNearby)
					{
						curtainSlot.npcLeftAt = FittingRoomCurtainGroup._npcSimulationTime;
						curtainSlot.npcRolledIds.Clear();
					}
					if (!flag && curtainSlot.npcOpened)
					{
						if (curtainSlot.npcLeftAt < 0f)
						{
							curtainSlot.npcLeftAt = FittingRoomCurtainGroup._npcSimulationTime;
						}
						if (FittingRoomCurtainGroup._npcSimulationTime - curtainSlot.npcLeftAt >= this.npcCloseDelay)
						{
							if (curtainSlot.isOpen && Random.value < this.npcCloseChance)
							{
								this.SetCurtainOpen(curtainSlot, false, false);
								if (this.verboseNpcLogs)
								{
									Debug.Log(string.Concat(new string[]
									{
										"[FittingRoom] NPC closed '",
										curtainSlot.curtain.name,
										"' on '",
										base.name,
										"' after leaving."
									}), this);
								}
							}
							curtainSlot.npcOpened = false;
						}
					}
					curtainSlot.npcWasNearby = flag;
				}
				IL_02C7:;
			}
		}

		// Token: 0x06008ED9 RID: 36569 RVA: 0x003A6D64 File Offset: 0x003A4F64
		private static void AssignGlobalApproachingNpcs()
		{
			for (int i = 0; i < FittingRoomCurtainGroup.ActiveGroups.Count; i++)
			{
				FittingRoomCurtainGroup fittingRoomCurtainGroup = FittingRoomCurtainGroup.ActiveGroups[i];
				if (((fittingRoomCurtainGroup != null) ? fittingRoomCurtainGroup._slots : null) != null)
				{
					for (int j = 0; j < fittingRoomCurtainGroup._slots.Length; j++)
					{
						if (fittingRoomCurtainGroup._slots[j] != null)
						{
							fittingRoomCurtainGroup._slots[j].approachingNpcId = 0;
						}
					}
				}
			}
			for (int k = 0; k < FittingRoomCurtainGroup.ActiveNpcs.Count; k++)
			{
				NPCSenses npcsenses = FittingRoomCurtainGroup.ActiveNpcs[k];
				if (!(npcsenses == null) && npcsenses.gameObject.activeInHierarchy)
				{
					int instanceID = npcsenses.GetInstanceID();
					FittingRoomCurtainGroup fittingRoomCurtainGroup2 = null;
					FittingRoomCurtainGroup.CurtainSlot curtainSlot = null;
					float num = float.PositiveInfinity;
					for (int l = 0; l < FittingRoomCurtainGroup.ActiveGroups.Count; l++)
					{
						FittingRoomCurtainGroup fittingRoomCurtainGroup3 = FittingRoomCurtainGroup.ActiveGroups[l];
						if (!(fittingRoomCurtainGroup3 == null) && fittingRoomCurtainGroup3.npcsCanOpenCurtains && fittingRoomCurtainGroup3._slots != null)
						{
							for (int m = 0; m < fittingRoomCurtainGroup3._slots.Length; m++)
							{
								FittingRoomCurtainGroup.CurtainSlot curtainSlot2 = fittingRoomCurtainGroup3._slots[m];
								float num2;
								if (!(((curtainSlot2 != null) ? curtainSlot2.curtain : null) == null) && curtainSlot2.curtain.gameObject.activeInHierarchy && !curtainSlot2.isOpen && !curtainSlot2.npcOpened && !curtainSlot2.npcBlockedUntilClear && curtainSlot2.approachingNpcId == 0 && !curtainSlot2.npcRolledIds.Contains(instanceID) && fittingRoomCurtainGroup3.TryGetApproachDistance(curtainSlot2, npcsenses, out num2) && num2 < num)
								{
									fittingRoomCurtainGroup2 = fittingRoomCurtainGroup3;
									curtainSlot = curtainSlot2;
									num = num2;
								}
							}
						}
					}
					if (!(fittingRoomCurtainGroup2 == null) && curtainSlot != null)
					{
						curtainSlot.approachingNpcId = instanceID;
					}
				}
			}
		}

		// Token: 0x06008EDA RID: 36570 RVA: 0x003A6F38 File Offset: 0x003A5138
		private bool UpdateNearbyNpcs(FittingRoomCurtainGroup.CurtainSlot slot)
		{
			Vector3 vector = base.transform.TransformPoint(slot.entranceLocalPosition);
			float num = Mathf.Max(Mathf.Max(0.1f, this.npcOpenDistance), this.npcNearbyDistance);
			float num2 = num * num;
			slot.nearbyNpcIds.Clear();
			for (int i = 0; i < FittingRoomCurtainGroup.ActiveNpcs.Count; i++)
			{
				NPCSenses npcsenses = FittingRoomCurtainGroup.ActiveNpcs[i];
				if (!(npcsenses == null) && npcsenses.gameObject.activeInHierarchy)
				{
					Vector3 position = npcsenses.transform.position;
					if (Mathf.Abs(position.y - vector.y) <= this.npcMaxHeightDelta)
					{
						Vector3 vector2 = position - vector;
						vector2.y = 0f;
						if (vector2.sqrMagnitude <= num2)
						{
							slot.nearbyNpcIds.Add(npcsenses.GetInstanceID());
						}
					}
				}
			}
			slot.staleRolledIds.Clear();
			foreach (int num3 in slot.npcRolledIds)
			{
				if (!slot.nearbyNpcIds.Contains(num3))
				{
					slot.staleRolledIds.Add(num3);
				}
			}
			for (int j = 0; j < slot.staleRolledIds.Count; j++)
			{
				slot.npcRolledIds.Remove(slot.staleRolledIds[j]);
			}
			return slot.nearbyNpcIds.Count > 0;
		}

		// Token: 0x06008EDB RID: 36571 RVA: 0x003A70C0 File Offset: 0x003A52C0
		private bool TryGetApproachDistance(FittingRoomCurtainGroup.CurtainSlot slot, NPCSenses npc, out float distance)
		{
			distance = float.PositiveInfinity;
			Vector3 vector = base.transform.TransformPoint(slot.entranceLocalPosition);
			Vector3 position = npc.transform.position;
			if (Mathf.Abs(position.y - vector.y) > this.npcMaxHeightDelta)
			{
				return false;
			}
			Vector3 vector2 = vector - position;
			vector2.y = 0f;
			distance = vector2.magnitude;
			if (distance > Mathf.Max(0.1f, this.npcOpenDistance))
			{
				return false;
			}
			Vector3 vector3 = base.transform.TransformDirection(slot.outsideLocalDirection);
			vector3.y = 0f;
			if (this.npcRequireOutsideSide && vector3.sqrMagnitude > 0.0001f)
			{
				vector3.Normalize();
				Vector3 vector4 = position - vector;
				vector4.y = 0f;
				if (Vector3.Dot(vector4, vector3) < -0.05f)
				{
					return false;
				}
			}
			if (distance < 0.0001f)
			{
				return true;
			}
			Vector3 forward = npc.transform.forward;
			forward.y = 0f;
			return forward.sqrMagnitude <= 0.0001f || Vector3.Dot(forward.normalized, vector2 / distance) >= this.npcFacingDot;
		}

		// Token: 0x06008EDC RID: 36572 RVA: 0x003A71F8 File Offset: 0x003A53F8
		private void SetActiveSlot(FittingRoomCurtainGroup.CurtainSlot slot, bool showPrompt)
		{
			bool flag = slot != this._activeSlot;
			bool flag2 = showPrompt != this._promptAllowed;
			this._activeSlot = slot;
			this._promptAllowed = showPrompt;
			if (flag || flag2)
			{
				this.RefreshPrompt();
			}
			if (this._activeSlot == null || !this._promptAllowed)
			{
				this.HidePrompt();
				return;
			}
			if (this._promptUI == null && this._promptRoutine == null)
			{
				this._promptRoutine = base.StartCoroutine(this.ResolvePromptTemplate());
			}
		}

		// Token: 0x06008EDD RID: 36573 RVA: 0x003A7278 File Offset: 0x003A5478
		private bool IsPlayerOutside(FittingRoomCurtainGroup.CurtainSlot slot, Vector3 playerPosition)
		{
			Vector3 vector = base.transform.TransformPoint(slot.entranceLocalPosition);
			Vector3 normalized = base.transform.TransformDirection(slot.outsideLocalDirection).normalized;
			return Vector3.Dot(playerPosition - vector, normalized) >= -0.05f;
		}

		// Token: 0x06008EDE RID: 36574 RVA: 0x003A72C8 File Offset: 0x003A54C8
		private FittingRoomCurtainGroup.CurtainSlot FindNearestCurtain(Vector3 playerPosition, out float nearestSqrDistance)
		{
			FittingRoomCurtainGroup.CurtainSlot curtainSlot = null;
			nearestSqrDistance = this.interactionDistance * this.interactionDistance;
			if (this._slots == null)
			{
				return null;
			}
			for (int i = 0; i < this._slots.Length; i++)
			{
				FittingRoomCurtainGroup.CurtainSlot curtainSlot2 = this._slots[i];
				if (!(((curtainSlot2 != null) ? curtainSlot2.curtain : null) == null) && curtainSlot2.curtain.gameObject.activeInHierarchy)
				{
					Vector3 promptPosition = this.GetPromptPosition(curtainSlot2);
					promptPosition.y = playerPosition.y;
					float sqrMagnitude = (promptPosition - playerPosition).sqrMagnitude;
					if (sqrMagnitude <= nearestSqrDistance)
					{
						nearestSqrDistance = sqrMagnitude;
						curtainSlot = curtainSlot2;
					}
				}
			}
			return curtainSlot;
		}

		// Token: 0x06008EDF RID: 36575 RVA: 0x003A7366 File Offset: 0x003A5566
		private void Toggle(FittingRoomCurtainGroup.CurtainSlot slot)
		{
			if (slot == null)
			{
				return;
			}
			this.SetCurtainOpen(slot, !slot.isOpen, true);
		}

		// Token: 0x06008EE0 RID: 36576 RVA: 0x003A7380 File Offset: 0x003A5580
		private void SetCurtainOpen(FittingRoomCurtainGroup.CurtainSlot slot, bool open, bool playerOperated)
		{
			if (slot == null || slot.curtain == null)
			{
				return;
			}
			if (playerOperated)
			{
				slot.lastPlayerInteractTime = FittingRoomCurtainGroup._npcSimulationTime;
				slot.npcOpened = false;
				slot.npcLeftAt = -1f;
				slot.npcBlockedUntilClear = true;
				slot.npcRolledIds.Clear();
			}
			if (slot.isOpen == open)
			{
				return;
			}
			slot.isOpen = open;
			Vector3 vector = (slot.isOpen ? slot.openScale : slot.closedScale);
			Vector3 vector2 = (slot.isOpen ? slot.openPosition : slot.closedPosition);
			if (slot.animation != null)
			{
				base.StopCoroutine(slot.animation);
			}
			slot.animationUsesNpcClock = !playerOperated;
			slot.animation = base.StartCoroutine(this.AnimateCurtain(slot, vector2, vector));
			this.PlayCurtainSound(slot, slot.isOpen);
			this.RefreshPrompt();
		}

		// Token: 0x06008EE1 RID: 36577 RVA: 0x003A7458 File Offset: 0x003A5658
		private static void ResetNpcState(FittingRoomCurtainGroup.CurtainSlot slot)
		{
			slot.npcWasNearby = false;
			slot.npcOpened = false;
			slot.npcLeftAt = -1f;
			slot.lastPlayerInteractTime = -999f;
			slot.npcBlockedUntilClear = false;
			slot.approachingNpcId = 0;
			slot.animationUsesNpcClock = false;
			slot.npcRolledIds.Clear();
			slot.nearbyNpcIds.Clear();
			slot.staleRolledIds.Clear();
		}

		// Token: 0x06008EE2 RID: 36578 RVA: 0x003A74C0 File Offset: 0x003A56C0
		private void PlayCurtainSound(FittingRoomCurtainGroup.CurtainSlot slot, bool opening)
		{
			if (this._audioSource == null)
			{
				return;
			}
			AudioClip audioClip = (opening ? FittingRoomCurtainGroup._generatedOpenSound : FittingRoomCurtainGroup._generatedCloseSound);
			if (audioClip == null)
			{
				return;
			}
			this._audioSource.transform.position = base.transform.TransformPoint(slot.entranceLocalPosition);
			this._audioSource.pitch = (opening ? 1.015f : 0.975f) * Random.Range(0.985f, 1.015f);
			this._audioSource.PlayOneShot(audioClip, this.soundVolume);
		}

		// Token: 0x06008EE3 RID: 36579 RVA: 0x003A7554 File Offset: 0x003A5754
		private void ApplyStateImmediately(FittingRoomCurtainGroup.CurtainSlot slot)
		{
			slot.curtain.localPosition = (slot.isOpen ? slot.openPosition : slot.closedPosition);
			slot.curtain.localScale = (slot.isOpen ? slot.openScale : slot.closedScale);
		}

		// Token: 0x06008EE4 RID: 36580 RVA: 0x003A75A3 File Offset: 0x003A57A3
		private IEnumerator AnimateCurtain(FittingRoomCurtainGroup.CurtainSlot slot, Vector3 targetPosition, Vector3 targetScale)
		{
			Vector3 startPosition = slot.curtain.localPosition;
			Vector3 startScale = slot.curtain.localScale;
			if (this.animationDuration <= 0.0001f)
			{
				slot.curtain.localPosition = targetPosition;
				slot.curtain.localScale = targetScale;
				slot.animation = null;
				slot.animationUsesNpcClock = false;
				yield break;
			}
			float elapsed = 0f;
			while (elapsed < this.animationDuration && slot.curtain != null)
			{
				float num = (slot.animationUsesNpcClock ? NPCSimulationPause.SimulationDeltaTime : Time.deltaTime);
				if (num <= 0f)
				{
					yield return null;
				}
				else
				{
					elapsed += num;
					float num2 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / this.animationDuration));
					slot.curtain.localPosition = Vector3.LerpUnclamped(startPosition, targetPosition, num2);
					slot.curtain.localScale = Vector3.LerpUnclamped(startScale, targetScale, num2);
					yield return null;
				}
			}
			if (slot.curtain != null)
			{
				slot.curtain.localPosition = targetPosition;
				slot.curtain.localScale = targetScale;
			}
			slot.animation = null;
			slot.animationUsesNpcClock = false;
			yield break;
		}

		// Token: 0x06008EE5 RID: 36581 RVA: 0x003A75C8 File Offset: 0x003A57C8
		private void RefreshPrompt()
		{
			if (this._promptUI == null)
			{
				return;
			}
			if (this._activeSlot == null || !this._promptAllowed)
			{
				this.HidePrompt();
				return;
			}
			this.UpdatePromptPosition(this._activeSlot);
			this._promptUI.Show(this._activeSlot.isOpen ? this._closeConfig : this._openConfig);
		}

		// Token: 0x06008EE6 RID: 36582 RVA: 0x003A762D File Offset: 0x003A582D
		private void HidePrompt()
		{
			if (this._promptUI != null)
			{
				this._promptUI.Hide();
			}
		}

		// Token: 0x06008EE7 RID: 36583 RVA: 0x003A7648 File Offset: 0x003A5848
		private void UpdatePromptPosition(FittingRoomCurtainGroup.CurtainSlot slot)
		{
			if (this._promptRoot != null && slot != null)
			{
				this._promptRoot.position = this.GetPromptPosition(slot);
			}
		}

		// Token: 0x06008EE8 RID: 36584 RVA: 0x003A766D File Offset: 0x003A586D
		private Vector3 GetPromptPosition(FittingRoomCurtainGroup.CurtainSlot slot)
		{
			return base.transform.TransformPoint(slot.promptLocalPosition);
		}

		// Token: 0x06008EE9 RID: 36585 RVA: 0x003A7680 File Offset: 0x003A5880
		private static Vector3 GetVisualCentre(Transform curtain, Renderer[] renderers)
		{
			bool flag = false;
			Bounds bounds = default(Bounds);
			foreach (Renderer renderer in renderers)
			{
				if (!(renderer == null) && renderer.enabled)
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (!flag)
			{
				return curtain.position;
			}
			return bounds.center;
		}

		// Token: 0x06008EEA RID: 36586 RVA: 0x003A76E8 File Offset: 0x003A58E8
		private static void SetWorldScale(Transform target, Vector3 worldScale)
		{
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(worldScale.x / Mathf.Max(0.0001f, Mathf.Abs(vector.x)), worldScale.y / Mathf.Max(0.0001f, Mathf.Abs(vector.y)), worldScale.z / Mathf.Max(0.0001f, Mathf.Abs(vector.z)));
		}

		// Token: 0x04009232 RID: 37426
		private const float ClosedCoverage = 0.9f;

		// Token: 0x04009233 RID: 37427
		private const float OpenCoverage = 0.18f;

		// Token: 0x04009234 RID: 37428
		private static readonly List<FittingRoomCurtainGroup> ActiveGroups = new List<FittingRoomCurtainGroup>();

		// Token: 0x04009235 RID: 37429
		private static readonly List<NPCSenses> ActiveNpcs = new List<NPCSenses>();

		// Token: 0x04009236 RID: 37430
		private static int _lastGlobalInteractionFrame = -1;

		// Token: 0x04009237 RID: 37431
		private static float _npcSimulationTime;

		// Token: 0x04009238 RID: 37432
		private static float _nextNpcRescanTime;

		// Token: 0x04009239 RID: 37433
		private static float _nextNpcPollTime;

		// Token: 0x0400923A RID: 37434
		[Header("Interaction")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x0400923B RID: 37435
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "X", "E");

		// Token: 0x0400923C RID: 37436
		[SerializeField]
		[Min(0.5f)]
		private float interactionDistance = 2.15f;

		// Token: 0x0400923D RID: 37437
		[SerializeField]
		[Min(0f)]
		private float promptOutsideOffset = 0.35f;

		// Token: 0x0400923E RID: 37438
		[Header("Curtain movement")]
		[SerializeField]
		[Min(0f)]
		private float animationDuration = 0.38f;

		// Token: 0x0400923F RID: 37439
		[Header("Audio")]
		[SerializeField]
		[Range(0f, 1f)]
		private float soundVolume = 0.55f;

		// Token: 0x04009240 RID: 37440
		[SerializeField]
		[Min(1f)]
		private float soundMaxDistance = 7f;

		// Token: 0x04009241 RID: 37441
		[Header("NPC behaviour")]
		[Tooltip("Lets nearby NPCs open closed fitting-room curtains without adding components to the NPC prefabs.")]
		[SerializeField]
		private bool npcsCanOpenCurtains = true;

		// Token: 0x04009242 RID: 37442
		[Tooltip("Chance that a closed curtain opens when an NPC first approaches it. The roll happens once per approach, not every poll.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float npcOpenChance = 0.35f;

		// Token: 0x04009243 RID: 37443
		[Tooltip("Chance that a curtain opened by an NPC closes after the last nearby NPC leaves. Player-opened curtains are never auto-closed.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float npcCloseChance = 0.6f;

		// Token: 0x04009244 RID: 37444
		[Tooltip("Seconds an NPC-opened curtain remains open after the last nearby NPC leaves before the close roll.")]
		[SerializeField]
		[Min(0f)]
		private float npcCloseDelay = 1.5f;

		// Token: 0x04009245 RID: 37445
		[Tooltip("How often the shared list of runtime-spawned NPCs is refreshed.")]
		[SerializeField]
		[Min(0.25f)]
		private float npcRescanInterval = 2f;

		// Token: 0x04009246 RID: 37446
		[Tooltip("How often NPC proximity is checked. All fitting rooms share a single global poll.")]
		[SerializeField]
		[Min(0.05f)]
		private float npcPollInterval = 0.2f;

		// Token: 0x04009247 RID: 37447
		[Tooltip("Minimum time NPC logic yields after the player uses a curtain. If an NPC is already nearby, it also waits for that NPC to clear the entrance.")]
		[SerializeField]
		[Min(0f)]
		private float npcPlayerYieldSeconds = 0.75f;

		// Token: 0x04009248 RID: 37448
		[Header("NPC approach")]
		[Tooltip("Horizontal distance from a stall entrance at which an NPC may roll to open its curtain.")]
		[SerializeField]
		[Min(0.1f)]
		private float npcOpenDistance = 1.35f;

		// Token: 0x04009249 RID: 37449
		[Tooltip("Wider radius that keeps an NPC-opened curtain open while somebody is still passing through.")]
		[SerializeField]
		[Min(0.1f)]
		private float npcNearbyDistance = 2.2f;

		// Token: 0x0400924A RID: 37450
		[Tooltip("How directly the NPC must face the stall entrance. 0.55 is roughly a 57 degree cone.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float npcFacingDot = 0.55f;

		// Token: 0x0400924B RID: 37451
		[Tooltip("Only NPCs approaching from outside the fitting room may initiate an opening. NPCs already passing through still keep it open.")]
		[SerializeField]
		private bool npcRequireOutsideSide = true;

		// Token: 0x0400924C RID: 37452
		[Tooltip("Maximum height difference between an NPC and an entrance, preventing NPCs on another mall floor from opening it.")]
		[SerializeField]
		[Min(0.1f)]
		private float npcMaxHeightDelta = 2f;

		// Token: 0x0400924D RID: 37453
		[Tooltip("Writes an entry when an NPC opens, closes, or ignores a curtain.")]
		[SerializeField]
		private bool verboseNpcLogs;

		// Token: 0x0400924E RID: 37454
		private static AudioClip _generatedOpenSound;

		// Token: 0x0400924F RID: 37455
		private static AudioClip _generatedCloseSound;

		// Token: 0x04009250 RID: 37456
		private FittingRoomCurtainGroup.CurtainSlot[] _slots;

		// Token: 0x04009251 RID: 37457
		private FittingRoomCurtainGroup.CurtainSlot _activeSlot;

		// Token: 0x04009252 RID: 37458
		private bool _promptAllowed;

		// Token: 0x04009253 RID: 37459
		private Transform _player;

		// Token: 0x04009254 RID: 37460
		private InteractablePromptUI _promptUI;

		// Token: 0x04009255 RID: 37461
		private Transform _promptRoot;

		// Token: 0x04009256 RID: 37462
		private InteractionConfigSO _openConfig;

		// Token: 0x04009257 RID: 37463
		private InteractionConfigSO _closeConfig;

		// Token: 0x04009258 RID: 37464
		private Coroutine _promptRoutine;

		// Token: 0x04009259 RID: 37465
		private AudioSource _audioSource;

		// Token: 0x0200120F RID: 4623
		private sealed class CurtainSlot
		{
			// Token: 0x06008EED RID: 36589 RVA: 0x003A7880 File Offset: 0x003A5A80
			public CurtainSlot(Transform curtain, Renderer[] renderers, bool isOpen, Vector3 openPosition, Vector3 closedPosition, Vector3 openScale, Vector3 closedScale, Vector3 promptLocalPosition, Vector3 entranceLocalPosition, Vector3 outsideLocalDirection)
			{
				this.curtain = curtain;
				this.renderers = renderers;
				this.isOpen = isOpen;
				this.openPosition = openPosition;
				this.closedPosition = closedPosition;
				this.openScale = openScale;
				this.closedScale = closedScale;
				this.promptLocalPosition = promptLocalPosition;
				this.entranceLocalPosition = entranceLocalPosition;
				this.outsideLocalDirection = outsideLocalDirection;
			}

			// Token: 0x0400925A RID: 37466
			public readonly Transform curtain;

			// Token: 0x0400925B RID: 37467
			public readonly Renderer[] renderers;

			// Token: 0x0400925C RID: 37468
			public readonly Vector3 openPosition;

			// Token: 0x0400925D RID: 37469
			public readonly Vector3 closedPosition;

			// Token: 0x0400925E RID: 37470
			public readonly Vector3 openScale;

			// Token: 0x0400925F RID: 37471
			public readonly Vector3 closedScale;

			// Token: 0x04009260 RID: 37472
			public readonly Vector3 promptLocalPosition;

			// Token: 0x04009261 RID: 37473
			public readonly Vector3 entranceLocalPosition;

			// Token: 0x04009262 RID: 37474
			public readonly Vector3 outsideLocalDirection;

			// Token: 0x04009263 RID: 37475
			public bool isOpen;

			// Token: 0x04009264 RID: 37476
			public Coroutine animation;

			// Token: 0x04009265 RID: 37477
			public bool npcWasNearby;

			// Token: 0x04009266 RID: 37478
			public bool npcOpened;

			// Token: 0x04009267 RID: 37479
			public float npcLeftAt = -1f;

			// Token: 0x04009268 RID: 37480
			public float lastPlayerInteractTime = -999f;

			// Token: 0x04009269 RID: 37481
			public bool npcBlockedUntilClear;

			// Token: 0x0400926A RID: 37482
			public int approachingNpcId;

			// Token: 0x0400926B RID: 37483
			public bool animationUsesNpcClock;

			// Token: 0x0400926C RID: 37484
			public readonly HashSet<int> npcRolledIds = new HashSet<int>();

			// Token: 0x0400926D RID: 37485
			public readonly HashSet<int> nearbyNpcIds = new HashSet<int>();

			// Token: 0x0400926E RID: 37486
			public readonly List<int> staleRolledIds = new List<int>();
		}
	}
}
