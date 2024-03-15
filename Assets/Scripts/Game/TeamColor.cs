using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TeamColor : object
{
    // dictionary to store the team colors
    public static Dictionary<int, Color> teamColors = new Dictionary<int, Color>
    {
        {-1, new Color(0.5f, 0.5f, 0.5f, 1f)},
        {0, new Color(250f/255f, 163f/255f, 16f/255f, 1f)},
        {1, new Color(43f/255f, 153f/255f, 249f/255f, 1f)},
        {2, new Color(80f/255f, 250f/255f, 100f/255f, 1f)},
        {3, new Color(200f/255f, 220f/255f, 40f/255f, 1f)}
    };
}
