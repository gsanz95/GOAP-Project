using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 Object that handles construction and processing path to find the most optimal sequence of actions
 to take at any given time.
 */
public class AIPlanner
{
    // Returns a queue of optimal actions to achieve goals based on current world conditions. 
    public Queue<AIAction> planActions(GameObject actor, HashSet<AIAction> allActions, Dictionary<string, object> worldConditions, Dictionary<string, object> goalConditions)
    {
        // No goal conditions
        if(goalConditions.Count == 0)
            return null;

        // Restart all actions
        foreach(AIAction action in allActions)
            action.reset();

        // Check if its a valid action
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

        // Add shortest path to queue of actions
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

    // Finds all possible nodes that lead to consecutiveAction and returns if any action has been found
    private bool findParentAction(ActionNode consecutiveAction, List<ActionNode> leaves, HashSet<AIAction> validActions, Dictionary<string, object> worldConditions)
    {
        bool hasFoundAction = false;

        foreach(AIAction action in validActions)
        {
            // The action leads to the nodes current state
            if(isLeadingToState(action.Effects, consecutiveAction.state))
            {
                // Update current state conditions
                Dictionary<string, object> currentState = updateState(consecutiveAction.state, action.Prerequisites);

                ActionNode precedingAction = new ActionNode(consecutiveAction, consecutiveAction.totalCost + action.costToPerform, currentState, action);

                // If it leads to current state add the action node
                if(isLeadingToState(worldConditions, currentState))
                {
                    leaves.Add(precedingAction);
                    hasFoundAction = true;
                }
                else
                {
                    // Remove action evaluated
                    HashSet<AIAction> updatedSet = removeFromSet(validActions, action);

                    // Search for more possible actions (which may be less costly)
                    if(findParentAction(precedingAction, leaves, updatedSet, worldConditions))
                        hasFoundAction = true;
                }
            }
        }

        return hasFoundAction;
    }

    // Return whether or not the effects passed lead to the current state passed
    private bool isLeadingToState(Dictionary<string, object> effects, Dictionary<string, object> currentStates)
    {
        bool isCauseOfState = false;
        foreach(string state in currentStates.Keys)
        {
            // The effect leads to 'state'
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

    // Update the current state with the change passed
    public Dictionary<string, object> updateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
    {
        Dictionary<string, object> updatedState = new Dictionary<string, object>(currentState);

        foreach(string key in stateChange.Keys)
        {
            // Update condition when it exists in current state
            if(currentState.ContainsKey(key))
            {
                updatedState[key] = stateChange[key];
            }
            // Add new condition to state
            else
            {
                updatedState.Add(key, stateChange[key]);
            }
        }
        
        return updatedState;
    }

    // Remove action from the set passed and return the updated set
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