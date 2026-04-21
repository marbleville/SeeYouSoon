using System;
using UnityEngine;

public class ParticleListener : MonoBehaviour
{
    public enum GameEvent
    {
        None,
        OnDocumentPickedUp,
        OnDocumentPutDown,
        OnRiddleOneInteracted,
        OnRiddleTwoInteracted,
        OnRiddleThreeInteracted,
        OnRiddleFourInteracted,
        OnElevatorInteracted,
        OnCafeTableInteracted
    }

    [Header("Particle Control")]
    public GameEvent listenFor = GameEvent.None;
    public GameEvent stopOn = GameEvent.None;
    public bool playOnStart = false;
    private ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        var childPS = GetComponentInChildren<ParticleSystem>();
        Debug.Log($"[ParticleListener] GetComponent={particles}, GetComponentInChildren={childPS}, GO active={gameObject.activeInHierarchy}");

        if (particles == null)
        {
            Debug.LogWarning("No particle system found.");
            return;
        }

        // Event-driven particles should never auto-play on scene load
        if (playOnStart)
        {
            particles.Play();
        } else if (listenFor != GameEvent.None)
        {
            var main = particles.main;
            main.playOnAwake = false;
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void OnEnable()
    {
        Subscribe(listenFor, PlayParticles);
        Subscribe(stopOn, StopParticles);
    }

    void OnDisable()
    {
        Unsubscribe(listenFor, PlayParticles);
        Unsubscribe(stopOn, StopParticles);
    }

    void PlayParticles()
    {
        if (particles)
            particles.Play();
    }

    void StopParticles()
    {
        if (particles) 
            particles.Stop();
    }

    void Subscribe(GameEvent gameEvent, Action callback)
    {
        switch (gameEvent)
        {
            case GameEvent.OnDocumentPickedUp: 
                GameEvents.OnDocumentPickedUp += callback;
                break;
            case GameEvent.OnDocumentPutDown:
                GameEvents.OnDocumentPutDown += callback;
                break;
            case GameEvent.OnRiddleOneInteracted:
                GameEvents.OnRiddleOneInteracted += callback;
                break;
            case GameEvent.OnRiddleTwoInteracted:
                GameEvents.OnRiddleTwoInteracted += callback;
                break;
            case GameEvent.OnRiddleThreeInteracted: 
                GameEvents.OnRiddleThreeInteracted += callback;
                break;
            case GameEvent.OnRiddleFourInteracted: 
                GameEvents.OnRiddleFourInteracted += callback;
                break;
            case GameEvent.OnElevatorInteracted:
                GameEvents.OnElevatorInteracted += callback;
                break;
            case GameEvent.OnCafeTableInteracted:
                GameEvents.OnCafeTableInteracted += callback;
                break;
        }
    }
 
    void Unsubscribe(GameEvent gameEvent, Action callback)
    {
        switch (gameEvent)
        {
            case GameEvent.OnDocumentPickedUp: 
                GameEvents.OnDocumentPickedUp -= callback; 
                break;
            case GameEvent.OnDocumentPutDown:
                GameEvents.OnDocumentPutDown -= callback;
                break;
            case GameEvent.OnRiddleOneInteracted:
                GameEvents.OnRiddleOneInteracted -= callback;
                break;
            case GameEvent.OnRiddleTwoInteracted:
                GameEvents.OnRiddleTwoInteracted -= callback;
                break;
            case GameEvent.OnRiddleThreeInteracted: 
                GameEvents.OnRiddleThreeInteracted -= callback;
                break;
            case GameEvent.OnRiddleFourInteracted: 
                GameEvents.OnRiddleFourInteracted -= callback;
                break;
            case GameEvent.OnElevatorInteracted:
                GameEvents.OnElevatorInteracted -= callback;
                break;
            case GameEvent.OnCafeTableInteracted:
                GameEvents.OnCafeTableInteracted -= callback;
                break;
        }
    }
}
