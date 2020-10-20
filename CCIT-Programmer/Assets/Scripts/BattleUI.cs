using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Color impossibleColor;
    public Color possibleColor;
    public GameObject judgementBox;
    public string towerDirectoryPath = null;
    public GameObject installationTower = null;
    public GameObject parentGrid = null;
    public GameObject[] deckObjs = new GameObject[5];

    #region 임시 변수
    //데이터베이스에서 덱 정보 가져와야함
    public string[] deck = new string[5]{"Cow", "Giraffe", "Elephant", "Lion", "Bird"};
    #endregion

    private void Start()
    {
        judgementBox = Resources.Load("JudgementBox") as GameObject;
        for(int i = 0; i < deck.Length; i++)
        {
            deckObjs[i] = Resources.Load(towerDirectoryPath  + deck[i]) as GameObject;
        }
    }

    void Update()
    {
        //타워를 선택했다면 레이캐스트로 그리드 선택하여 생성
        if(installationTower != null)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000.0f))
            {
                if (hit.transform.childCount != 0 || hit.transform.tag == "Tower") return;

                if(hit.transform.name == "Grid" )
                {
                    installationTower.transform.position = hit.transform.position + new Vector3(0f, 3.0f, 0f);
                    parentGrid = hit.transform.gameObject;
                }
                else if(hit.transform.gameObject != installationTower)
                {
                    installationTower.transform.position = hit.point;
                    parentGrid = null;
                }
            }
        }
    }

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

    public void Button_ReleaseTowerSelection()
    {
        if(parentGrid != null)
        {
            installationTower.transform.SetParent(parentGrid.transform);
            //생성 이펙트 넣기

            installationTower = null;
            parentGrid = null;
        }
        else
        {
            Destroy(installationTower);
            installationTower = null;
        }
    }
}
