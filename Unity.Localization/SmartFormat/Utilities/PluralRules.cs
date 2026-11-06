using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.SmartFormat.Utilities
{
	// Token: 0x02000044 RID: 68
	public static class PluralRules
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000865F File Offset: 0x0000685F
		private static PluralRules.PluralRuleDelegate Singular
		{
			get
			{
				return (decimal n, int c) => 0;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00008680 File Offset: 0x00006880
		private static PluralRules.PluralRuleDelegate DualOneOther
		{
			get
			{
				return delegate(decimal n, int c)
				{
					decimal num = Math.Abs(n);
					if (c == 2)
					{
						if (!(num == 1m))
						{
							return 1;
						}
						return 0;
					}
					else if (c == 3)
					{
						if (num == 0m)
						{
							return 0;
						}
						if (!(num == 1m))
						{
							return 2;
						}
						return 1;
					}
					else
					{
						if (c != 4)
						{
							return -1;
						}
						if (n < 0m)
						{
							return 0;
						}
						if (n == 0m)
						{
							return 1;
						}
						if (!(n == 1m))
						{
							return 3;
						}
						return 2;
					}
				};
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000086A1 File Offset: 0x000068A1
		private static PluralRules.PluralRuleDelegate DualWithZero
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (!(n == 0m) && !(n == 1m))
					{
						return 1;
					}
					return 0;
				};
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000086C2 File Offset: 0x000068C2
		private static PluralRules.PluralRuleDelegate DualFromZeroToTwo
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (!(n == 0m) && !(n == 1m))
					{
						return 1;
					}
					return 0;
				};
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000086E3 File Offset: 0x000068E3
		private static PluralRules.PluralRuleDelegate TripleOneTwoOther
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if (!(n == 2m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00008704 File Offset: 0x00006904
		private static PluralRules.PluralRuleDelegate RussianSerboCroatian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n % 10m == 1m && n % 100m != 11m)
					{
						return 0;
					}
					if (!(n % 10m).Between(2m, 4m) || (n % 100m).Between(12m, 14m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00008725 File Offset: 0x00006925
		private static PluralRules.PluralRuleDelegate Arabic
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 0m)
					{
						return 0;
					}
					if (n == 1m)
					{
						return 1;
					}
					if (n == 2m)
					{
						return 2;
					}
					if ((n % 100m).Between(3m, 10m))
					{
						return 3;
					}
					if (!(n % 100m).Between(11m, 99m))
					{
						return 5;
					}
					return 4;
				};
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00008746 File Offset: 0x00006946
		private static PluralRules.PluralRuleDelegate Breton
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 0m)
					{
						return 0;
					}
					if (n == 1m)
					{
						return 1;
					}
					if (n == 2m)
					{
						return 2;
					}
					if (n == 3m)
					{
						return 3;
					}
					if (!(n == 6m))
					{
						return 5;
					}
					return 4;
				};
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00008767 File Offset: 0x00006967
		private static PluralRules.PluralRuleDelegate Czech
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if (!n.Between(2m, 4m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00008788 File Offset: 0x00006988
		private static PluralRules.PluralRuleDelegate Welsh
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 0m)
					{
						return 0;
					}
					if (n == 1m)
					{
						return 1;
					}
					if (n == 2m)
					{
						return 2;
					}
					if (n == 3m)
					{
						return 3;
					}
					if (!(n == 6m))
					{
						return 5;
					}
					return 4;
				};
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000087A9 File Offset: 0x000069A9
		private static PluralRules.PluralRuleDelegate Manx
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (!(n % 10m).Between(1m, 2m) && !(n % 20m == 0m))
					{
						return 1;
					}
					return 0;
				};
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600026C RID: 620 RVA: 0x000087CA File Offset: 0x000069CA
		private static PluralRules.PluralRuleDelegate Langi
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 0m)
					{
						return 0;
					}
					if (!(n > 0m) || !(n < 2m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600026D RID: 621 RVA: 0x000087EB File Offset: 0x000069EB
		private static PluralRules.PluralRuleDelegate Lithuanian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n % 10m == 1m && !(n % 100m).Between(11m, 19m))
					{
						return 0;
					}
					if (!(n % 10m).Between(2m, 9m) || (n % 100m).Between(11m, 19m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000880C File Offset: 0x00006A0C
		private static PluralRules.PluralRuleDelegate Latvian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 0m)
					{
						return 0;
					}
					if (!(n % 10m == 1m) || !(n % 100m != 11m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000882D File Offset: 0x00006A2D
		private static PluralRules.PluralRuleDelegate Macedonian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (!(n % 10m == 1m) || !(n != 11m))
					{
						return 1;
					}
					return 0;
				};
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000884E File Offset: 0x00006A4E
		private static PluralRules.PluralRuleDelegate Moldavian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if (!(n == 0m) && (!(n != 1m) || !(n % 100m).Between(1m, 19m)))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000886F File Offset: 0x00006A6F
		private static PluralRules.PluralRuleDelegate Maltese
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if (n == 0m || (n % 100m).Between(2m, 10m))
					{
						return 1;
					}
					if (!(n % 100m).Between(11m, 19m))
					{
						return 3;
					}
					return 2;
				};
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00008890 File Offset: 0x00006A90
		private static PluralRules.PluralRuleDelegate Polish
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if ((n % 10m).Between(2m, 4m) && !(n % 100m).Between(12m, 14m))
					{
						return 1;
					}
					if (!(n % 10m).Between(0m, 1m) && !(n % 10m).Between(5m, 9m) && !(n % 100m).Between(12m, 14m))
					{
						return 3;
					}
					return 2;
				};
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000273 RID: 627 RVA: 0x000088B1 File Offset: 0x00006AB1
		private static PluralRules.PluralRuleDelegate Romanian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if (!(n == 0m) && !(n % 100m).Between(1m, 19m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000274 RID: 628 RVA: 0x000088D2 File Offset: 0x00006AD2
		private static PluralRules.PluralRuleDelegate Tachelhit
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n >= 0m && n <= 1m)
					{
						return 0;
					}
					if (!n.Between(2m, 10m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000088F3 File Offset: 0x00006AF3
		private static PluralRules.PluralRuleDelegate Slovak
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n == 1m)
					{
						return 0;
					}
					if (!n.Between(2m, 4m))
					{
						return 2;
					}
					return 1;
				};
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00008914 File Offset: 0x00006B14
		private static PluralRules.PluralRuleDelegate Slovenian
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (n % 100m == 1m)
					{
						return 0;
					}
					if (n % 100m == 2m)
					{
						return 1;
					}
					if (!(n % 100m).Between(3m, 4m))
					{
						return 3;
					}
					return 2;
				};
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00008935 File Offset: 0x00006B35
		private static PluralRules.PluralRuleDelegate CentralMoroccoTamazight
		{
			get
			{
				return delegate(decimal n, int c)
				{
					n = Math.Abs(n);
					if (!n.Between(0m, 1m) && !n.Between(11m, 99m))
					{
						return 1;
					}
					return 0;
				};
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00008958 File Offset: 0x00006B58
		public static PluralRules.PluralRuleDelegate GetPluralRule(string twoLetterIsoLanguageName)
		{
			PluralRules.PluralRuleDelegate pluralRuleDelegate;
			if (!PluralRules.IsoLangToDelegate.TryGetValue(twoLetterIsoLanguageName, out pluralRuleDelegate))
			{
				throw new ArgumentException("IsoLangToDelegate not found for " + twoLetterIsoLanguageName, "twoLetterIsoLanguageName");
			}
			return pluralRuleDelegate;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000898B File Offset: 0x00006B8B
		private static bool Between(this decimal value, decimal min, decimal max)
		{
			return value % 1m == 0m && value >= min && value <= max;
		}

		// Token: 0x04000090 RID: 144
		public static readonly Dictionary<string, PluralRules.PluralRuleDelegate> IsoLangToDelegate = new Dictionary<string, PluralRules.PluralRuleDelegate>
		{
			{
				"az",
				PluralRules.Singular
			},
			{
				"bm",
				PluralRules.Singular
			},
			{
				"bo",
				PluralRules.Singular
			},
			{
				"dz",
				PluralRules.Singular
			},
			{
				"fa",
				PluralRules.Singular
			},
			{
				"hu",
				PluralRules.Singular
			},
			{
				"id",
				PluralRules.Singular
			},
			{
				"ig",
				PluralRules.Singular
			},
			{
				"ii",
				PluralRules.Singular
			},
			{
				"ja",
				PluralRules.Singular
			},
			{
				"jv",
				PluralRules.Singular
			},
			{
				"ka",
				PluralRules.Singular
			},
			{
				"kde",
				PluralRules.Singular
			},
			{
				"kea",
				PluralRules.Singular
			},
			{
				"km",
				PluralRules.Singular
			},
			{
				"kn",
				PluralRules.Singular
			},
			{
				"ko",
				PluralRules.Singular
			},
			{
				"ms",
				PluralRules.Singular
			},
			{
				"my",
				PluralRules.Singular
			},
			{
				"root",
				PluralRules.Singular
			},
			{
				"sah",
				PluralRules.Singular
			},
			{
				"ses",
				PluralRules.Singular
			},
			{
				"sg",
				PluralRules.Singular
			},
			{
				"th",
				PluralRules.Singular
			},
			{
				"to",
				PluralRules.Singular
			},
			{
				"vi",
				PluralRules.Singular
			},
			{
				"wo",
				PluralRules.Singular
			},
			{
				"yo",
				PluralRules.Singular
			},
			{
				"zh",
				PluralRules.Singular
			},
			{
				"af",
				PluralRules.DualOneOther
			},
			{
				"bem",
				PluralRules.DualOneOther
			},
			{
				"bg",
				PluralRules.DualOneOther
			},
			{
				"bn",
				PluralRules.DualOneOther
			},
			{
				"brx",
				PluralRules.DualOneOther
			},
			{
				"ca",
				PluralRules.DualOneOther
			},
			{
				"cgg",
				PluralRules.DualOneOther
			},
			{
				"chr",
				PluralRules.DualOneOther
			},
			{
				"da",
				PluralRules.DualOneOther
			},
			{
				"de",
				PluralRules.DualOneOther
			},
			{
				"dv",
				PluralRules.DualOneOther
			},
			{
				"ee",
				PluralRules.DualOneOther
			},
			{
				"el",
				PluralRules.DualOneOther
			},
			{
				"en",
				PluralRules.DualOneOther
			},
			{
				"eo",
				PluralRules.DualOneOther
			},
			{
				"es",
				PluralRules.DualOneOther
			},
			{
				"et",
				PluralRules.DualOneOther
			},
			{
				"eu",
				PluralRules.DualOneOther
			},
			{
				"fi",
				PluralRules.DualOneOther
			},
			{
				"fo",
				PluralRules.DualOneOther
			},
			{
				"fur",
				PluralRules.DualOneOther
			},
			{
				"fy",
				PluralRules.DualOneOther
			},
			{
				"gl",
				PluralRules.DualOneOther
			},
			{
				"gsw",
				PluralRules.DualOneOther
			},
			{
				"gu",
				PluralRules.DualOneOther
			},
			{
				"ha",
				PluralRules.DualOneOther
			},
			{
				"haw",
				PluralRules.DualOneOther
			},
			{
				"he",
				PluralRules.DualOneOther
			},
			{
				"is",
				PluralRules.DualOneOther
			},
			{
				"it",
				PluralRules.DualOneOther
			},
			{
				"kk",
				PluralRules.DualOneOther
			},
			{
				"kl",
				PluralRules.DualOneOther
			},
			{
				"ku",
				PluralRules.DualOneOther
			},
			{
				"lb",
				PluralRules.DualOneOther
			},
			{
				"lg",
				PluralRules.DualOneOther
			},
			{
				"lo",
				PluralRules.DualOneOther
			},
			{
				"mas",
				PluralRules.DualOneOther
			},
			{
				"ml",
				PluralRules.DualOneOther
			},
			{
				"mn",
				PluralRules.DualOneOther
			},
			{
				"mr",
				PluralRules.DualOneOther
			},
			{
				"nah",
				PluralRules.DualOneOther
			},
			{
				"nb",
				PluralRules.DualOneOther
			},
			{
				"ne",
				PluralRules.DualOneOther
			},
			{
				"nl",
				PluralRules.DualOneOther
			},
			{
				"nn",
				PluralRules.DualOneOther
			},
			{
				"no",
				PluralRules.DualOneOther
			},
			{
				"nyn",
				PluralRules.DualOneOther
			},
			{
				"om",
				PluralRules.DualOneOther
			},
			{
				"or",
				PluralRules.DualOneOther
			},
			{
				"pa",
				PluralRules.DualOneOther
			},
			{
				"pap",
				PluralRules.DualOneOther
			},
			{
				"ps",
				PluralRules.DualOneOther
			},
			{
				"pt",
				PluralRules.DualOneOther
			},
			{
				"rm",
				PluralRules.DualOneOther
			},
			{
				"saq",
				PluralRules.DualOneOther
			},
			{
				"so",
				PluralRules.DualOneOther
			},
			{
				"sq",
				PluralRules.DualOneOther
			},
			{
				"ssy",
				PluralRules.DualOneOther
			},
			{
				"sw",
				PluralRules.DualOneOther
			},
			{
				"sv",
				PluralRules.DualOneOther
			},
			{
				"syr",
				PluralRules.DualOneOther
			},
			{
				"ta",
				PluralRules.DualOneOther
			},
			{
				"te",
				PluralRules.DualOneOther
			},
			{
				"tk",
				PluralRules.DualOneOther
			},
			{
				"tr",
				PluralRules.DualOneOther
			},
			{
				"ur",
				PluralRules.DualOneOther
			},
			{
				"wae",
				PluralRules.DualOneOther
			},
			{
				"xog",
				PluralRules.DualOneOther
			},
			{
				"zu",
				PluralRules.DualOneOther
			},
			{
				"ak",
				PluralRules.DualWithZero
			},
			{
				"am",
				PluralRules.DualWithZero
			},
			{
				"bh",
				PluralRules.DualWithZero
			},
			{
				"fil",
				PluralRules.DualWithZero
			},
			{
				"guw",
				PluralRules.DualWithZero
			},
			{
				"hi",
				PluralRules.DualWithZero
			},
			{
				"ln",
				PluralRules.DualWithZero
			},
			{
				"mg",
				PluralRules.DualWithZero
			},
			{
				"nso",
				PluralRules.DualWithZero
			},
			{
				"ti",
				PluralRules.DualWithZero
			},
			{
				"tl",
				PluralRules.DualWithZero
			},
			{
				"wa",
				PluralRules.DualWithZero
			},
			{
				"ff",
				PluralRules.DualFromZeroToTwo
			},
			{
				"fr",
				PluralRules.DualFromZeroToTwo
			},
			{
				"kab",
				PluralRules.DualFromZeroToTwo
			},
			{
				"ga",
				PluralRules.TripleOneTwoOther
			},
			{
				"iu",
				PluralRules.TripleOneTwoOther
			},
			{
				"ksh",
				PluralRules.TripleOneTwoOther
			},
			{
				"kw",
				PluralRules.TripleOneTwoOther
			},
			{
				"se",
				PluralRules.TripleOneTwoOther
			},
			{
				"sma",
				PluralRules.TripleOneTwoOther
			},
			{
				"smi",
				PluralRules.TripleOneTwoOther
			},
			{
				"smj",
				PluralRules.TripleOneTwoOther
			},
			{
				"smn",
				PluralRules.TripleOneTwoOther
			},
			{
				"sms",
				PluralRules.TripleOneTwoOther
			},
			{
				"be",
				PluralRules.RussianSerboCroatian
			},
			{
				"bs",
				PluralRules.RussianSerboCroatian
			},
			{
				"hr",
				PluralRules.RussianSerboCroatian
			},
			{
				"ru",
				PluralRules.RussianSerboCroatian
			},
			{
				"sh",
				PluralRules.RussianSerboCroatian
			},
			{
				"sr",
				PluralRules.RussianSerboCroatian
			},
			{
				"uk",
				PluralRules.RussianSerboCroatian
			},
			{
				"ar",
				PluralRules.Arabic
			},
			{
				"br",
				PluralRules.Breton
			},
			{
				"cs",
				PluralRules.Czech
			},
			{
				"cy",
				PluralRules.Welsh
			},
			{
				"gv",
				PluralRules.Manx
			},
			{
				"lag",
				PluralRules.Langi
			},
			{
				"lt",
				PluralRules.Lithuanian
			},
			{
				"lv",
				PluralRules.Latvian
			},
			{
				"mb",
				PluralRules.Macedonian
			},
			{
				"mo",
				PluralRules.Moldavian
			},
			{
				"mt",
				PluralRules.Maltese
			},
			{
				"pl",
				PluralRules.Polish
			},
			{
				"ro",
				PluralRules.Romanian
			},
			{
				"shi",
				PluralRules.Tachelhit
			},
			{
				"sk",
				PluralRules.Slovak
			},
			{
				"sl",
				PluralRules.Slovenian
			},
			{
				"tzm",
				PluralRules.CentralMoroccoTamazight
			}
		};

		// Token: 0x0200012A RID: 298
		// (Invoke) Token: 0x06000748 RID: 1864
		public delegate int PluralRuleDelegate(decimal value, int pluralCount);
	}
}
