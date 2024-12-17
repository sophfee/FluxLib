using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxLib.Scriptable
{
	[JsonObject]
	public struct Weapon
	{
		[JsonProperty]
		public string itemName;

		[JsonProperty]
		public string itemDescription;

		[JsonProperty]
		public string itemIcon;

		[JsonProperty]
		public bool iconIsExternal;

		[JsonProperty]
		public int itemLevel;

		[JsonProperty]
		public ItemRarity itemRarity;

		[JsonProperty]
		public ItemType itemType;

		[JsonProperty]
		public SortTag itemSortTag;

		[JsonProperty]
		public int vendorCost;

		[JsonProperty]
		public int maxStack;

		[JsonProperty]
		public string assetBundlePath;

		[JsonProperty]
		public string meshPath;

		public enum CombatElement : byte
		{
			Normal,
			Air,
			Earth,
			Fire,
			Holy,
			Shadow,
			Water,
			Custom
		}

		[JsonProperty]
		public CombatElement combatElement;

		[JsonProperty]
		public string customCombatElement;

		public enum WeaponType : byte
		{
			Unarmed,
			Katars,
			Melee,
			HeavyMelee,
			Polearm,
			Ranged,
			MagicScepter,
			MagicBell,
			Custom
		}

		[JsonProperty]
		public WeaponType weaponType;

		[JsonProperty]
		public string customWeaponType;

		[JsonProperty]
		public string texturePath;
	}
}
