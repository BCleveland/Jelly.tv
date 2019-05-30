using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

    [SerializeField] float m_moveSpeed = 10.0f;
    [SerializeField] Vector3 m_startPosition;
    [SerializeField] Vector3 m_holdPosition;
    private Slime m_target = null;
    private IEnumerator c_capture;

    public void SetTarget(Slime slime) {
        m_target = slime;
    }

    private IEnumerator Capture() {
        //Move to target
        while (transform.position.x != m_target.transform.position.x) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(m_target.transform.position.x, transform.position.y), Time.deltaTime * m_moveSpeed);
            yield return null;
        }
        //Capture target
        while (transform.position.y != m_target.transform.position.y) {
            transform.position = Vector3.Lerp(transform.position, m_target.transform.position, Time.deltaTime * m_moveSpeed);
            yield return null;
        }
        //Animation to grab slime here
        m_target.State = "Claw";
        //Move to hold position;
        while (transform.position.y != m_holdPosition.y) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, m_holdPosition.y), Time.deltaTime * m_moveSpeed);
            yield return null;
        }
        while (transform.position.x != m_holdPosition.x) {
            transform.position = Vector3.Lerp(transform.position, m_holdPosition, Time.deltaTime * m_moveSpeed);
            yield return null;
        }
        //Release target
        m_target.State = "Battle";
        yield return null;
    }
}
