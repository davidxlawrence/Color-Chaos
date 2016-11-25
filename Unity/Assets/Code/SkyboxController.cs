using UnityEngine;
using System.Collections;

public class SkyboxController : MonoBehaviour {

	void Start() 
	{
		RenderSettings.skybox.SetFloat("_Blend", 0.0f);
		m_timer = 0.0f;
	}
	
	void Update () 
	{
		m_timer += Time.deltaTime;
		RenderSettings.skybox.SetFloat("_Blend", Mathf.Sin(m_timer * 0.05f));
		transform.Rotate(Vector3.up, 4.0f * Time.deltaTime);
	}

	private float m_timer;
}
