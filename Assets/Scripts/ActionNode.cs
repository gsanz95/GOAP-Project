using System;
using UnityEngine;
using System.Collections.Generic;

/*
 This struct is used to build a regressive graph containing an action,
 total cost, and the current state required to perform an action.
 */
public class ActionNode {
    public ActionNode parent;
    public float totalCost;
    public Dictionary<string,object> state;
    public AIAction action;

    public ActionNode(ActionNode parent, float totalCost, Dictionary<string,object> state, AIAction actionToPerform) {
        this.parent = parent;
        this.totalCost = totalCost;
        this.state = state;
        this.action = actionToPerform;
    }
}