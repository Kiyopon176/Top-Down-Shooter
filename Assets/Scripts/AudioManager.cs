using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;

    private Dictionary<string, int> audioMappings = new Dictionary<string, int>
    {
        { "Shoot", 0 },
        { "EnemyKilled", 1 },
        { "Bonus", 2 }
    };

    private void OnEnable()
    {
        EventManager.OnSoundNeed += PlaySound;
    }

    private void PlaySound(string eventName)
    {
        if (audioMappings.TryGetValue(eventName, out int index))
        {
            if (index < audioSources.Length && audioSources[index] != null)
            {
                audioSources[index].Play();
            }
            else
            {
                Debug.LogWarning($"AudioSource {index} is null or not initialized.");
            }
        }
        else
        {
            Debug.LogWarning($"Audio clip for event {eventName} not found.");
        }
    }
}