using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public List<StepTask> tasks = new List<StepTask>();

    public async void Dotasks()
    {
        foreach (var stepTask in tasks)
        {
            await stepTask.DoTask();

        }
    }
}
