using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class GoalAction : AIAction
{
    private bool hasReachedGoal;
    public GoalAction(){
        this.addPrerequisite("HasOpenedDoor", true);
        this.addEffect("HasReachedGoal",true);
        this.maxDistance = 4f;
        this.costToPerform = 1f;
        this.hasReachedGoal = false;
    }

    public override bool checkRuntimePrerequisites(GameObject objectOfFocus)
    {
        if(this.target == null)
            this.target = GameObject.FindWithTag("Goal");
        return this.target != null;
    }
    public override bool isInRange()
    {
        float distanceToTarget = Vector3.Distance(gameObject.GetComponent<Rigidbody>().transform.position, this.target.transform.position);
        return distanceToTarget <= this.maxDistance;
    }
    public override bool performAction(GameObject actor)
    {
        if(this.target != null)
        {
            Destroy(this.target);
            this.hasReachedGoal = true;
        }
        return true;
    }
    public override bool isFinished()
    {
        Debug.Log("Reached Goal!");
        return this.hasReachedGoal;
    }
    public override void resetAction()
    {
        this.hasReachedGoal = false;
    }
}