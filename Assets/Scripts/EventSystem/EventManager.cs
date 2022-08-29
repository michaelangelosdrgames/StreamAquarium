using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    private List<IEventObserver>[] eventObservers;

    public EventManager()
    {
        eventObservers = new List<IEventObserver>[(int)EventId.Count];
    }

    public void AddObserver(IEventObserver eventObserver, EventId eventId)
    {
        List<IEventObserver> list = eventObservers[(int)eventId];
        if(list == null)
        {
            list = new List<IEventObserver>();
            eventObservers[(int)eventId] = list;
        }

        if(!list.Contains(eventObserver))
        {
            list.Add(eventObserver);
        }
    }

    public void SendEvent(EventId eventId, object payload)
    {
        List<IEventObserver> list = eventObservers[(int)eventId];
        if(list == null)
        {
            Debug.LogError("[EventMAnager]: Event sent but no observers yet on EventId: " + eventId);
            //No observers yet.
            return;
        }
        else
        {
            for(int i =0; i<list.Count; i++)
            {
                if(list[i] == null)
                {
                    Debug.LogError($"[EventManager]: observer received and event BUT was already deleted, dont forget to remove your observers! {list[i].GetType()}");
                    continue;
                }
                else
                {
                    list[i].OnEvent(eventId, payload);
                }
            }
        }
    }

    public void RemoveObserver(IEventObserver eventObserver, EventId eventId)
    {
        List<IEventObserver> list = eventObservers[(int)eventId];
        if(list == null)
        {
            //No observers on the designated event ID yet. return.
            return;
        }

        if(list.Contains(eventObserver))
        {
            list.Remove(eventObserver);
        }
    }

    private void OnDestroy()
    {
        eventObservers = null;
    }
}
