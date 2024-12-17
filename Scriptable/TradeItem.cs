using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxLib.Scriptable
{
	[JsonObject]
	public struct TradeItem
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

		/// <summary>
		/// Price is what the vendor will sell it for, the vendor will buy it for about 41.9% of this value
		/// </summary>
		[JsonProperty]
		public int price;

		[JsonProperty]
		public int maxStack;

		[JsonProperty]
		public ShopkeepData[] vendorData;
	}
}
