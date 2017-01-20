using UnityEngine;
using System.Collections;


//这里是枚举选择敌人类型
public enum EnemyType 
{
	Enemy0, 
	Enemy1
}


public class AI : MonoBehaviour {
	
	
	//敌人类型枚举 有策划人员选择
	public EnemyType enemyType = EnemyType.Enemy0;
	
	//主角游戏对象
	public GameObject player;
	
	
	//敌人状态 普通状态 旋转状态 奔跑状态 追击主角状态 攻击主角状态
	private const int EMEMY_NORMAL=0;
	private const int EMEMY_ROTATION=1;
	private const int EMEMY_RUN = 2;
	private const int EMEMY_CHASE = 3;
	private const int EMEMY_ATTACK = 4;
	
	//记录当前敌人状态 根据不同类型 敌人播放不同动画
	private int state;
	//旋转状态，敌人自身旋转
	private int rotation_state;
	//记录敌人上一次思考时间
	private float aiThankLastTime; 
	
	void Start () 
	{
		//初始话标志敌人状态 以及动画为循环播放
		state = EMEMY_NORMAL;
		this.GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}
	
	void Update () 
	{
		//根据策划选择的敌人类型 这里面会进行不同的敌人AI
		switch(enemyType)
		{
		case EnemyType.Enemy0:
			updateEnemyType0();
			break;
		case EnemyType.Enemy1:
			updateEnemyType1();
			break;
		}
	}
	
	
	//更新第一种敌人的AI
	void updateEnemyType0()
	{
		//这个AI比较简单， 当主角与他的距离小于10米时，他将始终朝向这主角
		if(Vector3.Distance(player.transform.position,this.transform.position) <= 10)
		{
			this.transform.LookAt(player.transform);
		}
	}
	
	//更新第二种敌人的AI
	void updateEnemyType1()
	{
		
		//判断敌人是否开始思考
		if(isAIthank())
		{
			//敌人开始思考
			AIthankEnemyState(3);
		}else
		{
			//更新敌人状态
			UpdateEmenyState();
		}
	}
	
	int getRandom(int count)
	{
		
		 return new System.Random().Next(count);
		
	}
	
	bool isAIthank()
	{
		//这里表示敌人每3秒进行一次思考
		if(Time.time - aiThankLastTime >=3.0f)
		{
			aiThankLastTime = Time.time;
			return true;
			
		}
		return false;
	}
	
	//敌人在这里进行思考
	void AIthankEnemyState(int count)
	{
		//开始随机数字。
		int d = getRandom(count);
	
		switch(d)
		{
		case 0:
			//设置敌人为站立状态
			setEmemyState(EMEMY_NORMAL);
			break;
		case 1:
			//设置敌人为旋转状态
			setEmemyState(EMEMY_ROTATION);
			break;
		case 2:
			//设置敌人为奔跑状态
			setEmemyState(EMEMY_RUN);
			break;
		}

	}
	
	void setEmemyState(int newState)
	{
		if(state == newState)
			return;
		state = newState;
		
		string animName = "Idle";
		switch(state)
		{
		case EMEMY_NORMAL:
			animName  =  "Idle";
			break;
		case EMEMY_RUN:
			animName  =  "Run";
			break;
		case EMEMY_ROTATION:
			animName  =  "Run";
			//当敌人为旋转时， 开始随机旋转的角度系数 
			rotation_state = getRandom(4);
			break;
		case EMEMY_CHASE:
			animName  =  "Run";
			//当敌人进入追击状态时，将面朝主角方向奔跑
			this.transform.LookAt(player.transform);	
			break;
		case EMEMY_ATTACK:
			animName  =  "Attack";
			//当敌人进入攻击状态时，继续朝向主角开始攻击砍人动画
			this.transform.LookAt(player.transform);
			break;
		}
		
		//避免重复播放动画，这里进行判断
		if(!this.GetComponent<Animation>().IsPlaying(animName))
		{
			//播放动画
			this.GetComponent<Animation>().Play(animName);
		}
		
	}
	
	//在这里更新敌人状态
	void UpdateEmenyState()
	{
		//判断敌人与主角之间的距离
		float distance = Vector3.Distance(player.transform.position,this.transform.position);
		//当敌人与主角的距离小于10 敌人将开始面朝主角追击
		if(distance <= 10)
		{
			//当敌人与主角的距离小与3 敌人将开始面朝主角攻击
			if(distance <= 3)
			{
				setEmemyState(EMEMY_ATTACK);
			}else
			{
			    //否则敌人将开始面朝主角追击
				setEmemyState(EMEMY_CHASE);
			}

		}else
		{
			//敌人攻击主角时 主角迅速奔跑 当它们之间的距离再次大于10的时候 敌人将再次进入正常状态 开始思考
			if(state == EMEMY_CHASE || state == EMEMY_ATTACK)
			{
				setEmemyState(EMEMY_NORMAL);
			}
			
		}
		
		switch(state)
		{
		case EMEMY_ROTATION:
			//旋转状态时 敌人开始旋转， 旋转时间为1秒 这样更加具有惯性
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(0,rotation_state * 90,0),  Time.deltaTime * 1);
			break;
		case EMEMY_RUN:
			//奔跑状态，敌人向前奔跑
			transform.Translate(Vector3.forward *0.02f);
			break;
		case EMEMY_CHASE:
			//追击状态 敌人向前开始追击
			transform.Translate(Vector3.forward *0.02f);
			break;
		case EMEMY_ATTACK:
			
			break;
		}
		
		
	}
}
