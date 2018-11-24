using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class actorControl : MonoBehaviour {

	private float movementX;
	private float movementZ;
	[SerializeField]
	public float movementSpeed;
	private Rigidbody actorRigidBody;
	private Vector3 mousePosition;
	private Vector3 lineStart;

	private NavMeshAgent actorNavigation;

	// Use this for initialization
	void Start () {
		actorRigidBody = gameObject.GetComponent<Rigidbody>();
		actorNavigation = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// updatePlayerMovement();
		// lookAtMouse();
		// navigateTowards;
	}

	void updatePlayerMovement() {
		Vector3 desiredPosition = new Vector3(checkMovementX(), 0f, checkMovementZ());
		actorRigidBody.MovePosition(actorRigidBody.position + desiredPosition);
	}

	float checkMovementX() {
		movementX = Input.GetAxisRaw("Horizontal");
		movementX *= movementSpeed;
		movementX *= Time.fixedDeltaTime;
		return movementX;
	}

	float checkMovementZ() {
		movementZ = Input.GetAxisRaw("Vertical");
		movementZ *= movementSpeed;
		movementZ *= Time.fixedDeltaTime;
		return movementZ;
	}

	void lookAtMouse() {
		mousePosition = Input.mousePosition;
		Ray path = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit targetHit;
		if(Physics.Raycast(path, out targetHit, Mathf.Infinity))
		{
			actorRigidBody.transform.LookAt(new Vector3(targetHit.point.x, actorRigidBody.transform.position.y, targetHit.point.z));
		}
	}

	public void navigateTowards(GameObject target)
	{
		actorNavigation.SetDestination(target.transform.position);
	}
}
