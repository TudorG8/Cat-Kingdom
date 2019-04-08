using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager> {
	[SerializeField] List<Resource> resources;

	Dictionary<ResourceType, List<Resource>> resourceDict;

	void Awake() {
		InitiateSingleton ();
		resourceDict = new Dictionary<ResourceType, List<Resource>> ();
		foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType))) {
			resourceDict.Add (type, new List<Resource> ());
		}
		for (int i = 0; i < resources.Count; i++) {
			resourceDict [resources [i].ResourceType].Add (resources [i]);
		}
	}

	public void RemoveResource(Resource resource) {
		resources.Remove (resource);
		resourceDict [resource.ResourceType].Remove (resource);
	}

	public Resource GetNextClosestResource(ResourceType resourceType, Vector3 position, float maxDistance) {
		Resource nextClosest = null;
		float minDistance = float.MaxValue;

		List<Resource> matchingResources = resourceDict [resourceType];

		for (int i = 0; i < matchingResources.Count; i++) {
			Resource matchingResource = matchingResources [i];

			if (matchingResource.gameObject == null) {
				continue;
			}

			if (!matchingResource.CanBeHarvested ()) {
				continue;
			}
			float distance = Vector3.Distance (position, matchingResource.transform.position);

			if (distance < maxDistance && distance < minDistance) {
				nextClosest = matchingResource;
				minDistance = distance;
			}
		}

		return nextClosest;
	}

	public List<Resource> GetAllOfType(ResourceType type) {
		return resourceDict [type];
	}
}
