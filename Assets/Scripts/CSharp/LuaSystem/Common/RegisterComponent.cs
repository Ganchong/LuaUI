using UnityEngine;
using UnityEngine.UI;

public class RegisterComponent
{
	public static Component GetComponent (Transform trans, int compName)
	{
		if (trans == null) {
			return null;
		}
		switch (compName) {
		case 1:
			return trans.GetComponent<Image> ();
		case 2:
			return trans.GetComponent<UIRawImage> ();
		case 3:
			return trans.GetComponent<Button> ();
		case 4:
			return trans.GetComponent<Text> ();
		case 5:
			return trans.GetComponent<Mask> ();
		case 6:
			return trans.GetComponent<Transform> ();
		case 7:
			return trans.GetComponent<Toggle> ();
		case 8:
			return trans.GetComponent<ToggleGroup> ();
		case 9:
			return trans.GetComponent<Slider> ();
		case 10:
			return trans.GetComponent<Scrollbar> ();
		case 11:
			return trans.GetComponent<Dropdown> ();
		case 12:
			return trans.GetComponent<ScrollRect> ();
		case 13:
			return trans.GetComponent<Selectable> ();
		case 14:
			return trans.GetComponent<InputField> ();
		case 15:
			return trans.GetComponent<RectTransform> ();
		case 16:
			return trans.GetComponent<LayoutElement> ();
		case 17:
			return trans.GetComponent<Outline> ();
		case 18:
			return trans.GetComponent<Camera> ();
		case 19:
			return trans.GetComponent<Animation> ();
		case 20:
			return trans.GetComponent<UIButton>();
		case 21:
			return trans.GetComponent<UIImage>();
		default:
			return null;
		}
	}
    
}
