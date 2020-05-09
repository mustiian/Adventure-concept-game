using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace TwineD
{
    public class ParsedDialogue
    {
        public ParsedDialogue()
        {
            npcText = new List<NPCText>();
            playerText = new List<PlayerText>();
        }

        public List<NPCText> npcText;
        public List<PlayerText> playerText;
    }

    public class NPCText
    {
        public NPCText(string sentence, string actor)
        {
            Sentence = sentence;
            ActorName = actor;
        }

        public string Sentence;
        public string ActorName;
    }

    public class PlayerText
    {
        public PlayerText(string sentence, string link)
        {
            Sentence = sentence;
            LinkText = link;
        }

        public string Sentence;
        public string LinkText;
    }

    public class DialogueTextParser
    {
        public static ParsedDialogue GetDialogueFromTextLines(string[] lines)
        {
            ParsedDialogue dialogues = new ParsedDialogue();
            Regex regex;

            for (int i = 0; i < lines.Length; i++)
            {
                regex = new Regex(@"\[\[.*\]\]");
                MatchCollection matches = regex.Matches(lines[i]);

                if (matches.Count != 0)
                {
                    regex = new Regex(@"^\[\[.*\->");
                    var sentence = GetDialogueString(regex, matches[0].Value, "->");
                    sentence = sentence.Replace(@"[[", "");

                    regex = new Regex(@"->.*]]");
                    var link = GetDialogueString(regex, matches[0].Value, "->");
                    link = link.Replace(@"]]", "");

                    dialogues.playerText.Add(new PlayerText(sentence, link));
                }
                else
                { 
                    regex = new Regex(@":.*");
                    var sentence = GetDialogueString(regex, lines[i], ":");
                    regex = new Regex(@".*:");
                    var actor = GetDialogueString(regex, lines[i], ":");

                    if (sentence != null)
                        dialogues.npcText.Add(new NPCText(sentence, actor));
                }
            }


            return dialogues;
        }

        private static string GetDialogueString(Regex regex, string line, string deleteChar)
        {
            MatchCollection matches = regex.Matches(line);

            if (matches.Count != 0)
            {
                var sentence = matches[0].Value.Replace(deleteChar, "");
                return sentence;
            }
            else return null;
        }
    }
}

