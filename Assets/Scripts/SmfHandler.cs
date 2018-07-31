using Groorine;
using Groorine.DataModel;
using Groorine.Events;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Xeltica.RainyTune
{
	public class SmfHandler : MonoBehaviour
	{
		[SerializeField]
		string path;

		MidiFile midi;

		[SerializeField]
		GameObject drop;

		public bool IsPlaying { get; set; }

		public float CurrentTime { get; set; }

		public float MaxTime => (float)(midi?.Conductor.ToMilliSeconds(midi.Length) ?? 0) / 1000;

		int prevTick, tick;

		[SerializeField]
		Slider slider;

		[SerializeField]
		Text text;

		private IEnumerator Start()
		{
			try
			{
				midi = SmfParser.Parse(new FileStream(StaticVariable.Path ?? path, FileMode.Open));
			}
			catch (Exception)
			{
				Back();
			}
			while (true)
			{
				yield return null;
				if (!IsPlaying)
					continue;

				CurrentTime += Time.deltaTime;
				float time = CurrentTime * 1000;

				tick = (int)midi.Conductor.ToTick(time);

				foreach (var track in midi.Tracks)
				{
					foreach (var e in track.GetDataBetweenTicks(prevTick - 1, tick).OfType<NoteEvent>())
					{
						var i = Instantiate(drop, new Vector3(e.Note - 60, 30, 0), default(Quaternion));
						i.GetComponent<Drop>().Velocity = e.Velocity;
					}
				}
				prevTick = tick;

				text.text = $"{TimeToString(CurrentTime)} / {TimeToString(MaxTime)}";

				//slider.maxValue = MaxTime;
				//slider.minValue = 0;
				//slider.value = CurrentTime;

				if (MaxTime < CurrentTime)
					Stop();
			}

		}

		public static string TimeToString(float time)
		{
			int h, m;
			float s;
			s = time % 60;
			m = (int)(time / 60);
			h = (int)(time / 3600);

			return $"{h:00}:{m:00}:{s:00.00}";
		}

		public void Play()
		{
			IsPlaying = true;
			Time.timeScale = 1;
		}

		public void Stop()
		{
			IsPlaying = false;
			CurrentTime = 0;
			tick = prevTick = 0;
		}

		public void Pause()
		{
			IsPlaying = false;
			Time.timeScale = 0;
		}

		public void Seek(float value)
		{
			CurrentTime = MaxTime * value;
			prevTick = tick = (int)midi.Conductor.ToTick(CurrentTime * 1000);
		}

		public void Back()
		{
			SceneManager.LoadScene("Title");
		}

	}

	public static class StaticVariable
	{
		public static string Path { get; set; }
	}

}