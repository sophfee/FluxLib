using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using static FluxLib.FluxLibPlugin;

namespace FluxLib
{
	public class ShopkeepWrangler
	{
		private static readonly Dictionary<string, ShopkeepItemTable> _shopkeepItems = new Dictionary<string, ShopkeepItemTable>();
		public static Dictionary<string, ShopkeepItemTable> ShopkeepDatabase => _shopkeepItems;

		[HarmonyPatch(typeof(NetNPC), "Init_ShopkeepListing")]
		class NetNPC__Init_ShopkeepListing
		{
			static void Postfix(NetNPC __instance, ScriptableShopkeep _scriptShopkeep)
			{
				GameObject npcObject = __instance.gameObject;
				string npc = npcObject.name;
				
				if (!_shopkeepItems.ContainsKey(npc))
				{
					return;
				}

				
				ShopkeepItemTable itemTable = _shopkeepItems[npc];
				
				int z = __instance._vendorItems.Count;
				
				for (int j = 0; j < itemTable._shopkeepItems.Length; j++)
				{
					ShopkeepItem shopItem = itemTable._shopkeepItems[j];
					// we can retrieve name data and whatnot from here :-)
					ScriptableItem item = null;
					string text = string.Empty;
					if ((bool)itemTable._shopkeepItems[j]._scriptItem)
					{
						item = itemTable._shopkeepItems[j]._scriptItem;
					}

					if (itemTable._shopkeepItems[j]._isGambleSlot)
					{
						item = itemTable._shopkeepItems[j]._gambleLootTable._itemDrops[UnityEngine.Random.Range(0, itemTable._shopkeepItems[j]._gambleLootTable._itemDrops.Length)]._item;
					}

					string key = item._itemName ?? "";
					if (item._itemType == ItemType.GEAR)
					{
						ScriptableEquipment scriptableEquipment = (ScriptableEquipment)item;
						if ((int)scriptableEquipment._itemRarity <= 1)
						{
							if (itemTable._shopkeepItems[j]._randomizeModifier && (bool)scriptableEquipment._statModifierTable)
							{
								if (UnityEngine.Random.Range(0, 2) == 1)
								{
									StatModifierSlot[] statModifierSlots = scriptableEquipment._statModifierTable._statModifierSlots;
									text = statModifierSlots[UnityEngine.Random.Range(0, statModifierSlots.Length)]._equipModifier._modifierTag;
								}
								else
								{
									text = string.Empty;
								}
							}
							else if ((bool)itemTable._shopkeepItems[j]._setStatModifier)
							{
								text = itemTable._shopkeepItems[j]._setStatModifier._modifierTag;
							}

							if (!string.IsNullOrEmpty(text))
							{
								key = text + " " + item._itemName;
							}
						}
					}

					ItemData itemData = new ItemData
					{
						_itemName = item._itemName,
						_maxQuantity = item._maxStackAmount,
						_isEquipped = false,
						_isAltWeapon = false,
						_quantity = 0,
						_slotNumber = __instance._vendorItems.Count // index 0
					};
					
					ShopkeepItemStruct shopkeepItemStruct = default(ShopkeepItemStruct);
					shopkeepItemStruct._itemName = item._itemName;
					shopkeepItemStruct._stockQuantity = itemTable._shopkeepItems[j]._initialStock;
					shopkeepItemStruct._isInfiniteStock = itemTable._shopkeepItems[j]._isInfiniteStock;
					shopkeepItemStruct._isbuybackItem = false;
					shopkeepItemStruct._isGambleItem = itemTable._shopkeepItems[j]._isGambleSlot;
					shopkeepItemStruct._gambleValue = itemTable._shopkeepItems[j]._gambleValue;
					shopkeepItemStruct._equipModifierTag = text;
					shopkeepItemStruct._itemData = itemData;
					shopkeepItemStruct._removeAtEmptyStock = itemTable._shopkeepItems[j]._removeAtEmptyStock;

					if (!__instance._vendorItems.ContainsKey(key))
					{
						__instance._vendorItems.Add(key, shopkeepItemStruct);
					}
					else
					{
						Warning($"Item {key} already exists in {npc}");
					}
				}
			}
		}

		public static void AddShopkeepItem(string npc, ShopkeepItem item)
		{
			if (_shopkeepItems.ContainsKey(npc))
			{
				_shopkeepItems[npc]._shopkeepItems.AddItem(item);
			}
			else
			{
				ShopkeepItemTable shopkeepItemTable = new ShopkeepItemTable
				{
					_shopkeepItems = new[] { item }
				};
				_shopkeepItems.Add(npc, shopkeepItemTable);
			}
		}
	}
}
