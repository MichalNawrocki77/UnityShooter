using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
	//Since I need the reference to PlayerInputActions I had to get a reference to PlayerInputActions rather than BaseInputActions(giving BaseInputActions a reference to PlayerInputActions doesn't seem right, since I want this class to be used by AI).
	[SerializeField] PlayerInputProvider inputProvider;
	[SerializeField] Transform shootOrigin;

	#region spread
	[Tooltip("The spread is going to be a circle, this field specifies the radius of said circle at max spread. Don't input big numbers, 10 is a lot")]
	[SerializeField] float maxSpread;
	float currentSpread;
	[Tooltip("The amount of shots required in full-auto to reach max spread")]
	[SerializeField] float shotsToMaxSpread;
	float increaseInSpreadPerShot;
	#endregion

	#region RPM
	[Tooltip("The amount of time (in seconds) it takes (after full-auto has been stopped) for spread to go back to 0")]
	[SerializeField] float timeToResetSpread;
	[SerializeField] float rpm;
	/// <summary>
	/// Delay between each shot (in full-auto) in seconds
	/// </summary>
	float shotDelay;
	#endregion

	#region magazine and reload
	[SerializeField] float maxMagazine;
	float currentMagazine;
	[SerializeField] TextMeshProUGUI magazineCount;
	#endregion
	[SerializeField] GameObject bulletHole;
	private void Awake()
	{
		shotDelay = 1 / (rpm / 60);
		increaseInSpreadPerShot = maxSpread / shotsToMaxSpread;
		currentSpread = 0f;
		currentMagazine = maxMagazine;

		inputProvider.ShootProvided += OnShootProvided;
		inputProvider.ShootFinished += OnShootFinished;
	}
	void UpdateMagazineUI()
	{
		magazineCount.text = $"{currentMagazine}/{maxMagazine}";
	}

	void OnShootProvided()
	{
		StopCoroutine(decreaseSpread(timeToResetSpread));
		InvokeRepeating(nameof(Shoot), 0f, shotDelay);
	}
	void OnShootFinished()
	{
		CancelInvoke(nameof(Shoot));
		StartCoroutine(decreaseSpread(timeToResetSpread));
	}
	public void reloadStarted()
	{
		Debug.Log("ReloadStarted()");
		inputProvider.playerInputActions.PlayerMap.ShootAction.Disable();

	}
	public void reloadFinished()
	{ 		
		Debug.Log("ReloadFinished()");
		inputProvider.playerInputActions.PlayerMap.ShootAction.Enable();
	}
	void Shoot()
	{
		RaycastHit hitInfo;
		recoil(currentSpread);
		if(Physics.Raycast(shootOrigin.transform.position,
							shootOrigin.transform.forward,
							out hitInfo))
		{			
			hitInfo.transform.GetComponent<IShootable>()?.ReactToGettingShot();
			Instantiate(bulletHole, hitInfo.point, Quaternion.Euler(hitInfo.normal));
		}
		currentSpread += increaseInSpreadPerShot;
		currentSpread = Mathf.Clamp(currentSpread, 0, maxSpread);

		currentMagazine--;
	}
	void recoil(float currentSpread)
	{
		Vector2 recoilVector = new Vector2(Random.Range(-currentSpread, currentSpread),
                                           Random.Range(-currentSpread, currentSpread));
		//if recoilVector is allowed to be (maxSpread,maxSpread), then the figure "drawn" by all possible hitPoints turns out to be a square (and I want a circle). Thus I have to check if the Vector falls out of this circle and if so change to the radius of this circle
		if(recoilVector.magnitude> maxSpread)
		{
			recoilVector = recoilVector.normalized * maxSpread;
		}
		shootOrigin.transform.localRotation = Quaternion.Euler(recoilVector.x,recoilVector.y,0);
	}
	IEnumerator decreaseSpread(float timeToCoolOff)
	{
		float recoilDecreasePerFixedFrame = maxSpread / (timeToCoolOff / Time.fixedDeltaTime);

		while (currentSpread >= 0)
		{
			currentSpread -= recoilDecreasePerFixedFrame;
			yield return new WaitForFixedUpdate();
		}
		currentSpread = 0;
	}

}
