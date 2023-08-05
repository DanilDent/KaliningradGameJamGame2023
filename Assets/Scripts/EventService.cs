using System;

public class EventService : Singleton<EventService>
{
    public Action DisplayInteractButton;
    public Action HideInteractButton;
    public Action InteractButtonPressed;
    public Action InteractButtonReleased;
    public Action BoostButtonPressed;
}