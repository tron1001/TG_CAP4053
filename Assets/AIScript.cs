using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class AIScript : MonoBehaviour
{
    #region variables
    #region cached variables
    CharacterController _controller;
    Transform _transform;
    Transform player;
	Transform _eyes;
    #endregion
    #region movement variables
    float speed = 5;
    float gravity = 100;
    Vector3 moveDirection;
    float maxRotSpeed = 200.0f;
    float minTime = 0.1f;
    float velocity;
    float range;
    float attackRange;
    bool isCorouting;
    #endregion
    #region waypoint variables
    int index;
    public string strTag;
    Dictionary<int, Transform> waypoint = new Dictionary<int, Transform>();
    #endregion
    #region delegate variable
    delegate void DelFunc();
    delegate IEnumerator DelEnum();
    DelFunc delFunc;
    DelEnum delEnum;
    bool del;
    #endregion
    #endregion
    bool seenAround;
    int layerMask = 1 << 8;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
		_eyes = transform.Find ("Eyes");
        player = GameObject.Find("Player").GetComponent<Transform>();
        if (player == null)
            Debug.LogError("No player on scene");
        if (string.IsNullOrEmpty(strTag)) 
            Debug.LogError("No waypoint tag given");
        
        index = 0;
        range = 2.5f; attackRange = 200f;
        
        GameObject[] gos = GameObject.FindGameObjectsWithTag(strTag);
        foreach (GameObject go in gos)
        {
            WaypointScript script = go.GetComponent<WaypointScript>();
            waypoint.Add(script.index, go.transform);
        }
        animation["victory"].wrapMode = WrapMode.Once;
        
        delFunc = this.Walk;
        delEnum = null;
        del = true;
        
        isCorouting = false;
        seenAround = false;
        layerMask = ~layerMask;
    }

    void Update()
    {
        if (AIFunction() && isCorouting)
        {
            StopAllCoroutines();
            del = true;
        }
        if (del)
            delFunc();
        else if (!isCorouting)
        {
            isCorouting = true;
            StartCoroutine(delEnum());
        }
    }

    #region movement functions
    void Move(Transform target)
    {
        //Movements
        moveDirection = _transform.forward;
        moveDirection *= speed;
        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(moveDirection * Time.deltaTime);
        //Rotation
        var newRotation = Quaternion.LookRotation(target.position - _transform.position).eulerAngles;
        var angles = _transform.rotation.eulerAngles;
        _transform.rotation = Quaternion.Euler(angles.x,
            Mathf.SmoothDampAngle(angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
    }

    void NextIndex()
    {
        if (++index == waypoint.Count) index = 0;
    }

    void Walk()
    {
        if ((_transform.position - waypoint[index].position).sqrMagnitude > range)
        {
            Move(waypoint[index]);
            animation.CrossFade("walk");
        }
        else
        {
            switch (index)
            {
                case 0:
                    del = false;
                    isCorouting = false;
                    delEnum = this.Victory;
                    break;
                case 1:
                    del = false;
                    isCorouting = false;
                    delEnum = this.Wait;
                    break;
                default:
                    NextIndex(); break;
            }
        }
    }

    void Attack()
    {
        if ((_transform.position - player.position).sqrMagnitude > range)
        {
            Move(player);
            animation.CrossFade("charge");
        }
        else
        {
            animation.CrossFade("attackOwn");
        }
    }

    #endregion
   
    #region animation functions
    IEnumerator Victory()
    {
        if (!animation.IsPlaying("victory")) animation.CrossFade("victory");
        yield return new WaitForSeconds(animation["victory"].length);
        NextIndex();
        del = true;
    }
    
    IEnumerator Wait()
    {
        animation.CrossFade("idle");
        yield return new WaitForSeconds(2.0f);
        NextIndex();
        del = true;
    }
    #endregion
    
    #region AI function
    bool AIFunction(){
        Vector3 direction = player.position - _transform.position;
        if (direction.sqrMagnitude < attackRange){
            if (seenAround){
                delFunc = this.Attack;
                return true;
            } else{
                if (Vector3.Dot(direction.normalized, _transform.forward) > 0 &&
                    !Physics.Linecast(_eyes.position, player.position, layerMask)){
	                    delFunc = this.Attack;
	                    seenAround = true;
	                    return true;
                }
                return false;
            }
        }else{
            delFunc = this.Walk;
            seenAround = false;
            return false;
        }
    }
    #endregion
}