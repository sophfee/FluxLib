using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxLib.Scriptable
{
	[JsonObject]
	public struct ShopkeepData
	{
		[JsonProperty]
		public string vendorName;

		[JsonProperty]
		public bool isGambleSlot;

		[JsonProperty]
		public int gambleCost;

		[JsonProperty]
		public bool randomizeModifier;
	}
}
