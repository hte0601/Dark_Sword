using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool objectPool;

        //object polling
        [SerializeField] private SwordGoblin swordGoblin;
        private Queue<Enemy> swordGoblinQueue = new Queue<Enemy>();

        [SerializeField] private FireGoblin fireGoblin;
        private Queue<Enemy> fireGoblinQueue = new Queue<Enemy>();

        [SerializeField] private SpearGoblin spearGoblin;
        private Queue<Enemy> spearGoblinQueue = new Queue<Enemy>();

        [SerializeField] private GameObject arrow;


        private Dictionary<ObjectType, Pool> objectDict = new();

        public enum ObjectType
        {
            SwordGoblin,
            FireGoblin,
            SpearGoblin
        }

        private class Pool
        {
            private ObjectType objectType;
            private PoolableObject objectPrefab;
            private Queue<PoolableObject> objectQueue = new();
            private int PreCreateNumber;

            public Pool(ObjectType objectType, PoolableObject objectPrefab, int preCreateNumber)
            {
                this.objectType = objectType;
                this.objectPrefab = objectPrefab;
                this.PreCreateNumber = preCreateNumber;

                for (int i = 0; i < PreCreateNumber; i++)
                {
                    objectQueue.Enqueue(CreateObject());
                }
            }

            public PoolableObject GetObject()
            {
                PoolableObject obj;

                if (objectQueue.Count > 0)
                    obj = objectQueue.Dequeue();
                else
                    obj = CreateObject();

                obj.transform.SetParent(null);

                return obj;
            }

            public void ReturnObject(PoolableObject obj)
            {
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(objectPool.transform);

                objectQueue.Enqueue(obj);
            }

            private PoolableObject CreateObject()
            {
                PoolableObject obj = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, objectPool.transform);
                obj.gameObject.SetActive(false);
                obj.objectType = objectType;

                return obj;
            }
        }

        //enemy create
        private void Awake()
        {
            objectPool = this;
            InitObject();
        }


        private void InitObject()
        {
            // 고블린 오브젝트를 큐에 각각 10개씩 넣음
            for (int i = 0; i < 10; i++)
            {
                spearGoblinQueue.Enqueue(CreateSpearGoblin());
                fireGoblinQueue.Enqueue(CreateFireGoblin());
            }
        }

        private SpearGoblin CreateSpearGoblin()
        {
            var enemy = Instantiate(spearGoblin, Vector3.zero, Quaternion.identity, transform);
            enemy.gameObject.SetActive(false);

            return enemy;
        }

        private FireGoblin CreateFireGoblin()
        {
            var enemy = Instantiate(fireGoblin, Vector3.zero, Quaternion.identity, transform);
            enemy.gameObject.SetActive(false);

            return enemy;
        }

        public Enemy GetSpearGoblin()
        {
            Enemy enemy;

            if (spearGoblinQueue.Count > 0)
                enemy = spearGoblinQueue.Dequeue();
            else
                enemy = CreateSpearGoblin();

            enemy.transform.SetParent(null);

            return enemy;
        }

        public Enemy GetFireGoblin()
        {
            Enemy enemy;

            if (fireGoblinQueue.Count > 0)
                enemy = fireGoblinQueue.Dequeue();
            else
                enemy = CreateFireGoblin();

            enemy.transform.SetParent(null);

            return enemy;
        }

        public void ReturnObject(Enemy enemy)
        {
            // 최고 점수 화살표 어떻게 지울 것인지

            enemy.transform.SetParent(transform);
            enemy.gameObject.SetActive(false);

            if (enemy is SpearGoblin)
            {
                spearGoblinQueue.Enqueue(enemy);
            }
            else if (enemy is FireGoblin)
            {
                fireGoblinQueue.Enqueue(enemy);
            }
        }

        // public void ReturnObject(Arrow obj)
        // {

        // }


        // 이 아래 코드들은 전부 삭제하거나 옮겨야 함
        // 오브젝트 풀의 역할은 오브젝트를 미리 준비시키고, 필요할 때 넘겨주고 다 쓰면 돌려받는 것이 끝임

        //enemy info send GameManager.cs
        //if queue is full, create more enemy
        public static Enemy GetEnemy()
        {
            Enemy enemy = objectPool.RandomEnemy();
            objectPool.DrawBestScoreArrow(enemy);
            objectPool.SetEnemyStartPosition(enemy);

            return enemy;
        }

        void SetEnemyStartPosition(Enemy enemy)
        {
            enemy.transform.position = GameManager.createPos;
        }

        private Enemy RandomEnemy()
        {
            Enemy enemy;

            if (Random.Range(0f, 1.0f) > GameManager.RedGoblinRate)
            {
                //초록 고블린
                if (objectPool.spearGoblinQueue.Count > 0)
                    enemy = objectPool.spearGoblinQueue.Dequeue();
                else
                    enemy = objectPool.CreateSpearGoblin();

                if (Random.Range(0f, 1.0f) > GameManager.AttackGoblinRate)
                {
                    enemy.SetState(1);
                    GameManager.expectScore += 1;
                }
                else
                {
                    enemy.SetState(2);
                    GameManager.expectScore += 2;
                }
            }
            else
            {
                //빨간 고블린
                if (objectPool.fireGoblinQueue.Count > 0)
                    enemy = objectPool.fireGoblinQueue.Dequeue();
                else
                    enemy = objectPool.CreateFireGoblin();

                enemy.SetState(3);
                GameManager.expectScore += 1;
            }

            return enemy;
        }

        void DrawBestScoreArrow(Enemy enemy)
        {
            if (!GameManager.isArrowDrawed && GameManager.expectScore > GameManager.bestScore)
            {
                Vector3 UIposition = enemy.gameObject.transform.position + new Vector3(0, 2, 0);
                var bestScoreUI = Instantiate(arrow, UIposition, Quaternion.Euler(0, 0, 180));
                bestScoreUI.transform.SetParent(enemy.gameObject.transform);

                GameManager.isArrowDrawed = true;
            }
        }

        //Enqueue enemy for reuse
        public static void ReturnObjectOld(Enemy obj)
        {
            objectPool.EraseBestScoreArrow(obj);
            obj.HideEnemy();

            if (obj is SpearGoblin)
            {
                objectPool.spearGoblinQueue.Enqueue(obj);
            }
            else if (obj is FireGoblin)
            {
                objectPool.fireGoblinQueue.Enqueue(obj);
            }
        }

        void EraseBestScoreArrow(Enemy obj)
        {
            if (obj.transform.Find("G_arrow(Clone)") != null)
            {
                Destroy(obj.transform.Find("G_arrow(Clone)").gameObject);
            }
        }
    }
}
