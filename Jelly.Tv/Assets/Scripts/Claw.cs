using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

    [SerializeField] float m_moveSpeed = 10.0f;
	[SerializeField] Transform m_LeftHook = null;
	[SerializeField] Transform m_RightHook = null;
	[SerializeField] float m_HookClosedAngle = 30.0f;
	[SerializeField] float m_HookOpenAngle = 90.0f;

	private void SetHookAngle(float angle)
	{
		
	}
    private IEnumerator Capture(Slime target, Vector3 startPos, Vector3 desiredPos)
	{
		Vector3 slimePos = target.transform.position;
        //Move to target
		for(float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime*m_moveSpeed)
		{
			transform.position = Vector3.Lerp(startPos, slimePos, timer);
			yield return null;
		}
		//Animation to grab slime here
		target.State = "Claw";
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			SetHookAngle(Mathf.Lerp(m_HookOpenAngle, m_HookClosedAngle, timer));
			yield return null;
		}
		//Move to hold position;
		Vector3 desiredYPos = slimePos;
		desiredYPos.y = desiredPos.y;
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(slimePos, desiredYPos, timer);
			yield return null;
		}
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(desiredYPos, desiredPos, timer);
			yield return null;
		}
		//Release target
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			SetHookAngle(Mathf.Lerp(m_HookClosedAngle, m_HookOpenAngle, timer));
			yield return null;
		}
		target.State = "Battle";
		//return
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(desiredPos, startPos, timer);
			yield return null;
		}
    }
}
