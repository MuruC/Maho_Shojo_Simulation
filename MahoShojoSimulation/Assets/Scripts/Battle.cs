using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using QuickTimeEventNamespace;
using TimeNameSpace;
using MonsterNamespace;
using TMPro;
namespace BattleNameSpace
{
    public enum TrackPatternEnum { circle, triangle, rectangle, none }
    public class Battle : MonoBehaviour
    {
        public float shifaVFXTime;
        public Transform patternVFXTransform;
        public GameObject failText;
        public CheckTrial checkTrialScript;
        public TMP_Text baseInputText;
        private TrackPatternEnum inputTrack = TrackPatternEnum.none;
        public TrackPatternEnum requiredTrack = TrackPatternEnum.none;
        private Vector3 patternVFXPosition;
        private bool bStartBattle = false;
        private GameObject currentVFX;

        public AudioClip magicSucceedSFX_cir;
        public AudioClip magicSucceedSFX_tri;
        public AudioClip magicSucceedSFX_rec;
        public AudioClip battleFailSFX;
        public Transform CameraTrans;

        public UIControl uIControlScript;
        // Start is called before the first frame update
        void Start()
        {
            patternVFXPosition = patternVFXTransform.position;
        }

        // Update is called once per frame
        void Update()
        {


        }
        // public void RegisterTrack(int shapeInput)
        // {
        //     Debug.Log("button input: " + shapeInput);
        //     if (shapeInput == -1)
        //     {
        //         inputTrack = TrackPatternEnum.circle;
        //     }
        //     else if (shapeInput == 0)
        //     {
        //         inputTrack = TrackPatternEnum.triangle;
        //     }
        //     else if (shapeInput == 1)
        //     {
        //         inputTrack = TrackPatternEnum.rectangle;
        //     }
        // }


        public void RegisterTrack(TrackPatternEnum shapeInput)
        {
            inputTrack = shapeInput;
            Debug.Log("inputTrack: " + inputTrack + "required shape: " + requiredTrack);
        }


        public void MonsterSpawnEvent(Monster monster)
        {
            //AddNewQTE(float timeInterval_, Action OnTriggerEvent_, Action SucceedEvent_, Action FailureEvent_, Func<bool> PassCondition_, bool bContinuous_, Action OnUpdateEvent_)
            //Debug.Log("start battle qte");
            QTEMgr.Instance.AddNewQTE(monster.QTETime, OnBattleEnter, OnBattleSucceed, OnBattleFail, PassBattleCondition, false, MonsterMgr.Instance.MonsterTowardPlayer);
            QTEMgr.Instance.StartNewQTE();
        }

        public void OnBattleEnter()
        {
            //Reset 
            //Destroy(currentVFX);
            //Generate Random Pattern
            Array values = Enum.GetValues(typeof(TrackPatternEnum));
            inputTrack = TrackPatternEnum.none;
            System.Random random = new System.Random();
            TrackPatternEnum randomPattern;
            do
            {
                randomPattern = (TrackPatternEnum)values.GetValue(random.Next(values.Length));
            } while (randomPattern == TrackPatternEnum.none);
            requiredTrack = randomPattern;
            Debug.Log("randomPattern: " + requiredTrack);
            //baseInputText.text = requiredTrack.ToString();
            checkTrialScript.SetCurMagicType(requiredTrack);
        }

        public void OnBattleSucceed()
        {
            //Destroy Monster and Play Particle Effect
            //currentVFX = Instantiate(patternVFX[inputTrack], patternVFXPosition, Quaternion.identity);
            MonsterMgr.Instance.StopMonsterMovement();
            PlayMagicSuccessClipByMagicType();
            TimeMgr.Instance.AddDelayEvent(shifaVFXTime, MonsterMgr.Instance.DestroyCurrentMonster);
            checkTrialScript.ResetCheckPointParent();
        }

        void PlayMagicSuccessClipByMagicType()
        {
            if (requiredTrack == TrackPatternEnum.circle)
                AudioSource.PlayClipAtPoint(magicSucceedSFX_cir, CameraTrans.position);
            if (requiredTrack == TrackPatternEnum.triangle)
                AudioSource.PlayClipAtPoint(magicSucceedSFX_tri, CameraTrans.position);
            if (requiredTrack == TrackPatternEnum.rectangle)
                AudioSource.PlayClipAtPoint(magicSucceedSFX_rec, CameraTrans.position);
        }

        public void OnBattleFail()
        {
            Debug.Log("required value: " + requiredTrack + " input value: " + inputTrack);
            failText.SetActive(true);
            AudioSource.PlayClipAtPoint(battleFailSFX, CameraTrans.position);
            bStartBattle = false;
            uIControlScript.SetLose();
            MonsterMgr.Instance.DestroyMonsterObjOnFail();
        }

        public void OnBattleVictory()
        {
            uIControlScript.SetWin();
        }

        public bool PassBattleCondition()
        {
            if (inputTrack != TrackPatternEnum.none && inputTrack == requiredTrack)
            {
                return true;
            }
            return false;
        }
    }
}

