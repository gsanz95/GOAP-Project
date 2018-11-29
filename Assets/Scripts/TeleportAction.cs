using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAction : AIAction {

	private bool hasTeleported;
	public TeleportAction(){
		this.addPrerequisite("HasKey", false);
		this.addEffect("HasOpenedDoor", true);
		this.maxDistance = 4f;
		this.costToPerform = 10f;
		this.hasTeleported = false;
	}

	public override void resetAction(){
		this.hasTeleported = false;
	}
	public override bool isFinished(){
		return this.hasTeleported;
	}
	public override bool checkRuntimePrerequisites(GameObject objectOfFocus){
		if(this.target == null)
            this.target = GameObject.FindWithTag("Door");        
        return this.target != null;
	}
	public override bool performAction(GameObject actor){
		if(this.target != null)
		{
			actorControl controller = actor.GetComponent<actorControl>();
			if(controller != null)
			{
				controller.teleportAcross(this.target.transform.position);
				this.hasTeleported = true;
			}
		}

		return true;
	}
	public override bool isInRange(){
		float distanceToTarget = Vector3.Distance(gameObject.GetComponent<Rigidbody>().transform.position, this.target.transform.position);
        return distanceToTarget <= this.maxDistance;
	}
}
