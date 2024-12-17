using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine.Experimental.GlobalIllumination;
using System.IO;

namespace FluxLib
{
	[BepInPlugin(Guid, Name, Version)]
    public class FluxLibPlugin : BaseUnityPlugin
    {
        private const string Guid = "sophfee.atlyss.fluxlib";
		private const string Name = "Sophfee.FluxLib";
		private const string Version = "0.0.1";

		private static FluxLibPlugin _instance;
		public static FluxLibPlugin Instance => _instance;

		public static ManualLogSource Log => _instance.Logger;

		void Awake()
		{
			_instance = this;

			Logger.LogInfo("FluxLibPlugin Awake");


			Harmony harmony = new Harmony(Guid);
			harmony.PatchAll();

			// load custom stuff

			var itemsPath = Path.Combine(Paths.GameRootPath, "Flux/Items");
			if (!Directory.Exists(itemsPath))
				Directory.CreateDirectory(itemsPath);

			var items = Directory.GetFiles(itemsPath);

			foreach (var item in items)
			{
				ModManager.LoadCustomItem(item);
			}

			var tradeItemsPath = Path.Combine(Paths.GameRootPath, "Flux/TradeItems");

			if (!Directory.Exists(tradeItemsPath))
				Directory.CreateDirectory(tradeItemsPath);

			var tradeItems = Directory.GetFiles(tradeItemsPath);

			foreach (var tradeItem in tradeItems)
			{
				ModManager.LoadCustomTradeItem(tradeItem);
			}
		}

		public static void Debug(string text)
		{
#if DEBUG
			Log.LogInfo(text);
#endif
		}

		public static void Warning(string text)
		{
			Log.LogWarning(text);
		}

		public static void Error(string text)
		{
			Log.LogError(text);
		}

		[HarmonyPatch(typeof(ChatBehaviour), "Awake")]
		class ChatHook
		{
			static void Postfix(ChatBehaviour __instance, ref Action<string> ___OnChatMessage)
			{
				FluxLibPlugin.Log.LogInfo("ChatHook Postfix");
				___OnChatMessage += Message;
			}

			static void Message(string message)
			{
				FluxLibPlugin.Log.LogInfo("ChatHook Message");

				if (message.StartsWith("/con"))
				{
				}
			}
		}
	}
}
