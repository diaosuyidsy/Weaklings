using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Inventory : MonoBehaviour, IHasChanged {

	[SerializeField] Transform activeSlots;
	[SerializeField] Transform collectSlots;
	[SerializeField] Text inventoryText;

	// Use this for initialization
	void Start () {
		HasChanged ();
	}

	#region IHasChanged implementation
	public void HasChanged ()
	{
		System.Text.StringBuilder builder = new System.Text.StringBuilder ();
		builder.Append (" - ");
		foreach (Transform slotTranform in activeSlots) {
			GameObject item = slotTranform.GetComponent<Slot> ().item;
			if (item) {
				builder.Append (item.name);
				builder.Append (" - ");
				item.GetComponent<Enable_Ability> ().enabltScript ();
			}
		}

		foreach (Transform slotTransform in collectSlots) {
			GameObject item = slotTransform.GetComponent<Slot> ().item;
			if (item) {
				item.GetComponent<Enable_Ability> ().disableScript ();
			}
		}

		inventoryText.text = builder.ToString ();
	}
	#endregion
}

namespace UnityEngine.EventSystems{
	public interface IHasChanged : IEventSystemHandler{
		void HasChanged () ;
	}
}