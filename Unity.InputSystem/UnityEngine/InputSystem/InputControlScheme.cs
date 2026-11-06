using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	public struct InputControlScheme : IEquatable<InputControlScheme>
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000E145 File Offset: 0x0000C345
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000E14D File Offset: 0x0000C34D
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000E155 File Offset: 0x0000C355
		public string bindingGroup
		{
			get
			{
				return this.m_BindingGroup;
			}
			set
			{
				this.m_BindingGroup = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000E15E File Offset: 0x0000C35E
		public ReadOnlyArray<InputControlScheme.DeviceRequirement> deviceRequirements
		{
			get
			{
				return new ReadOnlyArray<InputControlScheme.DeviceRequirement>(this.m_DeviceRequirements);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000E16C File Offset: 0x0000C36C
		public InputControlScheme(string name, IEnumerable<InputControlScheme.DeviceRequirement> devices = null, string bindingGroup = null)
		{
			this = default(InputControlScheme);
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			this.SetNameAndBindingGroup(name, bindingGroup);
			this.m_DeviceRequirements = null;
			if (devices != null)
			{
				this.m_DeviceRequirements = devices.ToArray<InputControlScheme.DeviceRequirement>();
				if (this.m_DeviceRequirements.Length == 0)
				{
					this.m_DeviceRequirements = null;
				}
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000E1C1 File Offset: 0x0000C3C1
		internal void SetNameAndBindingGroup(string name, string bindingGroup = null)
		{
			this.m_Name = name;
			if (!string.IsNullOrEmpty(bindingGroup))
			{
				this.m_BindingGroup = bindingGroup;
				return;
			}
			this.m_BindingGroup = (name.Contains(';') ? name.Replace(";", "") : name);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000E200 File Offset: 0x0000C400
		public static InputControlScheme? FindControlSchemeForDevices<TDevices, TSchemes>(TDevices devices, TSchemes schemes, InputDevice mustIncludeDevice = null, bool allowUnsuccesfulMatch = false) where TDevices : IReadOnlyList<InputDevice> where TSchemes : IEnumerable<InputControlScheme>
		{
			if (devices == null)
			{
				throw new ArgumentNullException("devices");
			}
			if (schemes == null)
			{
				throw new ArgumentNullException("schemes");
			}
			InputControlScheme inputControlScheme;
			InputControlScheme.MatchResult matchResult;
			if (!InputControlScheme.FindControlSchemeForDevices<TDevices, TSchemes>(devices, schemes, out inputControlScheme, out matchResult, mustIncludeDevice, allowUnsuccesfulMatch))
			{
				return null;
			}
			matchResult.Dispose();
			return new InputControlScheme?(inputControlScheme);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000E25C File Offset: 0x0000C45C
		public static bool FindControlSchemeForDevices<TDevices, TSchemes>(TDevices devices, TSchemes schemes, out InputControlScheme controlScheme, out InputControlScheme.MatchResult matchResult, InputDevice mustIncludeDevice = null, bool allowUnsuccessfulMatch = false) where TDevices : IReadOnlyList<InputDevice> where TSchemes : IEnumerable<InputControlScheme>
		{
			if (devices == null)
			{
				throw new ArgumentNullException("devices");
			}
			if (schemes == null)
			{
				throw new ArgumentNullException("schemes");
			}
			InputControlScheme.MatchResult? matchResult2 = null;
			InputControlScheme? inputControlScheme = null;
			foreach (InputControlScheme inputControlScheme2 in schemes)
			{
				InputControlScheme.MatchResult matchResult3 = inputControlScheme2.PickDevicesFrom<TDevices>(devices, mustIncludeDevice);
				if (!matchResult3.isSuccessfulMatch && (!allowUnsuccessfulMatch || matchResult3.score <= 0f))
				{
					matchResult3.Dispose();
				}
				else if (mustIncludeDevice != null && !matchResult3.devices.Contains(mustIncludeDevice))
				{
					matchResult3.Dispose();
				}
				else if (matchResult2 != null && matchResult2.Value.score >= matchResult3.score)
				{
					matchResult3.Dispose();
				}
				else
				{
					if (matchResult2 != null)
					{
						matchResult2.GetValueOrDefault().Dispose();
					}
					matchResult2 = new InputControlScheme.MatchResult?(matchResult3);
					inputControlScheme = new InputControlScheme?(inputControlScheme2);
				}
			}
			matchResult = matchResult2.GetValueOrDefault();
			controlScheme = inputControlScheme.GetValueOrDefault();
			return matchResult2 != null;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		public static InputControlScheme? FindControlSchemeForDevice<TSchemes>(InputDevice device, TSchemes schemes) where TSchemes : IEnumerable<InputControlScheme>
		{
			if (schemes == null)
			{
				throw new ArgumentNullException("schemes");
			}
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			return InputControlScheme.FindControlSchemeForDevices<OneOrMore<InputDevice, ReadOnlyArray<InputDevice>>, TSchemes>(new OneOrMore<InputDevice, ReadOnlyArray<InputDevice>>(device), schemes, null, false);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000E3DC File Offset: 0x0000C5DC
		public bool SupportsDevice(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			for (int i = 0; i < this.m_DeviceRequirements.Length; i++)
			{
				if (InputControlPath.TryFindControl(device, this.m_DeviceRequirements[i].controlPath, 0) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000E428 File Offset: 0x0000C628
		public InputControlScheme.MatchResult PickDevicesFrom<TDevices>(TDevices devices, InputDevice favorDevice = null) where TDevices : IReadOnlyList<InputDevice>
		{
			InputControlScheme.MatchResult matchResult;
			if (this.m_DeviceRequirements == null || this.m_DeviceRequirements.Length == 0)
			{
				matchResult = new InputControlScheme.MatchResult
				{
					m_Result = InputControlScheme.MatchResult.Result.AllSatisfied,
					m_Score = 0.5f
				};
				return matchResult;
			}
			bool flag = true;
			bool flag2 = true;
			int num = this.m_DeviceRequirements.Length;
			float num2 = 0f;
			InputControlList<InputControl> inputControlList = new InputControlList<InputControl>(Allocator.Persistent, num);
			try
			{
				bool flag3 = false;
				bool flag4 = false;
				for (int i = 0; i < num; i++)
				{
					bool isOR = this.m_DeviceRequirements[i].isOR;
					bool isOptional = this.m_DeviceRequirements[i].isOptional;
					if (isOR && flag3)
					{
						inputControlList.Add(null);
					}
					else
					{
						string controlPath = this.m_DeviceRequirements[i].controlPath;
						if (string.IsNullOrEmpty(controlPath))
						{
							num2 += 1f;
							inputControlList.Add(null);
						}
						else
						{
							InputControl inputControl = null;
							int j = 0;
							while (j < devices.Count)
							{
								InputDevice inputDevice = devices[j];
								if (favorDevice != null)
								{
									if (j == 0)
									{
										inputDevice = favorDevice;
									}
									else if (inputDevice == favorDevice)
									{
										inputDevice = devices[0];
									}
								}
								InputControl inputControl2 = InputControlPath.TryFindControl(inputDevice, controlPath, 0);
								if (inputControl2 != null && !inputControlList.Contains(inputControl2))
								{
									inputControl = inputControl2;
									InternedString internedString = new InternedString(InputControlPath.TryGetDeviceLayout(controlPath));
									if (internedString.IsEmpty())
									{
										num2 += 1f;
										break;
									}
									InternedString layout = inputControl2.device.m_Layout;
									int num3;
									if (InputControlLayout.s_Layouts.ComputeDistanceInInheritanceHierarchy(internedString, layout, out num3))
									{
										num2 += 1f + 1f / (float)(Math.Abs(num3) + 1);
										break;
									}
									num2 += 1f;
									break;
								}
								else
								{
									j++;
								}
							}
							if (i + 1 < num && this.m_DeviceRequirements[i + 1].isOR)
							{
								if (inputControl != null)
								{
									flag3 = true;
								}
								else if (!isOptional)
								{
									flag4 = true;
								}
							}
							else if (isOR && i == num - 1)
							{
								if (inputControl == null)
								{
									if (flag4)
									{
										flag = false;
									}
									else
									{
										flag2 = false;
									}
								}
							}
							else
							{
								if (inputControl == null)
								{
									if (isOptional)
									{
										flag2 = false;
									}
									else
									{
										flag = false;
									}
								}
								if (i > 0 && this.m_DeviceRequirements[i - 1].isOR)
								{
									if (!flag3)
									{
										if (flag4)
										{
											flag = false;
										}
										else
										{
											flag2 = false;
										}
									}
									flag3 = false;
								}
							}
							inputControlList.Add(inputControl);
						}
					}
				}
			}
			catch (Exception)
			{
				inputControlList.Dispose();
				throw;
			}
			matchResult = new InputControlScheme.MatchResult
			{
				m_Result = ((!flag) ? InputControlScheme.MatchResult.Result.MissingRequired : ((!flag2) ? InputControlScheme.MatchResult.Result.MissingOptional : InputControlScheme.MatchResult.Result.AllSatisfied)),
				m_Controls = inputControlList,
				m_Requirements = this.m_DeviceRequirements,
				m_Score = num2
			};
			return matchResult;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		public bool Equals(InputControlScheme other)
		{
			if (!string.Equals(this.m_Name, other.m_Name, StringComparison.InvariantCultureIgnoreCase) || !string.Equals(this.m_BindingGroup, other.m_BindingGroup, StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}
			if (this.m_DeviceRequirements == null || this.m_DeviceRequirements.Length == 0)
			{
				return other.m_DeviceRequirements == null || other.m_DeviceRequirements.Length == 0;
			}
			if (other.m_DeviceRequirements == null || this.m_DeviceRequirements.Length != other.m_DeviceRequirements.Length)
			{
				return false;
			}
			int num = this.m_DeviceRequirements.Length;
			for (int i = 0; i < num; i++)
			{
				InputControlScheme.DeviceRequirement deviceRequirement = this.m_DeviceRequirements[i];
				bool flag = false;
				for (int j = 0; j < num; j++)
				{
					if (other.m_DeviceRequirements[j] == deviceRequirement)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		public override bool Equals(object obj)
		{
			return obj != null && obj is InputControlScheme && this.Equals((InputControlScheme)obj);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
		public override int GetHashCode()
		{
			return (((((this.m_Name != null) ? this.m_Name.GetHashCode() : 0) * 397) ^ ((this.m_BindingGroup != null) ? this.m_BindingGroup.GetHashCode() : 0)) * 397) ^ ((this.m_DeviceRequirements != null) ? this.m_DeviceRequirements.GetHashCode() : 0);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000E828 File Offset: 0x0000CA28
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.m_Name))
			{
				return base.ToString();
			}
			if (this.m_DeviceRequirements == null)
			{
				return this.m_Name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.m_Name);
			stringBuilder.Append('(');
			bool flag = true;
			foreach (InputControlScheme.DeviceRequirement deviceRequirement in this.m_DeviceRequirements)
			{
				if (!flag)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(deviceRequirement.controlPath);
				flag = false;
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000E8C9 File Offset: 0x0000CAC9
		public static bool operator ==(InputControlScheme left, InputControlScheme right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000E8D3 File Offset: 0x0000CAD3
		public static bool operator !=(InputControlScheme left, InputControlScheme right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040000FE RID: 254
		[SerializeField]
		internal string m_Name;

		// Token: 0x040000FF RID: 255
		[SerializeField]
		internal string m_BindingGroup;

		// Token: 0x04000100 RID: 256
		[SerializeField]
		internal InputControlScheme.DeviceRequirement[] m_DeviceRequirements;

		// Token: 0x02000186 RID: 390
		public struct MatchResult : IEnumerable<InputControlScheme.MatchResult.Match>, IEnumerable, IDisposable
		{
			// Token: 0x17000557 RID: 1367
			// (get) Token: 0x060013B5 RID: 5045 RVA: 0x0005BD4B File Offset: 0x00059F4B
			public float score
			{
				get
				{
					return this.m_Score;
				}
			}

			// Token: 0x17000558 RID: 1368
			// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0005BD53 File Offset: 0x00059F53
			public bool isSuccessfulMatch
			{
				get
				{
					return this.m_Result != InputControlScheme.MatchResult.Result.MissingRequired;
				}
			}

			// Token: 0x17000559 RID: 1369
			// (get) Token: 0x060013B7 RID: 5047 RVA: 0x0005BD61 File Offset: 0x00059F61
			public bool hasMissingRequiredDevices
			{
				get
				{
					return this.m_Result == InputControlScheme.MatchResult.Result.MissingRequired;
				}
			}

			// Token: 0x1700055A RID: 1370
			// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0005BD6C File Offset: 0x00059F6C
			public bool hasMissingOptionalDevices
			{
				get
				{
					return this.m_Result == InputControlScheme.MatchResult.Result.MissingOptional;
				}
			}

			// Token: 0x1700055B RID: 1371
			// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0005BD78 File Offset: 0x00059F78
			public InputControlList<InputDevice> devices
			{
				get
				{
					if (this.m_Devices.Count == 0 && !this.hasMissingRequiredDevices)
					{
						int count = this.m_Controls.Count;
						if (count != 0)
						{
							this.m_Devices.Capacity = count;
							for (int i = 0; i < count; i++)
							{
								InputControl inputControl = this.m_Controls[i];
								if (inputControl != null)
								{
									InputDevice device = inputControl.device;
									if (!this.m_Devices.Contains(device))
									{
										this.m_Devices.Add(device);
									}
								}
							}
						}
					}
					return this.m_Devices;
				}
			}

			// Token: 0x1700055C RID: 1372
			public InputControlScheme.MatchResult.Match this[int index]
			{
				get
				{
					if (index < 0 || this.m_Requirements == null || index >= this.m_Requirements.Length)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					return new InputControlScheme.MatchResult.Match
					{
						m_RequirementIndex = index,
						m_Requirements = this.m_Requirements,
						m_Controls = this.m_Controls
					};
				}
			}

			// Token: 0x060013BB RID: 5051 RVA: 0x0005BE54 File Offset: 0x0005A054
			public IEnumerator<InputControlScheme.MatchResult.Match> GetEnumerator()
			{
				return new InputControlScheme.MatchResult.Enumerator
				{
					m_Index = -1,
					m_Requirements = this.m_Requirements,
					m_Controls = this.m_Controls
				};
			}

			// Token: 0x060013BC RID: 5052 RVA: 0x0005BE91 File Offset: 0x0005A091
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060013BD RID: 5053 RVA: 0x0005BE99 File Offset: 0x0005A099
			public void Dispose()
			{
				this.m_Controls.Dispose();
				this.m_Devices.Dispose();
			}

			// Token: 0x04000890 RID: 2192
			internal InputControlScheme.MatchResult.Result m_Result;

			// Token: 0x04000891 RID: 2193
			internal float m_Score;

			// Token: 0x04000892 RID: 2194
			internal InputControlList<InputDevice> m_Devices;

			// Token: 0x04000893 RID: 2195
			internal InputControlList<InputControl> m_Controls;

			// Token: 0x04000894 RID: 2196
			internal InputControlScheme.DeviceRequirement[] m_Requirements;

			// Token: 0x02000270 RID: 624
			internal enum Result
			{
				// Token: 0x04000CED RID: 3309
				AllSatisfied,
				// Token: 0x04000CEE RID: 3310
				MissingRequired,
				// Token: 0x04000CEF RID: 3311
				MissingOptional
			}

			// Token: 0x02000271 RID: 625
			public struct Match
			{
				// Token: 0x1700060A RID: 1546
				// (get) Token: 0x06001680 RID: 5760 RVA: 0x000670DF File Offset: 0x000652DF
				public InputControl control
				{
					get
					{
						return this.m_Controls[this.m_RequirementIndex];
					}
				}

				// Token: 0x1700060B RID: 1547
				// (get) Token: 0x06001681 RID: 5761 RVA: 0x000670F2 File Offset: 0x000652F2
				public InputDevice device
				{
					get
					{
						InputControl control = this.control;
						if (control == null)
						{
							return null;
						}
						return control.device;
					}
				}

				// Token: 0x1700060C RID: 1548
				// (get) Token: 0x06001682 RID: 5762 RVA: 0x00067105 File Offset: 0x00065305
				public int requirementIndex
				{
					get
					{
						return this.m_RequirementIndex;
					}
				}

				// Token: 0x1700060D RID: 1549
				// (get) Token: 0x06001683 RID: 5763 RVA: 0x0006710D File Offset: 0x0006530D
				public InputControlScheme.DeviceRequirement requirement
				{
					get
					{
						return this.m_Requirements[this.m_RequirementIndex];
					}
				}

				// Token: 0x1700060E RID: 1550
				// (get) Token: 0x06001684 RID: 5764 RVA: 0x00067120 File Offset: 0x00065320
				public bool isOptional
				{
					get
					{
						return this.requirement.isOptional;
					}
				}

				// Token: 0x04000CF0 RID: 3312
				internal int m_RequirementIndex;

				// Token: 0x04000CF1 RID: 3313
				internal InputControlScheme.DeviceRequirement[] m_Requirements;

				// Token: 0x04000CF2 RID: 3314
				internal InputControlList<InputControl> m_Controls;
			}

			// Token: 0x02000272 RID: 626
			private struct Enumerator : IEnumerator<InputControlScheme.MatchResult.Match>, IEnumerator, IDisposable
			{
				// Token: 0x06001685 RID: 5765 RVA: 0x0006713B File Offset: 0x0006533B
				public bool MoveNext()
				{
					this.m_Index++;
					return this.m_Requirements != null && this.m_Index < this.m_Requirements.Length;
				}

				// Token: 0x06001686 RID: 5766 RVA: 0x00067165 File Offset: 0x00065365
				public void Reset()
				{
					this.m_Index = -1;
				}

				// Token: 0x1700060F RID: 1551
				// (get) Token: 0x06001687 RID: 5767 RVA: 0x00067170 File Offset: 0x00065370
				public InputControlScheme.MatchResult.Match Current
				{
					get
					{
						if (this.m_Requirements == null || this.m_Index < 0 || this.m_Index >= this.m_Requirements.Length)
						{
							throw new InvalidOperationException("Enumerator is not valid");
						}
						return new InputControlScheme.MatchResult.Match
						{
							m_RequirementIndex = this.m_Index,
							m_Requirements = this.m_Requirements,
							m_Controls = this.m_Controls
						};
					}
				}

				// Token: 0x17000610 RID: 1552
				// (get) Token: 0x06001688 RID: 5768 RVA: 0x000671D9 File Offset: 0x000653D9
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x06001689 RID: 5769 RVA: 0x000671E6 File Offset: 0x000653E6
				public void Dispose()
				{
				}

				// Token: 0x04000CF3 RID: 3315
				internal int m_Index;

				// Token: 0x04000CF4 RID: 3316
				internal InputControlScheme.DeviceRequirement[] m_Requirements;

				// Token: 0x04000CF5 RID: 3317
				internal InputControlList<InputControl> m_Controls;
			}
		}

		// Token: 0x02000187 RID: 391
		[Serializable]
		public struct DeviceRequirement : IEquatable<InputControlScheme.DeviceRequirement>
		{
			// Token: 0x1700055D RID: 1373
			// (get) Token: 0x060013BE RID: 5054 RVA: 0x0005BEB1 File Offset: 0x0005A0B1
			// (set) Token: 0x060013BF RID: 5055 RVA: 0x0005BEB9 File Offset: 0x0005A0B9
			public string controlPath
			{
				get
				{
					return this.m_ControlPath;
				}
				set
				{
					this.m_ControlPath = value;
				}
			}

			// Token: 0x1700055E RID: 1374
			// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0005BEC2 File Offset: 0x0005A0C2
			// (set) Token: 0x060013C1 RID: 5057 RVA: 0x0005BECF File Offset: 0x0005A0CF
			public bool isOptional
			{
				get
				{
					return (this.m_Flags & InputControlScheme.DeviceRequirement.Flags.Optional) > InputControlScheme.DeviceRequirement.Flags.None;
				}
				set
				{
					if (value)
					{
						this.m_Flags |= InputControlScheme.DeviceRequirement.Flags.Optional;
						return;
					}
					this.m_Flags &= ~InputControlScheme.DeviceRequirement.Flags.Optional;
				}
			}

			// Token: 0x1700055F RID: 1375
			// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0005BEF2 File Offset: 0x0005A0F2
			// (set) Token: 0x060013C3 RID: 5059 RVA: 0x0005BEFD File Offset: 0x0005A0FD
			public bool isAND
			{
				get
				{
					return !this.isOR;
				}
				set
				{
					this.isOR = !value;
				}
			}

			// Token: 0x17000560 RID: 1376
			// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0005BF09 File Offset: 0x0005A109
			// (set) Token: 0x060013C5 RID: 5061 RVA: 0x0005BF16 File Offset: 0x0005A116
			public bool isOR
			{
				get
				{
					return (this.m_Flags & InputControlScheme.DeviceRequirement.Flags.Or) > InputControlScheme.DeviceRequirement.Flags.None;
				}
				set
				{
					if (value)
					{
						this.m_Flags |= InputControlScheme.DeviceRequirement.Flags.Or;
						return;
					}
					this.m_Flags &= ~InputControlScheme.DeviceRequirement.Flags.Or;
				}
			}

			// Token: 0x060013C6 RID: 5062 RVA: 0x0005BF3C File Offset: 0x0005A13C
			public override string ToString()
			{
				if (string.IsNullOrEmpty(this.controlPath))
				{
					return base.ToString();
				}
				if (this.isOptional)
				{
					return this.controlPath + " (Optional)";
				}
				return this.controlPath + " (Required)";
			}

			// Token: 0x060013C7 RID: 5063 RVA: 0x0005BF90 File Offset: 0x0005A190
			public bool Equals(InputControlScheme.DeviceRequirement other)
			{
				return string.Equals(this.m_ControlPath, other.m_ControlPath) && this.m_Flags == other.m_Flags && string.Equals(this.controlPath, other.controlPath) && this.isOptional == other.isOptional;
			}

			// Token: 0x060013C8 RID: 5064 RVA: 0x0005BFE3 File Offset: 0x0005A1E3
			public override bool Equals(object obj)
			{
				return obj != null && obj is InputControlScheme.DeviceRequirement && this.Equals((InputControlScheme.DeviceRequirement)obj);
			}

			// Token: 0x060013C9 RID: 5065 RVA: 0x0005C000 File Offset: 0x0005A200
			public override int GetHashCode()
			{
				return (((((((this.m_ControlPath != null) ? this.m_ControlPath.GetHashCode() : 0) * 397) ^ this.m_Flags.GetHashCode()) * 397) ^ ((this.controlPath != null) ? this.controlPath.GetHashCode() : 0)) * 397) ^ this.isOptional.GetHashCode();
			}

			// Token: 0x060013CA RID: 5066 RVA: 0x0005C06D File Offset: 0x0005A26D
			public static bool operator ==(InputControlScheme.DeviceRequirement left, InputControlScheme.DeviceRequirement right)
			{
				return left.Equals(right);
			}

			// Token: 0x060013CB RID: 5067 RVA: 0x0005C077 File Offset: 0x0005A277
			public static bool operator !=(InputControlScheme.DeviceRequirement left, InputControlScheme.DeviceRequirement right)
			{
				return !left.Equals(right);
			}

			// Token: 0x04000895 RID: 2197
			[SerializeField]
			internal string m_ControlPath;

			// Token: 0x04000896 RID: 2198
			[SerializeField]
			internal InputControlScheme.DeviceRequirement.Flags m_Flags;

			// Token: 0x02000273 RID: 627
			[Flags]
			internal enum Flags
			{
				// Token: 0x04000CF7 RID: 3319
				None = 0,
				// Token: 0x04000CF8 RID: 3320
				Optional = 1,
				// Token: 0x04000CF9 RID: 3321
				Or = 2
			}
		}

		// Token: 0x02000188 RID: 392
		[Serializable]
		internal struct SchemeJson
		{
			// Token: 0x060013CC RID: 5068 RVA: 0x0005C084 File Offset: 0x0005A284
			public InputControlScheme ToScheme()
			{
				InputControlScheme.DeviceRequirement[] array = null;
				if (this.devices != null && this.devices.Length != 0)
				{
					int num = this.devices.Length;
					array = new InputControlScheme.DeviceRequirement[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = this.devices[i].ToDeviceEntry();
					}
				}
				return new InputControlScheme
				{
					m_Name = (string.IsNullOrEmpty(this.name) ? null : this.name),
					m_BindingGroup = (string.IsNullOrEmpty(this.bindingGroup) ? null : this.bindingGroup),
					m_DeviceRequirements = array
				};
			}

			// Token: 0x060013CD RID: 5069 RVA: 0x0005C124 File Offset: 0x0005A324
			public static InputControlScheme.SchemeJson ToJson(InputControlScheme scheme)
			{
				InputControlScheme.SchemeJson.DeviceJson[] array = null;
				if (scheme.m_DeviceRequirements != null && scheme.m_DeviceRequirements.Length != 0)
				{
					int num = scheme.m_DeviceRequirements.Length;
					array = new InputControlScheme.SchemeJson.DeviceJson[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = InputControlScheme.SchemeJson.DeviceJson.From(scheme.m_DeviceRequirements[i]);
					}
				}
				return new InputControlScheme.SchemeJson
				{
					name = scheme.m_Name,
					bindingGroup = scheme.m_BindingGroup,
					devices = array
				};
			}

			// Token: 0x060013CE RID: 5070 RVA: 0x0005C1A4 File Offset: 0x0005A3A4
			public static InputControlScheme.SchemeJson[] ToJson(InputControlScheme[] schemes)
			{
				if (schemes == null || schemes.Length == 0)
				{
					return null;
				}
				int num = schemes.Length;
				InputControlScheme.SchemeJson[] array = new InputControlScheme.SchemeJson[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = InputControlScheme.SchemeJson.ToJson(schemes[i]);
				}
				return array;
			}

			// Token: 0x060013CF RID: 5071 RVA: 0x0005C1E8 File Offset: 0x0005A3E8
			public static InputControlScheme[] ToSchemes(InputControlScheme.SchemeJson[] schemes)
			{
				if (schemes == null || schemes.Length == 0)
				{
					return null;
				}
				int num = schemes.Length;
				InputControlScheme[] array = new InputControlScheme[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = schemes[i].ToScheme();
				}
				return array;
			}

			// Token: 0x04000897 RID: 2199
			public string name;

			// Token: 0x04000898 RID: 2200
			public string bindingGroup;

			// Token: 0x04000899 RID: 2201
			public InputControlScheme.SchemeJson.DeviceJson[] devices;

			// Token: 0x02000274 RID: 628
			[Serializable]
			public struct DeviceJson
			{
				// Token: 0x0600168A RID: 5770 RVA: 0x000671E8 File Offset: 0x000653E8
				public InputControlScheme.DeviceRequirement ToDeviceEntry()
				{
					return new InputControlScheme.DeviceRequirement
					{
						controlPath = this.devicePath,
						isOptional = this.isOptional,
						isOR = this.isOR
					};
				}

				// Token: 0x0600168B RID: 5771 RVA: 0x00067228 File Offset: 0x00065428
				public static InputControlScheme.SchemeJson.DeviceJson From(InputControlScheme.DeviceRequirement requirement)
				{
					return new InputControlScheme.SchemeJson.DeviceJson
					{
						devicePath = requirement.controlPath,
						isOptional = requirement.isOptional,
						isOR = requirement.isOR
					};
				}

				// Token: 0x04000CFA RID: 3322
				public string devicePath;

				// Token: 0x04000CFB RID: 3323
				public bool isOptional;

				// Token: 0x04000CFC RID: 3324
				public bool isOR;
			}
		}
	}
}
