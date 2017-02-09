using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//状态机管理类，有限状态机系统类
public class FSMSystem  {
    //当前状态机下面有哪些状态
    private Dictionary<StateID, FSMState> states;
    //状态机处于什么状态
    private FSMState currentState;
    public FSMState CurrentState
    {
        get { return currentState; }
    }

    public FSMSystem()
    {
        states = new Dictionary<StateID, FSMState>();
    }
    //往状态机里面添加状态
    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("The state you want to add is null."); return;
        }
        if (states.ContainsKey(state.ID))
        {
            Debug.LogError("The state " + state.ID + " you want to add has already been added."); return;
        }
        state.fsm = this;
        states.Add(state.ID, state);
    }
    //从状态机里面移除状态
    public void DeleteState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("The state you want to delete is null."); return;
        }
        if (states.ContainsKey(state.ID)==false)
        {
            Debug.LogError("The state " + state.ID + " you want to delete is not exit."); return;
        }

        states.Remove(state.ID);
    }
    //控制状态之间的转换
    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("NullTransition is not allowed  for a real transition.");
            return;
        }
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.Log("Transition is not to be happened!没有符合条件的转换");
            return;
        }
        FSMState state;
        states.TryGetValue(id, out state);
        currentState.DoBeforeLeaving();
        currentState = state;
        currentState.DoBeforeEntering();
    }
    //设置默认状态，启动状态机
    public void Start(StateID id)
    {
        FSMState state;
        bool isGet = states.TryGetValue(id, out state);
        if (isGet)
        {
            state.DoBeforeEntering();
            currentState = state;
        }
        else
        {
            Debug.LogError("The state " + id + " is not exit in the fsm.");
        }
    }
}
