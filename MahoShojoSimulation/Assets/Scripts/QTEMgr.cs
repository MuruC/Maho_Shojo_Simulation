using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using TimeNameSpace;

namespace QuickTimeEventNamespace {
    public enum QuickTimeEventState { run, success, fail, start }
    public class QuickTimeEvent
    {
        public QuickTimeEvent(float timeInterval_, Action OnTriggerEvent_, Action SucceedEvent_, Action FailureEvent_, Func<bool> PassCondition_, bool bContinuous_, Action OnUpdateEvent_)
        {
            timeInterval = timeInterval_;
            OnTriggerEvent = OnTriggerEvent_;
            SucceedEvent = SucceedEvent_;
            FailureEvent = FailureEvent_;
            PassCondition = PassCondition_;
            bContinuous = bContinuous_;
            OnUpdateEvent = OnUpdateEvent_;
        }
        public float timeInterval { get; set; }
        public Action OnTriggerEvent { get; set; }
        public Action SucceedEvent { get; set; }
        public Action FailureEvent { get; set; }
        public Func<bool> PassCondition { get; set; }
        public bool bContinuous { get; set; }
        public Action OnUpdateEvent { get; set; }

        public QuickTimeEventState state = QuickTimeEventState.start;

        public void Update()
        {
            if (state != QuickTimeEventState.run)
            {
                return;
            }
            bool result = PassCondition();
            if (OnUpdateEvent != null)
            {
                OnUpdateEvent();
            }            
            if (result)
            {
                Debug.Log("qte success! play success event!");
                SucceedEvent();
                state = QuickTimeEventState.success;
                QTEMgr.Instance.ContinueQTE();
            }
        }

        public void StartQTE()
        {
            //Debug.Log("Start QTE!!!");
            OnTriggerEvent();
            state = QuickTimeEventState.run;
            TimeMgr.Instance.AddDelayEvent(timeInterval, QTEFail);
        }

        public void QTEFail()
        {
            Debug.Log("state: " + state);
            if (state != QuickTimeEventState.run)
            {
                return;
            }
            FailureEvent();
            state = QuickTimeEventState.fail;
            QTEMgr.Instance.ContinueQTE();
        }
    }


    public class QTEMgr : MonoBehaviour
    {
        public static QTEMgr Instance = null;
        public float QTEInterval;
        private int currentQTEIndex = 0;
        private List<QuickTimeEvent> QTEQueue = new List<QuickTimeEvent>();
        private QuickTimeEvent currentQTE;

        // --------------------------example-------------------------------
        private int currentNum;
        public TMP_Text baseValueText;
        public GameObject successText;
        public GameObject failureText;
        private int userInput = -99;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            /*
            void addRandomNumberInputQte()
            {
                AddNewQTE(5.0f, RandomGenerateBaseValue, SuccessEvent, FailureEvent, CompareInputValue, true, null);
            }
            for (int ii = 1; ii <= 5; ii++)
            {
                addRandomNumberInputQte();
            }
            StartNewQTE();
            */
        }

        // Update is called once per frame
        void Update()
        {
            if (currentQTE == null)
            {
                return;
            }
            currentQTE.Update();
        }

        public void AddNewQTE(float timeInterval_, Action OnTriggerEvent_, Action SucceedEvent_, Action FailureEvent_, Func<bool> PassCondition_, bool bContinuous_, Action OnUpdateEvent_)
        {
            QTEQueue.Add(new QuickTimeEvent(timeInterval_, OnTriggerEvent_, SucceedEvent_, FailureEvent_, PassCondition_, bContinuous_, OnUpdateEvent_));
        }

        public void StartNewQTE()
        {
            currentQTE = QTEQueue[currentQTEIndex];
            currentQTE.StartQTE();
        }

        public void ContinueQTE()
        {
            currentQTE = null;
            currentQTEIndex++;
            if (currentQTEIndex >= QTEQueue.Count)
            {
                return;
            }
            QuickTimeEvent nextQTE = QTEQueue[currentQTEIndex];
            if (!nextQTE.bContinuous)
            {
                return;
            }
            TimeMgr.Instance.AddDelayEvent(QTEInterval, StartNewQTE);
        }

        // --------------------------example-------------------------------
        public void GetUiInput(int input)
        {
            userInput = input;
            print("userInput" + userInput);
        }

        public bool CompareInputValue()
        {
            if (userInput == currentNum)
            {
                return true;
            }
            return false;
        }

        public void RandomGenerateBaseValue()
        {
            userInput = -999;
            currentNum = UnityEngine.Random.Range(-1, 1);
            Debug.Log("random generate value: " + currentNum);
            baseValueText.text = currentNum.ToString();
            successText.SetActive(false);
            failureText.SetActive(false);
        }

        public void SuccessEvent()
        {
            successText.SetActive(true);
        }

        public void FailureEvent()
        {
            failureText.SetActive(true);
        }
    }
}

