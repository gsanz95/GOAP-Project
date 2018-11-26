using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 The GameObject holding this component will be controlled artificially
 */
public class AIActor : MonoBehaviour
{
    private Queue<AIAction> currentActions;
    private HashSet<AIAction> allActions;
    private Dictionary<string, object> worldConditions;
    private Dictionary<string, object> goals;

    private AIPlanner planner;
    private bool isRunningPlan;
    private actorControl controller;
    private AIAction actionToPerform;

    // Called at the beginning of the program
    void Start(){
        this.currentActions = new Queue<AIAction>();
        this.allActions = new HashSet<AIAction>();
        this.planner = new AIPlanner();
        this.isRunningPlan = false;
        this.controller = gameObject.GetComponent<actorControl>();
        this.worldConditions = new Dictionary<string, object>();
        this.goals = new Dictionary<string, object>();
        addGoal("HasReachedGoal",true);
        addWorldCondition("HasKey",false);
        fillActions();
    }

    // Called before every frame
    void Update(){
        if(!this.isRunningPlan)
        {
            runIdleState();
        }else
        {
            runActions();
        }

        if(!hasActionPlan())
            this.isRunningPlan = false;

    }

    // Adds an action to the set of all aactions available to this actor
    public void addAction(AIAction action)
    {
        this.allActions.Add(action);
    }

    // Returns an action if a plan has been determined for this actor
    public AIAction getAction()
    {
        // Get the action on top
        if(this.hasActionPlan())
            return this.currentActions.Peek();
        return null;
    }

    // Checks if an action is finished
    public void endAction()
    {
        if(this.actionToPerform.isFinished())
        {
            this.currentActions.Dequeue();
            this.actionToPerform = null;
        }
        return;
    }

    // Checks if there are any remaining actions to be performed
    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    // When invoked, it calls onto planner and if a plan is found, all the actions return will be added
    // to currentActions
    private void runIdleState()
    {
        Debug.Log("Idle");
        // Look for a plan
        Queue<AIAction> sequenceOfActions = planner.planActions(gameObject, this.allActions, this.worldConditions, this.goals);

        // There is a plan!
        if(sequenceOfActions != null)
        {
            this.isRunningPlan = true;
            this.currentActions = sequenceOfActions;
        }

        return;
    }

    /*
    * When invoked, looks for the action on top of the sequence.
    */
    private void runActions()
    {
        if(!this.hasActionPlan())
        {
            this.isRunningPlan = false;
            return;
        }

        // If there is no action or has been cleared
        if(this.actionToPerform == null)
            this.actionToPerform = getAction();

        // Check that the world state is valid for this action
        if(actionToPerform.checkRuntimePrerequisites(null))
        {
            if(actionToPerform.isInRange())
            {
                actionToPerform.performAction(gameObject);
                this.endAction();
            }
            // Move to target of action
            else{
                controller.navigateTowards(actionToPerform.target);
            }
        }
        return;
    }

    /*
     Fills all actions the actor has as components (usually all functions the actor can do)
     */
    private void fillActions() {
        AIAction[] actions = gameObject.GetComponents<AIAction>();
        foreach(AIAction a in actions)
            allActions.Add(a);

        return;
    }

    /*
     Adds a goal and its bool value into the goals dictionary
     */
    public void addGoal(string key, object value)
    {
        this.goals[key] = value;
        return;
    }

    /*
     Add a goal and its bool value into the current world condition dictionary
     */
    public void addWorldCondition(string key, object value)
    {
        this.worldConditions[key] = value;
        return;
    }
}