namespace Alteruna.TextChatCommands
{
	public interface ITextChatCommand
	{
		string Command { get; }
		string Description { get; }
		string Usage { get; }
		bool IsCheat { get; }
		bool IgnoreCase { get; }
		string Execute(TextChatSynchronizable textChat, string[] args);
	}
}