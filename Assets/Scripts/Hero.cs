using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public void RunCommands(IEnumerable<Command> commands)
    {
        foreach (var command in commands)
        {
            RunCommand(command);
        }
    }

    public void RunCommand(Command command)
    {
        Debug.Log($"Running command: {command}");

        switch (command)
        {
            case Command.DoNothing:
                break;

            case Command.MoveEast:
                transform.position += Vector3.right;
                break;

            case Command.MoveNorth:
                transform.position += Vector3.forward;
                break;

            case Command.MoveSouth:
                transform.position += Vector3.back;
                break;

            case Command.MoveWest:
                transform.position += Vector3.back;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(command), command, null);
        }
    }
}
