using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameStartStudio;

public class PopupText : MonoBehaviour
{
    public static void Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        PopupText popupText = Instantiate(ResourceManager.Instance.Load<PopupText>("PopupText"), position, Quaternion.identity);
        popupText.Setup(damageAmount, isCriticalHit);
    }

    private TextMeshPro _textMeshPro;
    private Color _textColor;

    [Header("Move Up")]
    
    public Vector3 moveUpVector = new Vector3(0, 1, 0);
    public float moveUpSpeed = 2.0f;

    [Header("Move Down")]
    public Vector3 moveDownVector = new Vector3(-0.7f, 1, 0);

    [Header("Disappear")]
    public float disappearSpeed = 3.0f;
    private const float DisappearTimeMax = 0.2f;
    private float _disappearTimer;

    [Header("Damage Color")]
    public Color normalColor;
    public Color criticalHitColor;

    private void Awake() 
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    private void Setup(int damageAmount, bool isCriticalHit)
    {
        _textMeshPro.SetText(damageAmount.ToString());
        if(isCriticalHit)
        {
            _textMeshPro.fontSize = 4;
            _textColor = criticalHitColor;
        }
        else
        {
            _textMeshPro.fontSize = 3;
            _textColor = normalColor;
        }

        _textMeshPro.color = _textColor;
        _disappearTimer = DisappearTimeMax;
    }
    
    private void Update()
    {
        
        // Move up
        if (_disappearTimer > DisappearTimeMax * 0.5)
        {
            transform.position += moveUpVector * Time.deltaTime;
            transform.position += Vector3.up * Time.deltaTime * moveUpSpeed;
            transform.localScale += Vector3.one * (Time.deltaTime * 1);
        }
        else
        {
            // Move Down
            transform.position -= moveDownVector * Time.deltaTime;
            transform.localScale -= Vector3.one * (Time.deltaTime * 1);
        }

        //Disappear
        _disappearTimer -= Time.deltaTime;
        if (_disappearTimer < 0)
        {
            //alpha
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _textMeshPro.color = _textColor;
            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
