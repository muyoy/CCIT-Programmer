using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    //설치 가능 여부 박스 색깔
    public Color impossibleColor;
    public Color possibleColor;
    //설치 가능 여부 박스
    public GameObject judgementBox;
    //리소스 폴더 아래 타워 경로
    public string towerDirectoryPath = null;
    //현재 설치중인 타워
    private GameObject installationTower = null;
    //설치하려는 그리드
    private GameObject parentGrid = null;
    //덱에 있는 타워 프리펩
    public GameObject[] deckObjs = new GameObject[5];

    private RaycastHit hit = new RaycastHit();
    private int LayerMask_Ground;
    private int LayerMask_Product;
    #region 임시 변수
    //데이터베이스에서 덱 정보 가져와야함
    //public string[] deck = new string[5] { "Cow", "Giraffe", "Elephant", "Lion", "Bird" };
    public int temp_MilkCount = 0;
    #endregion

    public BattleManager battleManager;
    public ParticleSystem particleSystem_CreateTower;

    public float temp_CoolTime = 1.0f;
    private int curButtonIndex = -1;
    public Transform[] createButtones;

    public Text milkCount;
    public Image progressBar;

    private void Start()
    {
        milkCount.text = temp_MilkCount.ToString();
        LayerMask_Ground = 1 << LayerMask.NameToLayer("Ground");
        LayerMask_Product = 1 << LayerMask.NameToLayer("Product");
        judgementBox.layer = 31;
        particleSystem_CreateTower.gameObject.layer = 31;

        //judgementBox = Instantiate(Resources.Load("JudgementBox") as GameObject, judgementBoxInitPos, Quaternion.identity);

        //for (int i = 0; i < deck.Length; i++)
        //{
            //deckObjs[i] = Resources.Load(towerDirectoryPath + deck[i]) as GameObject;
            // deckObjs[i].GetComponent<Collider>().enabled = false;
        //}
    }

    void Update()
    {
        //타워를 선택했다면 레이캐스트로 그리드 선택하여 생성
        if (installationTower != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Ground layer만 캐스트
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000.0f, LayerMask_Ground))
            {
                if (!hit.transform.CompareTag("Grid"))
                {
                    //그리드가 아닐 경우
                    judgementBox.layer = 31;
                    //judgementBox.transform.position = judgementBoxInitPos;
                    installationTower.transform.position = hit.point;
                    parentGrid = null;
                }
                else
                {
                    //그리드일 경우
                    installationTower.transform.position = hit.transform.position + new Vector3(0f, 3.0f, 0f);

                    if (hit.transform.childCount != 0)
                    {
                        //그리드에 타워가 존재할 때
                        judgementBox.layer = 0;
                        judgementBox.transform.position = hit.transform.position + new Vector3(0f, 3.0f, 0f);
                        judgementBox.GetComponent<Renderer>().material.color = impossibleColor;
                        parentGrid = null;
                    }
                    else
                    {
                        //그리드에 타워존재하지 않을 경우
                        judgementBox.layer = 31;
                        //judgementBox.transform.position = judgementBoxInitPos;
                        //judgementBox.GetComponent<Renderer>().material.color = possibleColor; 
                        parentGrid = hit.transform.gameObject;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000.0f, LayerMask_Product))
            {
                if(hit.transform.CompareTag("Milk"))
                {
                    SetMilkCount(10);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    #region Button Event
    //버튼을 누르면 타워 생성
    public void Button_InstallationTowerSelection(int index)
    {
        if (deckObjs.Length - 1 < index || deckObjs[index] == null || temp_MilkCount < 50) return;
        if (installationTower != null)
        {
            Destroy(installationTower);
            installationTower = null;
        }

        curButtonIndex = index;
        installationTower = Instantiate(deckObjs[index]);
    }

    //드래그 종료시 설치 가능 여부에 따라 설치 또는 삭제
    public void Button_ReleaseTowerSelection()
    {
        judgementBox.layer = 31;
        if (parentGrid != null)
        {
            StartCoroutine(CO_CoolDown(createButtones[curButtonIndex], temp_CoolTime));
            //타워 설치 가능할 경우
            installationTower.transform.SetParent(parentGrid.transform);
            //생성 이펙트
            particleSystem_CreateTower.transform.position = parentGrid.transform.position + new Vector3(0.0f, 3.5f, 0.0f);
            particleSystem_CreateTower.gameObject.layer = 0;
            particleSystem_CreateTower.Play();
            SetMilkCount(-50);

            StartCoroutine(installationTower.GetComponent<Tower>().Start_On(battleManager.Tower_Line_Check(installationTower)));

            curButtonIndex = -1;
            installationTower = null;
            parentGrid = null;
        }
        else
        {
            //타워 설치가 불가능할 경우
            curButtonIndex = -1;
            Destroy(installationTower);
            installationTower = null;
        }
    }
    #endregion

    private IEnumerator CO_CoolDown(Transform createButton, float coolTime)
    {
        createButton.GetComponent<UICreateTowerButton>().enabled = false;
        Image coolTimeImage = createButton.transform.Find("CoolTimeImage").GetComponent<Image>();
        float time = 0.0f;
        coolTimeImage.fillAmount = 1.0f;

        while (coolTime >= time)
        {
            time += Time.deltaTime;
            coolTimeImage.fillAmount = (coolTime - time) / coolTime;
            yield return null;
        }

        coolTimeImage.fillAmount = 0.0f;
        createButton.GetComponent<UICreateTowerButton>().enabled = true;
    }

    public void SetMilkCount(int _milkCount)
    {
        //if (_milkCount <= 0) _milkCount = 0;
        temp_MilkCount += _milkCount;
        milkCount.text = temp_MilkCount.ToString();
    }

    public void SetRoundProgress(float totalRoundMonsters, float monsterKillCount)
    {
        if (totalRoundMonsters == 0)
        {
#if UNITY_EDITOR
            Debug.LogError("라운드 프로그레스 바 - 입력 전체 몬스터 : 0");
#endif
            return;
        }

        progressBar.fillAmount = monsterKillCount / totalRoundMonsters;
    }
}
