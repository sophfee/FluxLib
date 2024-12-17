using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxLib.Scriptable
{
	[JsonObject]
	public struct Consumable
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

		public enum ConsumableType : byte
		{
			Simple,
			Custom
		}

		[JsonProperty]
		public ConsumableType consumableType;

		[JsonProperty]
		public int healthApply;

		[JsonProperty]
		public int manaApply;

		[JsonProperty]
		public int staminaApply;

	}
}
