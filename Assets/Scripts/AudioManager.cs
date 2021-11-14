using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;
	public bool loop;

	[Range(0f, 1f)]
	public float vol = 50;
	private AudioSource source;

	public void setSource(AudioSource _source)
	{
		source = _source;
		source.clip = clip;
		source.loop = loop;
	}

	public void play()
	{
		source.volume = vol;
		source.Play();
	}
	public void playIfNotPlayerd()
	{
		source.volume = vol;
		if (!source.isPlaying)
			source.Play();
	}

	public void pause()
	{
		source.Pause();
	}
}
public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	[SerializeField]
	Sound[] sounds;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			Debug.Log("More then one audio manager!");
			return;
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(instance);
		}

		for (int i = 0; i < sounds.Length; i++)
		{
			GameObject _go = new GameObject("Sound" + i + "_" + sounds[i].name);
			_go.transform.SetParent(this.transform);
			sounds[i].setSource(_go.AddComponent<AudioSource>());
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		PlayIfNot("LevelMusic");
		PlayIfNot("Forest");

	}
	public void PlayIfNot(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				sounds[i].playIfNotPlayerd();
				return;
			}
		}
	}
	public void PlaySound(string _name)
	{
		if (_name == "")
			return;
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				sounds[i].play();
				return;
			}
		}

		//no sound with name
		Debug.LogWarning("Sound not found in sounds array:" + _name);
	}

	public void StopSound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				sounds[i].pause();
				return;
			}
		}

		//no sound with name
		Debug.LogWarning("Sound not found in sounds array:" + _name);
	}
}