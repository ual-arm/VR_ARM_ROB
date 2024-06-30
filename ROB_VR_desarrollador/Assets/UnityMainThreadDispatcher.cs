using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Enqueue(Action action)
    {
        if (instance != null)
            instance.dispatcher.Enqueue(action);
    }

    private class Dispatcher : MonoBehaviour
    {
        private readonly Queue<Action> actionQueue = new Queue<Action>();

        void Update()
        {
            lock (actionQueue)
            {
                while (actionQueue.Count > 0)
                {
                    actionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (actionQueue)
            {
                actionQueue.Enqueue(action);
            }
        }
    }

    private Dispatcher dispatcher;

    void Start()
    {
        dispatcher = gameObject.AddComponent<Dispatcher>();
    }
}
