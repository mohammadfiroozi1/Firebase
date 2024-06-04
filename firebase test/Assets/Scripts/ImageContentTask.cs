using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ImageContentTask : ContentTask
{
    public override void BackToPreviousTask()
    {
        base.BackToPreviousTask();
    }

    public override async Task DoTask()
    {
        await base.DoTask();
        print("image task done");
    }

    public override void GoToNextTask()
    {
        base.GoToNextTask();
    }

    public override void OnCompleteTask()
    {
        base.OnCompleteTask();
    }

    public override void PauseTask()
    {
        base.PauseTask();
    }
}
