using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperDeathSMB : SceneLinkedSMB<EnemyBehaviour>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.DisableDamage();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.gameObject.SetActive(false);
    }
}
