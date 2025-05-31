using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class CustomAction : Action
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success; // Placeholder action
    }

}
