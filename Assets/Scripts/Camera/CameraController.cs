using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [Serializable]
    public struct ShakeParameters
    {
        public float intensity;
        public float durationSeconds;
        public int priority;
    }

    public ShakeParameters attackPerformedShake;
    public ShakeParameters knockBackShake;
    
    private CinemachineVirtualCamera _virtualCamera;
    private int _currentShakePriority;

    private ActorController _actorController;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _actorController = GetComponentInParent<ActorController>();

        _actorController.OnHit += KnockBackShakeCallback;
        _actorController.OnAttack += AttackShakeCallback;
    }

    private void OnDestroy()
    {
        _actorController.OnHit -= KnockBackShakeCallback;
        _actorController.OnAttack -= AttackShakeCallback;
    }

    private void AttackShakeCallback()
    {
        StartCoroutine(ScreenShake(attackPerformedShake));
    }
    
    private void KnockBackShakeCallback()
    {
        StartCoroutine(ScreenShake(knockBackShake));
    }


    private IEnumerator ScreenShake(ShakeParameters parameters)
    {
        if (parameters. priority < _currentShakePriority)
            yield break;

        _currentShakePriority = parameters.priority;
        var noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = parameters.intensity;
        yield return new WaitForSeconds(parameters.durationSeconds);

        // Recheck if a new shake with higher priority is playing
        if (parameters.priority < _currentShakePriority)
            yield break;
        
        noise.m_AmplitudeGain = 0;
        _currentShakePriority = 0;
    }
}