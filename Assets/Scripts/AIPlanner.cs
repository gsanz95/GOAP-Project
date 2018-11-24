using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlanner
{
    public Queue<AIAction> planActions(GameObject actor, HashSet<AIAction> allActions, Dictionary<string, object> worldConditions, Dictionary<string, object> goalConditions)
    {
        if(goalConditions.Count == 0)
            return null;

        foreach(AIAction action in allActions)
            action.reset();

        HashSet<AIAction> validActions = new HashSet<AIAction>();
        foreach(AIAction action in allActions)
            if(action.checkRuntimePrerequisites(actor))
                validActions.Add(action);

        List<ActionNode> leaves = new List<ActionNode>();

        // Build graph of actions
        ActionNode root = new ActionNode(null, 0, goalConditions, null);
        bool hasSolution = findParentAction(root, leaves, validActions, worldConditions);

        if(!hasSolution)
        {
            Debug.Log("NO PLAN FOUND.");
            return null;
        }

        // Find Cheapest Leaf
        ActionNode cheapestLeaf = null;
        foreach(ActionNode leaf in leaves)
        {
            if(cheapestLeaf == null)
                cheapestLeaf = leaf;
            else
                if(leaf.totalCost < cheapestLeaf.totalCost)
                    cheapestLeaf = leaf;
        }

        Queue<AIAction> actionQueue = new Queue<AIAction>();

        ActionNode actionToAdd = cheapestLeaf;
        while(actionToAdd.action != null)
        {
            actionQueue.Enqueue(actionToAdd.action);
            actionToAdd = actionToAdd.parent;
        }

        Debug.Log("PLAN FOUND!!");
        return actionQueue;
    }

    private bool findParentAction(ActionNode consecutiveAction, List<ActionNode> leaves, HashSet<AIAction> validActions, Dictionary<string, object> worldConditions)
    {
        bool hasFoundAction = false;

        foreach(AIAction action in validActions)
        {
            //Debug.Log(action.ToString() + "is " + isLeadingToState(action.Effects, consecutiveAction.state).ToString());

            if(isLeadingToState(action.Effects, consecutiveAction.state))
            {
                Dictionary<string, object> currentState = updateState(consecutiveAction.state, action.Prerequisites);

                ActionNode precedingAction = new ActionNode(consecutiveAction, consecutiveAction.totalCost + action.costToPerform, currentState, action);

                if(isLeadingToState(worldConditions, currentState))
                {
                    leaves.Add(precedingAction);
                    hasFoundAction = true;
                }
                else
                {
                    HashSet<AIAction> updatedSet = removeFromSet(validActions, action);

                    // More possible preceding actions found
                    if(findParentAction(precedingAction, leaves, updatedSet, worldConditions))
                        hasFoundAction = true;
                }
            }
        }

        return hasFoundAction;
    }

    private bool isLeadingToState(Dictionary<string, object> effects, Dictionary<string, object> currentStates)
    {
        bool isCauseOfState = false;
        foreach(string state in currentStates.Keys)
        {
            if(effects.ContainsKey(state))
            {
                if(currentStates[state].Equals(effects[state]))
                {
                    isCauseOfState = true;
                    //Debug.Log("Found a match!");
                }
            }
        }
        return isCauseOfState;
    }

    public Dictionary<string, object> updateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
    {
        Dictionary<string, object> updatedState = new Dictionary<string, object>(currentState);

        foreach(string key in stateChange.Keys)
        {
            if(currentState.ContainsKey(key))
            {
                updatedState[key] = stateChange[key];
            }
            else
            {
                updatedState.Add(key, stateChange[key]);
            }
        }
        
        return updatedState;
    }

    private HashSet<AIAction> removeFromSet(HashSet<AIAction> actionSet, AIAction actionToRemove)
    {
        HashSet<AIAction> updatedSet = new HashSet<AIAction>();
        foreach(AIAction action in actionSet)
        {
            if(!action.Equals(actionToRemove))
                updatedSet.Add(action);
        }
        return updatedSet;
    }


}