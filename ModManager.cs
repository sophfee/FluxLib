using BepInEx;
using FluxLib.Scriptable;
using HarmonyLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.ScriptableObject;

#pragma warning disable IDE0051

namespace FluxLib
{
	public class ModManager
	{
		private static List<ScriptableQuest> _angelaQuests = new List<ScriptableQuest>();
		public static List<ScriptableQuest> AngelaQuests => _angelaQuests;
		private static List<ScriptableItem> customItems = new List<ScriptableItem>();
		public static List<ScriptableItem> CustomItems => customItems;

		public static Sprite LoadSprite(string fileName)
		{
			var iconPath = Path.Combine(Paths.GameRootPath, "Flux/Assets", fileName);
			if (!File.Exists(iconPath))
			{
				FluxLibPlugin.Log.LogError($"Icon file not found: {iconPath}");
				return null;
			}
			var texture = new Texture2D(2, 2);
			byte[] imageData = File.ReadAllBytes(iconPath);
			if (texture.LoadImage(imageData))
			{
				return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			}
			else
			{
				FluxLibPlugin.Log.LogError("Failed to load image");
				return null;
			}
		}

		public static void LoadCustomTradeItem(string file)
		{
			if (!File.Exists(file))
			{
				FluxLibPlugin.Log.LogError($"TradeItem file not found: {file}");
				return;
			}

			var data = File.ReadAllText(file);
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				NullValueHandling = NullValueHandling.Ignore
			};

			var tradeItemData = JsonConvert.DeserializeObject<TradeItem>(data, settings);
			var tradeItem = CreateInstance<ScriptableTradeItem>();

			tradeItem._itemName = tradeItemData.itemName;
			tradeItem._itemDescription = tradeItemData.itemDescription;
			tradeItem._itemRarity = tradeItemData.itemRarity;
			tradeItem._itemType = tradeItemData.itemType;
			tradeItem._itemSortTag = tradeItemData.itemSortTag;
			tradeItem._vendorCost = tradeItemData.price;
			tradeItem._maxStackAmount = tradeItemData.maxStack;
			tradeItem._itemIcon = LoadSprite(tradeItemData.itemIcon);

			if (tradeItemData.vendorData != null && tradeItemData.vendorData.Length > 0)
			{
				FluxLibPlugin.Log.LogMessage($"{tradeItem._itemName} has vendorData for {tradeItemData.vendorData.Length} vendor(s).");

				foreach (var vendorData in tradeItemData.vendorData)
				{
					FluxLibPlugin.Log.LogMessage($"Adding {tradeItem._itemName} to {vendorData.vendorName}");

					ShopkeepItem shopkeepItem = new ShopkeepItem
					{
						_scriptItem = tradeItem,
						_isGambleSlot = vendorData.isGambleSlot,
						_randomizeModifier = vendorData.randomizeModifier,
						_gambleValue = vendorData.gambleCost
					};

					ShopkeepWrangler.AddShopkeepItem(vendorData.vendorName, shopkeepItem);
				}
			}

			CustomItems.Add(tradeItem);
		}

		public static void LoadCustomConsumable(string file)
		{
			if (!File.Exists(file))
			{
				FluxLibPlugin.Log.LogError($"Consumable file not found: {file}");
				return;
			}

			var data = File.ReadAllText(file);

			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				NullValueHandling = NullValueHandling.Ignore
			};

			var consumableData = JsonConvert.DeserializeObject<Consumable>(data, settings);

			var consumable = CreateInstance<ScriptableConsumable>();
		}

		public static void LoadCustomItem(string file)
		{
			/*
			var data = File.ReadAllText(file);
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				NullValueHandling = NullValueHandling.Ignore
			};

			var item = JsonConvert.DeserializeObject<Item>(data, settings);

			ScriptableItem?\ scriptableItem;
			switch (item.itemType)
			{
				case ItemType.TRADE:
					break;
				case ItemType.CONSUMABLE:
					scriptableItem = CreateInstance<ScriptableConsumable>();
					break;
				case ItemType.GEAR:
					scriptableItem = CreateInstance<ScriptableEquipment>();
					break;
				default:
					scriptableItem = null;
					break;
			}

			if (scriptableItem is null)
			{
				FluxLibPlugin.Log.LogError("Failed to create ScriptableItem");
				return;
			}

			scriptableItem._itemName = item.itemName;
			scriptableItem._itemDescription = item.itemDescription;

			if (!item.iconIsExternal)
			{

			}
			else
			{
				var iconPath = Path.Combine(Paths.ExecutablePath, "Flux/Assets", item.itemIcon);

				if (!File.Exists(iconPath))
				{
					FluxLibPlugin.Log.LogError($"Icon file not found: {iconPath}");
					return;
				}

				var texture = new Texture2D(2, 2);

				byte[] imageData = File.ReadAllBytes(iconPath);

				if (texture.LoadImage(imageData))
				{
					scriptableItem._itemIcon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
				}
				else
				{
					FluxLibPlugin.Log.LogError("Failed to load image");
				}
			}

			scriptableItem._itemRarity = item.itemRarity;
			scriptableItem._itemType = item.itemType;
			scriptableItem._itemSortTag = item.itemSortTag;
			scriptableItem._vendorCost = item.vendorCost;
			scriptableItem._maxStackAmount = item.maxStack;

			CustomItems.Add(scriptableItem);
			*/
		}

		[HarmonyPatch(typeof(GameManager), "Cache_ScriptableAssets")]
		class GameManager_Cache_ScriptableAssets
		{
			static void Postfix(
				GameManager __instance,
				ref Dictionary<string, ScriptableItem> ____cachedScriptableItems,
				ref Dictionary<string, ScriptableQuest> ____cachedScriptableQuests,
				ref Dictionary<string, ScriptableCreep> ____cachedScriptableCreeps
			)
			{

				FluxLibPlugin.Log.LogInfo("GameManager_Cache_ScriptableAssets Prefix");

				foreach (var item in CustomItems)
				{
					____cachedScriptableItems.Add(item._itemName, item);
				}
			}
		}


		[HarmonyPatch(typeof(DialogTrigger), "Start")]
		class LoadCustomQuests
		{
			static void Postfix(DialogTrigger __instance)
			{
				string name = __instance.gameObject.transform.parent.gameObject.name;
				FluxLibPlugin.Log.LogInfo($"dialogue trigger is {name}");

				switch (name)
				{
					case "_npc_Angela":
						FluxLibPlugin.Log.LogInfo($"npc angela has {_angelaQuests.Count}");
						for (int i = 0; i < _angelaQuests.Count; i++)
						{
							__instance._scriptDialogData._scriptableQuests.AddItem(_angelaQuests[i]);
						}
						break;
				}
			}
		}
	}
}
