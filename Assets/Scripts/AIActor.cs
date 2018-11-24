using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public void addAction(AIAction action)
    {
        this.allActions.Add(action);
    }

    public AIAction getAction()
    {
        if(this.hasActionPlan())
            return this.currentActions.Peek();
        return null;
    }

    public void endAction()
    {
        if(this.actionToPerform.isFinished())
        {
            this.currentActions.Dequeue();
            this.actionToPerform = null;
        }
        return;
    }

    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void runIdleState()
    {
        Debug.Log("Idle");
        // Look for a plan
        Queue<AIAction> sequenceOfActions = planner.planActions(gameObject, this.allActions, this.worldConditions, this.goals);

        if(sequenceOfActions != null)
        {
            this.isRunningPlan = true;
            currentActions = sequenceOfActions;
        }

        return;
    }

    private void runActions()
    {
        if(!this.hasActionPlan())
        {
            this.isRunningPlan = false;
            return;
        }

        if(this.actionToPerform == null)
            this.actionToPerform = getAction();

        if(actionToPerform.checkRuntimePrerequisites(null))
        {
            //Debug.Log("Running action");
            if(actionToPerform.isInRange())
            {
                actionToPerform.performAction(gameObject);
                this.endAction();
            }else{
                controller.navigateTowards(actionToPerform.target);
            }
        }
        return;
    }

    private void fillActions() {
        AIAction[] actions = gameObject.GetComponents<AIAction>();
        foreach(AIAction a in actions)
            allActions.Add(a);

        return;
    }

    public void addGoal(string key, object value)
    {
        this.goals[key] = value;
        return;
    }

    public void addWorldCondition(string key, object value)
    {
        this.worldConditions[key] = value;
        return;
    }
}