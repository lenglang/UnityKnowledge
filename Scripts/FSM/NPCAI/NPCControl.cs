using UnityEngine;
using System.Collections;

public class NPCControl : MonoBehaviour {

    private FSMSystem fsm;
    private GameObject player;

    public Transform[] waypoints;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        InitFSM();
	}

    /// <summary>
    /// 初始化状态机
    /// </summary>
    void InitFSM() {
        fsm = new FSMSystem();

        PatrolState patrolState = new PatrolState(waypoints,this.gameObject,player);
        patrolState.AddTransition(Transition.SawPlayer, StateID.Chase);

        ChaseState chaseState = new ChaseState(this.gameObject,player);
        chaseState.AddTransition(Transition.LostPlayer, StateID.Patrol);

        fsm.AddState(patrolState);
        fsm.AddState(chaseState);

        fsm.Start(StateID.Patrol);
    }
    void Update()
    {
        fsm.CurrentState.DoUpdate();
    }
}
