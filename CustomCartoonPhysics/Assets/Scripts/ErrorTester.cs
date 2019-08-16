using CustomPhysics;
using UnityEngine;

public class ErrorTester : MonoBehaviour
{

	void Start()
	{
		TestLineCreation();
	}

	public void TestLineCreation()
	{
		ModelPoint p1 = new ModelPoint(0.15f, 80f);
		ModelPoint p2 = new ModelPoint(0.4f, 60f);

		ModelPointList list = new ModelPointList(initialStrength: 100.0f, lifetime: 1.0f);
		list.Add(p1);
		list.Add(p2);

		MagnitudeDropoffModel model = new MagnitudeDropoffModel(list);
		Debug.Log(model.ToString());
	}

}
