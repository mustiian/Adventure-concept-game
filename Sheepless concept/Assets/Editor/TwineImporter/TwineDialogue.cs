using UnityEngine;

namespace TwineD
{
    [System.Serializable]
    public class TwineDialogue
    {
        public Passage[] passages;
        public string name;
        public int startnode;
        public static TwineDialogue CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<TwineDialogue>(jsonString);
        }
    }

    [System.Serializable]
    public class Passage
    {
        public string text;
        public Link[] links;
        public string name;
        public int pid;
        public NodePosition position;

    }

    [System.Serializable]
    public class Link
    {
        public string name;
        public string link;
        public int pid;
    }

    [System.Serializable]
    public class NodePosition
    {
        public int x;
        public int y;
    }
}
