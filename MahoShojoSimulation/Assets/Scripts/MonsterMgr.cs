using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeNameSpace;
using BattleNameSpace;

namespace MonsterNamespace
{
    public class MonsterMgr : MonoBehaviour
    {
        public static MonsterMgr Instance = null;
        public Vector3 initialMonsterPos;
        public float attackVFXTime;
        public List<Monster> monsters;
        public float spawnNewMonsterInterval;
        public Battle battleScript;
        public float monsterMoveSpd;
        public GameObject player;
        private bool bStart = false;
        private int currentMonsterIndex = -1;
        GameObject currentMonsterObj;
        private Monster currentMonster;
        private Vector3 playerPos;

        public AudioClip magicHitSFX_cir;
        public AudioClip magicHitSFX_tri;
        public AudioClip magicHitSFX_rec;
        public GameObject magicPoint;

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
        void Start()
        {
            playerPos = player.transform.position;
            StartSpawnMonster();
            //SpawnMonster(0);
            SpawnNextMonster();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartSpawnMonster()
        {
            bStart = true;

        }

        public void SpawnNextMonster()
        {
            if (!bStart)
            {
                return;
            }
            if (currentMonsterObj)
            {
                Destroy(currentMonsterObj);
            }
            currentMonsterIndex++;
            if (currentMonsterIndex >= monsters.Count)
            {
                currentMonsterIndex = 0;
            }
            SpawnMonster(currentMonsterIndex);
            battleScript.MonsterSpawnEvent(currentMonster);
            //int nextMonsterIndex = currentMonsterIndex + 1;
            //if (nextMonsterIndex >= monsters.Count)
            //{
            //    nextMonsterIndex = 0;
            //}
            //TimeMgr.Instance.AddDelayEvent(monsters[nextMonsterIndex].instantiateTime, SpawnNextMonster);
        }

        public void SpawnMonster(int index)
        {
            currentMonster = monsters[index];
            currentMonsterObj = Instantiate(currentMonster.monsterPrefab, currentMonster.spawnPlace.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(currentMonster.spawnClip, currentMonsterObj.transform.position);
            //SoundMgr.Instance.PlayAudio(currentMonster.spawnClip);
            SetMagicPointTrans();
        }

        public void DestroyCurrentMonster()
        {
            currentMonsterObj.transform.Find(battleScript.requiredTrack.ToString()).gameObject.SetActive(true);
            //SoundMgr.Instance.PlayAudio(battleScript.requiredTrack.ToString() + "Hit");
            if (currentMonsterObj != null)
            {
                PlayHitClipByMagicType();
                Destroy(currentMonsterObj, attackVFXTime);
            }
            if (currentMonsterIndex == monsters.Count - 1)
            {
                TimeMgr.Instance.AddDelayEvent(attackVFXTime, battleScript.OnBattleVictory);
                return;
            }
            TimeMgr.Instance.AddDelayEvent(spawnNewMonsterInterval + attackVFXTime, SpawnNextMonster);
        }

        public void DestroyMonsterObjOnFail()
        {
            Destroy(currentMonsterObj);
        }

        public void MonsterTowardPlayer()
        {
            currentMonsterObj.transform.LookAt(player.transform, currentMonsterObj.transform.up);
            if (!currentMonster.bMoveToPlayer)
            {
                return;
            }
            //monsterMoveSpd
            Vector3 direction = (playerPos - currentMonsterObj.transform.position).normalized;
            direction.y = 0;
            currentMonsterObj.transform.position = currentMonsterObj.transform.position + direction * monsterMoveSpd * Time.deltaTime;

            //currentMonsterObj.
        }
        public void StopMonsterMovement()
        {
            currentMonster.bMoveToPlayer = false;
        }

        void PlayHitClipByMagicType()
        {
            if (battleScript.requiredTrack == TrackPatternEnum.circle)
                AudioSource.PlayClipAtPoint(magicHitSFX_cir, currentMonsterObj.transform.position);
            if (battleScript.requiredTrack == TrackPatternEnum.triangle)
                AudioSource.PlayClipAtPoint(magicHitSFX_tri, currentMonsterObj.transform.position);
            if (battleScript.requiredTrack == TrackPatternEnum.rectangle)
                AudioSource.PlayClipAtPoint(magicHitSFX_rec, currentMonsterObj.transform.position);
        }

        void SetMagicPointTrans()
        {
            magicPoint.transform.position = currentMonster.magicPointPlace.position;
            magicPoint.transform.rotation = currentMonster.magicPointPlace.rotation;
        }
    }

    [System.Serializable]
    public class Monster
    {
        public float QTETime;
        public GameObject monsterPrefab;
        public Transform spawnPlace;
        public bool bMoveToPlayer;
        public AudioClip spawnClip;
        public Transform magicPointPlace;
    }

}

