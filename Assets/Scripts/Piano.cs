using Groorine.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xeltica.RainyTune
{
	[RequireComponent(typeof(AudioSource))]
	public class Piano : MonoBehaviour
	{
		private float animationTime = .4f;

		int noteNumber;

		AudioSource aud;

		float y;

		static string[] note = { "C", "C+", "D", "D+", "E", "F", "F+", "G", "G+", "A", "A+", "B" };

		Coroutine prevAnimating;

		private void Start()
		{
			aud = GetComponent<AudioSource>();

			if (System.Array.IndexOf(note, name) == -1)
			{
				Debug.LogError("Don't change piano objects' name from default! It's used to calculate pitch.");
				Debug.Break();
			}
			var pitch = System.Array.IndexOf(note, name);

			int octave;
			if (!int.TryParse(transform.parent.name.Remove(0, 7), out octave))
			{
				Debug.LogError("Octave Prefabs' name must have a valid format 'Octave (num)'. It's used to calculate pitch.");
				Debug.Break();
			}

			noteNumber = (octave + 1) * 12 + pitch; 

			aud.pitch = Mathf.Pow(2, (noteNumber - 69) / 12f);

			y = transform.position.y;
		}

		private void OnTriggerEnter(Collider other)
		{
			var drop = other.GetComponent<Drop>();
			if (drop == null)
				return;
			aud.volume = drop.Velocity / 127f;
			aud.Play();
			Destroy(other.gameObject);
			if (prevAnimating != null)
			{
				transform.position = new Vector3(transform.position.x, y, transform.position.z);
				StopCoroutine(prevAnimating);
			}
			prevAnimating = StartCoroutine(NoteOnAnimation());
		}

		/// <summary>
		/// 鍵盤を押すアニメーション。
		/// </summary>
		/// <returns></returns>
		IEnumerator NoteOnAnimation()
		{
			var time = 0f;
			while (time < animationTime)
			{
				transform.position = new Vector3(transform.position.x, (float)MathHelper.EaseOut(time / animationTime, y - 1, y), transform.position.z);

				time += Time.deltaTime;
				yield return null;
			}
		}
	}
}