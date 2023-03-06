using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;

public class Dashingskill :  BaseSkillBoss
{

    public override float CD_Skill()
    {
        return 20;
    }

    public override float FirstTimeUse()
    {
        return Time.time + 2;
    }

    public override int LVToUse()
    {
        return 0;
    }
    public ObjectLookUp ObjectLookUp;// ve ra 1 duong line , khi diem cuoi cua line cham vao nhan vat thi notify cho skill nay
    public override void RunSkill(GameObject Boss)
    {
        runSkill = true;
        SkillEnd = false;
        timeToRepeatSkill = Time.time;
    }
    public float bossSpeed = 30;
    Vector3 playerPosition;
    bool dashing = false;
    public void Dashing(Vector3 target)
    {
        runSkill = false;//tien hanh dash thi khong warning nua
        dashing = true;
        this.playerPosition = target;
    }
    // Start is called before the first frame update
    void Start()
    {
        UnityEvent<Vector3> dashEvent = new UnityEvent<Vector3>();
        dashEvent.AddListener(Dashing);
        ObjectLookUp.Action = dashEvent;
    }
    bool runSkill = false;
    float timeToRepeatSkill;
    // Update is called once per frame
    void Update()
    {
        UpdateSkillBaseOnCharacterLv();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (timeToRepeatSkill < Time.time)
        {
            if (runSkill && player != null)
            {
               
                ObjectLookUp.gameObject.transform.LookAt(player.transform.position);
                ObjectLookUp.DrawLineWarning(player.transform.position);
            }
            if (dashing)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, bossSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, playerPosition) <= 0.2)
                {
                    dashing = false;
                    timeRunSkill++;
                    if (timeRunSkill == ableToDo)
                    {
                        SkillEnd = true;
                        timeRunSkill = 0;
                    }
                    else
                    {
                        runSkill = true;// thuc hien run skill lai cho den khi du so luot
                        timeToRepeatSkill = Time.time + 2;
                    }
                }
            }
        }
    }
   
    public override bool AbleToTriggerWithOtherSkill()
    {
        return false;
    }
    bool SkillEnd = true;
    public override bool isSkillEnd()
    {
        return SkillEnd;
    }

    public override bool AbleToTrigger()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;
        if (RangeSkill(player.transform.position))
            return true;
        return false;
    }

    public override bool RangeSkill(Vector3 position)
    {
        //vs lv thap (BaseRange.Range(1) <= Vector3.Distance(transform.position, position)) - khi chua co JumpSkill
        if (Vector3.Distance(transform.position, position) <= BaseRange.Range(2) && BaseRange.Range(1) < Vector3.Distance(transform.position, position)) return true;
        return false;
    }
    [Header("Chi so nang cap")]
    public int ableToDo = 1;//so lan nay se gia tang neu nhan vat len cap
    int timeRunSkill = 0;
    public override void UpdateSkillBaseOnCharacterLv()
    {
        //ableToDo
        return;
    }

    protected override void SetAtkSkill()
    {
        AtkSkill = Mathf.RoundToInt(bossStatus.Atk*1.5f);
    }
}
