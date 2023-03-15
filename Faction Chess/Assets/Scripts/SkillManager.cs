using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameManager gm;
    string whiteFaction;
    string blackFaction;
    public int whiteSkillUsesLeft = 0;
    public int blackSkillUsesLeft = 0;

    public void SetWhiteFaction(string faction)
    {
        whiteFaction = faction;
    }
    public void SetBlackFaction(string faction)
    {
        blackFaction = faction;
    }
    public string GetWhiteFaction()
    {
        return whiteFaction;
    }
    public string GetBlackFaction()
    {
        return blackFaction;
    }
}
