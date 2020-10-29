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
    //설치 가능 여부 박스 초기화 위치
    public Vector3 judgementBoxInitPos;
    //리소스 폴더 아래 타워 경로
    public string towerDirectoryPath = null;
    //현재 설치중인 타워
    public GameObject installationTower = null;
    //설치하려는 그리드
    public GameObject parentGrid = null;
    //덱에 있는 타워 프리펩
    public GameObject[] deckObjs = new GameObject[5];

    private RaycastHit hit = new RaycastHit();
    private int LayerMask_Ground;
    #region 임시 변수
    //데이터베이스에서 덱 정보 가져와야함
    public string[] deck = new string[5]{"Cow", "Giraffe", "Turtle", "Lion", "Bird"};
    #endregion

    public BattleManager battleManager;

    private void Start()
    {
        LayerMask_Ground = 1 << LayerMask.NameToLayer("Ground");

        //judgementBox = Instantiate(Resources.Load("JudgementBox") as GameObject, judgementBoxInitPos, Quaternion.identity);

        for (int i = 0; i < deck.Length; i++)
        {
            deckObjs[i] = Resources.Load(towerDirectoryPath + deck[i]) as GameObject;
            // deckObjs[i].GetComponent<Collider>().enabled = false;
        }
    }

    void Update()
    {
        //타워를 선택했다면 레이캐스트로 그리드 선택하여 생성
        if(installationTower != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Ground layer만 캐스트
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000.0f, LayerMask_Ground))
            {
                if(!hit.transform.CompareTag("Grid"))
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

                    if(hit.transform.childCount != 0)
                    {
                        //그리드에 타워존재하지 않을 경우
                        judgementBox.layer = 0;
                        judgementBox.GetComponent<Renderer>().material.color = impossibleColor;
                        parentGrid = null;
                    }
                    else
                    {
                        //그리드에 타워가 존재할 때
                        judgementBox.layer = 31;
                        //judgementBox.transform.position = judgementBoxInitPos;
                        judgementBox.GetComponent<Renderer>().material.color = possibleColor; 
                        parentGrid = hit.transform.gameObject;
                    }
                }
            }
        }
    }

    //버튼을 누르면 타워 생성
    public void Button_InstallationTowerSelection(int index)
    {
        if (deck.Length - 1 < index) return;
        if (installationTower != null)
        {
            Destroy(installationTower);
            installationTower = null;
        }

        installationTower = Instantiate(deckObjs[index]);
    }

    //드래그 종료시 설치 가능 여부에 따라 설치 또는 삭제
    public void Button_ReleaseTowerSelection()
    {
        judgementBox.transform.position = judgementBoxInitPos;
        if(parentGrid != null)
        {
            //타워 설치 가능할 경우
            installationTower.transform.SetParent(parentGrid.transform);
            //생성 이펙트 넣기

            //installationTower.GetComponent<Collider>().enabled = true;
            StartCoroutine(installationTower.GetComponent<Tower>().Start_On(battleManager.Tower_Line_Check(installationTower)));

            installationTower = null;
            parentGrid = null;
        }
        else
        {
            //타워 설치가 불가능할 경우
            Destroy(installationTower);
            installationTower = null;
        }
    }
}
