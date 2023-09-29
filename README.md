# GUIRegistrationEvent

A simple Vintage Story mod creating a clientside subscribable event that fires when a GUI is registered.

## NOTICE

This mod uses a Harmony patch and, therefore, a static event. Make sure that every subscriber to the event created
by this mod is also properly unsubscribed, or there could be potential data leaks. This also means that you 
can subscribe to the event without directly pulling the modsystem itself, since it is static. This
is by no means perfect, but from testing it works well enough.

## USAGE

```C#
public class YourMod : ModSystem
{
	ICoreClientAPI capi;

	public override void StartClientSide(ICoreClientAPI capi)
	{
		this.capi = capi;
			
		GUIRegistrationEventModSystem.GUIRegistrationEvent += onGuiRegistered;
	}
	
	public override void Dispose()
	{
		if (capi != null) GUIRegistrationEventModSystem.GUIRegistrationEvent -= onGuiRegistered;
	}
	
	public void onGuiRegistered(GuiDialog dialog)
	{
		Console.WriteLine(dialog.DebugName);
	}
}
```