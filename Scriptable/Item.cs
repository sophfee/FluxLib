using Newtonsoft.Json;

namespace FluxLib.Scriptable
{
	[JsonObject]
	public struct Item
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
		public int price;

		[JsonProperty]
		public int maxStack;
	}
}
