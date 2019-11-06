using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Start Status 
    /// </summary>

    public int Max_Health;
    public int Max_Stamina;
    public int Max_AttackDmg;
    public int Max_Defense;
    public int Max_MoveSpeed;
    public int Max_RunSpeed;
    public int Max_Health_Regen; //체력 자연재생
    public int Max_Health_Regen_Time;//체력 자연재생 빈도

    /// <summary>
    /// Current Status 
    /// </summary>

    private int Current_Health;
    public int Current_Stamina;
    private int Current_AttackDmg;
    private int Current_Defense;
    private int Current_MoveSpeed;
    private int Current_Health_Regen;
    private int Current_Health_Regen_Time;
    private int[] DmgSize = new int[] { 50, 25, 10 }; //날라가는 정도의 기준 데미지
    private int KOspeed = 1000;
    private int[] Stun = new int[] { 3,2,1};
    private bool Moveable = true;
    private bool Runnable = true;
    private bool Running = false;

    /// <summary>
    /// Add Components
    /// </summary>
    public Rigidbody rb;
    public AudioSource sound;
    public Collider col;
    public Animator ani;
    public GameObject HealthBar;
    public GameObject StaminaBar;
    private float Hori;
    private float Vert;

    /// <summary>
    /// etc
    /// </summary>
    
    public float Regen_Timer;
    private bool stamina_reduce = true;


    IEnumerator RunCoroutine()
    {
           
           
                stamina_reduce = false;
                Current_Stamina -= 1;
                Stamina.StaminaCurrent = Current_Stamina;
               
           
            yield return new WaitForSeconds(1f);
            stamina_reduce = true;
    }
    public void TakeDmg(int dmg, Vector3 BossPos)
    {
        Vector3 dir = this.transform.position - BossPos;
        dir.Normalize();
        int dmgSize = (int)(dmg * 100 / Max_Health); //받은 데미지 백분율
        Debug.Log(dmgSize);
        for (int i = 0; i < DmgSize.Length; i++)
        {
            if (dmgSize >= DmgSize[i])
            {
                Debug.Log(dmgSize);
                rb.AddForce(dir * KOspeed );
                break;
            }
        }
        
        Current_Health -= dmg;
       
    }
    
    public int RestoreHealth(int dmg)
    {
        Current_Health += dmg;
        return 0;    
    }
   
    // Start is called before the first frame update
    void Start()
    {
        Current_Health = Max_Health;
        Current_Stamina = Max_Stamina;
        Current_AttackDmg = Max_AttackDmg;
        Current_Defense = Max_Defense;
        Current_MoveSpeed = Max_MoveSpeed;
        Current_Health_Regen = Max_Health_Regen;
        Current_Health_Regen_Time = Max_Health_Regen_Time;

        Health.HealthMax = Max_Health;
        Health.HealthCurrent = Current_Health;

        Stamina.StaminaMax = Max_Stamina;
        Stamina.StaminaCurrent = Current_Stamina;

      Regen_Timer = 0;

        
    }

    // Update is called once per frame
    void Update()
    {
        Regen_Timer += Time.deltaTime;
        if (Regen_Timer>=Current_Health_Regen_Time)
        {
            if (Current_Health  < Max_Health)
            {
                Current_Health += Current_Health_Regen;
            }
            Regen_Timer = 0;
        }
    }
    
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running = true;
            Current_MoveSpeed = Max_RunSpeed;
            if (stamina_reduce&&Moveable)
            {
                StartCoroutine("RunCoroutine");
            }
            
            
        }
        else
        {
            Running = false;
            Current_MoveSpeed = Max_MoveSpeed;
        }
        if (Moveable)
        {
            Hori = Input.GetAxis("Horizontal");
            Vert = Input.GetAxis("Vertical");
            rb.velocity=new Vector3(Hori,0, Vert) * Current_MoveSpeed;
            transform.rotation = Quaternion.LookRotation(new Vector3(Hori, 0, Vert));
        }
    }

}
