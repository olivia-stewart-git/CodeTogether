namespace CodeTogether.Client.Integration;

// A class to represent either single-keypress/single-word additions, or entire state changes (for backspace, paste, etc.)
public class KeyPressDTO
{
	public required string StateChange { get; set; }
	public int StateChangePosition { get; set; } = 0;
	public required bool IsEntireState { get; set; }
}