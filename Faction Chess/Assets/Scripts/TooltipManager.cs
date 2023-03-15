using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager current;
    public Tooltip tooltip;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }

    public static void Show(string header = "", string content = "")
    {
        current.tooltip.SetText(header, content);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
