using UnityEngine;
using System.Collections;

public class Chese : MonoBehaviour {
	private BoxCollider m_box;
	public GameObject gm;

	public int id;
	private int bg; //blue = 1 or green = 2

	void Awake()
	{
		m_box = GetComponent<BoxCollider> ();
	}

	void OnMouseDown()
	{
		if (!GameManger.wingame) {
			m_box.enabled = false;
			AddNewChese ();
			gm.SendMessage ("GetID", id);
			gm.SendMessage ("GetColor", bg);
			gm.SendMessage ("WhoTurn");
		}
	}

	void AddNewChese()
	{
		Vector3 temp = transform.position;
		GameObject cube;
		Renderer cube_color;
		temp.z = -0.4f;
		cube = Instantiate (Resources.Load ("Chese"), temp, transform.rotation)as GameObject;
		cube_color = cube.GetComponent<Renderer> ();
		if (GameManger.w_color) {
			bg = 1;
			cube_color.material.color = Color.black;
		} else {
			bg = 2;
			cube_color.material.color = Color.gray;
		}
	}
}
