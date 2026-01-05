using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCreator : MonoBehaviour {
	[SerializeField] GameObject effect;

	public void SpawnEffect(Vector3 position) {
		GameObject o = Instantiate (effect, position, effect.transform.rotation);
		o.transform.SetParent (Folders.Instance.Effects);
		ParticleSystem particleSystem = o.GetComponent<ParticleSystem> ();
		particleSystem.Play ();
	}

	public void SpawnEffect() {
		GameObject o = Instantiate (effect, transform.position, effect.transform.rotation);
		o.transform.SetParent (Folders.Instance.Effects);
		ParticleSystem particleSystem = o.GetComponent<ParticleSystem> ();
		particleSystem.Play ();
	}
}
