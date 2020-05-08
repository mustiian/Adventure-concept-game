using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCommand : DialogueCommand
{
    public enum Type { If, Set }
    public Type CommandType;
}
