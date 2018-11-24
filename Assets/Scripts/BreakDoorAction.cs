using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDoorAction : AIAction {

	private bool hasBrokenDoor;
	public BreakDoorAction()
	{
		this.addPrerequisite("HasKey", false);
		this.addEffect("HasOpenedDoor", true);
		this.maxDistance = 4f;
		this.costToPerform = 20f;
		this.hasBrokenDoor = false;
	}

	public override void resetAction()
	{
		this.hasBrokenDoor = false;
	}
	public override bool isFinished()
	{
		Debug.Log("Broken Door");
		return this.hasBrokenDoor;
	}
	public override bool checkRuntimePrerequisites(GameObject objectOfFocus)
	{
		if(this.target == null)
            this.target = GameObject.FindWithTag("Door");
        return this.target != null;
	}
	public override bool performAction(GameObject actor)
	{
		if(this.target != null)
		{
			this.hasBrokenDoor = true;
			Destroy(this.target);
		}

		return true;
	}
	public override bool isInRange()
	{
		float distanceToTarget = Vector3.Distance(gameObject.GetComponent<Rigidbody>().transform.position, this.target.transform.position);
        return distanceToTarget <= this.maxDistance;
	}
}
