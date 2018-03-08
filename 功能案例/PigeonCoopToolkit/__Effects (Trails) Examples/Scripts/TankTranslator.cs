using UnityEngine;
using System.Collections;
using PigeonCoopToolkit.Effects.Trails;

public class TankTranslator : MonoBehaviour {

    public float TranslateDistance;

    public bool TrailTranslationEnabled = false;

	// Update is called once per frame
	void Update () {

        Vector3 translationVector = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.A))
        {
            translationVector = transform.right * TranslateDistance;
            
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            translationVector = -transform.right * TranslateDistance;
        }

        if(translationVector != Vector3.zero)
        {
            transform.Translate(translationVector);

            if (TrailTranslationEnabled)
            {
                foreach (TrailRenderer_Base trail in GetComponentsInChildren<TrailRenderer_Base>())
                {
                    trail.Translate(translationVector);
                }
            }
        }
	
	}
}
