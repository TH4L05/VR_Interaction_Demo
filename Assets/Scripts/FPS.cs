using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private float dt;
    [SerializeField] private TextMeshProUGUI fps;
    [SerializeField] private bool dislpay = false;

    private void Update()
    {
        if (dislpay)
        {
            dt += (Time.deltaTime - dt) * 0.1f;
            float frames = 1.0f / dt;
            frames = Mathf.Clamp(frames, 0.0f, 999f);
            if (fps != null) fps.text = "FPS: " + Mathf.Ceil(frames).ToString();
        }

    }
}
