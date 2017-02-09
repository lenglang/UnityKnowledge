using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//有哪些状态转换的条件
public enum Transition
{
    NullTransition = 0,
    SawPlayer,//看到主角
    LostPlayer//看不到主角
}
//状态ID，是每一个状态的唯一表示，一个状态有一个stateid，而且跟其他的状态不可以重复
public enum StateID
{
    NullStateID = 0,
    Patrol,//巡逻
    Chase//追主角状态
}

public abstract class FSMState  {
    protected StateID stateID;
    public StateID ID
    {
        get { return stateID;}
    }

    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

    public FSMSystem fsm;

    public void AddTransition(Transition trans, StateID id)
    {
        if (trans == Transition.NullTransition || id == StateID.NullStateID)
        {
            Debug.LogError("Transition or stateid is null!");
            return;
        }
        if (map.ContainsKey(trans))
        {
            Debug.LogError("State " + stateID + " is already has transition " + trans);
            return;
        }
        map.Add(trans, id);
    }
    public void DeleteTransition(Transition trans)
    {
        if (map.ContainsKey(trans) == false)
        {
            Debug.LogWarning("The transition " + trans + " you want to delete is not exit in map !");
            return;
        }
        map.Remove(trans);
    }
    //根据传递过来的转换条件，判断一下是否可以发生转换
    public StateID GetOutputState(Transition trans)
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    //在进入当前状态之前，需要做的事情
    public virtual void DoBeforeEntering() { }
    public virtual void DoBeforeLeaving() { }

    public abstract void DoUpdate();//在状态机处于当前状态的时候，会一直调用
}