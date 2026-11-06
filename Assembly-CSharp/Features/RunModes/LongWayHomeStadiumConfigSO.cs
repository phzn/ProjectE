using System;
using Features.CielVoice;
using Features.Detection;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CFB RID: 3323
	[CreateAssetMenu(fileName = "LongWayHomeStadiumConfig", menuName = "Project Exhibitionist/Long Way Home/Stadium Config")]
	public sealed class LongWayHomeStadiumConfigSO : ScriptableObject
	{
		// Token: 0x060060B4 RID: 24756 RVA: 0x00265981 File Offset: 0x00263B81
		public static LongWayHomeStadiumConfigSO Load()
		{
			return Resources.Load<LongWayHomeStadiumConfigSO>("LongWayHomeStadiumConfig");
		}

		// Token: 0x060060B5 RID: 24757 RVA: 0x0026598D File Offset: 0x00263B8D
		public float BounceScaleFor(int boobsType)
		{
			if (this.bounceSizeScale == null || this.bounceSizeScale.Length == 0)
			{
				return 1f;
			}
			return Mathf.Max(0f, this.bounceSizeScale[Mathf.Clamp(boobsType, 0, this.bounceSizeScale.Length - 1)]);
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x002659C8 File Offset: 0x00263BC8
		public bool HasRequiredAssets(out string missing)
		{
			string text;
			if (!(this.wallPose == null))
			{
				if (!(this.fieldRun == null))
				{
					if (!(this.lookAtMe == null))
					{
						if (!(this.escapeIdle == null))
						{
							if (!(this.escapeRun == null))
							{
								if (!(this.faceCoverArms == null))
								{
									if (!(this.armsMask == null))
									{
										if (LongWayHomeStadiumConfigSO.HasRuntimeFace(this.hornyFaceAnimation))
										{
											if (LongWayHomeStadiumConfigSO.HasRuntimeFace(this.embarrassedFaceAnimation))
											{
												if (!(this.headBag == null) && !(this.headBag.handPrefab == null) && !string.IsNullOrEmpty(this.headBag.mcStylePartId) && !(this.headBag.mcStylePreset == null))
												{
													if (!(this.flyingBag == null))
													{
														if (!(this.screenPresets == null) && this.screenPresets.Count != 0)
														{
															if (!(this.giggleVoice == null))
															{
																if (this.footstepClips != null)
																{
																	if (!Array.TrueForAll<AudioClip>(this.footstepClips, (AudioClip clip) => clip == null))
																	{
																		text = ((this.wallShots == null || this.wallShots.Length == 0) ? "wallShots" : ((this.runShots == null || this.runShots.Length == 0) ? "runShots" : ((this.centerShots == null || this.centerShots.Length == 0) ? "centerShots" : ((this.bagShot == null) ? "bagShot" : ((this.bagOffShot == null) ? "bagOffShot" : ((this.escapeFaceShot == null) ? "escapeFaceShot" : ((this.escapeSideShot == null) ? "escapeSideShot" : ((this.fieldPath == null || this.fieldPath.Length < 4) ? "fieldPath" : (string.IsNullOrEmpty(this.commentatorLine) ? "commentatorLine" : ((this.escapePath == null || this.escapePath.Length < 2) ? "escapePath" : null))))))))));
																		goto IL_02BD;
																	}
																}
																text = "footstepClips";
															}
															else
															{
																text = "giggleVoice";
															}
														}
														else
														{
															text = "screenPresets";
														}
													}
													else
													{
														text = "flyingBag";
													}
												}
												else
												{
													text = "headBag";
												}
											}
											else
											{
												text = "embarrassedFaceAnimation";
											}
										}
										else
										{
											text = "hornyFaceAnimation";
										}
									}
									else
									{
										text = "armsMask";
									}
								}
								else
								{
									text = "faceCoverArms";
								}
							}
							else
							{
								text = "escapeRun";
							}
						}
						else
						{
							text = "escapeIdle";
						}
					}
					else
					{
						text = "lookAtMe";
					}
				}
				else
				{
					text = "fieldRun";
				}
			}
			else
			{
				text = "wallPose";
			}
			IL_02BD:
			missing = text;
			return missing == null;
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x00265C98 File Offset: 0x00263E98
		private static bool HasRuntimeFace(LongWayHomeFaceAnimationSO face)
		{
			if (face == null || face.duration <= 0f || face.curves == null || face.curves.Length == 0)
			{
				return false;
			}
			foreach (LongWayHomeFaceAnimationSO.CurveBinding curveBinding in face.curves)
			{
				if (curveBinding == null || string.IsNullOrEmpty(curveBinding.blendShapeName) || curveBinding.curve == null || curveBinding.curve.length == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x00265D10 File Offset: 0x00263F10
		public static Vector3[] DefaultFieldLap()
		{
			Vector2[] array = new Vector2[]
			{
				new Vector2(-3.8f, -18.65f),
				new Vector2(-2.4f, -12.3f),
				new Vector2(-10f, -2.6f),
				new Vector2(-1.6f, 9.7f),
				new Vector2(-5.8f, 21f),
				new Vector2(-7.6f, 34f),
				new Vector2(-3.5f, 40.5f),
				new Vector2(1.5f, 42f),
				new Vector2(5.8f, 39.5f),
				new Vector2(3.4f, 32.8f),
				new Vector2(5.1f, 21f),
				new Vector2(5.2f, 4.25f),
				new Vector2(4.2f, -12.3f),
				new Vector2(5f, -25.6f),
				new Vector2(4.6f, -32.5f),
				new Vector2(0f, -35.5f),
				new Vector2(-4.2f, -33f),
				new Vector2(-5.2f, -25f),
				new Vector2(-3.8f, -18.65f)
			};
			Vector3[] array2 = new Vector3[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = new Vector3(array[i].x, -6.62f, array[i].y);
			}
			return array2;
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x00265F14 File Offset: 0x00264114
		public static LongWayHomeStadiumConfigSO.BodyShot[] DefaultWallShots()
		{
			return new LongWayHomeStadiumConfigSO.BodyShot[]
			{
				new LongWayHomeStadiumConfigSO.BodyShot("Face and breasts", HumanBodyBones.Chest, new Vector3(0.16f, 0.2f, 0.88f), new Vector3(0f, 0.2f, 0.05f), 40f, 0.12f),
				new LongWayHomeStadiumConfigSO.BodyShot("Breasts", HumanBodyBones.Chest, new Vector3(0.1f, 0.02f, 0.55f), new Vector3(0f, -0.03f, 0.1f), 38f, 0.12f),
				new LongWayHomeStadiumConfigSO.BodyShot("Crotch", HumanBodyBones.Hips, new Vector3(0.08f, -0.08f, 0.62f), new Vector3(0f, -0.1f, 0.1f), 38f, 0.12f),
				new LongWayHomeStadiumConfigSO.BodyShot("Whole body", HumanBodyBones.Hips, new Vector3(0.45f, 0.25f, 2f), new Vector3(0f, 0.12f, 0f), 44f, 0.2f)
			};
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x00266028 File Offset: 0x00264228
		public static LongWayHomeStadiumConfigSO.BodyShot[] DefaultRunShots()
		{
			return new LongWayHomeStadiumConfigSO.BodyShot[]
			{
				new LongWayHomeStadiumConfigSO.BodyShot("Front", HumanBodyBones.Hips, new Vector3(0.5f, 0.45f, 3.3f), new Vector3(0f, 0.32f, 0f), 42f, 0.18f),
				new LongWayHomeStadiumConfigSO.BodyShot("Bag", HumanBodyBones.Head, new Vector3(0.18f, 0.05f, 0.95f), new Vector3(0f, -0.02f, 0.05f), 40f, 0.1f),
				new LongWayHomeStadiumConfigSO.BodyShot("Breasts", HumanBodyBones.Chest, new Vector3(0.12f, 0.04f, 0.78f), new Vector3(0f, -0.03f, 0.1f), 40f, 0.08f),
				new LongWayHomeStadiumConfigSO.BodyShot("Crotch", HumanBodyBones.Hips, new Vector3(0.1f, -0.08f, 0.85f), new Vector3(0f, -0.1f, 0.1f), 40f, 0.08f),
				new LongWayHomeStadiumConfigSO.BodyShot("Butt", HumanBodyBones.Hips, new Vector3(-0.12f, 0.02f, -0.95f), new Vector3(0f, -0.05f, -0.1f), 40f, 0.08f),
				new LongWayHomeStadiumConfigSO.BodyShot("Side", HumanBodyBones.Hips, new Vector3(2.4f, 0.25f, 0.4f), new Vector3(0f, 0.15f, 0f), 44f, 0.18f),
				new LongWayHomeStadiumConfigSO.BodyShot("Low", HumanBodyBones.Hips, new Vector3(0.35f, -0.62f, 2.1f), new Vector3(0f, 0.35f, 0f), 50f, 0.15f)
			};
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x002661FC File Offset: 0x002643FC
		public static LongWayHomeStadiumConfigSO.BodyShot[] DefaultCenterShots()
		{
			return new LongWayHomeStadiumConfigSO.BodyShot[]
			{
				new LongWayHomeStadiumConfigSO.BodyShot("Front", HumanBodyBones.Hips, new Vector3(0.4f, 0.35f, 2.9f), new Vector3(0f, 0.3f, 0f), 44f, 0.2f),
				new LongWayHomeStadiumConfigSO.BodyShot("Bag and breasts", HumanBodyBones.Chest, new Vector3(0.15f, 0.12f, 0.95f), new Vector3(0f, 0.12f, 0.05f), 40f, 0.12f),
				new LongWayHomeStadiumConfigSO.BodyShot("Crotch", HumanBodyBones.Hips, new Vector3(0.1f, -0.1f, 0.8f), new Vector3(0f, -0.1f, 0.1f), 40f, 0.12f),
				new LongWayHomeStadiumConfigSO.BodyShot("From the stand", HumanBodyBones.Hips, new Vector3(-1.2f, 1.6f, 7.5f), new Vector3(0f, 0.25f, 0f), 34f, 0.25f),
				new LongWayHomeStadiumConfigSO.BodyShot("Behind", HumanBodyBones.Hips, new Vector3(-0.5f, 0.6f, -3f), new Vector3(0f, 0.25f, 0f), 46f, 0.2f),
				new LongWayHomeStadiumConfigSO.BodyShot("Low", HumanBodyBones.Hips, new Vector3(0.3f, -0.6f, 1.6f), new Vector3(0f, 0.45f, 0f), 54f, 0.15f)
			};
		}

		// Token: 0x0400624F RID: 25167
		public const string ResourceName = "LongWayHomeStadiumConfig";

		// Token: 0x04006250 RID: 25168
		[Header("Clock (Azure hours)")]
		[Tooltip("The match is played in daylight: the clock holds this hour through the whole opening, and the run goes on from it once she is handed over.")]
		[Range(0f, 24f)]
		public float dayHour = 12f;

		// Token: 0x04006251 RID: 25169
		[Header("Ciel animation")]
		[Tooltip("The game's standing wall cover idle (Cover Locomotion/CoverStanding, right side): her back to the wall.")]
		public AnimationClip wallPose;

		// Token: 0x04006252 RID: 25170
		[Tooltip("Her naked run on the pitch: the Confident layer's run.")]
		public AnimationClip fieldRun;

		// Token: 0x04006253 RID: 25171
		[Tooltip("The game's \"Look at me\" emote (T-Pose_Spread), looping at the centre spot.")]
		public AnimationClip lookAtMe;

		// Token: 0x04006254 RID: 25172
		[Tooltip("Where she stands embarrassed once the bag has flown off: the game's EMB idle.")]
		public AnimationClip escapeIdle;

		// Token: 0x04006255 RID: 25173
		[Tooltip("She runs off the pitch with the game's EMB run (Run EMB 02).")]
		public AnimationClip escapeRun;

		// Token: 0x04006256 RID: 25174
		[Tooltip("The game's embarrassed \"Face Cover\" pose (CielHideFace_Layered_01): her hands over her face as she runs off, on her arms only.")]
		public AnimationClip faceCoverArms;

		// Token: 0x04006257 RID: 25175
		[Tooltip("The Animator's Arms layer mask (OnlyArms), for Face Cover.")]
		public AvatarMask armsMask;

		// Token: 0x04006258 RID: 25176
		[Header("Faces (rebased and baked by Tools > Long Way Home)")]
		public AnimationClip hornyFace;

		// Token: 0x04006259 RID: 25177
		[Tooltip("Horny Supreme, from the tunnel until the bag flies off.")]
		public LongWayHomeFaceAnimationSO hornyFaceAnimation;

		// Token: 0x0400625A RID: 25178
		[Tooltip("Embarrassed Supreme the moment the bag is gone (baked by the gym opening).")]
		public LongWayHomeFaceAnimationSO embarrassedFaceAnimation;

		// Token: 0x0400625B RID: 25179
		[Header("Head bag (the game's Bag item)")]
		[Tooltip("HeadBagItemSO: its hand prefab and hand offsets, its worn MCStyle part and its BAG_ON shape.")]
		public WearableItemSO headBag;

		// Token: 0x0400625C RID: 25180
		[Tooltip("The worn bag's mesh (HeadBag_Worn): the copy the gust carries away.")]
		public GameObject flyingBag;

		// Token: 0x0400625D RID: 25181
		[Tooltip("Where her right hand holds the bag, low by her right hip: from her hips in her body frame (X right, Y up, Z forward).")]
		public Vector3 holdBagHand = new Vector3(0.2f, 0.12f, 0.28f);

		// Token: 0x0400625E RID: 25182
		[Tooltip("Where her right elbow points while she holds it, from her right shoulder in her body frame.")]
		public Vector3 holdBagElbow = new Vector3(0.3f, -0.26f, -0.1f);

		// Token: 0x0400625F RID: 25183
		[Tooltip("She holds the bag low and looks down at it (and at the low camera) this long; then it is on her head at once, as wearing it in the game.")]
		[Min(0.5f)]
		public float bagHoldSeconds = 3.4f;

		// Token: 0x04006260 RID: 25184
		[Header("Look at the bag (and at the low camera)")]
		[Tooltip("0 looks at the bag, 1 straight at the camera: the head bends down between the two.")]
		[Range(0f, 1f)]
		public float lookAtCameraShare = 0.45f;

		// Token: 0x04006261 RID: 25185
		[Range(0f, 1f)]
		public float lookAtHeadWeight = 0.9f;

		// Token: 0x04006262 RID: 25186
		[Range(0f, 1f)]
		public float lookAtBodyWeight = 0.25f;

		// Token: 0x04006263 RID: 25187
		[Range(0f, 1f)]
		public float lookAtClampWeight = 0.4f;

		// Token: 0x04006264 RID: 25188
		[Min(0.05f)]
		public float lookAtBlendSeconds = 0.55f;

		// Token: 0x04006265 RID: 25189
		[Header("The gust")]
		[Tooltip("The wind rises and the bag shivers on her head this long before it lifts off.")]
		[Min(0.1f)]
		public float gustTrembleSeconds = 1.6f;

		// Token: 0x04006266 RID: 25190
		[Tooltip("How hard the bag shivers at its strongest, in degrees.")]
		[Range(0f, 30f)]
		public float gustTrembleDegrees = 8f;

		// Token: 0x04006267 RID: 25191
		[Tooltip("How far it lifts off her head while it shivers, in metres.")]
		[Range(0f, 0.1f)]
		public float gustTrembleLift = 0.03f;

		// Token: 0x04006268 RID: 25192
		[Tooltip("The bag's first velocity off her head, in her yaw frame (m/s): up, a little sideways, back.")]
		public Vector3 gustVelocity = new Vector3(0.25f, 0.75f, -0.35f);

		// Token: 0x04006269 RID: 25193
		[Tooltip("Spin of the flying bag (degrees per second around its own axes).")]
		public Vector3 gustSpin = new Vector3(140f, 80f, 50f);

		// Token: 0x0400626A RID: 25194
		[Tooltip("The wind keeps lifting it: gravity on the light bag (m/s²).")]
		public float gustGravity = 0.3f;

		// Token: 0x0400626B RID: 25195
		[Min(0f)]
		public float gustDrag = 0.35f;

		// Token: 0x0400626C RID: 25196
		[Tooltip("Steady push of the wind on the bag, in her yaw frame (m/s²).")]
		public Vector3 gustWind = new Vector3(0.35f, 0.4f, -1.5f);

		// Token: 0x0400626D RID: 25197
		[Min(0.5f)]
		public float bagFlightSeconds = 6f;

		// Token: 0x0400626E RID: 25198
		[Tooltip("The swoosh as it lifts off.")]
		public AudioClip gustSound;

		// Token: 0x0400626F RID: 25199
		[Range(0f, 1f)]
		public float gustVolume = 0.85f;

		// Token: 0x04006270 RID: 25200
		[Tooltip("The gust itself (HeavyWind, the game's wind gags' loop): it rises with the shiver and dies down as the bag flies off.")]
		public AudioClip gustWindSound;

		// Token: 0x04006271 RID: 25201
		[Range(0f, 1f)]
		public float gustWindVolume = 0.7f;

		// Token: 0x04006272 RID: 25202
		[Tooltip("Wind lines across the screen while it blows.")]
		[Range(0f, 80f)]
		public int windLineCount = 40;

		// Token: 0x04006273 RID: 25203
		[Range(0f, 1f)]
		public float windLineOpacity = 0.6f;

		// Token: 0x04006274 RID: 25204
		[Tooltip("Screen widths a second the lines travel.")]
		[Range(0.1f, 5f)]
		public float windLineSpeed = 1.5f;

		// Token: 0x04006275 RID: 25205
		[Tooltip("Slant of the lines, in degrees (positive rises to the left).")]
		[Range(-45f, 45f)]
		public float windLineTilt = 8f;

		// Token: 0x04006276 RID: 25206
		[Tooltip("Strength of the gust on her hair, in m/s: a Magica wind zone around her, as the game's wind gags (manhole, cheerleader and university uniforms: 20 m/s, turbulence 1).")]
		[Range(0f, 40f)]
		public float hairWind = 20f;

		// Token: 0x04006277 RID: 25207
		[Tooltip("Gustiness of that wind (Magica's turbulence rate).")]
		[Range(0f, 1f)]
		public float hairWindTurbulence = 1f;

		// Token: 0x04006278 RID: 25208
		[Tooltip("The wind dies down over this long once the bag is off.")]
		[Min(0.1f)]
		public float gustFadeSeconds = 2.2f;

		// Token: 0x04006279 RID: 25209
		[Header("Commentator (she is exposed)")]
		[Tooltip("The speaker shown over the line, as the stadium's own commentary.")]
		public string commentatorSpeaker = "Commentator";

		// Token: 0x0400627A RID: 25210
		[Tooltip("The stadium commentator's line once the bag is gone; it stays until the player presses Space, then she runs.")]
		[TextArea(2, 4)]
		public string commentatorLine = "Oh, that's NOT our naked pitch invader's lucky day! The wind's whipped the bag clean off... and now we can see absolutely EVERYTHING!";

		// Token: 0x0400627B RID: 25211
		[Header("Voice and sound")]
		[Tooltip("Voice_LewdEnjoyment1: the gameplay Stimulation voice with its subtitle, at the centre spot.")]
		public CielVoiceSO giggleVoice;

		// Token: 0x0400627C RID: 25212
		[Tooltip("After she starts the pose at the centre spot.")]
		[Min(0f)]
		public float giggleDelaySeconds = 0.6f;

		// Token: 0x0400627D RID: 25213
		public AudioClip realizationGasp;

		// Token: 0x0400627E RID: 25214
		[Range(0f, 1f)]
		public float gaspVolume = 0.9f;

		// Token: 0x0400627F RID: 25215
		[Tooltip("Ciel's barefoot steps, the same clips as her FootstepController.")]
		public AudioClip[] footstepClips = new AudioClip[2];

		// Token: 0x04006280 RID: 25216
		[Range(0f, 1f)]
		public float runFootstepVolume = 0.45f;

		// Token: 0x04006281 RID: 25217
		[Header("Tunnel (the players' tunnel pit by the dugouts)")]
		[Tooltip("Where she stands with her back to the pit's south wall (z 2.38), facing the locker on the other side; the cover idle turns her head towards the pitch.")]
		public Vector3 wallMark = new Vector3(-42.1f, -8.1f, 2.56f);

		// Token: 0x04006282 RID: 25218
		[Tooltip("The way her body faces at the wall (the cover idle's own root is turned from it; the director measures and compensates that).")]
		public float wallYaw;

		// Token: 0x04006283 RID: 25219
		public LongWayHomeStadiumConfigSO.BodyShot[] wallShots = LongWayHomeStadiumConfigSO.DefaultWallShots();

		// Token: 0x04006284 RID: 25220
		[Tooltip("At the wall the mouse (right stick, wheel) turns the camera around her, as the gameplay camera does (its Default state's sensitivity, limits and zoom). This only keeps it from looking up from under the floor of the pit.")]
		[Range(-40f, 30f)]
		public float orbitMinPitch = -20f;

		// Token: 0x04006285 RID: 25221
		[Tooltip("...and from going round behind her: her back is to the wall, so it turns this far either way from straight in front of her.")]
		[Range(30f, 180f)]
		public float orbitYawLimit = 80f;

		// Token: 0x04006286 RID: 25222
		[Tooltip("The bag appears: a low camera at crotch height, looking up past the bag at her face. It stays as the bag goes on her head.")]
		public LongWayHomeStadiumConfigSO.BodyShot bagShot = new LongWayHomeStadiumConfigSO.BodyShot("Bag - from below", HumanBodyBones.Hips, new Vector3(-0.1f, -0.1f, 0.98f), new Vector3(0.04f, 0.42f, 0.12f), 54f, 0.2f);

		// Token: 0x04006287 RID: 25223
		[Header("Pitch run")]
		[Tooltip("Her lap, round and round until the player continues: up the pitch weaving between the footballers of the left side, round, and down between those of the right side (2.2-3.2 m from each of them once the corners are rounded). It starts and ends at the same point, on a straight.")]
		public Vector3[] fieldPath = LongWayHomeStadiumConfigSO.DefaultFieldLap();

		// Token: 0x04006288 RID: 25224
		[Tooltip("Confident_Run covers 3.81 m a second.")]
		[Min(0.5f)]
		public float fieldMetersPerSecond = 3.81f;

		// Token: 0x04006289 RID: 25225
		public LongWayHomeStadiumConfigSO.BodyShot[] runShots = LongWayHomeStadiumConfigSO.DefaultRunShots();

		// Token: 0x0400628A RID: 25226
		[Header("Big screen")]
		[Tooltip("The stadium screen's own player-focus shots (PlayerFocusStadiumPresetsSO): from the run on they follow her, changing on their own.")]
		public DetectionCameraPresetCollectionSO screenPresets;

		// Token: 0x0400628B RID: 25227
		[Tooltip("Seconds on each big-screen shot while she runs and poses.")]
		[Min(1f)]
		public float screenShotSeconds = 4f;

		// Token: 0x0400628C RID: 25228
		[Tooltip("The shot the big screen holds from the gust on: her face (in front of her head).")]
		public string screenFacePreset = "Default";

		// Token: 0x0400628D RID: 25229
		[Header("Slow motion (C / Y on the pitch)")]
		[Range(0.1f, 0.9f)]
		public float slowMotionScale = 0.3f;

		// Token: 0x0400628E RID: 25230
		[Tooltip("Real seconds to slow down or speed up again.")]
		[Min(0.01f)]
		public float slowMotionRampSeconds = 0.35f;

		// Token: 0x0400628F RID: 25231
		[Tooltip("What you hear slows down too: at full slow motion every sound (the crowd, her steps and voice, the wind) plays at this pitch. 1 keeps the audio as it is.")]
		[Range(0.2f, 1f)]
		public float slowMotionAudioPitch = 0.55f;

		// Token: 0x04006290 RID: 25232
		[Header("Anime bounce (while she runs on the pitch)")]
		[Tooltip("Each breast is a soft mass on a loose spring hung from her chest: it lags behind her steps and every foot strike jolts one down and flings the other up, in turn. Off: the breast physics stays the game's.")]
		public bool animeBounce = true;

		// Token: 0x04006291 RID: 25233
		[Tooltip("Swings a second of the breast springs: lower is softer and slower to come back.")]
		[Range(1f, 10f)]
		public float bounceFrequency = 3.7f;

		// Token: 0x04006292 RID: 25234
		[Tooltip("How far apart the two springs are tuned, as a share of the frequency: they drift in and out of step, so one often rises while the other drops.")]
		[Range(0f, 0.5f)]
		public float bounceAsymmetry = 0.14f;

		// Token: 0x04006293 RID: 25235
		[Tooltip("0 bounces for ever, 1 settles without bouncing.")]
		[Range(0.02f, 1f)]
		public float bounceDamping = 0.12f;

		// Token: 0x04006294 RID: 25236
		[Tooltip("Speed of the jolt each foot strike gives a breast, in metres a second.")]
		[Range(0f, 3f)]
		public float bounceKick = 0.6f;

		// Token: 0x04006295 RID: 25237
		[Tooltip("Random variation of each jolt, as a share of Bounce Kick.")]
		[Range(0f, 1f)]
		public float bounceKickRandom = 0.3f;

		// Token: 0x04006296 RID: 25238
		[Tooltip("At each foot strike one breast is jolted down and the other flung up by this share of the jolt: the next strike the other way round, so they bounce in turn.")]
		[Range(0f, 1.5f)]
		public float bounceCounterKick = 0.75f;

		// Token: 0x04006297 RID: 25239
		[Tooltip("How much of the simulated swing shows: above 1 is the anime exaggeration.")]
		[Range(0f, 4f)]
		public float bounceGain = 1.5f;

		// Token: 0x04006298 RID: 25240
		[Tooltip("How much of the bounce both breasts share: lower makes one rise while the other drops.")]
		[Range(0f, 1f)]
		public float bounceUnison = 0.45f;

		// Token: 0x04006299 RID: 25241
		[Tooltip("The farthest a breast swings around its base, in degrees.")]
		[Range(5f, 60f)]
		public float bounceMaxAngle = 32f;

		// Token: 0x0400629A RID: 25242
		[Tooltip("The farthest the simulated mass may swing from its rest, in metres (before the gain).")]
		[Range(0.01f, 0.2f)]
		public float bounceMaxSwing = 0.06f;

		// Token: 0x0400629B RID: 25243
		[Tooltip("Share of the sideways swing that shows; up and down always shows whole.")]
		[Range(0f, 1f)]
		public float bounceSway = 0.5f;

		// Token: 0x0400629C RID: 25244
		[Tooltip("How much of the swing the top of the breast (c_breast_02, the bone it swings around) follows.")]
		[Range(0f, 1f)]
		public float bounceBaseFollow = 0.3f;

		// Token: 0x0400629D RID: 25245
		[Tooltip("Scale of the swing per breast size (BoobsType 0 Flat .. 5 Balloon).")]
		public float[] bounceSizeScale = new float[] { 0f, 0.55f, 0.75f, 1f, 1.1f, 1.25f };

		// Token: 0x0400629E RID: 25246
		[Min(0.05f)]
		public float bounceBlendSeconds = 0.35f;

		// Token: 0x0400629F RID: 25247
		[Header("Centre spot")]
		[Tooltip("Next to the ball on the centre spot, facing the west stand.")]
		public Vector3 centerMark = new Vector3(0.6f, -6.62f, 5.6f);

		// Token: 0x040062A0 RID: 25248
		public float centerYaw = 270f;

		// Token: 0x040062A1 RID: 25249
		public LongWayHomeStadiumConfigSO.BodyShot[] centerShots = LongWayHomeStadiumConfigSO.DefaultCenterShots();

		// Token: 0x040062A2 RID: 25250
		[Tooltip("The crowd keeps cheering while she poses; while her voice plays it drops to this share of its volume, under her voice (the stadium's own duck under her arrival lines: 0.1).")]
		[Range(0f, 1f)]
		public float voiceCrowdLevel = 0.1f;

		// Token: 0x040062A3 RID: 25251
		[Tooltip("Real seconds for the crowd to drop under her voice (the stadium's: 0.2)...")]
		[Min(0.05f)]
		public float crowdDuckSeconds = 0.2f;

		// Token: 0x040062A4 RID: 25252
		[Tooltip("...and to come back once she is quiet (the stadium's: 2.8).")]
		[Min(0.05f)]
		public float crowdRestoreSeconds = 2.8f;

		// Token: 0x040062A5 RID: 25253
		[Tooltip("The bag flies off: from the waist up, with room above her head to watch it go.")]
		public LongWayHomeStadiumConfigSO.BodyShot bagOffShot = new LongWayHomeStadiumConfigSO.BodyShot("Bag off - waist up", HumanBodyBones.Chest, new Vector3(0.12f, 0.12f, 1.25f), new Vector3(0f, 0.36f, 0f), 46f, 0.2f);

		// Token: 0x040062A6 RID: 25254
		[Header("Escape")]
		[Tooltip("Embarrassed, she runs off towards the west stand; the shot fades before she gets anywhere.")]
		public Vector3[] escapePath = new Vector3[]
		{
			new Vector3(0.6f, -6.62f, 5.6f),
			new Vector3(-6f, -6.62f, 5.2f),
			new Vector3(-20f, -6.62f, 4.6f),
			new Vector3(-34f, -6.62f, 4.4f)
		};

		// Token: 0x040062A7 RID: 25255
		[Tooltip("She runs off hiding her face: close on her face (her eyes peek over her fingers) from a little below, as she runs looking down. 1 m out: running towards it she closes in to about 0.8 m.")]
		public LongWayHomeStadiumConfigSO.BodyShot escapeFaceShot = new LongWayHomeStadiumConfigSO.BodyShot("Escape - face", HumanBodyBones.Head, new Vector3(0.05f, -0.22f, 1f), new Vector3(0f, 0.02f, 0.07f), 40f, 0.08f);

		// Token: 0x040062A8 RID: 25256
		[Tooltip("Then from her side, farther out, as she runs on.")]
		public LongWayHomeStadiumConfigSO.BodyShot escapeSideShot = new LongWayHomeStadiumConfigSO.BodyShot("Escape - side", HumanBodyBones.Hips, new Vector3(2.8f, 0.1f, 0.8f), new Vector3(0f, 0.15f, 0.9f), 46f, 0.15f);

		// Token: 0x040062A9 RID: 25257
		[Min(0.5f)]
		public float escapeMetersPerSecond = 3.3f;

		// Token: 0x040062AA RID: 25258
		[Tooltip("Seconds on her face as she runs off...")]
		[Min(0.5f)]
		public float escapeFaceSeconds = 3f;

		// Token: 0x040062AB RID: 25259
		[Tooltip("...then from her side, before the screen goes black.")]
		[Min(0.5f)]
		public float escapeSideSeconds = 3f;

		// Token: 0x040062AC RID: 25260
		[Tooltip("Player control starts here: the concourse high in the west stand (11.5 m above the pitch), away from the fans and connected to the Stadium's exit.")]
		public Vector3 handoffMark = new Vector3(-68.6f, 4.95f, -40.1f);

		// Token: 0x040062AD RID: 25261
		public float handoffYaw;

		// Token: 0x040062AE RID: 25262
		[Header("Timing")]
		[Min(0.1f)]
		public float transitionSeconds = 0.8f;

		// Token: 0x040062AF RID: 25263
		[Min(0.1f)]
		public float quickCutSeconds = 0.35f;

		// Token: 0x040062B0 RID: 25264
		[Min(0.1f)]
		public float handoffFadeSeconds = 0.45f;

		// Token: 0x040062B1 RID: 25265
		[Min(0.05f)]
		public float poseBlendSeconds = 0.4f;

		// Token: 0x040062B2 RID: 25266
		[Tooltip("With the bag on, before the tunnel fades out.")]
		[Min(0f)]
		public float wornHoldSeconds = 1.1f;

		// Token: 0x040062B3 RID: 25267
		[Tooltip("The bag-off shot holds this long before the gust.")]
		[Min(0f)]
		public float gustDelaySeconds = 0.8f;

		// Token: 0x040062B4 RID: 25268
		[Tooltip("Uncovered, she stands frozen in shock this long, the bag drifting away, before the commentator speaks.")]
		[Min(0f)]
		public float realizeSeconds = 1.2f;

		// Token: 0x040062B5 RID: 25269
		[Tooltip("Black screen in the concourse while her cloth settles, before control returns.")]
		[Min(0f)]
		public float handoffHoldSeconds = 1f;

		// Token: 0x02000CFC RID: 3324
		[Serializable]
		public sealed class BodyShot
		{
			// Token: 0x060060BD RID: 24765 RVA: 0x00266900 File Offset: 0x00264B00
			public BodyShot()
			{
			}

			// Token: 0x060060BE RID: 24766 RVA: 0x0026695C File Offset: 0x00264B5C
			public BodyShot(string label, HumanBodyBones anchor, Vector3 cameraOffset, Vector3 lookOffset, float fieldOfView, float damping = 0.18f)
			{
				this.label = label;
				this.anchor = anchor;
				this.cameraOffset = cameraOffset;
				this.lookOffset = lookOffset;
				this.fieldOfView = fieldOfView;
				this.damping = damping;
			}

			// Token: 0x040062B6 RID: 25270
			public string label = "Shot";

			// Token: 0x040062B7 RID: 25271
			public HumanBodyBones anchor;

			// Token: 0x040062B8 RID: 25272
			[Tooltip("Camera position from the anchor bone, in Ciel's yaw frame.")]
			public Vector3 cameraOffset = new Vector3(0f, 0.5f, 2f);

			// Token: 0x040062B9 RID: 25273
			[Tooltip("Point looked at, from the anchor bone, in Ciel's yaw frame.")]
			public Vector3 lookOffset = Vector3.zero;

			// Token: 0x040062BA RID: 25274
			[Range(10f, 90f)]
			public float fieldOfView = 45f;

			// Token: 0x040062BB RID: 25275
			[Tooltip("Position damping: 0 is rigid, higher values float behind the body.")]
			[Range(0f, 1f)]
			public float damping = 0.18f;
		}
	}
}
