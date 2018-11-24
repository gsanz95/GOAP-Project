using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class GrabKeyAction : AIAction
{
    private bool hasKey;
    public GrabKeyAction(){
        this.addPrerequisite("HasKey", false);
        this.addEffect("HasKey",true);
        this.hasKey = false;
        this.maxDistance = 4f;
        this.costToPerform = 2f;
    }
    public override bool checkRuntimePrerequisites(GameObject objectOfFocus){
        if(this.target == null)
            this.target = GameObject.FindWithTag("Usable");        
        return this.target != null;
    }
    public override bool isInRange(){
        float distanceToTarget = Vector3.Distance(gameObject.GetComponent<Rigidbody>().transform.position, this.target.transform.position);
        return distanceToTarget <= this.maxDistance;
    }
    public override bool performAction(GameObject actor){
        if(this.target != null)
        {
            this.hasKey = true;
            Destroy(this.target);
        }

        return true;
    }
    public override bool isFinished(){
        Debug.Log("Grabbed key!");
        return this.hasKey;
    }
    public override void resetAction(){
        this.hasKey = false;
    }
}