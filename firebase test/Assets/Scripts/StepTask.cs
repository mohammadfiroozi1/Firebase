using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum TaskState
{
    InProgress, Finished, Paused
}



public abstract class StepTask : MonoBehaviour
{
    public TaskState taskState;

    public event Action OnstartUp;
    public event Action OnPauseTask;
    public event Action OnResumeTask;


    public StepTask previousTask;
    public StepTask nextTask;

    public virtual void Start()
    {
        
    }

    public virtual async Task DoTask()
    {
        OnstartUp?.Invoke();
    }

    public virtual void OnCompleteTask()
    {

    }

    public virtual void PauseTask()
    {
    }

    public virtual void ResumeTask()
    {

    }
    public virtual void BackToPreviousTask()
    {

    }
    public virtual void GoToNextTask()
    {

    }

    public virtual void LoadData()
    {

    }

}

