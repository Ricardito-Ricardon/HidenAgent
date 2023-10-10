using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Sense : ScriptableObject
{
    public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool successfullySensed);
    public event OnPerceptionUpdated onPerceptionUpdated;

    [SerializeField] float forgetTime = 2f;
    HashSet<PerceptionStimuli> currentlyPercievableStimuli = new HashSet<PerceptionStimuli>();
    Dictionary<PerceptionStimuli, Coroutine> currentForgettingCoroutines = new Dictionary<PerceptionStimuli, Coroutine>();

    public MonoBehaviour Owner
    {
        get;
        private set;
    }

    public virtual void Init(MonoBehaviour owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        foreach(PerceptionStimuli stimuli in registeredStimulis)
        {
            if(IsStimuliSensable(stimuli) && !IsStimuliSensed(stimuli))
            {
                currentlyPercievableStimuli.Add(stimuli);
                if(currentForgettingCoroutines.ContainsKey(stimuli))
                {
                    StopForgettingStimuli(stimuli);
                }
                else
                {
                    Debug.Log($"I sensed : {stimuli.gameObject.name}");
                }
            }

            if(!IsStimuliSensable(stimuli) && IsStimuliSensed(stimuli))
            {
                currentlyPercievableStimuli.Remove(stimuli);
                StartForgettingStimuli(stimuli);
            }
        }
    }

    private void StopForgettingStimuli(PerceptionStimuli stimuli)
    {
        Coroutine forgettingCoroutine = Owner.StartCoroutine(ForgettingCoroutine(stimuli));
        currentForgettingCoroutines.Remove(stimuli);
    }

    private void StartForgettingStimuli(PerceptionStimuli stimuli)
    {
        Coroutine forgettingCoroutine = Owner.StartCoroutine(ForgettingCoroutine(stimuli));
        currentForgettingCoroutines.Add(stimuli, forgettingCoroutine);
    }

    private IEnumerator ForgettingCoroutine(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(forgetTime);
        onPerceptionUpdated.Invoke(stimuli, false);
        currentForgettingCoroutines.Remove(stimuli); //we have forgot it already, coroutine is done.
    }

    private bool IsStimuliSensed(PerceptionStimuli stimuli)
    {
        return currentlyPercievableStimuli.Contains(stimuli);
    }

    public abstract bool IsStimuliSensable(PerceptionStimuli stimuli);

    public virtual void DrawDebug()
    {

    }




































    static HashSet<PerceptionStimuli> registeredStimulis = new HashSet<PerceptionStimuli>();
    static public void RegisteredStimuli(PerceptionStimuli stimuli)
    {
        registeredStimulis.Add(stimuli);
    }

    static public void UnRegisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStimulis.Remove(stimuli);
    }
}
