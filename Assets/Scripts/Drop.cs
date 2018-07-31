using UnityEngine;

namespace Xeltica.RainyTune
{
	[RequireComponent(typeof(Rigidbody))]
	public class Drop : MonoBehaviour
	{
		private void Start()
		{
			var col = GetComponent<Collider>();
			if (col == null)
				return;
			col.isTrigger = true;
		}

		public int Velocity { get; set; }
	}
}