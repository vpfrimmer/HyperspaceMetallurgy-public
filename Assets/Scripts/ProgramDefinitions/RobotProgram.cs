using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotProgram : ScriptableObject
{
    public void CallCommand(Robot caller)
    {
        Execute(caller);
    }

    public virtual void Execute(Robot caller) { }
}
