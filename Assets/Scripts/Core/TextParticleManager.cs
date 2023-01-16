using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextParticleManager : MonoBehaviour
{
    public AnimationCurve initialVelocity = AnimationCurve.Constant(0f, 1f, 5f);
    public AnimationCurve angleSpread = AnimationCurve.Constant(0f, 1f, 0f);

    public float particleLiveTime = 3f;
    
    // Character size that allows to approximate font size to be equal 0.01 unit
    private const float UNIT_CHARACTER_SIZE = 0.105f;

    private static TextParticleManager _instance;
    private static GameObject _particlePrototype;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if (_instance != null)
            throw new Exception("There are more than one TextParticleManager in the scene!");

        _instance = this;
        
        CreatePrototypeParticle();
    }

    private void CreatePrototypeParticle()
    {
        _particlePrototype = new GameObject("TextParticle prototype");
        _particlePrototype.hideFlags = HideFlags.HideAndDontSave;
        _particlePrototype.SetActive(false);
        
        _particlePrototype.AddComponent<Rigidbody2D>();
        
        var textMesh = _particlePrototype.AddComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.characterSize = UNIT_CHARACTER_SIZE;
    }

    private GameObject CreateParticleInstance(string text, Vector3 position, Color color, int fontSize)
    {
        var particle = Instantiate(_particlePrototype);
        particle.hideFlags = HideFlags.HideAndDontSave;
        particle.SetActive(true);
        particle.transform.position = position;

        var textMesh = particle.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;

        var rigidBody = particle.GetComponent<Rigidbody2D>();

        var initialAngle = (angleSpread.Evaluate(Random.Range(0f, 1f)) + 90) * Mathf.Deg2Rad;
        rigidBody.velocity = new Vector2(Mathf.Cos(initialAngle), Mathf.Sin(initialAngle)) * initialVelocity.Evaluate(Random.Range(0f, 1f));

        Destroy(particle, particleLiveTime);
        
        return particle;
    }

    public static GameObject Create(string text, Vector3 position, Color color = default, int fontSize = 24)
    {
        return _instance.CreateParticleInstance(text, position, color, fontSize);
    }
}
