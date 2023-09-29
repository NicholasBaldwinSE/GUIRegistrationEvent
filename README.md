# GUIRegistrationEvent

A simple Vintage Story mod creating a clientside subscribable event that fires when a GUI is registered.

## NOTICE

This mod uses a Harmony patch and, therefore, a static event. Make sure that every subscriber to the event created
by this mod is also properly unsubscribed, or there could be potential data leaks.