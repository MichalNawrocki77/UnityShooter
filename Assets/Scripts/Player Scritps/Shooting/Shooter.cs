using System.Collections;
using System.Collections.Generic;

using Unity.Properties;

using UnityEngine;

public class Shooter : MonoBehaviour
{
	[SerializeField] BaseInputProvider inputProvider;
	[Header("In degrees")]
	[Tooltip("The spread is going to be a circle, this field specifies the radius of said circle at max spread")]
	[SerializeField] float maxSpread;

	[SerializeField] float rpm;
	/// <summary>
	/// Delay between each shot (in full-auto) in seconds
	/// </summary>
	float shotDelay;
	[SerializeField] GameObject bulletHole;
	
	private void Awake()
	{
		shotDelay = 1 / (rpm / 60);

		inputProvider.OnShootProvided += Shoot;
	}

	void Shoot()
	{
		RaycastHit hitInfo;
		if(Physics.Raycast(transform.position,transform.forward, out hitInfo))
		{			
			hitInfo.transform.GetComponent<IShootable>()?.ReactToGettingShot();
			Instantiate(bulletHole, hitInfo.point, Quaternion.Euler(hitInfo.normal));
		}
	}

}
