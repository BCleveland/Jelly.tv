using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

    [SerializeField] float m_moveSpeed = 10.0f;
	[SerializeField] Transform m_LeftHook = null;
	[SerializeField] Transform m_RightHook = null;
	[SerializeField] public Transform m_GrabPosition = null;
	[SerializeField] float m_HookClosedAngle = 30.0f;
	[SerializeField] float m_HookOpenAngle = 90.0f;

	Vector3 m_startPosition;
	private void Awake()
	{
		m_startPosition = transform.position;
	}

	private void SetHookAngle(float angle)
	{
		m_RightHook.rotation = Quaternion.Euler(0, 0, angle);
		m_LeftHook.rotation = Quaternion.Euler(0, 0, -angle);
	}
	public IEnumerator MoveToAndGrabSlime(Slime target)
	{
		SetHookAngle(m_HookOpenAngle);
		Vector3 slimePos = target.transform.position - m_GrabPosition.localPosition;
		//Move to target
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(m_startPosition, slimePos, timer);
			yield return null;
		}
		//Animation to grab slime here
		target.State = "Claw";
		target.Claw = this;
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			SetHookAngle(Mathf.Lerp(m_HookOpenAngle, m_HookClosedAngle, timer));
			yield return null;
		}
	}
	public IEnumerator MoveAboveDesiredPos(Slime target, Vector3 desiredPos)
	{
		Vector3 currentPos = transform.position;
		Vector3 upPos = currentPos;
		upPos.y = m_startPosition.y;
		desiredPos.y = m_startPosition.y;
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(currentPos, upPos, timer);
			yield return null;
		}
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(upPos, desiredPos, timer);
			yield return null;
		}
	}
	public IEnumerator DropAndMoveBack(Slime target)
	{
		//Release target
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			SetHookAngle(Mathf.Lerp(m_HookClosedAngle, m_HookOpenAngle, timer));
			yield return null;
		}
		target.State = "Battle";
		//return back to start position and close
		Vector3 currentPos = transform.position;
		for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * m_moveSpeed)
		{
			transform.position = Vector3.Lerp(currentPos, m_startPosition, timer);
			SetHookAngle(Mathf.Lerp(m_HookOpenAngle, m_HookClosedAngle, timer));
			yield return null;
		}
	}
}
