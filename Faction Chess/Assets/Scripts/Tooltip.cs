using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    RectTransform rectTransform;
    float xOffset = 0.2f;
    float yOffset = 0.2f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    //header and content are both optional
    public void SetText(string header = "", string content = "")
    {
        //checking to disable if empty
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        if (string.IsNullOrEmpty(content))
        {
            contentField.gameObject.SetActive(false);
        }
        else
        {
            contentField.gameObject.SetActive(true);
            contentField.text = content;
        }

        //When under WrapLimit, tooltip scales with text. When over, tooltip is controlled by layoutElement
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        if (headerLength > characterWrapLimit || contentLength > characterWrapLimit)
            layoutElement.enabled = true;
        else
            layoutElement.enabled = false;
    }

    private void Update()
    {
        //Position tooltip at mouse
        Vector2 mouseScreenPos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        rectTransform.position = new Vector3(mouseWorldPos.x + xOffset, mouseWorldPos.y + yOffset, rectTransform.position.z);

        //Shift the tooltip so that it avoids going outside the screen...
        float pivotX = mouseScreenPos.x / Screen.width;
        float pivotY = mouseScreenPos.y / Screen.height;

        //...but keep the optimal default pivot of 0,0 if there is no need to shift it
        if (pivotX < 0.7)
            pivotX = 0;
        if (pivotY < 0.7)
            pivotY = 0;

        if (pivotX < 0.5 && xOffset < 0)
            xOffset = -xOffset;
        if (pivotX >= 0.5 && xOffset > 0)
            xOffset = -xOffset;

        if (pivotY < 0.5 && yOffset < 0)
            yOffset = -yOffset;
        if (pivotY >= 0.5 && yOffset > 0)
            yOffset = -yOffset;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }
}
