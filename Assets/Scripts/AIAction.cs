using System.Collections.Generic;
using UnityEngine;
using System.Text;

public abstract class AIAction : MonoBehaviour {

	private Dictionary<string, object> prerequisites;
	private Dictionary<string, object> effects;
	[HideInInspector]
	public float maxDistance;
	public float costToPerform{ get; set; }
	[HideInInspector]
	public GameObject target;

	public abstract void resetAction();
	public abstract bool isFinished();
	public abstract bool checkRuntimePrerequisites(GameObject objectOfFocus);
	public abstract bool performAction(GameObject actor);
	public abstract bool isInRange();

	public AIAction() {
		prerequisites = new Dictionary<string, object>();
		effects = new Dictionary<string, object>();
	}

	public void reset() {
		target = null;
		resetAction();
	}

	public void addPrerequisite(string identifier, object action){
		this.prerequisites.Add(identifier, action);
	}

	public void removePrerequisite(string identifier) {
		this.prerequisites.Remove(identifier);
	}

	public void addEffect(string identifier, object action){
		this.effects.Add(identifier, action);
	}

	public void removeEffect(string identifier) {
		this.effects.Remove(identifier);
	}

	public Dictionary<string, object> Prerequisites{
		get{
			return this.prerequisites;
		}
	}

	public Dictionary<string, object> Effects{
		get{
			return this.effects;
		}
	}

	public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(this.GetType().Name);
        foreach(KeyValuePair<string, object> pair in this.Prerequisites)
        {
            sb.Append(pair.Key);
            sb.Append(",");
            sb.Append(pair.Value);
            sb.Append("\n");
        }

        foreach(KeyValuePair<string, object> pair in this.Effects)
        {
            sb.Append(pair.Key);
            sb.Append(",");
            sb.Append(pair.Value);
            sb.Append("\n");
        }

        return sb.ToString();
    }
}
