using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

/*
 If actor has key the door will be unlocked
*/
public class UnlockDoorAction : AIAction
{
    private bool isUnlocked;
    public UnlockDoorAction(){
        this.addPrerequisite("HasKey", true);
        this.addEffect("HasOpenedDoor", true);
        this.maxDistance = 4f;
        this.costToPerform = 5f;
        this.isUnlocked = false;
    }

    public override bool checkRuntimePrerequisites(GameObject objectOfFocus)
    {
        if(this.target == null)
            this.target = GameObject.FindWithTag("Door");        
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
            this.isUnlocked = true;
        }
        return true;
    }
    public override bool isFinished()
    {
        Debug.Log("Opened Door!");

        return this.isUnlocked;
    }
    public override void resetAction()
    {
        this.isUnlocked = false;
    }
}