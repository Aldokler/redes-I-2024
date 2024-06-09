using System;
using UnityEngine;

namespace Alteruna.TextChatCommands
{
	public class CommandScale : ITextChatCommand
	{
		public string Command { get; } = "scale";
		public string Description { get; } = "scale target (get target using /target) currently looking at (use ~ for relative coordinates)";
		public string Usage { get; } = "/scale x y z";
		public bool IsCheat { get; } = true;
		public bool IgnoreCase { get; } = true;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
		public static void Init() => TextChatSynchronizable.Commands.Add(new CommandScale());

		public string Execute(TextChatSynchronizable textChat, string[] args)
		{
			if (!CommandTarget.LastTarget)
			{
				textChat.LogError("No valid target. use /target to get one.");
				return null;
			}

			CommandTarget.LastTarget.localScale = CommandTp.StringToVector3(CommandTarget.LastTarget.localScale, args);

			return null;
		}
	}
}