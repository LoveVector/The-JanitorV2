using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StatesCollection
{
    public List<string> states;

    public StatesCollection()
    {
        states = new List<string>();
    }

    public StatesCollection(StatesCollection copyStates)
    {
        states = new List<string>(copyStates.states);
    }

    public int CompareStates(StatesCollection comparingState)
    {
        int difference = 0;
        for (int c = 0; c < comparingState.states.Count; c++)
        {
            if (!states.Contains(comparingState.states[c]))
            {
                difference++;
            }
        }

        return difference;
    }

    public void AddStates(StatesCollection addingStates)
    {
        for (int i = 0; i < addingStates.states.Count; i++)
        {
            if (!states.Contains(addingStates.states[i]))
            {
                states.Add(addingStates.states[i]);
            }
        }
    }

    public void RemoveStates(StatesCollection removingStates)
    {
        for (int i = 0; i < removingStates.states.Count; i++)
        {
                states.Remove(removingStates.states[i]);
        }
    }
}
