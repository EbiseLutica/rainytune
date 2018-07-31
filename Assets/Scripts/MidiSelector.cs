using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Xeltica.RainyTune
{
	public class MidiSelector : MonoBehaviour
	{
		[SerializeField]
		GameObject SelectorContainer;

		[SerializeField]
		Button ButtonPrefab;

		// Use this for initialization
		void Start()
		{
			foreach (var file in Directory.EnumerateFiles("./songs").Where(t => new []{ ".mid", ".midi" }.Contains(Path.GetExtension(t))))
			{
				var btn = Instantiate(ButtonPrefab.gameObject, SelectorContainer.transform);
				btn.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(file);
				btn.GetComponent<Button>().onClick.AddListener(() =>
				{
					StaticVariable.Path = file;
					SceneManager.LoadScene("Play");
				});
			}
		}
	}
}