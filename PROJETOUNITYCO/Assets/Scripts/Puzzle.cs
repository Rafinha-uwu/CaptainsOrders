using UnityEngine;
using System;

public class Puzzle : MonoBehaviour
{
    public event Action<bool> OnPuzzleEnded;
    // true = success, false = failure

    [Header("Audio")]
    public AudioClip successClip; // assign in inspector
    public AudioClip failClip;    // assign in inspector
    private AudioSource audioSource;

    private void Awake()
    {
        // Try to get AudioSource or add one if missing
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void CompletePuzzle()
    {
        Debug.Log($"{gameObject.name} completed");
        PlaySound(successClip);
        OnPuzzleEnded?.Invoke(true);
    }

    public void FailPuzzle()
    {
        Debug.Log($"{gameObject.name} failed");
        PlaySound(failClip);
        OnPuzzleEnded?.Invoke(false);
    }

    public void ActivatePuzzle()
    {
        gameObject.SetActive(true);
    }

    public void DeactivatePuzzle()
    {
        gameObject.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
