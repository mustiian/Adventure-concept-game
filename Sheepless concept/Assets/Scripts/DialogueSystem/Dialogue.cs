using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwineD;

public class Dialogue : MonoBehaviour
{
    public int ID;
    [HideInInspector]
    public string Name;
    [TextArea]
    public string Text;

    public List<Link> Links = new List<Link>();

    public List<DialogueCommand> Commands;
}
