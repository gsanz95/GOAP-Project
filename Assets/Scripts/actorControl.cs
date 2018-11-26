using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class actorControl : MonoBehaviour {

	private float movementX;
	private float movementZ;
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
		// navigateTowards();
	}

	// Updates gameObject position according to user input
	void updatePlayerMovement() {
		Vector3 desiredPosition = new Vector3(checkMovementX(), 0f, checkMovementZ());
		actorRigidBody.MovePosition(actorRigidBody.position + desiredPosition);
	}

	// Updates X-axis position relative to user input
	float checkMovementX() {
		movementX = Input.GetAxisRaw("Horizontal");
		movementX *= movementSpeed;
		movementX *= Time.fixedDeltaTime;
		return movementX;
	}

	// Updates Z-axis position relative to user input
	float checkMovementZ() {
		movementZ = Input.GetAxisRaw("Vertical");
		movementZ *= movementSpeed;
		movementZ *= Time.fixedDeltaTime;
		return movementZ;
	}

	// Rotates the gameObject to point towards the mouse position projected on the screen
	void lookAtMouse() {
		mousePosition = Input.mousePosition;
		Ray path = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit targetHit;

		// If Ray hits a point in the game
		if(Physics.Raycast(path, out targetHit, Mathf.Infinity))
		{
			actorRigidBody.transform.LookAt(new Vector3(targetHit.point.x, actorRigidBody.transform.position.y, targetHit.point.z));
		}
	}

	// Uses NavMesh to navigate the gameObject containing this script towards the end
	public void navigateTowards(GameObject target)
	{
		actorNavigation.SetDestination(target.transform.position);
	}
}
