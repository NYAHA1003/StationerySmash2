using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MoneyEffect : MonoBehaviour
{
	//�ν����� ���� ����
	[SerializeField]
	private ParticleSystem _pSystem;
	[SerializeField]
	private RectTransform _target;
	[SerializeField]
	private float _speed = 5f;

	//����
	private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[1000];
	private int _numParticlesAlive;
	private float _step = 0;
	private float _lifeTime = 0f;
	private Coroutine _coroutine = null;

	public void OnEnable()
	{
		_pSystem.Clear();
		_pSystem.Play();
		_step = 0;
		_lifeTime = _pSystem.main.startLifetimeMultiplier;
		if(_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		_coroutine = StartCoroutine(FollowEffect());
	}

	private IEnumerator FollowEffect()
	{
		yield return new WaitForSeconds(0.8f);
		while (_lifeTime > 0)
		{
			if (_particles == null)
			{
				_particles = new ParticleSystem.Particle[_pSystem.main.maxParticles];
			}
			_numParticlesAlive = _pSystem.GetParticles(_particles);
			_step = _speed * Time.deltaTime;
			for (int i = 0; i < _numParticlesAlive; i++)
			{
				_particles[i].position = Vector3.MoveTowards(_particles[i].position, _target.anchoredPosition3D, _step);
				if(_particles[i].position == _target.anchoredPosition3D)
				{
					_particles[i].remainingLifetime = 0f;
				}
			}
			_pSystem.SetParticles(_particles, _numParticlesAlive);
			_lifeTime -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}